using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
namespace opcode_make
{
    internal partial class Program
    {
        private static string[] CC_STR = { "T", "F", "HI", "LS", "CC", "CS", "NE", "EQ"
                                , "VC", "VS", "PL", "MI", "GE", "LT", "GT", "LE"};
        private static string[] g_opcheck;
        private static void phase2()
        {
            g_opcheck = new string[65536];

            File.Delete("md_m68k_initialize2.cs");
            outsub2("using System; ");
            outsub2("namespace MDTracer");
            outsub2("{");
            outsub2("    internal partial class md_m68k");
            outsub2("    {");
            outsub2("         private void initialize2()");
            outsub2("         { ");

            foreach (Opinfo w_opinfo in g_opinfo)
            {
                opcode_add(w_opinfo.funcname
                    , w_opinfo.opname
                    , w_opinfo.type
                    , w_opinfo.op1
                    , w_opinfo.op2
                    , w_opinfo.op3
                    , w_opinfo.op4
                    , w_opinfo.opleng
                    , w_opinfo.datasize
                    , w_opinfo.op_size_mark);
            }
            outsub2("         } ");
            outsub2("    }");
            outsub2("}");
            //-----------------------
            File.Delete("..\\..\\..\\MDTracer\\md_m68k_initialize2.cs");
            File.Move("md_m68k_initialize2.cs"
                , "..\\..\\..\\MDTracer\\md_m68k_initialize2.cs");
        }
        private static void opcode_add(string in_funcname
                                , string in_opname
                                , int in_type, int in_op1, int in_op2, int in_op3, int in_op4
                                , int in_opleng, int in_datasize, bool in_op_size_mark)
        {
            //-27:アドレッシングモードのカテゴリ：メモリ可変のみ
            //-99:アドレッシングモードのカテゴリ：データ可変のみ
            //-97:アドレッシングモードのカテゴリ：データ可変からop3=7でop4=4を除く
            //-88:アドレッシングモードのカテゴリ：可変のみ
            //-87:アドレッシングモードのカテゴリ：可変からop3=1を除く
            //-85:アドレッシングモードのカテゴリ：コントロールのみ
            //-84:アドレッシングモードのカテゴリ：ALL
            //-100: bit7～0(1byte)が0ではない。
            int wop1_s = in_op1;
            int wop1_e = in_op1;
            int wop2_s = in_op2;
            int wop2_e = in_op2;
            int wop3_s = in_op3;
            int wop3_e = in_op3;
            int wop4_s = in_op4;
            int wop4_e = in_op4;
            if ((-77 <= in_op1) && (in_op1 < 0))
            {
                string w_moji = in_op1.ToString("d2");
                wop1_s = int.Parse(w_moji.Substring(1, 1));
                wop1_e = int.Parse(w_moji.Substring(2, 1));
            }
            else if (in_op1 == 0) { wop1_s = 0; wop1_e = 0; }
            else if (in_op1 == -99) { wop1_s = 0; wop1_e = 7; };

            if ((-77 <= in_op2) && (in_op2 < 0))
            {
                string w_moji = in_op2.ToString("d2");
                wop2_s = int.Parse(w_moji.Substring(1, 1));
                wop2_e = int.Parse(w_moji.Substring(2, 1));
            }
            else if (in_op2 == 0) { wop2_s = 0; wop2_e = 0; }
            else if (in_op2 == -99) { wop2_s = 0; wop2_e = 7; }
            else if (in_op2 == -87) { wop2_s = 0; wop2_e = 7; };

            if ((-77 <= in_op3) && (in_op3 < 0))
            {
                string w_moji = in_op3.ToString("d2");
                wop3_s = int.Parse(w_moji.Substring(1, 1));
                wop3_e = int.Parse(w_moji.Substring(2, 1));
            }
            else if (in_op3 == 0) { wop3_s = 0; wop3_e = 0; }
            else if (in_op3 == -97) { wop3_s = 0; wop3_e = 7; }
            else if (in_op3 == -88) { wop3_s = 0; wop3_e = 7; }
            else if (in_op3 == -99) { wop3_s = 0; wop3_e = 7; }
            else if (in_op3 == -85) { wop3_s = 2; wop3_e = 7; }
            else if (in_op3 == -84) { wop3_s = 0; wop3_e = 7; };

            if ((-77 <= in_op4) && (in_op4 < 0))
            {
                string w_moji = in_op4.ToString("d2");
                wop4_s = int.Parse(w_moji.Substring(1, 1));
                wop4_e = int.Parse(w_moji.Substring(2, 1));
            }
            else if (in_op4 == 0) { wop4_s = 0; wop4_e = 0; }
            else if (in_op4 == -99) { wop4_s = 0; wop4_e = 7; }
            else if (in_op4 == -100) { wop4_s = 0; wop4_e = 7; };
            for (int w1 = wop1_s; w1 <= wop1_e; w1++)
            {
                if ((in_op1 == -99) && (w1 == 1)) continue;
                for (int w2 = wop2_s; w2 <= wop2_e; w2++)
                {
                    if ((in_op2 == -99) && (w2 == 1)) continue;
                    if ((in_op2 == -87) && (w2 == 7) && (w1 >= 2)) continue;
                    if ((in_op2 == -87) && (w2 == 1)) continue;
                    for (int w3 = wop3_s; w3 <= wop3_e; w3++)
                    {
                        if ((in_op3 == -99) && (w3 == 1)) continue;
                        if ((in_op3 == -85) && ((w3 == 3) || (w3 == 4))) continue;
                        for (int w4 = wop4_s; w4 <= wop4_e; w4++)
                        {
                            if ((in_op3 == -88) && (w3 == 7) && (w4 >= 2)) continue;
                            if ((in_op3 == -97) && (w3 == 7) && (w4 >= 4)) continue;
                            if ((in_op3 == -99) && (w3 == 7) && (w4 > 4)) continue;
                            if ((in_op3 == -87) && (w3 == 7) && (w4 > 4)) continue;
                            if ((in_op3 == -85) && (w3 == 7) && (w4 > 4)) continue;
                            if ((in_op3 == -84) && (w3 == 7) && (w4 > 4)) continue;
                            if ((in_op3 == -27) && (w3 == 7) && (w4 > 4)) continue;
                            if (in_op4 == -100)
                            {
                                if (((w2 & 0x03) == 0)
                                  && (w3 == 0)
                                  && (w4 == 0)) continue;
                            }
                            int w_opnum = (in_type << 12)
                                                + (w1 << 9)
                                                + (w2 << 6)
                                                + (w3 << 3)
                                                + w4;
                            if (g_opcheck[w_opnum] != null)
                            {
                                Debug.WriteLine("operand missing", "error");
                            }

                            string w_opdata = "";
                            string w_opname = in_opname;
                            int w_memaccess = 0;
                            int w_len = in_opleng;
                            int w_ret1 = 0;
                            string w_ret2 = "";
                            string w_size_str = SIZE_STR3[in_datasize];
                            switch (in_opname)
                            {
                                case "ABCD":
                                case "ADDX":
                                case "SBCD":
                                case "SUBX":
                                    if ((w3 & 0x01) == 0)
                                    {
                                        w_opdata = "D" + w4 + w_size_str
                                                 + ", D" + w1 + w_size_str;
                                    }
                                    else
                                    {
                                        w_opdata = "(-A" + w4 + w_size_str + ")"
                                                 + ", (-A" + w1 + w_size_str + ")";
                                    }
                                    break;
                                case "CMPM":
                                    w_opdata = "(A" + w4 + w_size_str + "+)"
                                            + ", (A" + w1 + w_size_str + "+)";
                                    break;
                                case "EXG":
                                    if ((w2 == 5) && (w3 == 0))
                                    {
                                        w_opdata = "D" + w1 + w_size_str
                                                + ", D" + w4 + w_size_str;
                                    }
                                    else
                                    if ((w2 == 5) && (w3 == 1))
                                    {
                                        w_opdata = "A" + w1 + w_size_str
                                                + ", A" + w4 + w_size_str;
                                    }
                                    else
                                    {
                                        w_opdata = "D" + w1 + w_size_str
                                                + ", A" + w4 + w_size_str;
                                    }
                                    break;
                                case "ADD":
                                case "AND":
                                case "CHK":
                                case "CMP":
                                case "EOR":
                                case "OR":
                                case "SUB":
                                    (w_ret1, w_ret2) = opcode_addressing(w_len, w3, w4, in_datasize);
                                    if((w3 == 7) && (w4 == 1)) w_memaccess = w_len;
                                    w_len += w_ret1;
                                    if (w2 <= 2)
                                    {
                                        w_opdata = w_ret2 + ", D" + w1 + w_size_str;
                                    }
                                    else
                                    {
                                        w_opdata = "D" + w1 + w_size_str + ", " +w_ret2;
                                    }
                                    break;
                                case "ADDA":
                                case "CMPA":
                                case "LEA":
                                case "SUBA":
                                    (w_ret1, w_ret2) = opcode_addressing(w_len, w3, w4, in_datasize);
                                    if ((w3 == 7) && (w4 == 1)) w_memaccess = w_len;
                                    w_len += w_ret1;
                                    w_opdata = w_ret2 + ", A" + w1 + w_size_str;
                                    break;
                                case "DIVS":
                                case "DIVU":
                                case "MULS":
                                case "MULU":
                                    (w_ret1, w_ret2) = opcode_addressing(w_len, w3, w4, in_datasize);
                                    if ((w3 == 7) && (w4 == 1)) w_memaccess = w_len;
                                    w_len += w_ret1;
                                    w_opdata = w_ret2 + ", D" + w1 + w_size_str;
                                    break;
                                case "ADDI":
                                case "ANDI":
                                case "CMPI":
                                case "EORI":
                                case "ORI":
                                case "SUBI":
                                    (w_ret1, w_ret2) = opcode_addressing(w_len, w3, w4, in_datasize);
                                    if ((w3 == 7) && (w4 == 1)) w_memaccess = w_len;
                                    w_len += w_ret1;
                                    if (in_datasize == 0)
                                    {
                                        w_opdata = "##OP2LEN1, " + w_ret2;
                                    }
                                    else if (in_datasize == 1)
                                    {
                                        w_opdata = "##OP2LEN2, " + w_ret2;
                                    }
                                    else
                                    {
                                        w_opdata = "##OP2LEN4, " + w_ret2;
                                        w_len += 2;
                                    }
                                    break;
                                case "ANDITOCCR":
                                case "EORITOCCR":
                                case "ORITOCCR":
                                    w_opdata = "##OP2LEN1, " + w_ret2;
                                    w_len += 2;
                                    break;
                                case "ANDITOSR":
                                case "EORITOSR":
                                case "ORITOSR":
                                    w_opdata = "##OP2LEN2, " + w_ret2;
                                    w_len += 2;
                                    break;
                                case "ADDQ":
                                case "SUBQ":
                                    (w_ret1, w_ret2) = opcode_addressing(w_len, w3, w4, in_datasize);
                                    if ((w3 == 7) && (w4 == 1)) w_memaccess = w_len;
                                    w_len += w_ret1;
                                    w_opdata = "#" + w1 + ", " + w_ret2;
                                    break;
                                case "AS":
                                case "LS":
                                case "RO":
                                case "ROX":
                                    if ((w2 & 0x03) == 0x03)
                                    {
                                        (w_ret1, w_ret2) = opcode_addressing(w_len, w3, w4, in_datasize);
                                        if ((w3 == 7) && (w4 == 1)) w_memaccess = w_len;
                                        w_opdata = w_ret2 + ".w";
                                    }
                                    else
                                    {
                                        if ((w3 & 0x04) == 0)
                                        {
                                            w_opdata = "#" + w1 + ", D" + w4;
                                        }
                                        else
                                        {
                                            w_opdata = "D" + w1 + ", D" + w4;
                                        }
                                    }
                                    break;
                                case "BCC":
                                    w_opname = "B" + CC_STR[(w1 << 1) + ((w2 >> 2) & 0x01)];
                                    if (((w2 & 0x03) == 0) && (w3 == 0) && (w4 == 0))
                                    {
                                        w_opdata = "##PCOP2LEN2";
                                    }
                                    else
                                    {
                                        w_opdata = "##PCOP1LEN1";
                                    }
                                    break;
                                case "BRA":
                                case "BSR":
                                    if (((w2 & 0x03) == 0) && (w3 == 0) && (w4 == 0))
                                    {
                                        w_opdata = "##PCOP2LEN2";
                                    }
                                    else
                                    {
                                        w_opdata = "##PCOP1LEN1";
                                    }
                                    break;
                                case "BCHG":
                                case "BCLR":
                                case "BSET":
                                case "BTST":
                                    (w_ret1, w_ret2) = opcode_addressing(w_len, w3, w4, in_datasize);
                                    if ((w3 == 7) && (w4 == 1)) w_memaccess = w_len;
                                    w_len += w_ret1;
                                    if((w2 & 0x04) == 0x04)
                                    {
                                        w_opdata = "D" + w1 + ", " + w_ret2;
                                    }
                                    else
                                    {
                                        w_opdata = "##OP2LEN1, " + w_ret2;
                                        w_len += 2;
                                    }
                                    break;
                                case "CLR":
                                case "JMP":
                                case "JSR":
                                case "NBCD":
                                case "NEG":
                                case "NEGX":
                                case "NOT":
                                case "PEA":
                                case "TAS":
                                case "TST":
                                    (w_ret1, w_ret2) = opcode_addressing(w_len, w3, w4, in_datasize);
                                    if ((w3 == 7) && (w4 == 1)) w_memaccess = w_len;
                                    w_len += w_ret1;
                                    w_opdata = w_ret2;
                                    break;
                                case "DBCC":
                                    w_opname = "DB" + CC_STR[(w1 << 1) + ((w2 >> 2) & 0x01)];
                                    w_opdata = "D" + w4 + ".l, ##PCOP2LEN2";
                                    break;
                                case "EXT":
                                    w_opdata = "D" + w4 + w_size_str;
                                    break;
                                case "LINK":
                                    w_opdata = "A" + w4 + ", ##OP2LEN2";
                                    break;
                                case "MOVE":
                                case "MOVEA":
                                    (w_ret1, w_ret2) = opcode_addressing(w_len, w3, w4, in_datasize);
                                    if ((w3 == 7) && (w4 == 1)) w_memaccess = w_len;
                                    w_len += w_ret1;
                                    w_opdata = w_ret2;
                                    (w_ret1, w_ret2) = opcode_addressing(w_len, w2, w1, in_datasize);
                                    if ((w1 == 7) && (w2 == 1)) w_memaccess = w_len;
                                    w_len += w_ret1;
                                    w_opdata += ", " + w_ret2;
                                    break;
                                case "MOVETOCCR":
                                    (w_ret1, w_ret2) = opcode_addressing(w_len, w3, w4, in_datasize);
                                    if ((w3 == 7) && (w4 == 1)) w_memaccess = w_len;
                                    w_len += w_ret1;
                                    w_opdata = w_ret2 + ", CCR";
                                    break;
                                case "MOVETOSR":
                                    (w_ret1, w_ret2) = opcode_addressing(w_len, w3, w4, in_datasize);
                                    if ((w3 == 7) && (w4 == 1)) w_memaccess = w_len;
                                    w_len += w_ret1;
                                    w_opdata = w_ret2 + ", SR";
                                    break;
                                case "MOVEFROMSR":
                                    (w_ret1, w_ret2) = opcode_addressing(w_len, w3, w4, in_datasize);
                                    if ((w3 == 7) && (w4 == 1)) w_memaccess = w_len;
                                    w_len += w_ret1;
                                    w_opdata = "SR, " + w_ret2;
                                    break;
                                case "MOVEUSP":
                                    if(w3 == 4)
                                    {
                                        w_opdata = "A" + w4 + ", USP";
                                    }
                                    else
                                    {
                                        w_opdata = "USP, A" + w4;
                                    }
                                    break;
                                case "MOVEM":
                                    (w_ret1, w_ret2) = opcode_addressing(w_len, w3, w4, in_datasize);
                                    if ((w3 == 7) && (w4 == 1)) w_memaccess = w_len;
                                    w_len += w_ret1;
                                    if (w1 == 4)
                                    {
                                        w_opdata = "#MOVEM_ATOM, " + w_ret2;
                                    }
                                    else
                                    {
                                        w_opdata = w_ret2 + ", #MOVEM_MTOA";
                                    }
                                    break;
                                case "MOVEP":
                                    if ((w2 & 0x02) == 0)
                                    {
                                        w_opdata = "##OP2LEN2(A" + w4 + " + )" + w_size_str + ")"
                                                + ", D" + w1 + w_size_str;
                                    }
                                    else
                                    {
                                        w_opdata = "D" + w1 + w_size_str
                                                + ", ##OP2LEN2(A" + w4 + " + )" + w_size_str;
                                    }
                                    break;
                                case "MOVEQ":
                                    w_opdata = "##OP1LEN1, D" + w1;
                                    break;
                                case "NOP":
                                case "RESET":
                                case "RTE":
                                case "RTR":
                                case "RTS":
                                case "TRAPV":
                                    break;
                                case "SCC":
                                    w_opname = "S" + CC_STR[(w1 << 1) + ((w2 >> 2) & 0x01)];
                                    (w_ret1, w_ret2) = opcode_addressing(w_len, w3, w4, in_datasize);
                                    if ((w3 == 7) && (w4 == 1)) w_memaccess = w_len;
                                    w_len += w_ret1;
                                    w_opdata = w_ret2;
                                    break;
                                case "STOP":
                                    w_opdata = "##OP2LEN2";
                                    break;
                                case "SWAP":
                                    w_opdata = "D" + w4 + ".w";
                                    break;
                                case "TRAP":
                                    w_opdata = "##OP1LEN30";
                                    break;
                                case "UNLK":
                                    w_opdata = "A" + w4;
                                    break;
                                default:
                                    w_opdata = "error";
                                    break;
                            }
                            //------------------------------------------------------
                            g_opcheck[w_opnum] = w_opname.ToLower();
                            string w_out_opname = w_opname;
                            if (in_op_size_mark == true)
                            {
                                w_out_opname += SIZE_STR3[in_datasize];
                            }

                            outsub2("             opcode_add( "
                                + "0x" + w_opnum.ToString("x4")
                                + ", " + in_funcname
                                + ", \"" + in_opname + "\""
                                + ", \"" + w_opname + "\""
                                + ", \"" + w_out_opname + "\""
                                + ", \"" + w_opdata + "\""
                                + ", " + w_len
                                + ", " + in_datasize
                                + ", " + w_memaccess
                                + " );");
                            
                        }
                    }
                }
            }
        }
        private static (int, string) opcode_addressing(int in_offset, int in_mode, int in_register, int in_size)
        {
            int w_len = 0;
            string w_moji = "";
            int w_opnum = 1 + (in_offset / 2);
            switch (in_mode)
            {
                case 0:
                    w_moji = "D" + in_register + SIZE_STR3[in_size];
                    break;
                case 1:
                    w_moji = "A" + in_register + SIZE_STR3[in_size];
                    break;
                case 2:
                    w_moji = "(A" + in_register + ")" + SIZE_STR3[in_size];
                    break;
                case 3:
                    w_moji = "(A" + in_register + "+)" + SIZE_STR3[in_size];
                    break;
                case 4:
                    w_moji = "(-A" + in_register + ")" + SIZE_STR3[in_size];
                    break;
                case 5:
                    w_moji = "(A" + in_register + "+#OP"+ w_opnum + "LEN2U)" + SIZE_STR3[in_size];
                    w_len = 2;
                    break;
                case 6:
                    w_moji = "(A" + in_register + "+#OP" + w_opnum + "IND)" + SIZE_STR3[in_size];
                    w_len = 2;
                    break;
                case 7:
                    switch (in_register)
                    {
                        case 0:
                            w_moji = "(" + "#OP" + w_opnum + "LEN2)";
                            w_len = 2;
                            break;
                        case 1:
                            w_moji = "(" + "#OP" + w_opnum + "LEN4)";
                            w_len = 4;
                            break;
                        case 2:
                            w_moji = "(#PCOP" + w_opnum + "LEN2)" + SIZE_STR3[in_size];
                            w_len = 2;
                            break;
                        case 3:
                            w_moji = "(PC +#OP" + w_opnum + "IND)" + SIZE_STR3[in_size];
                            w_len = 2;
                            break;
                        case 4:
                            if (in_size == 0)
                            {
                                w_moji = "##OP" + w_opnum + "LEN1";
                                w_len = 2;
                            }
                            else if (in_size == 1)
                            {
                                w_moji = "##OP" + w_opnum + "LEN2";
                                w_len = 2;
                            }
                            else
                            {
                                w_moji = "##OP" + w_opnum + "LEN4";
                                w_len += 4;
                            }
                            break;
                    }
                    break;
            }
            return (w_len, w_moji);
        }
    }
}
