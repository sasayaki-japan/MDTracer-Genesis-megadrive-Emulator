namespace opcode_make
{
    internal partial class Program
    {
        private static void code_type1(string in_opcode)
        {
            out_head(in_opcode);
            string wenzan = "";
            string wfuncname = string_add("analyse_", in_opcode);
            for (int wop2 = 0; wop2 <= 7; wop2++)
            {
                byte wsize = (byte)(wop2 & 0x03);
                int w_op = 0;
                int w_op3 = 0;
                switch (in_opcode)
                {
                    case "ADD":
                        if ((wop2 == 3) || (wop2 == 7)) continue;
                        wenzan = " + ";
                        w_op = 13;
                        if (wop2 == 0) w_op3 = -99;
                        else
                        if (4 <= wop2) w_op3 = -27;
                        else w_op3 = -84;
                        break;
                    case "SUB":
                        if ((wop2 == 3) || (wop2 == 7)) continue;
                        wenzan = " - ";
                        w_op = 9;
                        if (wop2 == 0) w_op3 = -99;
                        else
                        if (4 <= wop2) w_op3 = -27;
                        else w_op3 = -84;
                        break;
                    case "CMP":
                        if (wop2 >= 3) continue;
                        wenzan = " - ";
                        w_op = 11;
                        if (wop2 == 0) w_op3 = -99;
                        else w_op3 = -84;
                        break;
                    case "AND":
                        if ((wop2 == 3) || (wop2 == 7)) continue;
                        wenzan = " & ";
                        w_op = 12;
                        if (wop2 == 0) w_op3 = -99;
                        else
                        if (4 <= wop2) w_op3 = -27;
                        else w_op3 = -84;
                        break;
                    case "OR":
                        if ((wop2 == 3) || (wop2 == 7)) continue;
                        wenzan = " | ";
                        w_op = 8;
                        if (wop2 == 0) w_op3 = -99;
                        else
                        if (4 <= wop2) w_op3 = -27;
                        else w_op3 = -84;
                        break;
                    case "EOR":
                        if ((wop2 < 4) || (6 < wop2)) continue;
                        wenzan = " ^ ";
                        w_op = 11;
                        w_op3 = -99;
                        break;
                    case "ADDA":
                        if (wop2 == 3) wsize = 1;
                        else if (wop2 == 7) wsize = 2;
                        else continue;
                        wenzan = " + ";
                        w_op = 13;
                        w_op3 = -84;
                        break;
                    case "SUBA":
                        if (wop2 == 3) wsize = 1;
                        else if (wop2 == 7) wsize = 2;
                        else continue;
                        wenzan = " - ";
                        w_op = 9;
                        w_op3 = -84;
                        break;
                    case "CMPA":
                        if (wop2 == 3) wsize = 1;
                        else if (wop2 == 7) wsize = 2;
                        else continue;
                        wenzan = " - ";
                        w_op = 11;
                        w_op3 = -84;
                        break;
                }
                out_opcode(wfuncname, w_op, -07, wop2, w_op3, -07, 2, wsize, true);
            }
            outtext("        private void ", wfuncname, "()");
            outtext("        {");
            outtext("            g_reg_PC += 2;");

            if ((in_opcode == "ADDA") || (in_opcode == "SUBA") || (in_opcode == "CMPA"))
            {
                outtext("            int w_size = (g_op2 >> 2) + 1;");
                outtext("            g_clock = (w_size == 1) ? 8 : 6;");
                outtext("            g_work_val1.l = g_reg_addr[g_op1].l;");
                outtext("            adressing_func_address(g_op3, g_op4, w_size);");
                outtext("            g_work_val2.l = adressing_func_read(g_op3, g_op4, w_size);");
                outtext("            g_work_val2.l = get_int_cast(g_work_val2.l, w_size);");
                outtext("            g_work_data.l = g_work_val1.l ", wenzan, " g_work_val2.l;");
                if (in_opcode != "CMPA")
                {
                    outtext("            g_reg_addr[g_op1].l = g_work_data.l;");
                }
            }
            else
            if (in_opcode == "CMP")
            {
                outtext("            int w_size = g_op2 & 0x03;");
                outtext("                g_clock = (w_size == 2) ? 6 : 4;");
                outtext("                g_work_val1.l = read_g_reg_data(g_op1, w_size);");
                outtext("                adressing_func_address(g_op3, g_op4, w_size);");
                outtext("                g_work_val2.l = adressing_func_read(g_op3, g_op4, w_size);");
                outtext("                g_work_data.l = g_work_val1.l ", wenzan, " g_work_val2.l;");
            }
            else
            {
                outtext("            int w_size = g_op2 & 0x03;");
                outtext("            if ((g_op2 & 0x04) == 0)");
                outtext("            {");
                outtext("                g_clock = (w_size == 2) ? 6 : 4;");
                outtext("                g_work_val1.l = read_g_reg_data(g_op1, w_size);");
                outtext("                adressing_func_address(g_op3, g_op4, w_size);");
                outtext("                g_work_val2.l = adressing_func_read(g_op3, g_op4, w_size);");
                outtext("                g_work_data.l = g_work_val1.l ", wenzan, " g_work_val2.l;");
                outtext("                write_g_reg_data(g_op1, w_size, g_work_data.l);");
                outtext("            }else{");
                outtext("                g_clock = (w_size == 2) ? 14 : 9;");
                outtext("                adressing_func_address(g_op3, g_op4, w_size);");
                outtext("                g_work_val1.l = adressing_func_read(g_op3, g_op4, w_size);");
                outtext("                g_work_val2.l = read_g_reg_data(g_op1, w_size);");
                outtext("                g_work_data.l = g_work_val1.l ", wenzan, " g_work_val2.l;");
                outtext("                adressing_func_write(g_op3, g_op4, w_size, g_work_data.l);");
                outtext("            }");
            }
            switch (in_opcode)
            {
                case "ADD":
                    status_update(2, 2, 2, 2, 2, "g_work_val1", "g_work_val2", "g_work_data", "w_size");
                    break;
                case "SUB":
                    status_update(2, 2, 3, 3, 2, "g_work_val1", "g_work_val2", "g_work_data", "w_size");
                    break;
                case "AND":
                case "OR":
                case "EOR":
                    status_update(2, 2, 0, 0, 9, "g_work_val1", "g_work_val2", "g_work_data", "w_size");
                    break;
                case "CMP":
                    status_update(2, 2, 3, 3, 9, "g_work_val1", "g_work_val2", "g_work_data", "w_size");
                    break;
                case "CMPA":
                    status_update(2, 2, 3, 3, 9, "g_work_val1", "g_work_val2", "g_work_data", "2");
                    break;
            }
            outtext("        }");
            out_tail();
        }
        private static void code_type2(string in_opcode)
        {
            out_head(in_opcode);
            string wfuncname = "";
            string wenzan = "";
            for (byte wsize = 0; wsize <= 2; wsize++)
            {
                int w_op = 0;
                int w_op2 = 0;
                switch (in_opcode)
                {
                    case "ADDI":
                        w_op = 3;
                        w_op2 = -99;
                        wenzan = " + ";
                        break;
                    case "SUBI":
                        w_op = 2;
                        w_op2 = -99;
                        wenzan = " - ";
                        break;
                    case "CMPI":
                        w_op = 6;
                        w_op2 = -99;
                        wenzan = " - ";
                        break;
                    case "ANDI":
                        w_op = 1;
                        w_op2 = -97;
                        wenzan = " & ";
                        break;
                    case "EORI":
                        w_op = 5;
                        w_op2 = -97;
                        wenzan = " ^ ";
                        break;
                    case "ORI":
                        w_op = 0;
                        w_op2 = -97;
                        wenzan = " | ";
                        break;
                }
                wfuncname = string_add("analyse_", in_opcode);
                if(wsize == 2)
                {
                    out_opcode(wfuncname, 0, w_op, wsize, w_op2, -07, 6, wsize, true);
                }
                else
                {
                    out_opcode(wfuncname, 0, w_op, wsize, w_op2, -07, 4, wsize, true);
                }
            }

            outtext("        private void " + wfuncname + "()");
            outtext("        {");
            switch (in_opcode)
            {
                case "CMPI":
                    outtext("           if(g_op3 == 0) if(g_op2 == 2)g_clock = 14; else g_clock = 8;");
                    outtext("                     else if(g_op2 == 2)g_clock = 12; else g_clock = 8;");
                    break;
                default:
                    outtext("           if(g_op3 == 0) if(g_op2 == 2)g_clock = 16; else g_clock = 8;");
                    outtext("                     else if(g_op2 == 2)g_clock = 22; else g_clock = 13;");
                    break;
            }
            outtext("            g_reg_PC += 2;");
            outtext("            switch (g_op2)");
            outtext("            {");
            outtext("                case 0: g_work_val2.l = (uint)(md_main.g_md_bus.read16(g_reg_PC) & 0x00ff); g_reg_PC += 2; break;");
            outtext("                case 1: g_work_val2.l = md_main.g_md_bus.read16(g_reg_PC); g_reg_PC += 2; break;");
            outtext("                default: g_work_val2.l = md_main.g_md_bus.read32(g_reg_PC); g_reg_PC += 4; break;");
            outtext("            }");
            outtext("            adressing_func_address(g_op3, g_op4, g_op2);");
            outtext("            g_work_val1.l = adressing_func_read(g_op3, g_op4, g_op2);");
            outtext("            g_work_data.l = g_work_val1.l " + wenzan + " g_work_val2.l;");
            if (in_opcode != "CMPI")
            {
                outtext("            adressing_func_write(g_op3, g_op4, g_op2, g_work_data.l);");
            }
            switch (in_opcode)
            {
                case "ADDI":
                    status_update(2, 2, 2, 2, 2, "g_work_val1", "g_work_val2", "g_work_data", "g_op2");
                    break;
                case "SUBI":
                    status_update(2, 2, 3, 3, 2, "g_work_val1", "g_work_val2", "g_work_data", "g_op2");
                    break;
                case "CMPI":
                    status_update(2, 2, 3, 3, 9, "g_work_val1", "g_work_val2", "g_work_data", "g_op2");
                    break;
                default:
                    status_update(2, 2, 0, 0, 9, "g_work_val1", "g_work_val2", "g_work_data", "g_op2");
                    break;
            }
            outtext("        }");
            out_tail();
        }
        private static void code_type3(string in_opcode)
        {
            out_head(in_opcode);
            string wfuncname = "";
            string wenzan = "";
            for (byte wsize = 0; wsize <= 2; wsize++)
            {
                int w_op2 = 0;
                if (in_opcode == "ADDQ")
                {
                    w_op2 = wsize;
                    wenzan = " + ";
                }
                else if (in_opcode == "SUBQ")
                {
                    w_op2 = 4 + wsize;
                    wenzan = " - ";
                }
                wfuncname = string_add("analyse_", in_opcode);
                out_opcode(wfuncname, 5, -07, w_op2, -88, -07, 2, wsize, true);
            }
            outtext("        private void ", wfuncname + "()");
            outtext("        {");
            outtext("            int w_size = g_op2 & 0x03;");
            outtext("            if(w_size == 2)");
            outtext("            {");
            outtext("                if (g_op3 == 0) g_clock = 8; else g_clock = 14;");
            outtext("            }else{");
            outtext("                if (g_op3 == 0) g_clock = 4; else g_clock = 9;");
            outtext("            }");
            outtext("            g_reg_PC += 2;");
            outtext("            g_work_val2.l = (byte)((g_opcode >> 9) & 0x07);");
            outtext("            if(g_work_val2.l == 0) g_work_val2.l = 8;");
            outtext("            adressing_func_address(g_op3, g_op4, w_size);");
            outtext("            g_work_val1.l = adressing_func_read(g_op3, g_op4, w_size);");
            outtext("            g_work_data.l = g_work_val1.l", wenzan, "g_work_val2.l;");
            outtext("            adressing_func_write(g_op3, g_op4, w_size, g_work_data.l);");
            outtext("            if(g_op3 != 1){");
            if (in_opcode == "ADDQ")
            {
                status_update(2, 2, 2, 2, 2, "g_work_val1", "g_work_val2", "g_work_data", "g_op2");
            }
            else
            if (in_opcode == "SUBQ")
            {
                status_update(2, 2, 3, 3, 2, "g_work_val1", "g_work_val2", "g_work_data", "g_op2 & 0x03");
            }
            outtext("            }");
            outtext("        }");
            out_tail();
        }
        private static void code_type4(string in_opcode)
        {
            out_head(in_opcode);
            //レジスタ・ローテイト
            string wfuncname = "";
            for (byte wir = 0; wir <= 1; wir++)
            {
                for (byte wsize = 0; wsize <= 2; wsize++)
                {
                    int w_op3 = 0;
                    switch (in_opcode)
                    {
                        case "AS": w_op3 = (wir * 4); break;
                        case "LS": w_op3 = (wir * 4 + 1); break;
                        case "ROX": w_op3 = (wir * 4 + 2); break;
                        case "RO": w_op3 = (wir * 4 + 3); break;
                    }
                    wfuncname = string_add("analyse_", in_opcode, "_reg");
                    out_opcode(wfuncname, 14, -07, (wsize), w_op3, -07, 2, wsize, true);
                    out_opcode(wfuncname, 14, -07, (4 + wsize), w_op3, -07, 2, wsize, true);
                }
            }
            outtext("        private void ", wfuncname, "()");
            outtext("        {");
            outtext("            int w_size = g_op2 & 0x03;");
            outtext("            int w_ir = g_op3 & 0x04;");
            outtext("            int w_dr = g_op2 & 0x04;");
            outtext("            if (w_size == 2) g_clock = 8; else g_clock = 6;");
            outtext("            g_reg_PC += 2;");
            outtext("            uint wcnt = 0;");
            outtext("            if(w_ir == 0)");
            outtext("            {");
            outtext("                wcnt = g_op1;");
            outtext("                if(wcnt == 0) wcnt = 8;");
            outtext("            }else{");
            outtext("                wcnt = g_reg_data[g_op1].l & 0x3f;");
            outtext("            }");
            outtext("            g_work_data.l = read_g_reg_data(g_op4, w_size);");
            code_type4_2(in_opcode);
            outtext("            if(w_ir != 0) g_clock += 2;"); 
            outtext("            write_g_reg_data(g_op4, w_size, g_work_data.l);");
            status_update(2, 2, 9, 9, 9, "g_work_val1", "g_work_val2", "g_work_data", "g_op2 & 0x03");
            outtext("        }");
            //メモリ・ローテイト
            int w_op = 0;
            wfuncname = string_add("analyse_", in_opcode, "_mem");
            switch (in_opcode)
            {
                case "AS": w_op = 0; break;
                case "LS": w_op = 1; break;
                case "ROX": w_op = 2; break;
                case "RO": w_op = 3; break;
            }
            out_opcode(wfuncname, 14, w_op, 3, -27, -07, 2, 0, false);
            out_opcode(wfuncname, 14, w_op, 7, -27, -07, 2, 0, false);
            outtext("        private void ", wfuncname, "()");
            outtext("        {");
            outtext("            int w_size = 1;");
            outtext("            int w_dr = g_op2 & 0x04;");
            outtext("            g_clock = 9;");
            outtext("            g_reg_PC += 2;");
            outtext("            uint wcnt = 1;");
            outtext("            adressing_func_address(g_op3, g_op4, 1);");
            outtext("            g_work_data.w = (ushort)adressing_func_read(g_op3, g_op4, 1);");
            code_type4_2(in_opcode);
            outtext("            adressing_func_write(g_op3, g_op4, 1, g_work_data.w);");
            status_update(2, 2, 9, 9, 9, "g_work_val1", "g_work_val2", "g_work_data", "1");
            outtext("        }");
            out_tail();
        }
        private static void code_type4_2(string in_opcode)
        {
            outtext("            g_status_V = false;");
            outtext("            g_status_C = false;");
            outtext("            if(w_dr == 0)");
            outtext("            {");
            if (in_opcode == "AS")
            {
                outtext("                g_work_val1.l = g_work_data.l & MOSTBIT[w_size];");
            }
            outtext("                for (int i = 0; i < wcnt; i++)");
            outtext("                {");
            outtext("                    g_clock += 2;");
            switch (in_opcode)
            {
                case "AS":
                    outtext("                    g_status_C = ((g_work_data.l & 0x01) == 0x01);");
                    outtext("                    g_work_data.l = (g_work_data.l >> 1);");
                    outtext("                    g_work_data.l = g_work_data.l & ~g_work_val1.l;");
                    outtext("                    g_work_data.l = g_work_data.l | g_work_val1.l;");
                    outtext("                    g_status_X = g_status_C;");
                    break;
                case "LS":
                    outtext("                    g_status_C = ((g_work_data.l & 0x01) == 0x01);");
                    outtext("                    g_work_data.l = (g_work_data.l >> 1);");
                    outtext("                    g_status_X = g_status_C;");
                    break;
                case "ROX":
                    outtext("                    g_work_val1.l = (uint)((g_status_X == true) ?MOSTBIT[w_size]: 0);");
                    outtext("                    g_status_C = ((g_work_data.l & 0x01) == 0x01);");
                    outtext("                    g_status_X = g_status_C;");
                    outtext("                    g_work_data.l = (g_work_data.l >> 1);");
                    outtext("                    g_work_data.l = (g_work_data.l | g_work_val1.l);");
                    break;
                case "RO":
                    outtext("                    g_status_C = ((g_work_data.l & 0x01) == 0x01);");
                    outtext("                    g_work_data.l = (g_work_data.l >> 1);");
                    outtext("                    if (g_status_C == true) {");
                    outtext("                        g_work_data.l = (g_work_data.l | MOSTBIT[w_size]);");
                    outtext("                    }");
                    break;
            }
            outtext("                }");
            outtext("            }else{");
            if (in_opcode == "AS")
            {
                outtext("                if ((g_work_data.l & MOSTBIT[w_size]) != 0) g_status_N = true; else g_status_N = false;");
            }
            outtext("                for (int i = 0; i < wcnt; i++)");
            outtext("                {");
            outtext("                    g_clock += 2;");
            switch (in_opcode)
            {
                case "AS":
                    outtext("                    g_status_C = ((g_work_data.l & MOSTBIT[w_size]) != 0);");
                    outtext("                    g_work_data.l = (uint)(g_work_data.l << 1);");
                    outtext("                    if (((g_work_data.l & MOSTBIT[w_size]) != 0) != g_status_N) g_status_V = true;");
                    outtext("                    g_status_X = g_status_C;");
                    break;
                case "LS":
                    outtext("                    g_status_C = ((g_work_data.l & MOSTBIT[w_size]) != 0);");
                    outtext("                    g_work_data.l = (uint)(g_work_data.l << 1);");
                    outtext("                    g_status_X = g_status_C;");
                    break;
                case "ROX":
                    outtext("                    g_work_val1.l = (uint)((g_status_X == true) ? 1: 0);");
                    outtext("                    g_status_C = ((g_work_data.l & MOSTBIT[w_size]) != 0);");
                    outtext("                    g_status_X = g_status_C;");
                    outtext("                    g_work_data.l = (uint)(g_work_data.l << 1);");
                    outtext("                    g_work_data.l = (uint)(g_work_data.l | g_work_val1.l);");
                    break;
                case "RO":
                    outtext("                    g_status_C = ((g_work_data.l & MOSTBIT[w_size]) != 0);");
                    outtext("                    g_work_data.l = (uint)(g_work_data.l << 1);");
                    outtext("                    if (g_status_C == true) {");
                    outtext("                        g_work_data.l = (uint)(g_work_data.l | 0x01);");
                    outtext("                    }");
                    break;
            }
            outtext("                }");
            outtext("            }");
        }
        //----------------------
        //ORI TO SR , ORI TO CCR
        //EORI TO SR , EORI TO CCR
        //ANDI TO SR , ANDI TO CCR
        private static void code_type7(string in_opcode)
        {
            out_head(in_opcode);
            byte wsize = 0;
            int w_op = 0;
            string wenzan = "";
            if (in_opcode == "ANDITOCCR")
            {
                w_op = 1;
                wenzan = " & ";
            }
            else if (in_opcode == "EORITOCCR")
            {
                w_op = 5;
                wenzan = " ^ ";
            }
            else if (in_opcode == "ORITOCCR")
            {
                w_op = 0;
                wenzan = " | ";
            }
            if (in_opcode == "ANDITOSR")
            {
                w_op = 1;
                wenzan = " & ";
                wsize = 1;
            }
            else if (in_opcode == "EORITOSR")
            {
                w_op = 5;
                wenzan = " ^ ";
                wsize = 1;
            }
            else if (in_opcode == "ORITOSR")
            {
                w_op = 0;
                wenzan = " | ";
                wsize = 1;
            }
            string wfuncname = string_add("analyse_", in_opcode, "_", SIZE_STR2[wsize]);
            out_opcode(wfuncname , 0, w_op, wsize, 7, 4, 2, wsize, true);
            outtext("        private void " + wfuncname + "()");
            outtext("        {");
            outtext("            g_clock += 20;");
            outtext("            g_reg_PC += 2;");
            if (wsize == 0)
            {
                outtext("            g_work_val2.b0 = (byte)(md_main.g_md_bus.read16(g_reg_PC) & 0x00ff);");
                outtext("            g_reg_PC += 2;");
                outtext("            g_work_val1.b0 = g_status_CCR;");
                outtext("            g_work_data.b0 = (byte)(g_work_val1.b0 " + wenzan + " g_work_val2.b0);");
                outtext("            g_status_CCR = g_work_data.b0;");
            }
            else
            {
                outtext("            g_work_val2.w = md_main.g_md_bus.read16(g_reg_PC);");
                outtext("            g_reg_PC += 2;");
                outtext("            g_work_val1.w = g_reg_SR;");
                outtext("            g_work_data.w = (ushort)(g_work_val1.w " + wenzan + " g_work_val2.w);");
                outtext("            g_reg_SR = g_work_data.w;");
            }
            outtext("        }");
            out_tail();
        }
        //----------------------
        //BTST,BCHG,BCLR,BSET
        private static void code_type8(string in_opcode)
        {
            out_head(in_opcode);
            for (int w_mode = 0; w_mode <= 1; w_mode++)
            {
                for (int wtype = 0; wtype <= 1; wtype++)
                {
                    int wop2_dynamic = 0;
                    int wop2_static = 0;
                    int g_clock = 0;
                    switch (in_opcode)
                    {
                        case "BTST":
                            wop2_dynamic = 4;
                            wop2_static = 0;
                            if (w_mode == 0) { if(wtype == 0) g_clock = 6; else g_clock = 4;}
                                       else { if (wtype == 0) g_clock = 10; else g_clock = 8; }
                            break;
                        case "BCHG":
                            wop2_dynamic = 5;
                            wop2_static = 1;
                            if (w_mode == 0) { if (wtype == 0) g_clock = 8; else g_clock = 9; }
                            else { if (wtype == 0) g_clock = 12; else g_clock = 13; }
                            break;
                        case "BCLR":
                            wop2_dynamic = 6;
                            wop2_static = 2;
                            if (w_mode == 0) { if (wtype == 0) g_clock = 10; else g_clock = 9; }
                            else { if (wtype == 0) g_clock = 14; else g_clock = 13; }
                            break;
                        case "BSET":
                            wop2_dynamic = 7;
                            wop2_static = 3;
                            if (w_mode == 0) { if (wtype == 0) g_clock = 8; else g_clock = 9; }
                            else { if (wtype == 0) g_clock = 12; else g_clock = 13; }
                            break;
                    }
                    string wfuncname = string_add("analyse_" + in_opcode + "_" + ((w_mode == 0) ? "dynamic" : "static") + "_" + ((wtype == 0) ? "long" : "byte"));
                    if (w_mode == 0)
                    {
                        if (wtype == 0)
                        {
                            out_opcode(wfuncname, 0, -07, wop2_dynamic, 0, -07, 2, 2, true);
                        }
                        else
                        {
                            out_opcode(wfuncname, 0, -07, wop2_dynamic, -27, -07, 2, 0, true);
                        }
                    }
                    else
                    {
                        if (wtype == 0)
                        {
                            out_opcode(wfuncname, 0, 4, wop2_static, 0, -07, 2, 2, true);
                        }
                        else
                        {
                            out_opcode(wfuncname, 0, 4, wop2_static, -27, -07, 2, 0, true);
                        }
                    }
                    outtext("        private void ", wfuncname, "()");
                    outtext("        {");
                    outtext("            g_clock += " + g_clock.ToString() + ";");
                    outtext("            g_reg_PC += 2;");
                    if (w_mode == 0)
                    {
                        outtext("            int w_bit = g_reg_data[g_op1].b0;");
                    }
                    else
                    {
                        outtext("            int w_bit = md_main.g_md_bus.read16(g_reg_PC);");
                        outtext("            g_reg_PC += 2;");
                    }
                    if(wtype == 0)
                    {
                        outtext("            w_bit = w_bit & 0x1f;");
                        outtext("            g_work_data.l = adressing_func_read(0, g_op4, 2);");
                        outtext("            g_status_Z = ((g_work_data.l & BITHIT[w_bit]) == 0);");
                    }
                    else
                    {
                        outtext("            w_bit = w_bit & 0x07;");
                        outtext("            adressing_func_address(g_op3, g_op4, 0);");
                        outtext("            g_work_data.b0 = (byte)adressing_func_read(g_op3, g_op4, 0);");
                        outtext("            g_status_Z = ((g_work_data.b0 & BITHIT[w_bit]) == 0);");
                    }
                    if (in_opcode == "BCHG")
                    {
                        if (wtype == 0)
                        {
                            outtext("            if (g_status_Z == true) {g_work_data.l = (uint)(g_work_data.l | BITHIT[w_bit]);}");
                            outtext("                             else{g_work_data.l = (uint)(g_work_data.l & ~BITHIT[w_bit]);}");
                            outtext("            adressing_func_write(0, g_op4, 2, g_work_data.l);");
                        }
                        else
                        {
                            outtext("            if (g_status_Z == true) {g_work_data.b0 = (byte)(g_work_data.b0 | BITHIT[w_bit]);}");
                            outtext("                             else{g_work_data.b0 = (byte)(g_work_data.b0 & ~BITHIT[w_bit]);}");
                            outtext("            adressing_func_write(g_op3, g_op4, 0, g_work_data.b0);");
                        }
                    }
                    else
                    if (in_opcode == "BCLR")
                    {
                        if (wtype == 0)
                        {
                            outtext("            g_work_data.l = (uint)(g_work_data.l & ~BITHIT[w_bit]);");
                            outtext("            adressing_func_write(0, g_op4, 2, g_work_data.l);");
                        }
                        else
                        {
                            outtext("            g_work_data.b0 = (byte)(g_work_data.b0 & ~BITHIT[w_bit]);");
                            outtext("            adressing_func_write(g_op3, g_op4, 0, g_work_data.b0);");
                        }
                    }
                    else
                    if (in_opcode == "BSET")
                    {
                        if (wtype == 0)
                        {
                            outtext("            g_work_data.l = (uint)(g_work_data.l | BITHIT[w_bit]);");
                            outtext("            adressing_func_write(0, g_op4, 2, g_work_data.l);");
                        }
                        else
                        {
                            outtext("            g_work_data.b0 = (byte)(g_work_data.b0 | BITHIT[w_bit]);");
                            outtext("            adressing_func_write(g_op3, g_op4, 0, g_work_data.b0);");
                        }
                    }
                    outtext("        }");
                }
            }
            out_tail();
        }
    }
}