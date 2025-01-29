 namespace MDTracer
{
    public partial class Form_Code_Trace
    {
        public void update()
        {
            for (int i = 0; i < ROMSIZE; i++)
            {
                g_analyse_code[i] = new TRACECODE();
                g_analyse_code[i].type = TRACECODE.TYPE.NON;
                g_analyse_code[i].leng2 = 1;
                g_analyse_code[i].address = i * 2;
                g_analyse_code[i].val = md_main.g_md_m68k.read16((uint)i * 2);
                g_analyse_code[i].stack = new List<STACK_LIST>();
            }
            for (int i = 0; i < RAMSIZE; i++)
            {
                g_analyse_code[ROMSIZE + i] = new TRACECODE();
                g_analyse_code[ROMSIZE + i].leng2 = 1;
                g_analyse_code[ROMSIZE + i].address = 0xff0000 + (i * 2);
                g_analyse_code[ROMSIZE + i].type = TRACECODE.TYPE.NON;
                g_analyse_code[ROMSIZE + i].stack = new List<STACK_LIST>();
            }
            g_analyse_code[0].type = TRACECODE.TYPE.UNIQUE;
            g_analyse_code[0].leng2 = 2;
            g_analyse_code[0].comment1 = "Vector:Reset: Initial SSP";
            g_analyse_code[1].leng2 = 1;
            g_analyse_code[1].front = 1;
            g_analyse_code[2].type = TRACECODE.TYPE.UNIQUE;
            g_analyse_code[2].leng2 = 2;
            g_analyse_code[2].comment1 = "Vector:Reset: Initial PC";
            g_analyse_code[3].leng2 = 1;
            g_analyse_code[3].front = 1;
            g_analyse_code[28].type = TRACECODE.TYPE.UNIQUE;
            g_analyse_code[28].leng2 = 2;
            g_analyse_code[28].comment1 = "Vector:TRAPV lnstruction";
            g_analyse_code[29].leng2 = 1;
            g_analyse_code[29].front = 1;
            g_analyse_code[52].type = TRACECODE.TYPE.UNIQUE;
            g_analyse_code[52].leng2 = 2;
            g_analyse_code[52].comment1 = "Vector:Leve1 2 Interrupt Autovector";
            g_analyse_code[53].leng2 = 1;
            g_analyse_code[53].front = 1;
            g_analyse_code[56].type = TRACECODE.TYPE.UNIQUE;
            g_analyse_code[56].leng2 = 2;
            g_analyse_code[56].comment1 = "Vector:Leve1 4 Interrupt Autovector";
            g_analyse_code[57].leng2 = 1;
            g_analyse_code[57].front = 1;
            g_analyse_code[60].type = TRACECODE.TYPE.UNIQUE;
            g_analyse_code[60].leng2 = 2;
            g_analyse_code[60].comment1 = "Vector:Leve1 6 Interrupt Autovector";
            g_analyse_code[61].leng2 = 1;
            g_analyse_code[61].front = 1;
            for (int i = 0; i < 16;i++)
            {
                g_analyse_code[128 + (i * 2)].type = TRACECODE.TYPE.UNIQUE;
                g_analyse_code[128 + (i * 2)].leng2 = 2;
                g_analyse_code[128 + (i * 2)].comment1 = "Vector:TRAP lnstruction vectors " + i;
                g_analyse_code[129 + (i * 2)].leng2 = 1;
                g_analyse_code[129 + (i * 2)].front = 1;
            }
            g_analyse_code[(int)(((g_analyse_code[2].val << 16) + g_analyse_code[3].val)) / 2].type = TRACECODE.TYPE.CHK;


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
            analyses();
        }
        //-------------------------------------------------
        public void analyses_reset()
        {
            for (int i = 0; i < RAMSIZE; i++)
            {
                g_analyse_code[ROMSIZE + i].type = TRACECODE.TYPE.NON;
                g_analyse_code[ROMSIZE + i].val = 0;
                g_analyse_code[ROMSIZE + i].operand = "";
                g_analyse_code[ROMSIZE + i].leng2 = 1;
                g_analyse_code[ROMSIZE + i].front = 0;
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
    }
}
