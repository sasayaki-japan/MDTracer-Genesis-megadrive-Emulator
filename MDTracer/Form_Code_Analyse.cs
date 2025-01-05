 namespace MDTracer
{
    public partial class Form_Code_Trace
    {
        public struct OP_COMMENT1
        {
            public int address;
            public string comment;
            public OP_COMMENT1(int in_address, string in_comment)
            {
                address = in_address;
                comment = in_comment;
            }
        };
        private List<OP_COMMENT1> g_op_comment;

        public enum TRACECODE_TYPE : int
        {
            NON,
            OPC,
            OPR,
            UNIQUE,
            CHK
        }
        public struct TRACECODE
        {
            public TRACECODE_TYPE type;
            public int address;
            public ushort val;
            public string operand;
            public int leng;
            public int next_line;
            public int prev_line;
            public string comment1;
            public bool break_static;
            public bool break_flash;
            public int jmp_address;
            public bool ret_line;
            public uint func_address;
            public bool operand_jsr;
            public List<STACK_LIST> stack;
        }
        public const int ROMSIZE = 0x200000;
        public const int RAMSIZE = 0x8000;
        public const int MEMSIZE = ROMSIZE + RAMSIZE;
        public TRACECODE[] g_analyse_code;

        private string[] MOVEM_MTOA = { "A7","A6","A5","A4","A3","A2","A1","A0"
                                                ,"D7","D6","D5","D4","D3","D2","D1","D0"};
        private string[] MOVEM_ATOM = { "D0","D1","D2","D3","D4","D5","D6","D7"
                                                ,"A0","A1","A2","A3","A4","A5","A6","A7" };

        public void Form_Code_Analyse_initialize()
        {            
            Form_Code_Analyse_initialize_comment();

            for (int i = 0; i < ROMSIZE; i++)
            {
                g_analyse_code[i] = new TRACECODE();
                g_analyse_code[i].type = TRACECODE_TYPE.NON;
                g_analyse_code[i].leng = 1;
                g_analyse_code[i].address = i * 2;
                g_analyse_code[i].val = md_main.g_md_m68k.read16((uint)i * 2);
                g_analyse_code[i].next_line = i + 1;
                g_analyse_code[i].prev_line = i - 1;
                g_analyse_code[i].stack = new List<STACK_LIST>();
            }
            for (int i = 0; i < RAMSIZE; i++)
            {
                g_analyse_code[ROMSIZE + i] = new TRACECODE();
                g_analyse_code[ROMSIZE + i].leng = 1;
                g_analyse_code[ROMSIZE + i].address = 0xff0000 + (i * 2);
                g_analyse_code[ROMSIZE + i].type = TRACECODE_TYPE.NON;
                g_analyse_code[ROMSIZE + i].stack = new List<STACK_LIST>();
            }
            g_analyse_code[0].type = TRACECODE_TYPE.UNIQUE;
            g_analyse_code[0].leng = 2;
            g_analyse_code[0].comment1 = "Vector:Reset: Initial SSP";
            g_analyse_code[2].type = TRACECODE_TYPE.UNIQUE;
            g_analyse_code[2].leng = 2;
            g_analyse_code[2].comment1 = "Vector:Reset: Initial PC";
            g_analyse_code[28].type = TRACECODE_TYPE.UNIQUE;
            g_analyse_code[28].leng = 2;
            g_analyse_code[28].comment1 = "Vector:TRAPV lnstruction";
            g_analyse_code[52].type = TRACECODE_TYPE.UNIQUE;
            g_analyse_code[52].leng = 2;
            g_analyse_code[52].comment1 = "Vector:Leve1 2 Interrupt Autovector";
            g_analyse_code[56].type = TRACECODE_TYPE.UNIQUE;
            g_analyse_code[56].leng = 2;
            g_analyse_code[56].comment1 = "Vector:Leve1 4 Interrupt Autovector";
            g_analyse_code[60].type = TRACECODE_TYPE.UNIQUE;
            g_analyse_code[60].leng = 2;
            g_analyse_code[60].comment1 = "Vector:Leve1 6 Interrupt Autovector";
            for(int i = 0; i < 16;i++)
            {
                g_analyse_code[128 + (i * 2)].type = TRACECODE_TYPE.UNIQUE;
                g_analyse_code[128 + (i * 2)].leng = 2;
                g_analyse_code[128 + (i * 2)].comment1 = "Vector:TRAP lnstruction vectors " + i;
            }
            g_analyse_code[(int)(((g_analyse_code[2].val << 16) + g_analyse_code[3].val)) / 2].type = TRACECODE_TYPE.CHK;
            analyses();
        }
        //-------------------------------------------------
        public void analyses_reset()
        {
            for (int i = 0; i < RAMSIZE; i++)
            {
                g_analyse_code[ROMSIZE + i].type = TRACECODE_TYPE.NON;
                g_analyse_code[ROMSIZE + i].val = 0;
                g_analyse_code[ROMSIZE + i].operand = "";
                g_analyse_code[ROMSIZE + i].leng = 1;
                g_analyse_code[ROMSIZE + i].next_line = 0;
                g_analyse_code[ROMSIZE + i].prev_line = 0;
                g_analyse_code[ROMSIZE + i].comment1 = "";
                g_analyse_code[ROMSIZE + i].break_static = false;
                g_analyse_code[ROMSIZE + i].break_flash = false;
                g_analyse_code[ROMSIZE + i].jmp_address = 0;
                g_analyse_code[ROMSIZE + i].ret_line = false;
                g_analyse_code[ROMSIZE + i].func_address = 0;
                g_analyse_code[ROMSIZE + i].operand_jsr = false;
                g_analyse_code[ROMSIZE + i].stack.Clear();
            }
        }
        public void analyses()
        {
            for (int i = 0; i < RAMSIZE; i++)
            {
                ushort w_val = md_main.g_md_m68k.read16((uint)(0xff0000 + (i * 2)));
                if (g_analyse_code[ROMSIZE + i].val != w_val)
                {
                    g_analyse_code[ROMSIZE + i].type = TRACECODE_TYPE.NON;
                    g_analyse_code[ROMSIZE + i].leng = 1;
                    g_analyse_code[ROMSIZE + i].val = md_main.g_md_m68k.read16((uint)(0xff0000 + (i * 2)));
                    g_analyse_code[ROMSIZE + i].comment1 = "";
                    g_analyse_code[ROMSIZE + i].next_line = i + 1;
                    g_analyse_code[ROMSIZE + i].prev_line = i - 1;
                    g_analyse_code[ROMSIZE + i].ret_line = false;
                }
            }
            int wchange = 0;
            do
            {
                wchange = 0;
                int w_line = 0;
                do
                {
                    int w_next = 0;
                    TRACECODE w_code = g_analyse_code[w_line];
                    if (w_code.type == TRACECODE_TYPE.CHK)
                    {
                        ushort w_op1 = w_code.val;
                        ushort w_op2 = 0;
                        ushort w_op3 = 0;
                        ushort w_op4 = 0;
                        ushort w_op5 = 0;
                        int w_leng = md_main.g_md_m68k.g_opcode_info[w_op1].opleng / 2;
                        w_next = w_line + w_leng;
                        if (w_leng != 0)
                        {
                            g_analyse_code[w_line].type = TRACECODE_TYPE.OPC;
                            g_analyse_code[w_line].leng = w_leng;
                            if ((w_leng >= 2) && (w_line + 1 < MEMSIZE))
                            {
                                w_op2 = g_analyse_code[w_line + 1].val;
                                g_analyse_code[w_line + 1].type = TRACECODE_TYPE.OPR;
                            }
                            if ((w_leng >= 3) && (w_line + 2 < MEMSIZE))
                            {
                                w_op3 = g_analyse_code[w_line + 2].val;
                                g_analyse_code[w_line + 2].type = TRACECODE_TYPE.OPR;
                            }
                            if ((w_leng >= 4) && (w_line + 3 < MEMSIZE))
                            {
                                w_op4 = g_analyse_code[w_line + 3].val;
                                g_analyse_code[w_line + 3].type = TRACECODE_TYPE.OPR;
                            }
                            if ((w_leng >= 5) && (w_line + 4 < MEMSIZE))
                            {
                                w_op5 = g_analyse_code[w_line + 4].val;
                                g_analyse_code[w_line + 4].type = TRACECODE_TYPE.OPR;
                            }

                            //opcode
                            string w_opstr = md_main.g_md_m68k.g_opcode_info[w_op1].opname_out.PadRight(14)
                                                        + md_main.g_md_m68k.g_opcode_info[w_op1].format;
                            w_opstr = analyses_opcode(w_opstr, w_op1, w_op2, w_op3, w_op4, w_code);
                            g_analyse_code[w_line].operand = w_opstr;

                            string w_opname_org = md_main.g_md_m68k.g_opcode_info[w_op1].opname_org;
                            if ((w_opname_org == "JSR")||(w_opname_org == "BSR"))
                            {
                                g_analyse_code[w_line].operand_jsr = true;
                            }

                            //comment
                            int w_memaccess = md_main.g_md_m68k.g_opcode_info[w_op1].memaccess;
                            int w_jmpaddr = analyses_comaddr(w_memaccess, w_op2, w_op3, w_op4, w_op5);
                            g_analyse_code[w_line].comment1 = analyses_comment(w_jmpaddr);

                            //next
                            analyses_next(w_line, w_next, w_code.address, w_jmpaddr, w_op1, w_op2);
                            wchange += 1;
                        }
                        else
                        {
                            g_analyse_code[w_line].type = TRACECODE_TYPE.NON;
                            w_next = w_line + 1;
                        }
                    }
                    else
                    if (w_code.type == TRACECODE_TYPE.NON)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            w_next = w_line + i + 1;
                            if (w_next >= MEMSIZE) break;
                            if (g_analyse_code[w_next].type != TRACECODE_TYPE.NON) break;
                        }
                    }
                    else
                    {
                        w_next = w_line + w_code.leng;
                    }
                    //chain
                    int w_prev = g_analyse_code[w_line].prev_line;
                    for (int i = w_line; i < w_next; i++)
                    {
                        g_analyse_code[i].next_line = w_next;
                        g_analyse_code[i].prev_line = w_prev;
                    }
                    if (w_next < MEMSIZE) g_analyse_code[w_next].prev_line = w_line;
                    w_line = w_next;
                } while (w_line < MEMSIZE);
            } while (wchange > 0);
        }
        public void analyses_next(int in_line, int in_next, int in_addr, int in_jmpaddr, int in_op1 ,int in_op2)
        {
            if ((MEMSIZE <= in_line) || (MEMSIZE <= in_next)) return;

            string w_opname_org = md_main.g_md_m68k.g_opcode_info[in_op1].opname_org;
            if ((w_opname_org != "BRA")
                && (w_opname_org != "JMP")
                && (w_opname_org != "RTE")
                && (w_opname_org != "RTR")
                && (w_opname_org != "RTS"))
            {
                if (g_analyse_code[in_next].type == TRACECODE_TYPE.NON)
                {
                    g_analyse_code[in_next].type = TRACECODE_TYPE.CHK;
                }
            }
            else
            {
                g_analyse_code[in_line].ret_line = true;
            }
            if ((w_opname_org == "BCC") || (w_opname_org == "BSR") || (w_opname_org == "BRA"))
            {
                int w_jmp_offset = (sbyte)(in_op1 & 0x0ff);
                if (w_jmp_offset == 0)
                {
                    w_jmp_offset = (short)in_op2;
                }
                int w_jmp_cur = in_line + 1 + (w_jmp_offset / 2);
                int w_jmp_addr = in_addr + 2 + w_jmp_offset;
                g_analyse_code[in_line].jmp_address = w_jmp_addr;
                if (g_analyse_code[w_jmp_cur].type == TRACECODE_TYPE.NON)
                {
                    g_analyse_code[w_jmp_cur].type = TRACECODE_TYPE.CHK;
                }
            }
            if (w_opname_org == "DBCC")
            {
                int w_jmp_offset = (short)in_op2;
                int w_jmp_cur = in_line + 1 + (w_jmp_offset / 2);
                int w_jmp_addr = in_addr + 2 + w_jmp_offset;
                g_analyse_code[in_line].jmp_address = w_jmp_addr;
                if (g_analyse_code[w_jmp_cur].type == TRACECODE_TYPE.NON)
                {
                    g_analyse_code[w_jmp_cur].type = TRACECODE_TYPE.CHK;
                }
            }
            if ((w_opname_org == "JMP") || (w_opname_org == "JSR"))
            {
                if (in_jmpaddr != 0)
                {
                    int w_jmp_cur = in_jmpaddr & 0x00ffffff;
                    g_analyse_code[in_line].jmp_address = w_jmp_cur;
                    int w_jmp_line = get_code_from_addr((uint)w_jmp_cur);
                    if (g_analyse_code[w_jmp_line].type == TRACECODE_TYPE.NON)
                    {
                        g_analyse_code[w_jmp_line].type = TRACECODE_TYPE.CHK;
                    }
                }
            }
        }
        public string analyses_comment(int in_comaddr)
        {
            string out_comment = "";
            for(int i = 0; i < g_op_comment.Count - 1; i++)
            {
                if ((g_op_comment[i].address <= in_comaddr) &&(in_comaddr < g_op_comment[i + 1].address))
                {
                    out_comment = g_op_comment[i].comment;
                    break;
                }
            }
            return out_comment;
        }
        public int analyses_comaddr(int in_memaccess, int in_op2, int in_op3, int in_op4, int in_op5)
        {
            int out_comaddr = 0;
            if (in_memaccess != 0)
            {
                if (in_memaccess == 2) out_comaddr = (in_op2 << 16) + in_op3;
                else
                if (in_memaccess == 4) out_comaddr = (in_op3 << 16) + in_op4;
                else
                if (in_memaccess == 6) out_comaddr = (in_op4 << 16) + in_op5;
            }
            return out_comaddr;
        }
        public string analyses_opcode(string in_opstr, int in_op1, int in_op2, int in_op3, int in_op4, TRACECODE in_code)
        {
            if (in_opstr.Contains("#OP2LEN2U"))
            {
                string w_chg = "";
                if (in_op2 < 0x8000)
                {
                    w_chg = "0x" + in_op2.ToString("x4");
                }
                else
                {
                    w_chg = "(-0x" + (0x10000 - in_op2).ToString("x4") + ")";
                }
                in_opstr = in_opstr.Replace("#OP2LEN2U", w_chg);
            }
            if (in_opstr.Contains("#OP3LEN2U"))
            {
                string w_chg = "";
                if (in_op3 < 0x8000)
                {
                    w_chg = "0x" + in_op3.ToString("x4");
                }
                else
                {
                    w_chg = "(-0x" + (0x10000 - in_op3).ToString("x4") + ")";
                }
                in_opstr = in_opstr.Replace("#OP3LEN2U", w_chg);
            }
            if (in_opstr.Contains("#OP4LEN2U"))
            {
                string w_chg = "";
                if (in_op4 < 0x8000)
                {
                    w_chg = "0x" + in_op4.ToString("x4");
                }
                else
                {
                    w_chg = "(-0x" + (0x10000 - in_op4).ToString("x4") + ")";
                }
                in_opstr = in_opstr.Replace("#OP4LEN2U", w_chg);
            }
            if (in_opstr.Contains("#OP1LEN1"))
            {
                string w_chg = "0x" + (in_op1 & 0xff).ToString("x2");
                in_opstr = in_opstr.Replace("#OP1LEN1", w_chg);
            }
            if (in_opstr.Contains("#OP2LEN1"))
            {
                string w_chg = "0x" + (in_op2 & 0xff).ToString("x2");
                in_opstr = in_opstr.Replace("#OP2LEN1", w_chg);
            }
            if (in_opstr.Contains("#OP3LEN1"))
            {
                string w_chg = "0x" + (in_op3 & 0xff).ToString("x2");
                in_opstr = in_opstr.Replace("#OP3LEN1", w_chg);
            }
            if (in_opstr.Contains("#OP2LEN2"))
            {
                string w_chg = "0x" + in_op2.ToString("x4");
                in_opstr = in_opstr.Replace("#OP2LEN2", w_chg);
            }
            if (in_opstr.Contains("#OP3LEN2"))
            {
                string w_chg = "0x" + in_op3.ToString("x4");
                in_opstr = in_opstr.Replace("#OP3LEN2", w_chg);
            }
            if (in_opstr.Contains("#OP2LEN4"))
            {
                string w_chg = "0x" + ((in_op2 << 16) + in_op3).ToString("x8");
                in_opstr = in_opstr.Replace("#OP2LEN4", w_chg);
            }
            if (in_opstr.Contains("#OP3LEN4"))
            {
                string w_chg = "0x" + ((in_op3 << 16) + in_op4).ToString("x8");
                in_opstr = in_opstr.Replace("#OP3LEN4", w_chg);
            }
            if (in_opstr.Contains("#OP1LEN30"))
            {
                string w_chg = "0x" + (in_op1 & 0x0f).ToString("x2");
                in_opstr = in_opstr.Replace("#OP1LEN30", w_chg);
            }
            if (in_opstr.Contains("#OP2IND"))
            {
                string w_chg = "";
                if ((in_op2 & 0x8000) == 0) w_chg = "D"; else w_chg = "A";
                w_chg += (in_op2 >> 12) & 0x07;
                if ((in_op2 & 0x0800) == 0) w_chg += ".w"; else w_chg += ".l";
                if (in_op2 < 0x8000)
                {
                    w_chg += "+0x" + in_op2.ToString("x4");
                }
                else
                {
                    w_chg += "-0x" + Math.Abs((0 - (in_op2 & 0xff))).ToString("x4");
                }
                in_opstr = in_opstr.Replace("#OP2IND", w_chg);
            }
            if (in_opstr.Contains("#OP3IND"))
            {
                string w_chg = "";
                if ((in_op3 & 0x8000) == 0) w_chg = "D"; else w_chg = "A";
                w_chg += (in_op3 >> 12) & 0x07;
                if ((in_op3 & 0x0800) == 0) w_chg += ".w"; else w_chg += ".l";
                if (in_op3 < 0x8000)
                {
                    w_chg += "+0x" + in_op3.ToString("x4");
                }
                else
                {
                    w_chg += "-0x" + Math.Abs((0 - (in_op3 & 0xff))).ToString("x4");
                }
                in_opstr = in_opstr.Replace("#OP3IND", w_chg);
            }
            if (in_opstr.Contains("#OP4IND"))
            {
                string w_chg = "";
                if ((in_op4 & 0x8000) == 0) w_chg = "D"; else w_chg = "A";
                w_chg += (in_op4 >> 12) & 0x07;
                if ((in_op4 & 0x0800) == 0) w_chg += ".w"; else w_chg += ".l";
                if (in_op4 < 0x8000)
                {
                    w_chg += "+0x" + in_op4.ToString("x4");
                }
                else
                {
                    w_chg += "-0x" + Math.Abs((0 - (in_op4 & 0xff))).ToString("x4");
                }
                in_opstr = in_opstr.Replace("#OP4IND", w_chg);
            }
            if (in_opstr.Contains("#PCOP1LEN1"))
            {
                int w_pc = in_code.address + (in_code.leng * 2) + (sbyte)(in_op1 & 0xff);
                string w_chg = "0x" + w_pc.ToString("x6");
                in_opstr = in_opstr.Replace("#PCOP1LEN1", w_chg);
            }
            if (in_opstr.Contains("#PCOP2LEN2"))
            {
                int w_pc = in_code.address + (in_code.leng * 2) + (short)in_op2;
                string w_chg = "0x" + w_pc.ToString("x6");
                in_opstr = in_opstr.Replace("#PCOP2LEN2", w_chg);
            }
            if (in_opstr.Contains("#MOVEM_MTOA"))
            {
                string w_chg = "";
                uint w_chk = 0x8000;
                for (int i = 0; i < 16; i++)
                {
                    if ((in_op2 & w_chk) == w_chk)
                    {
                        if (w_chg != "") w_chg += ",";
                        w_chg += MOVEM_MTOA[i];
                    }
                    w_chk >>= 1;
                }
                w_chg = "{" + w_chg + "}";
                in_opstr = in_opstr.Replace("#MOVEM_MTOA", w_chg);
            }
            if (in_opstr.Contains("#MOVEM_ATOM"))
            {
                string w_chg = "";
                uint w_chk = 0x8000;
                for (int i = 0; i < 16; i++)
                {
                    if ((in_op2 & w_chk) == w_chk)
                    {
                        if (w_chg != "") w_chg += ",";
                        w_chg += MOVEM_ATOM[i];
                    }
                    w_chk >>= 1;
                }
                w_chg = "{" + w_chg + "}";
                in_opstr = in_opstr.Replace("#MOVEM_ATOM", w_chg);
            }

            return in_opstr;
        }
        //----------------------------------------------------------------
        public void Form_Code_Analyse_initialize_comment()
        {
            g_op_comment = new List<OP_COMMENT1>();
            g_op_comment.Add(new OP_COMMENT1(0x000000, ""));
            g_op_comment.Add(new OP_COMMENT1(0xa00000, "Z80 SOUND RAM(0xa00000)"));
            g_op_comment.Add(new OP_COMMENT1(0xa02000, ""));
            g_op_comment.Add(new OP_COMMENT1(0xa04000, "Z80 SOUND CHIP(0xa04000)"));
            g_op_comment.Add(new OP_COMMENT1(0xa04004, ""));
            g_op_comment.Add(new OP_COMMENT1(0xa06000, "Z80 BANK REGISTER(0xa06000)"));
            g_op_comment.Add(new OP_COMMENT1(0xa06002, ""));
            g_op_comment.Add(new OP_COMMENT1(0xa10000, "I/O Version No.(0xa10000)"));
            g_op_comment.Add(new OP_COMMENT1(0xa10002, "I/O DATA1(0xa10002)"));
            g_op_comment.Add(new OP_COMMENT1(0xa10004, "I/O DATA2(0xa10004)"));
            g_op_comment.Add(new OP_COMMENT1(0xa10006, "I/O DATA3(0xa10006)"));
            g_op_comment.Add(new OP_COMMENT1(0xa10008, "I/O CNTROL1(0xa10008)"));
            g_op_comment.Add(new OP_COMMENT1(0xa1000a, "I/O CNTROL2(0xa1000a)"));
            g_op_comment.Add(new OP_COMMENT1(0xa1000c, "I/O CNTROL3(0xa1000c)"));
            g_op_comment.Add(new OP_COMMENT1(0xa1000e, "I/O TxDATA1(0xa1000e)"));
            g_op_comment.Add(new OP_COMMENT1(0xa10010, "I/O RxDATA1(0xa10010)"));
            g_op_comment.Add(new OP_COMMENT1(0xa10012, "I/O S-MODE1(0xa10012)"));
            g_op_comment.Add(new OP_COMMENT1(0xa10014, "I/O TxDATA2(0xa10014)"));
            g_op_comment.Add(new OP_COMMENT1(0xa10016, "I/O RxDATA2(0xa10016)"));
            g_op_comment.Add(new OP_COMMENT1(0xa10018, "I/O S-MODE2(0xa10018)"));
            g_op_comment.Add(new OP_COMMENT1(0xa1001a, "I/O TxDATA3(0xa1001a)"));
            g_op_comment.Add(new OP_COMMENT1(0xa1001c, "I/O RxDATA3(0xa1001c)"));
            g_op_comment.Add(new OP_COMMENT1(0xa1001e, "I/O S-MODE3(0xa1001e"));
            g_op_comment.Add(new OP_COMMENT1(0xa10020, ""));
            g_op_comment.Add(new OP_COMMENT1(0xa11000, "CONTROL MEMORY MODE(0xa11000)"));
            g_op_comment.Add(new OP_COMMENT1(0xa11002, ""));
            g_op_comment.Add(new OP_COMMENT1(0xa11100, "CONTROL Z80 BUSREQ(0xa11100)"));
            g_op_comment.Add(new OP_COMMENT1(0xa11102, ""));
            g_op_comment.Add(new OP_COMMENT1(0xa11200, "CONTROL Z80 RESET(0xa11200)"));
            g_op_comment.Add(new OP_COMMENT1(0xa11202, ""));
            g_op_comment.Add(new OP_COMMENT1(0xa130f1, "SRAM MODE(0xa130f1)"));
            g_op_comment.Add(new OP_COMMENT1(0xa130f2, ""));
            g_op_comment.Add(new OP_COMMENT1(0xa14000, "TMSS(0xa14000)"));
            g_op_comment.Add(new OP_COMMENT1(0xa14002, ""));
            g_op_comment.Add(new OP_COMMENT1(0xc00000, "VDP DATA(0xc00000)"));
            g_op_comment.Add(new OP_COMMENT1(0xc00004, "VDP CONTROL(0xc00004)"));
            g_op_comment.Add(new OP_COMMENT1(0xc00008, "VDP HV COUNTER(0xc00008)"));
            g_op_comment.Add(new OP_COMMENT1(0xc0000a, ""));
            g_op_comment.Add(new OP_COMMENT1(0xc00011, "VDP PSG(0xc00011)"));
            g_op_comment.Add(new OP_COMMENT1(0xc00012, ""));
        }
        public int get_code_from_addr(uint in_addr)
        {
            int w_out;
            int w_addr = (int)(in_addr & 0x00ffffff);
            if (w_addr < 0x400000)
            {
                w_out = w_addr / 2;
            }
            else
            {
                w_out = ROMSIZE + ((w_addr - 0xff0000) / 2);
            }
            return w_out;
        }
    }
}
