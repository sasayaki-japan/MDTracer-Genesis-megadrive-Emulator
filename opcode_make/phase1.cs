using System.IO;
namespace opcode_make
{
    internal partial class Program
    {
        private static void phase1()
        {
            //-----------------------
            //ABCD
            out_head("ABCD");
            for (int w_mode = 0; w_mode <= 1; w_mode++)
            {
                string wfuncname = string_add("analyse_ABCD_mode_", w_mode);
                out_opcode(wfuncname, 12, -07, 4, w_mode, -07, 2, 0, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("           g_reg_PC += 2;");
                if (w_mode == 0)
                {
                    //Dy TO Dx
                    outtext("           g_work_val1.b0 = g_reg_data[g_op1].b0;");   //dest
                    outtext("           g_work_val2.b0 = g_reg_data[g_op4].b0;");   //source
                    outtext("           g_clock += 6;");
                }
                else
                {
                    //Ay@- TO Ax@-
                    outtext("           g_reg_addr[g_op1].l -= 1;");
                    outtext("           g_work_val1.b0 = md_main.g_md_bus.read8(g_reg_addr[g_op1].l);");
                    outtext("           g_reg_addr[g_op4].l -= 1;");
                    outtext("           g_work_val2.b0 = md_main.g_md_bus.read8(g_reg_addr[g_op4].l);");
                    outtext("           g_clock += 19;");
                }
                outtext("           int wkekka1 = (g_work_val1.b0 & 0xf) + (g_work_val2.b0 & 0xf);");
                outtext("           if (g_status_X == true) wkekka1 += 1;");
                outtext("           if(wkekka1 > 9) { wkekka1 -= 10; g_status_C = true; }");
                outtext("           else g_status_C = false;");
                outtext("           int wkekka2 = ((g_work_val1.b0 >> 4) & 0xf) + ((g_work_val2.b0 >> 4) & 0xf);");
                outtext("           if (g_status_C == true) wkekka2 += 1;");
                outtext("           if(wkekka2 > 9) { wkekka2 -= 10; g_status_C = true; }");
                outtext("           else g_status_C = false;");
                outtext("           g_work_data.b0 = (byte)((wkekka2 << 4) + wkekka1);");
                if (w_mode == 0)
                {
                    outtext("           g_reg_data[g_op1].b0 = g_work_data.b0;");
                }
                else
                {
                    outtext("           md_main.g_md_bus.write8(g_reg_addr[g_op1].l, g_work_data.b0);");
                }
                outtext("           if (g_work_data.b0 != 0) g_status_Z = false;");
                outtext("           g_status_X = g_status_C;");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //ADD
            code_type1("ADD");
            //-----------------------
            //ADDA
            code_type1("ADDA");
            //-----------------------
            //ADDI
            code_type2("ADDI");
            //-----------------------
            //ADDQ
            code_type3("ADDQ");
            //-----------------------
            //ADDX
            out_head("ADDX");
            for (byte wsize = 0; wsize <= 2; wsize++)
            {
                for (int w_mode = 0; w_mode <= 1; w_mode++)
                {
                    string wfuncname = string_add("analyse_ADDX_", SIZE_STR2[wsize], "_mode_", w_mode);
                    out_opcode(wfuncname, 13, -07, (4 + wsize), w_mode, -07, 2, wsize, true);
                    outtext("        private void ", wfuncname, "()");
                    outtext("        {");
                    outtext("            g_reg_PC += 2;");
                    outtext("            int w_size = g_op2 & 0x03;");
                    if (w_mode == 0)
                    {
                        //Dy TO Dx
                        outtext("            g_work_val1.l = ", make_reg_data("g_op1", wsize), ";");
                        outtext("            g_work_val2.l = ", make_reg_data("g_op4", wsize), ";");
                        if (wsize == 2) outtext("            g_clock += 8;");
                        else outtext("            g_clock += 4;");
                    }
                    else
                    {
                        //Ay@- TO Ax@-
                        outtext("            g_reg_addr[g_op1].l -= ", DATA_LENG[wsize], ";");
                        outtext("            g_reg_addr[g_op4].l -= ", DATA_LENG[wsize], ";");
                        outtext("            g_work_val1.l = md_main.g_md_bus.read", DATA_BIT[wsize], "(g_reg_addr[g_op1].l);");
                        outtext("            g_work_val2.l = md_main.g_md_bus.read", DATA_BIT[wsize], "(g_reg_addr[g_op4].l);");
                        if (wsize == 2) outtext("            g_clock += 32;");
                        else outtext("            g_clock += 19;");
                    }

                    outtext("            g_work_data.l = g_work_val1.l + g_work_val2.l;");
                    outtext("            if (g_status_X == true) g_work_data.l += 1;");
                    if (w_mode == 0)
                    {
                        outtext("            write_g_reg_data(g_op1, w_size, g_work_data.l);");
                    }
                    else
                    {
                        outtext("            md_main.g_md_bus.write", DATA_BIT[wsize], "(g_reg_addr[g_op1].l, g_work_data." + SIZE_STR[wsize] + ");");
                    }
                    status_update(2, 4, 2, 2, 2, "g_work_val1", "g_work_val2", "g_work_data", "g_op2 & 0x03");
                    outtext("        }");
                }
            }
            out_tail();
            //-----------------------
            //AND
            code_type1("AND");
            //-----------------------
            //ANDI
            code_type2("ANDI");
            //-----------------------
            //ANDI TO CCR
            code_type7("ANDITOCCR");
            //-----------------------
            //ANDI TO SR
            code_type7("ANDITOSR");
            //-----------------------
            //ASL ASR
            code_type4("AS");
            //-----------------------
            //BCC
            out_head("BCC");
            for (int w_mode = 0; w_mode <= 1; w_mode++)
            {
                string wfuncname = string_add("analyse_Bcc_", ((w_mode == 0) ? "w" : "b"));
                if (w_mode == 0)
                {
                    out_opcode(wfuncname, 6, -17, 0, 0, 0, 4, 1, true);
                    out_opcode(wfuncname, 6, -17, -44, 0, 0, 4, 1, true);
                }
                else
                {
                    out_opcode(wfuncname, 6, -17, -07, -07, -100, 2, 0, true);
                }
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_clock += 10;");
                outtext("            g_reg_PC += 2;");
                if (w_mode == 0)
                {
                    outtext("           uint w_next_pc_work = (uint)(g_reg_PC + (short)md_main.g_md_bus.read16(g_reg_PC));");
                    outtext("           g_reg_PC += 2;");
                }
                else
                {
                    outtext("           uint w_next_pc_work = (uint)(g_reg_PC + (sbyte)(g_opcode & 0x00ff));");
                }
                outtext("           if(g_flag_chack[(g_opcode >> 8) & 0x0f]()) g_reg_PC = w_next_pc_work;");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //BCHG
            code_type8("BCHG");
            //-----------------------
            //BCLR
            code_type8("BCLR");
            //-----------------------
            //BRA
            out_head("BRA");
            for (int w_mode = 0; w_mode <= 1; w_mode++)
            {
                string wfuncname = string_add("analyse_BRA_", ((w_mode == 0) ? "w" : "b"));
                if (w_mode == 0)
                {
                    out_opcode(wfuncname, 6, 0, 0, 0, 0, 4, 1, true);
                }
                else
                {
                    out_opcode(wfuncname, 6, 0, -03, -07, -100, 2, 0, true);
                }
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("           g_clock += 10;");
                outtext("           g_reg_PC += 2;");
                if (w_mode == 0)
                {
                    outtext("           g_reg_PC = (uint)(g_reg_PC + (short)md_main.g_md_bus.read16(g_reg_PC));");
                }
                else
                {
                    outtext("           g_work_data.b0 = (byte)(g_opcode & 0x00ff);");
                    outtext("           g_reg_PC = (uint)(g_reg_PC + (sbyte)g_work_data.b0);");
                }
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //BSET
            code_type8("BSET");
            //-----------------------
            //BSR
            out_head("BSR");
            for (int w_mode = 0; w_mode <= 1; w_mode++)
            {
                string wfuncname = string_add("analyse_BSR_", ((w_mode == 0) ? "w" : "b"));
                if (w_mode == 0)
                {
                    out_opcode(wfuncname, 6, 0, 4, 0, 0, 4, 1, true);
                }
                else
                {
                    out_opcode(wfuncname, 6, 0, -47, -07, -100,  2, 0, true);
                }
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_clock += 20;");
                outtext("            uint w_pc = g_reg_PC;");
                outtext("            g_reg_PC += 2;");
                if (w_mode == 0)
                {
                    outtext("            uint w_start_address = (uint)(g_reg_PC + (short)md_main.g_md_bus.read16(g_reg_PC));");
                    outtext("            stack_push32(g_reg_PC + 2);");
                    outtext("            md_main.g_form_code_trace.CPU_Trace_push(Form_Code_Trace.STACK_LIST_TYPE.BSR, w_pc, w_start_address, g_reg_PC + 2, g_reg_addr[7].l);");
                    outtext("            g_reg_PC = w_start_address;");
                }
                else
                {
                    outtext("            g_work_data.b0 = (byte)(g_opcode & 0x00ff);");
                    outtext("            uint w_start_address = (uint)(g_reg_PC + (sbyte)g_work_data.b0);");
                    outtext("            stack_push32(g_reg_PC);");
                    outtext("            md_main.g_form_code_trace.CPU_Trace_push(Form_Code_Trace.STACK_LIST_TYPE.BSR, w_pc, w_start_address, g_reg_PC, g_reg_addr[7].l);");
                    outtext("            g_reg_PC = w_start_address;");
                }
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //BTST
            code_type8("BTST");
            //-----------------------
            //CHK
            out_head("CHK");
            {
                string wfuncname = string_add("analyse_CHK");
                out_opcode(wfuncname, 4, -07, 6, -99, -07, 2, 1, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_clock += 43;");
                outtext("            g_reg_PC += 2;");
                outtext("            g_work_val1.w = g_reg_data[g_op1].w;");
                outtext("            adressing_func_address(g_op3, g_op4, 1);");
                outtext("            g_work_val2.w = (ushort)adressing_func_read(g_op3, g_op4, 1);");
                outtext("            if (g_work_val1.w < 0){ g_status_N = true; g_reg_PC = md_main.g_md_bus.read32(24); }");
                outtext("            else if (g_work_val2.w < g_work_val1.w) { g_status_N = false; g_reg_PC = md_main.g_md_bus.read32(24); }");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //CLR
            out_head("CLR");
            for (byte wsize = 0; wsize <= 2; wsize++)
            {
                string wfuncname = string_add("analyse_CLR_", SIZE_STR2[wsize]);
                out_opcode(wfuncname, 4, 1, wsize, -99, -07 , 2, wsize, true);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            if(g_op3 <= 1){");
                if (wsize == 2) outtext("                g_clock += 6;");
                           else outtext("                g_clock += 4;");
                outtext("            }else{");
                if (wsize == 2) outtext("                g_clock += 14;");
                           else outtext("                g_clock += 9;");
                outtext("            }");
                outtext("            g_reg_PC += 2;");
                outtext("            adressing_func_address(g_op3, g_op4, ", wsize, ");");
                outtext("            adressing_func_write(g_op3, g_op4, "+wsize+", 0);");
                status_update(0, 1, 0, 0, 9, "0", "0", "0", "g_op2");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //CMP
            code_type1("CMP");
            //-----------------------
            //CMPA
            code_type1("CMPA");
            //-----------------------
            //CMPI
            code_type2("CMPI");
            //-----------------------
            //CMPM
            out_head("CMPM");
            for (byte wsize = 0; wsize <= 2; wsize++)
            {
                string wfuncname = string_add("analyse_CMPM_", SIZE_STR2[wsize]);
                out_opcode(wfuncname, 11, -07, (4 + wsize), 1, -07, 2, wsize, true);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                if (wsize == 2) outtext("            g_clock += 20;");
                else outtext("            g_clock += 12;");
                outtext("            g_reg_PC += 2;");
                outtext("            g_work_val1.l = md_main.g_md_bus.read", DATA_BIT[wsize], "(g_reg_addr[g_op1].l);");
                outtext("            g_work_val2.l = md_main.g_md_bus.read", DATA_BIT[wsize], "(g_reg_addr[g_op4].l);");
                outtext("            g_work_data.l = g_work_val1.l - g_work_val2.l;");
                outtext("            g_reg_addr[g_op1].l += ", DATA_LENG[wsize], ";");
                outtext("            g_reg_addr[g_op4].l += ", DATA_LENG[wsize], ";");
                status_update(2, 2, 3, 3, 9, "g_work_val1", "g_work_val2", "g_work_data", "g_op2 & 0x03");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //DBCC
            out_head("DBCC");
            {
                string wfuncname = string_add("analyse_DBcc");
                out_opcode(wfuncname, 5, -07, 3, 1, -07, 4, 1, false);
                out_opcode(wfuncname, 5, -07, 7, 1, -07, 4, 1, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("           g_clock += 12;");
                outtext("           g_reg_PC += 2;");
                outtext("           uint w_next_pc_work = (uint)(g_reg_PC +(short)md_main.g_md_bus.read16(g_reg_PC));");
                outtext("           g_reg_PC += 2;");
                outtext("            if(g_flag_chack[(g_opcode >> 8) & 0x0f]()) { }");
                outtext("            else {");
                outtext("                g_reg_data[g_op4].w -= 1;");
                outtext("                if((short)g_reg_data[g_op4].w != -1) g_reg_PC = w_next_pc_work;");
                outtext("            }");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //DIVS
            out_head("DIVS");
            {
                string wfuncname = string_add("analyse_DIVS");
                out_opcode(wfuncname, 8, -07, 7, -99, -07, 2, 1, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_reg_PC += 2;");
                outtext("            adressing_func_address(g_op3, g_op4, 1);");
                outtext("            g_clock = 158;");
                outtext("            g_work_data.w = (ushort)adressing_func_read(g_op3, g_op4, 1);");
                outtext("            if (g_work_data.w == 0) { g_reg_PC = md_main.g_md_bus.read32(20); return; }");
                outtext("            g_work_val1.l = (uint)((int)g_reg_data[g_op1].l / (short)g_work_data.w);");
                outtext("            if (((int)g_work_val1.l < -32768)|| (32767 < (int)g_work_val1.l)) { g_status_V = true; }");
                outtext("            else {");
                outtext("                g_status_V = false;");
                outtext("                g_work_val2.l = (uint)((int)g_reg_data[g_op1].l % (short)g_work_data.w);");
                outtext("                g_work_data.w = g_work_val1.w;");
                outtext("                g_work_data.wup = g_work_val2.w;");
                outtext("                g_reg_data[g_op1].l = g_work_data.l;");
                outtext("                if((g_work_val1.w & 0x8000) == 0x8000) g_status_N = true; else g_status_N = false;");
                outtext("                if(g_work_val1.w == 0) g_status_Z = true; else g_status_Z = false;");
                outtext("            }");
                outtext("            g_status_C = false;");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //DIVU
            out_head("DIVU");
            {
                string wfuncname = string_add("analyse_DIVU");
                out_opcode(wfuncname,  8, -07, 3, -99, -07, 2, 1, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_reg_PC += 2;");
                outtext("            adressing_func_address(g_op3, g_op4, 1);");
                outtext("            g_clock = 140;");
                outtext("            g_work_data.w = (ushort)adressing_func_read(g_op3, g_op4, 1);");
                outtext("            if (g_work_data.w == 0) { g_reg_PC = md_main.g_md_bus.read32(20); return; }");
                outtext("            g_work_val1.l = (uint)(g_reg_data[g_op1].l / g_work_data.w);");
                outtext("            if ((uint)g_work_val1.l > 0xffff) { g_status_V = true; }");
                outtext("            else {");
                outtext("                g_status_V = false;");
                outtext("                g_work_val2.l = (uint)(g_reg_data[g_op1].l % g_work_data.w);");
                outtext("                g_work_data.w = g_work_val1.w;");
                outtext("                g_work_data.wup = g_work_val2.w;");
                outtext("                g_reg_data[g_op1].l = g_work_data.l;");
                outtext("                if((g_work_val1.w & 0x8000) == 0x8000) g_status_N = true; else g_status_N = false;");
                outtext("                if(g_work_val1.w == 0) g_status_Z = true; else g_status_Z = false;");
                outtext("            }");
                outtext("            g_status_C = false;");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //EOR
            code_type1("EOR");
            //-----------------------
            //EORI
            code_type2("EORI");
            //-----------------------
            //EORI TO CCR
            code_type7("EORITOCCR");
            //-----------------------
            //EORI TO SR
            code_type7("EORITOSR");
            //-----------------------
            //EXG
            out_head("EXG");
            {
                for (int wop = 0; wop <= 2; wop++)
                {
                    byte g_op2 = 0;
                    byte g_op3 = 0;
                    if (wop == 0)
                    {
                        g_op2 = 5;
                        g_op3 = 0;
                    }
                    else
                    if (wop == 1)
                    {
                        g_op2 = 5;
                        g_op3 = 1;
                    }
                    else
                    {
                        g_op2 = 6;
                        g_op3 = 1;
                    }
                    string wfuncname = string_add("analyse_EXG_", wop);
                    out_opcode(wfuncname,  12, -07, g_op2, g_op3, -07, 2, 2, false);
                    outtext("        private void ", wfuncname, "()");
                    outtext("        {");
                    outtext("            g_reg_PC += 2;");
                    if (wop == 0)
                    {
                        outtext("            g_work_data.l = g_reg_data[g_op1].l;");
                        outtext("            g_reg_data[g_op1].l = g_reg_data[g_op4].l;");
                        outtext("            g_reg_data[g_op4].l = g_work_data.l;");
                    }
                    else
                    if (wop == 1)
                    {
                        outtext("            g_work_data.l = g_reg_addr[g_op1].l;");
                        outtext("            g_reg_addr[g_op1].l = g_reg_addr[g_op4].l;");
                        outtext("            g_reg_addr[g_op4].l = g_work_data.l;");
                    }
                    else
                    {
                        outtext("            g_work_data.l = g_reg_data[g_op1].l;");
                        outtext("            g_reg_data[g_op1].l = g_reg_addr[g_op4].l;");
                        outtext("            g_reg_addr[g_op4].l = g_work_data.l;");
                    }
                    outtext("           g_clock += 6;");
                    outtext("        }");
                }
            }
            out_tail();
            //-----------------------
            //EXT
            out_head("EXT");
            {
                string wfuncname = string_add("analyse_EXT2");
                out_opcode(wfuncname,  4, 4, 2, 0, -07, 2, 1, true);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_reg_PC += 2;");
                outtext("            if((g_reg_data[g_op4].b0 & 0x80) == 0){");
                outtext("                g_reg_data[g_op4].b1 = 0;");
                outtext("            }else{");
                outtext("                g_reg_data[g_op4].b1 = 0xff;");
                outtext("            }");
                outtext("            g_work_data.w = g_reg_data[g_op4].w;");
                status_update(2, 2, 0, 0, 9, "g_work_data", "g_work_data", "g_work_data", "1");
                outtext("           g_clock += 4;");
                outtext("        }");
                wfuncname = string_add("analyse_EXT3");
                out_opcode(wfuncname,  4, 4, 3, 0, -07, 2, 2, true);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_reg_PC += 2;");
                outtext("            if((g_reg_data[g_op4].w & 0x8000) == 0){");
                outtext("                g_reg_data[g_op4].wup = 0;");
                outtext("            }else{");
                outtext("                g_reg_data[g_op4].wup = 0xffff;");
                outtext("            }");
                outtext("            g_work_data.l = g_reg_data[g_op4].l;");
                status_update(2, 2, 0, 0, 9, "g_work_data", "g_work_data", "g_work_data", "2");
                outtext("           g_clock += 4;");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //JMP
            out_head("JMP");
            {
                string wfuncname = string_add("analyse_JMP");
                out_opcode(wfuncname,  4, 7, 3, -85, -07, 2, 0, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_clock += 4;");
                outtext("            g_reg_PC += 2;");
                outtext("            adressing_func_address(g_op3, g_op4, 2);");
                outtext("            g_reg_PC = g_analyze_address;");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //JSR
            out_head("JSR");
            {
                string wfuncname = string_add("analyse_JSR");
                out_opcode(wfuncname,  4, 7, 2, -85, -07, 2, 0, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_clock += 14;");
                outtext("            uint w_pc = g_reg_PC;");
                outtext("            g_reg_PC += 2;");
                outtext("            adressing_func_address(g_op3, g_op4, 2);");
                outtext("            stack_push32(g_reg_PC);");
                outtext("            md_main.g_form_code_trace.CPU_Trace_push(Form_Code_Trace.STACK_LIST_TYPE.JSR, w_pc, g_analyze_address, g_reg_PC, g_reg_addr[7].l);");
                outtext("            g_reg_PC = g_analyze_address;");
                outtext("        }");
            }
            out_tail();

            //-----------------------
            //LEA
            out_head("LEA");
            {
                string wfuncname = string_add("analyse_LEA");
                out_opcode(wfuncname,  4, -07, 7, -85, -07, 2, 2, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_reg_PC += 2;");
                outtext("            adressing_func_address(g_op3, g_op4, 2);");
                outtext("            g_reg_addr[g_op1].l = g_analyze_address;");
                outtext("        }");
            }
            out_tail();

            //-----------------------
            //LINK
            out_head("LINK");
            {
                string wfuncname = string_add("analyse_LINK");
                out_opcode(wfuncname,  4, 7, 1, 2, -07, 4, 0, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_clock += 18;");
                outtext("            g_reg_PC += 2;");
                outtext("            short w_ext_disp = (short)md_main.g_md_bus.read16(g_reg_PC);");
                outtext("            g_reg_PC += 2;");
                outtext("            stack_push32(g_reg_addr[g_op4].l);");
                outtext("            g_reg_addr[g_op4].l = g_reg_addr[7].l;");
                outtext("            g_reg_addr[7].l = (uint)(g_reg_addr[7].l + w_ext_disp);");
                outtext("        }");
            }
            out_tail();

            //-----------------------
            //LSL LSR
            code_type4("LS");
            //-----------------------
            //MOVE
            out_head("MOVE");
            for (byte wsize = 0; wsize <= 2; wsize++)
            {
                string wfuncname = string_add("analyse_MOVE_", SIZE_STR2[wsize]);
                if (wsize == 0)
                {
                    out_opcode(wfuncname,  1, -07, -87, -84, -07, 2, 0, true);
                }
                else
                if (wsize == 1)
                {
                    out_opcode(wfuncname,  3, -07, -87, -84, -07, 2, 1, true);
                }
                else
                {
                    out_opcode(wfuncname,  2, -07, -87, -84, -07, 2, 2, true);
                }
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            int w_size = 0;");
                outtext("            int w_src = (g_op3 < 7) ? g_op3 : 7 + g_op4;");
                outtext("            int w_dest= (g_op2 < 7) ? g_op2 : 7 + g_op1;");
                outtext("            int w_clock = 0;");
                outtext("            switch(g_op)");
                outtext("            {");
                outtext("                case 1:"); 
                outtext("                    w_size = 0;");
                outtext("                    w_clock = MOVE_CLOCK[w_src, w_dest];");
                outtext("                    break;");
                outtext("                case 3: ");
                outtext("                    w_size = 1; ");
                outtext("                    w_clock = MOVE_CLOCK[w_src, w_dest];");
                outtext("                    break; ");
                outtext("                default: ");
                outtext("                    w_size = 2; ");
                outtext("                    w_clock = MOVE_CLOCK_L[w_src, w_dest];");
                outtext("                    break; ");
                outtext("            } ");
                outtext("            g_reg_PC += 2; ");
                outtext("            adressing_func_address(g_op3, g_op4, w_size); ");
                outtext("            g_work_data.l = (uint)adressing_func_read(g_op3, g_op4, w_size); ");
                outtext("            adressing_func_address(g_op2, g_op1, w_size); ");
                outtext("            adressing_func_write(g_op2, g_op1, w_size, g_work_data.l); ");
                outtext("            g_clock = w_clock;");
                status_update(2, 2, 0, 0, 9, "g_work_data", "g_work_data", "g_work_data", "w_size");
                outtext("        }");
            }
            out_tail();

            //-----------------------
            //MOVEA
            out_head("MOVEA");
            for (byte wsize = 1; wsize <= 2; wsize++)
            {
                string wfuncname = string_add("analyse_MOVEA_", SIZE_STR2[wsize]);
                if (wsize == 1)
                {
                    out_opcode(wfuncname,  3, -07, 1, -84, -07, 2, wsize, true);
                }
                else
                {
                    out_opcode(wfuncname,  2, -07, 1, -84, -07, 2, wsize, true);
                }
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            if((g_op2 <= 1)&&(g_op3 <=1)) g_clock += 4; else g_clock += 5;");
                outtext("            g_reg_PC += 2;");
                outtext("            adressing_func_address(g_op3, g_op4, ", wsize, ");");
                outtext("            g_work_data.l = adressing_func_read(g_op3, g_op4, " + wsize + ");");
                if (wsize == 1)
                {
                    outtext("            g_reg_addr[g_op1].l = get_int_cast(g_work_data.w, 1);");
                }
                else
                {
                    outtext("            g_reg_addr[g_op1].l = g_work_data.l;");
                }
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //MOVETOCCR
            out_head("MOVETOCCR");
            {
                string wfuncname = string_add("analyse_MOVETOCCR");
                out_opcode(wfuncname,  4, 2, 3, -99, -07, 2, 1, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_clock += 12;");
                outtext("            g_reg_PC += 2;");
                outtext("            adressing_func_address(g_op3, g_op4, 1);");
                outtext("            g_work_data.w = (ushort)adressing_func_read(g_op3, g_op4, 1);");
                outtext("            g_status_CCR = g_work_data.b0;");
                outtext("        }");
                out_tail();
            }
            //-----------------------
            //MOVETOSR
            out_head("MOVETOSR");
            {
                string wfuncname = string_add("analyse_MOVETOSR");
                out_opcode(wfuncname,  4, 3, 3, -99, -07, 2, 1, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_clock += 12;");
                outtext("            g_reg_PC += 2;");
                outtext("            adressing_func_address(g_op3, g_op4, 1);");
                outtext("            g_work_data.w = (ushort)adressing_func_read(g_op3, g_op4, 1);");
                outtext("            g_reg_SR = g_work_data.w;");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //MOVEFROMSR
            out_head("MOVEFROMSR");
            {
                string wfuncname = string_add("analyse_MOVEFROMSR");
                out_opcode(wfuncname,  4, 0, 3, -99, -07, 2, 1, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            if(g_op3 <= 1) g_clock += 6; else g_clock += 9;");
                outtext("            g_reg_PC += 2;");
                outtext("            g_work_data.w = g_reg_SR;");
                outtext("            adressing_func_address(g_op3, g_op4, 1);");
                outtext("            adressing_func_write(g_op3, g_op4, 1, g_work_data.w);");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //MOVEUSP
            out_head("MOVEUSP");
            {
                //An→SP
                string wfuncname = string_add("analyse_MOVEUSP_1");
                out_opcode(wfuncname,  4, 7, 1, 4, -07, 2, 2, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_clock += 4;");
                outtext("            g_reg_PC += 2;");
                outtext("            g_reg_addr_usp.l = g_reg_addr[g_op4].l;");
                outtext("        }");
                //SP→An
                wfuncname = string_add("analyse_MOVEUSP_2");
                out_opcode(wfuncname,  4, 7, 1, 5, -07, 2, 2, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_clock += 4;");
                outtext("            g_reg_PC += 2;");
                outtext("            g_reg_addr[g_op4].l = g_reg_addr_usp.l;");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //MOVEM
            out_head("MOVEM");
            for (int wsize = 1; wsize <= 2; wsize++)
            {
                for (int w_mode = 0; w_mode <= 3; w_mode++)
                {
                    string w_mode_name = "";
                    string w_dr_name = "";
                    int w_op1 = 0;
                    int w_op3 = 0;
                    switch (w_mode)
                    {
                        case 0:                //reg ->mem (+) op3=2567 
                            w_mode_name = "";
                            w_dr_name = "r2m";
                            w_op1 = 4;
                            w_op3 = -85;
                            break;
                        case 1:                //reg ->mem (-) op3=4
                            w_mode_name = "_4";
                            w_dr_name = "r2m";
                            w_op1 = 4;
                            w_op3 = 4;
                            break;
                        case 2:                //mem ->reg (+) op3=2567 
                            w_mode_name = "";
                            w_dr_name = "m2r";
                            w_op1 = 6;
                            w_op3 = -85;
                            break;
                        case 3:                //mem ->reg (+) op3=3 
                            w_mode_name = "_3";
                            w_dr_name = "m2r";
                            w_op1 = 6;
                            w_op3 = 3;
                            break;
                    }
                    string wfuncname = string_add("analyse_MOVEM_", SIZE_STR2[wsize], "_", w_dr_name, w_mode_name);
                    out_opcode(wfuncname, 4, w_op1, wsize + 1, w_op3, -07, 4, wsize, true);
                    outtext("        private void ", wfuncname, "()");
                    outtext("        {");
                    outtext("            g_reg_PC += 2;");
                    outtext("            uint w_mask = md_main.g_md_bus.read16(g_reg_PC);");
                    outtext("            g_reg_PC += 2;");
                    switch (w_mode)
                    {
                        case 0:
                        case 1:
                            outtext("            g_clock += 8;");
                            break;
                        case 2:
                        case 3:
                            outtext("            g_clock += 12;");
                            break;
                    }
                    switch (w_mode)
                    {
                        case 0:
                        case 2:
                            outtext("            adressing_func_address(g_op3, g_op4, ", wsize, ");");
                            outtext("            uint wdata = g_analyze_address;");
                            break;
                        case 1:
                        case 3:
                            outtext("            uint wdata = g_reg_addr[g_op4].l;");
                            break;
                    }
                    ushort wmask_chk = 1;
                    switch (w_mode)
                    {
                        case 0:                //reg ->mem (+) op3=2567 
                            for (int j = 0; j < 8; j++)
                            {
                                outtext("            if ((w_mask & 0x" + wmask_chk.ToString("x4") + ") != 0) { md_main.g_md_bus.write" + DATA_BIT[wsize] + "(wdata, ", make_reg_data(j.ToString(), wsize), "); wdata += " + DATA_LENG[wsize] + "; ", ((wsize == 1)?"g_clock += 5;": "g_clock += 10;"), "};");
                                wmask_chk = (ushort)(wmask_chk << 1);
                            }
                            for (int j = 0; j < 8; j++)
                            {
                                outtext("            if ((w_mask & 0x" + wmask_chk.ToString("x4") + ") != 0) { md_main.g_md_bus.write" + DATA_BIT[wsize] + "(wdata, ", make_reg_addr(j.ToString(), wsize), "); wdata += " + DATA_LENG[wsize] + "; ", ((wsize == 1) ? "g_clock += 5;" : "g_clock += 10;"), "};");
                                wmask_chk = (ushort)(wmask_chk << 1);
                            }
                            break;
                        case 1:                //reg ->mem (-) op3=4
                            for (int j = 0; j < 8; j++)
                            {
                                outtext("            if ((w_mask & 0x" + wmask_chk.ToString("x4") + ") != 0) { wdata -= " + DATA_LENG[wsize] + "; md_main.g_md_bus.write" + DATA_BIT[wsize] + "(wdata, ", make_reg_addr((7 - j).ToString(), wsize), "); ", ((wsize == 1) ? "g_clock += 5;" : "g_clock += 10;"), "};");
                                wmask_chk = (ushort)(wmask_chk << 1);
                            }
                            for (int j = 0; j < 8; j++)
                            {
                                outtext("            if ((w_mask & 0x" + wmask_chk.ToString("x4") + ") != 0) { wdata -= " + DATA_LENG[wsize] + "; md_main.g_md_bus.write" + DATA_BIT[wsize] + "(wdata, ", make_reg_data((7 - j).ToString(), wsize), "); ", ((wsize == 1) ? "g_clock += 5;" : "g_clock += 10;"), "};");
                                wmask_chk = (ushort)(wmask_chk << 1);
                            }
                            outtext("            g_reg_addr[g_op4].l = wdata;");
                            break;
                        case 2:                //mem ->reg (+) op3=2567 
                        case 3:                //mem ->reg (+) op3=3
                            if(wsize == 1)
                            {
                                for (int j = 0; j < 8; j++)
                                {
                                    outtext("            if ((w_mask & 0x" + wmask_chk.ToString("x4") + ") != 0) { g_reg_data[", j, "].l = get_int_cast(md_main.g_md_bus.read" + DATA_BIT[wsize] + "(wdata), ", wsize, "); wdata += " + DATA_LENG[wsize] + "; ", ((wsize == 1) ? "g_clock += 4;" : "g_clock += 8;"), "};");
                                    wmask_chk = (ushort)(wmask_chk << 1);
                                }
                                for (int j = 0; j < 8; j++)
                                {
                                    outtext("            if ((w_mask & 0x" + wmask_chk.ToString("x4") + ") != 0) { g_reg_addr[", j, "].l = get_int_cast(md_main.g_md_bus.read" + DATA_BIT[wsize] + "(wdata), ", wsize, "); wdata += " + DATA_LENG[wsize] + "; ", ((wsize == 1) ? "g_clock += 4;" : "g_clock += 8;"), "};");
                                    wmask_chk = (ushort)(wmask_chk << 1);
                                }
                                if (w_mode == 3)
                                {
                                    outtext("            g_reg_addr[g_op4].l = wdata;");
                                }
                            }
                            else
                            {
                                for (int j = 0; j < 8; j++)
                                {
                                    outtext("            if ((w_mask & 0x" + wmask_chk.ToString("x4") + ") != 0) { g_reg_data[", j, "].l = md_main.g_md_bus.read" + DATA_BIT[wsize] + "(wdata); wdata += " + DATA_LENG[wsize] + "; ", ((wsize == 1) ? "g_clock += 4;" : "g_clock += 8;"), "};");
                                    wmask_chk = (ushort)(wmask_chk << 1);
                                }
                                for (int j = 0; j < 8; j++)
                                {
                                    outtext("            if ((w_mask & 0x" + wmask_chk.ToString("x4") + ") != 0) { g_reg_addr[", j, "].l = md_main.g_md_bus.read" + DATA_BIT[wsize] + "(wdata); wdata += " + DATA_LENG[wsize] + "; ", ((wsize == 1) ? "g_clock += 4;" : "g_clock += 8;"), "};");
                                    wmask_chk = (ushort)(wmask_chk << 1);
                                }
                                if (w_mode == 3)
                                {
                                    outtext("            g_reg_addr[g_op4].l = wdata;");
                                }
                            }
                            break;
                    }
                    outtext("        }");
                }
            }
            out_tail();
            //-----------------------
            //MOVEP
            out_head("MOVEP");
            for (int w_mode = 4; w_mode <= 7; w_mode++)
            {
                //基本情報
                //ヘッダー
                string wfuncname = string_add("analyse_MOVEP_", w_mode);
                if((w_mode == 4)|| (w_mode == 6))
                {
                    out_opcode(wfuncname, 0, -07, w_mode, 1, -07, 4, 1, true);
                }
                else
                {
                    out_opcode(wfuncname, 0, -07, w_mode, 1, -07, 4, 2, true);
                }
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                //明細
                outtext("            g_reg_PC += 2;");
                outtext("            ushort w_ext = md_main.g_md_bus.read16(g_reg_PC);");
                outtext("            g_reg_PC += 2;");
                switch (w_mode)
                {
                    case 4:
                        outtext("            g_reg_data[g_op1].b1 = md_main.g_md_bus.read8(g_reg_addr[g_op4].l + w_ext);");
                        outtext("            g_reg_data[g_op1].b0 = md_main.g_md_bus.read8(g_reg_addr[g_op4].l + w_ext + 2);");
                        outtext("            g_clock = 16;");
                        break;
                    case 5:
                        outtext("            g_reg_data[g_op1].b3 = md_main.g_md_bus.read8(g_reg_addr[g_op4].l + w_ext);");
                        outtext("            g_reg_data[g_op1].b2 = md_main.g_md_bus.read8(g_reg_addr[g_op4].l + w_ext + 2);");
                        outtext("            g_reg_data[g_op1].b1 = md_main.g_md_bus.read8(g_reg_addr[g_op4].l + w_ext + 4);");
                        outtext("            g_reg_data[g_op1].b0 = md_main.g_md_bus.read8(g_reg_addr[g_op4].l + w_ext + 6);");
                        outtext("            g_clock = 24;");
                        break;
                    case 6:
                        outtext("            md_main.g_md_bus.write8(g_reg_addr[g_op4].l + w_ext, g_reg_data[g_op1].b1);");
                        outtext("            md_main.g_md_bus.write8(g_reg_addr[g_op4].l + w_ext + 2, g_reg_data[g_op1].b0);");
                        outtext("            g_clock = 18;");
                        break;
                    case 7:
                        outtext("            md_main.g_md_bus.write8(g_reg_addr[g_op4].l + w_ext, g_reg_data[g_op1].b3);");
                        outtext("            md_main.g_md_bus.write8(g_reg_addr[g_op4].l + w_ext + 2, g_reg_data[g_op1].b2);");
                        outtext("            md_main.g_md_bus.write8(g_reg_addr[g_op4].l + w_ext + 4, g_reg_data[g_op1].b1);");
                        outtext("            md_main.g_md_bus.write8(g_reg_addr[g_op4].l + w_ext + 6, g_reg_data[g_op1].b0);");
                        outtext("            g_clock = 28;");
                        break;
                }
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //MOVEQ
            {
                out_head("MOVEQ");
                string wfuncname = string_add("analyse_MOVEQ");
                out_opcode(wfuncname,  7, -07, -03, -07, -07, 2, 2, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_reg_PC += 2;");
                outtext("            g_work_data.l = get_int_cast((byte)(g_opcode & 0x00ff), 0);");
                outtext("            g_reg_data[g_op1].l = g_work_data.l;");
                status_update(2, 2, 0, 0, 9, "g_work_val1", "g_work_val2", "g_work_data", "2");
                outtext("            g_clock += 4;");
                outtext("        }");
                out_tail();
            }
            //-----------------------
            //MULS
            out_head("MULS");
            {
                string wfuncname = string_add("analyse_MULS_addr");
                out_opcode(wfuncname,  12, -07, 7, -99, -07, 2, 1, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_reg_PC += 2;");
                outtext("            adressing_func_address(g_op3, g_op4, 1);");
                outtext("            g_clock = 70;");
                outtext("            g_work_data.w = (ushort)adressing_func_read(g_op3, g_op4, 1);");
                outtext("            g_work_data.l = (uint)((short)g_work_data.w * (short)g_reg_data[g_op1].w);");
                outtext("            g_reg_data[g_op1].l = g_work_data.l;");
                status_update(2, 2, 0, 0, 9, "g_work_data", "g_work_data", "g_work_data", "2");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //MULU
            out_head("MULU");
            {
                string wfuncname = string_add("analyse_MULU");
                out_opcode(wfuncname,  12, -07, 3, -99, -07, 2, 1, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_reg_PC += 2;");
                outtext("            adressing_func_address(g_op3, g_op4, 1);");
                outtext("            g_clock = 70;");
                outtext("            g_work_data.w = (ushort)adressing_func_read(g_op3, g_op4, 1);");
                outtext("            g_work_data.l = (uint)(g_work_data.w * g_reg_data[g_op1].w);");
                outtext("            g_reg_data[g_op1].l = g_work_data.l;");
                status_update(2, 2, 0, 0, 9, "g_work_val1", "g_work_val2", "g_work_data", "2");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //NBCD
            out_head("NBCD");
            {
                string wfuncname = string_add("analyse_NBCD");
                out_opcode(wfuncname,  4, 4, 0, -99, -07, 2, 0, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            if(g_op3 <= 1) g_clock += 6; else g_clock += 9;");
                outtext("            g_reg_PC += 2;");
                outtext("            adressing_func_address(g_op3, g_op4, 0);");
                outtext("            g_work_data.b0 = (byte)adressing_func_read(g_op3, g_op4, 0);");
                outtext("            int wkekka1 = 10 - (g_work_data.b0 & 0xf);");
                outtext("            if (g_status_X == true) wkekka1 -= 1;");
                outtext("            if(wkekka1 < 10) g_status_C = true;");
                outtext("            else { g_status_C = false; wkekka1 = 0; }");
                outtext("            int wkekka2 = 10 - ((g_work_data.b0 >> 4) & 0xf);");
                outtext("            if (g_status_C == true) wkekka2 -= 1;");
                outtext("            if(wkekka2 < 10) g_status_C = true;");
                outtext("            else { g_status_C = false; wkekka2 = 0; }");
                outtext("            g_work_data.b0 = (byte)((wkekka2 << 4) + wkekka1);");
                outtext("            adressing_func_write(g_op3, g_op4, 0, g_work_data.b0);");
                outtext("            if (g_work_data.b0 != 0) g_status_Z = false;");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //NEG
            out_head("NEG");
            for (byte wsize = 0; wsize <= 2; wsize++)
            {
                string wfuncname = string_add("analyse_NEG_", SIZE_STR2[wsize]);
                out_opcode(wfuncname,  4, 2, wsize, -84, -07, 2, wsize, true);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            if(g_op3 <= 1){");
                if (wsize == 2) outtext("                g_clock += 6;");
                           else outtext("                g_clock += 4;");
                outtext("            }else{");
                if (wsize == 2) outtext("                g_clock += 14;");
                           else outtext("                g_clock += 9;");
                outtext("            }");
                outtext("            g_reg_PC += 2;");
                outtext("            adressing_func_address(g_op3, g_op4, ", wsize, ");");
                outtext("            g_work_val2.l = adressing_func_read(g_op3, g_op4, "+wsize+");");
                outtext("            g_work_val1.l = 0;");
                outtext("            g_work_data.l = g_work_val1.l - g_work_val2.l;");
                outtext("            adressing_func_write(g_op3, g_op4, " + wsize + ", g_work_data.l);");
                status_update(2, 2, 3, 3, 2, "g_work_val1", "g_work_val2", "g_work_data", "g_op2");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //NEGX
            out_head("NEGX");
            for (byte wsize = 0; wsize <= 2; wsize++)
            {
                string wfuncname = string_add("analyse_NEGX_", SIZE_STR2[wsize]);
                out_opcode(wfuncname,  4, 0, wsize, -84, -07, 2, wsize, true);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            if(g_op3 <= 1){");
                if (wsize == 2) outtext("                g_clock += 6;");
                           else outtext("                g_clock += 4;");
                outtext("            }else{");
                if (wsize == 2) outtext("                g_clock += 14;");
                           else outtext("                g_clock += 9;");
                outtext("            }");
                outtext("            g_reg_PC += 2;");
                outtext("            adressing_func_address(g_op3, g_op4, ", wsize, ");");
                outtext("            g_work_val2.l = adressing_func_read(g_op3, g_op4, " + wsize + ");");
                outtext("            g_work_val1.l = 0;");
                outtext("            g_work_data.l = g_work_val1.l - g_work_val2.l;");
                outtext("            if (g_status_X == true) g_work_data.l -= 1;");
                outtext("            adressing_func_write(g_op3, g_op4, " + wsize + ", g_work_data.l);");
                status_update(2, 4, 3, 3, 2, "g_work_val1", "g_work_val2", "g_work_data", "g_op2");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //NOP
            {
                out_head("NOP");
                string wfuncname = string_add("analyse_NOP");
                out_opcode(wfuncname,  4, 7, 1, 6, 1, 2, 0, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_reg_PC += 2;");
                outtext("            g_clock += 4;");
                outtext("        }");
                out_tail();
            }
            //-----------------------
            //NOT
            out_head("NOT");
            for (byte wsize = 0; wsize <= 2; wsize++)
            {
                string wfuncname = string_add("analyse_NOT_", SIZE_STR2[wsize]);
                out_opcode(wfuncname,  4, 3, wsize, -99, -07, 2, wsize, true);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            if(g_op3 <= 1){");
                if (wsize == 2) outtext("                g_clock += 6;");
                           else outtext("                g_clock += 4;");
                outtext("            }else{");
                if (wsize == 2) outtext("                g_clock += 14;");
                           else outtext("                g_clock += 9;");
                outtext("            }");
                outtext("            g_reg_PC += 2;");
                outtext("            adressing_func_address(g_op3, g_op4, ", wsize, ");");
                outtext("            g_work_data.l = adressing_func_read(g_op3, g_op4, " + wsize + ");");
                outtext("            g_work_data.l = ~g_work_data.l;");
                outtext("            adressing_func_write(g_op3, g_op4, " + wsize + ", g_work_data.l);");
                status_update(2, 2, 0, 0, 9, "g_work_val1", "g_work_val2", "g_work_data", "g_op2");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //OR
            code_type1("OR");
            //-----------------------
            //ORI
            code_type2("ORI");
            //-----------------------
            //ORI TO CCR
            code_type7("ORITOCCR");
            //-----------------------
            //ORI TO SR
            code_type7("ORITOSR");
            //-----------------------
            //PEA
            out_head("PEA");
            {
                string wfuncname = string_add("analyse_PEA");
                out_opcode(wfuncname, 4, 4, 1, -85, -07, 2, 2, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_clock += 12;");
                outtext("            g_reg_PC += 2;");
                outtext("            adressing_func_address(g_op3, g_op4, 2);");
                outtext("            stack_push32(g_analyze_address);");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //ROL ROR
            code_type4("RO");
            //-----------------------
            //ROXL、ROXR
            code_type4("ROX");
            //-----------------------
            //RTE
            out_head("RTE");
            {
                string wfuncname = string_add("analyse_RTE");
                out_opcode(wfuncname, 4, 7, 1, 6, 3, 2, 0, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_clock += 20;");
                outtext("            uint w_pc = g_reg_PC;");
                outtext("            md_main.g_md_vdp.g_vdp_status_7_vinterrupt = 0;");
                outtext("            if (md_main.g_md_m68k.g_interrupt_H_act == true) md_main.g_md_m68k.g_interrupt_H_act = false;");
                outtext("            else if (md_main.g_md_m68k.g_interrupt_V_act == true) md_main.g_md_m68k.g_interrupt_V_act = false;");
                outtext("            else md_main.g_md_m68k.g_interrupt_EXT_act = false;");
                outtext("            g_reg_SR = stack_pop16();");
                outtext("            g_reg_PC = stack_pop32();");
                outtext("            md_main.g_form_code_trace.CPU_Trace_pop(g_reg_PC, w_pc, g_reg_addr[7].l);");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //RTR
            out_head("RTR");
            {
                string wfuncname = string_add("analyse_RTR");
                out_opcode(wfuncname,  4, 7, 1, 6, 7, 2, 0, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_clock += 20;");
                outtext("            uint w_pc = g_reg_PC;");
                outtext("            g_reg_SR = stack_pop16();");
                outtext("            g_reg_PC = stack_pop32();");
                outtext("            md_main.g_form_code_trace.CPU_Trace_pop(g_reg_PC, w_pc, g_reg_addr[7].l);");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //RTS
            out_head("RTS");
            {
                string wfuncname = string_add("analyse_RTS");
                out_opcode(wfuncname,  4, 7, 1, 6, 5, 2, 0, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_clock += 16;");
                outtext("            uint w_pc = g_reg_PC;");
                outtext("            g_reg_PC = stack_pop32();");
                outtext("            md_main.g_form_code_trace.CPU_Trace_pop(g_reg_PC, w_pc, g_reg_addr[7].l);");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //SBCD
            out_head("SBCD");
            for (int w_mode = 0; w_mode <= 1; w_mode++)
            {
                string wfuncname = string_add("analyse_SBCD_", w_mode);
                out_opcode(wfuncname,  8, -07, 4, w_mode, -07, 2, 0, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("           g_reg_PC += 2;");
                if (w_mode == 0)
                {
                    //Dy TO Dx
                    outtext("           g_work_val1.b0 = g_reg_data[g_op1].b0;");   //dest
                    outtext("           g_work_val2.b0 = g_reg_data[g_op4].b0;");   //source
                    outtext("           g_clock += 6;");
                }
                else
                {
                    //Ay@- TO Ax@-
                    outtext("           g_reg_addr[g_op1].l -= 1;");
                    outtext("           g_work_val1.b0 = md_main.g_md_bus.read8(g_reg_addr[g_op1].l);");
                    outtext("           g_reg_addr[g_op4].l -= 1;");
                    outtext("           g_work_val2.b0 = md_main.g_md_bus.read8(g_reg_addr[g_op4].l);");
                    outtext("           g_clock += 19;");
                }
                outtext("           int wkekka1 = (g_work_val1.b0 & 0xf) - (g_work_val2.b0 & 0xf);");
                outtext("           if (g_status_X == true) wkekka1 -= 1;");
                outtext("           if(wkekka1 < 0) { wkekka1 += 10; g_status_C = true; }");
                outtext("           else g_status_C = false;");
                outtext("           int wkekka2 = ((g_work_val1.b0 >> 4) & 0xf) - ((g_work_val2.b0 >> 4) & 0xf);");
                outtext("           if (g_status_C == true) wkekka2 -= 1;");
                outtext("           if(wkekka2 < 0) { wkekka2 += 10; g_status_C = true; }");
                outtext("           else g_status_C = false;");
                outtext("           g_work_data.b0 = (byte)((wkekka2 << 4) + wkekka1);");
                if (w_mode == 0)
                {
                    outtext("           g_reg_data[g_op1].b0 = g_work_data.b0;");
                }
                else
                {
                    outtext("           md_main.g_md_bus.write8(g_reg_addr[g_op1].l, g_work_data.b0);");
                }
                outtext("           if (g_work_data.b0 != 0) g_status_Z = false;");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //SCC
            out_head("SCC");
            {
                string wfuncname = string_add("analyse_Scc");
                out_opcode(wfuncname,  5, -07, 3, -99, -07, 2, 0, false);
                out_opcode(wfuncname,  5, -07, 7, -99, -07, 2, 0, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_reg_PC += 2;");
                outtext("            if(g_flag_chack[(g_opcode >> 8) & 0x0f]()){");
                outtext("                g_work_data.b0 = 0xff;");
                outtext("                if(g_op3 <= 1) g_clock += 6; else g_clock += 9;");
                outtext("            }else{");
                outtext("                g_work_data.b0 = 0;");
                outtext("                if(g_op3 <= 1) g_clock += 4; else g_clock += 9;");
                outtext("            };");
                outtext("            adressing_func_address(g_op3, g_op4, 0);");
                outtext("            adressing_func_write(g_op3, g_op4, 0, g_work_data.b0);");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //STOP
            out_head("STOP");
            {
                string wfuncname = string_add("analyse_STOP");
                out_opcode(wfuncname,  4, 7, 1, 6, 2, 4, 0, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_reg_PC += 2;");
                outtext("            g_work_data.w = md_main.g_md_bus.read16(g_reg_PC);");
                outtext("            g_reg_PC += 2;");
                outtext("            g_reg_SR = g_work_data.w;");
                outtext("            g_68k_stop = true;");
                outtext("           g_clock += 4;");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //SUB
            code_type1("SUB");
            //-----------------------
            //SUBA
            code_type1("SUBA");
            //-----------------------
            //SUBI
            code_type2("SUBI");
            //-----------------------
            //SUBQ
            code_type3("SUBQ");
            //-----------------------
            //SUBX
            out_head("SUBX");
            for (byte wsize = 0; wsize <= 2; wsize++)
            {
                for (int w_mode = 0; w_mode <= 1; w_mode++)
                {
                    string wfuncname = string_add("analyse_SUBX_", SIZE_STR2[wsize], "_mode_", w_mode);
                    out_opcode(wfuncname,  9, -07, (4 + wsize), w_mode, -07, 2, wsize, true);
                    outtext("        private void ", wfuncname, "()");
                    outtext("        {");
                    outtext("            g_reg_PC += 2;");
                    outtext("            int w_size = g_op2 & 0x03;");
                    if (w_mode == 0)
                    {
                        //Dy TO Dx
                        outtext("            g_work_val1.l = ", make_reg_data("g_op1", wsize), ";");
                        outtext("            g_work_val2.l = ", make_reg_data("g_op4", wsize), ";");
                        if (wsize == 2) outtext("                g_clock += 8;");
                        else outtext("                g_clock += 4;");
                    }
                    else
                    {
                        //Ay@- TO Ax@-
                        outtext("            g_reg_addr[g_op1].l -= ", DATA_LENG[wsize], ";");
                        outtext("            g_reg_addr[g_op4].l -= ", DATA_LENG[wsize], ";");
                        outtext("            g_work_val1.l = md_main.g_md_bus.read", DATA_BIT[wsize], "(g_reg_addr[g_op1].l);");
                        outtext("            g_work_val2.l = md_main.g_md_bus.read", DATA_BIT[wsize], "(g_reg_addr[g_op4].l);");
                        if (wsize == 2) outtext("                g_clock += 32;");
                        else outtext("                g_clock += 19;");
                    }
                    outtext("           g_work_data.l = g_work_val1.l - g_work_val2.l;");
                    outtext("           if (g_status_X == true) g_work_data.l -= 1;");
                    if (w_mode == 0)
                    {
                        outtext("           write_g_reg_data(g_op1, w_size, g_work_data.l);");
                    }
                    else
                    {
                        outtext("           md_main.g_md_bus.write", DATA_BIT[wsize], "(g_reg_addr[g_op1].l, g_work_data." + SIZE_STR[wsize] + ");");
                    }
                    status_update(2, 4, 3, 3, 2, "g_work_val1", "g_work_val2", "g_work_data", "g_op2 & 0x03");
                    outtext("        }");
                }
            }
            out_tail();
            //-----------------------
            //SWAP
            out_head("SWAP");
            {
                string wfuncname = string_add("analyse_SWAP");
                out_opcode(wfuncname,  4, 4, 1, 0, -07, 2, 1, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("           g_reg_PC += 2;");
                outtext("           g_work_data.l = g_reg_data[g_op4].l;");
                outtext("           g_reg_data[g_op4].w =  g_work_data.wup;");
                outtext("           g_reg_data[g_op4].wup =  g_work_data.w;");
                status_update(2, 2, 0, 0, 9, "g_work_data", "g_work_data", "g_work_data", "1");
                outtext("           g_clock += 4;");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //TAS
            out_head("TAS");
            {
                string wfuncname = string_add("analyse_TAS");
                out_opcode(wfuncname, 4, 5, 3, -84, -07, 2, 0, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_reg_PC += 2;");
                outtext("            adressing_func_address(g_op3, g_op4, 0);");
                outtext("            g_work_val1.b0 = (byte)adressing_func_read(g_op3, g_op4, 0);");
                outtext("            g_work_val2.b0 = (byte)(g_work_val1.b0 | 0x80);");
                outtext("            adressing_func_write(g_op3, g_op4, 0, g_work_val2.b0);");
                status_update(2, 2, 0, 0, 9, "g_work_val1", "g_work_val1", "g_work_val1", "0");
                outtext("           g_clock += 4;");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //TRAP
            out_head("TRAP");
            {
                string wfuncname = string_add("analyse_TRAP");
                out_opcode(wfuncname, 4, 7, 1, -01, -07, 2, 0, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_clock += 37;");
                outtext("            uint w_pc = g_reg_PC;");
                outtext("            g_reg_PC += 2;");
                outtext("            uint w_start_address = md_main.g_md_bus.read32((uint)(0x0080 + ((g_opcode & 0x0f) << 2)));");
                outtext("            stack_push32(g_reg_PC);");
                outtext("            md_main.g_form_code_trace.CPU_Trace_push(Form_Code_Trace.STACK_LIST_TYPE.TRAP, w_pc, w_start_address, g_reg_PC, g_reg_addr[7].l);");
                outtext("            stack_push16(g_reg_SR);");
                outtext("            g_reg_PC = w_start_address;");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //TRAPV
            out_head("TRAPV");
            {
                string wfuncname = string_add("analyse_TRAPV");
                out_opcode(wfuncname, 4, 7, 1, 6, 6, 2, 0, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_reg_PC += 2;");
                outtext("            if(g_status_V == true) g_reg_PC = md_main.g_md_bus.read32(28);");
                outtext("            g_clock += 37;");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //TST
            out_head("TST");
            for (byte wsize = 0; wsize <= 2; wsize++)
            {
                string wfuncname = string_add("analyse_TST_", SIZE_STR2[wsize]);
                out_opcode(wfuncname,  4, 5, wsize, -84, -07, 2, wsize, true);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_clock += 4;");
                outtext("            g_reg_PC += 2;");
                outtext("            adressing_func_address(g_op3, g_op4, ", wsize, ");");
                outtext("            g_work_data.l = adressing_func_read(g_op3, g_op4, "+wsize+");");
                status_update(2, 2, 0, 0, 9, "g_work_val1", "g_work_val2", "g_work_data", "g_op2");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //UNLK
            out_head("UNLK");
            {
                string wfuncname = string_add("analyse_UNLK");
                out_opcode(wfuncname,  4, 7, 1, 3, -07, 2, 0, false);
                outtext("        private void ", wfuncname, "()");
                outtext("        {");
                outtext("            g_clock += 12;");
                outtext("            g_reg_PC += 2;");
                outtext("            g_reg_addr[7].l = g_reg_addr[g_op4].l;");
                outtext("            g_reg_addr[g_op4].l = stack_pop32();");
                outtext("        }");
            }
            out_tail();
            //-----------------------
            //RESET
        }
    }
}
