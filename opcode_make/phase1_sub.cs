using System;
using System.IO;
namespace opcode_make
{
    internal partial class Program
    {
        static void out_head(string in_outfile)
        {
            g_op_name = in_outfile;
            Console.WriteLine("start:" + g_op_name);
            File.Delete("md_m68k_ope" + g_op_name + ".cs");
            outtext("using System;");
            outtext("using static MDTracer.md_m68k;");
            outtext("namespace MDTracer");
            outtext("{");
            outtext("    internal partial class md_m68k");
            outtext("    {");
        }
        static void out_tail()
        {
            outtext("   }");
            outtext("}");
            File.Delete("..\\..\\..\\MDTracer\\opc\\md_m68k_ope" + g_op_name + ".cs");
            File.Move("md_m68k_ope" + g_op_name + ".cs"
                , "..\\..\\..\\MDTracer\\opc\\md_m68k_ope" + g_op_name + ".cs");
        }
        static void outtext(params object[] in_val)
        {
            string w_moji = string_add(in_val);
            System.IO.File.AppendAllText("md_m68k_ope" + g_op_name + ".cs", g_outtext_tab + w_moji + Environment.NewLine);
        }
        static string string_add(params object[] in_val)
        {
            string w_out = "";
            foreach (var wval in in_val)
            {
                w_out += wval.ToString();
            }
            return w_out;
        }
        static void out_opcode(string in_funcname
            , int in_type, int in_op1, int in_op2, int in_op3, int in_op4
            , int in_opleng, int in_datasize, bool in_op_size_mark)
        {
            g_opinfo.Add(new Opinfo(in_funcname
                , g_op_name
                , in_type
                , in_op1
                , in_op2
                , in_op3
                , in_op4
                , in_opleng
                , in_datasize
                , in_op_size_mark));
        }

        //----------------------------------------------------------------
        private static string make_reg_data(string in_regnum, int in_size)
        {
            return "g_reg_data[" + in_regnum + "]." + SIZE_STR[in_size];
        }
        private static string make_reg_addr(string in_regnum, int in_size)
        {
            return "g_reg_addr[" + in_regnum + "]." + SIZE_STR[in_size];
        }
        private static string variable_size(string in_variable, int in_size)
        {
            return in_variable + "." + SIZE_STR[in_size];
        }
        //----------------------------------------------------------------
        private static void status_update(int in_n, int in_z, int in_v, int in_c, int in_x,
                                        string in_val1, string in_val2, string in_data, string in_size_str)
        {
            if ((in_z == 2) || (in_z == 4))
            {
                outtext("            uint w_mask = MASKBIT[" + in_size_str + "];");
            }
            if ((in_v == 2) || (in_v == 3) || (in_c == 2) || (in_c == 3))
            {
                outtext("            uint w_most = MOSTBIT[" + in_size_str + "];");
                outtext("            bool SMC = ((", in_val2, ".l", " & w_most)) == 0 ? false : true;");
                outtext("            bool DMC = ((", in_val1, ".l", " & w_most)) == 0 ? false : true;");
                outtext("            bool RMC = ((", in_data, ".l", " & w_most)) == 0 ? false : true;");
            }
            else
            if (in_n == 2)
            {
                outtext("            uint w_most = MOSTBIT[" + in_size_str + "];");
            }
            switch (in_n)
            {
                case 0:
                    outtext("            g_status_N = false;");
                    break;
                case 2:
                    outtext("            g_status_N = ((", in_data, ".l & w_most) == w_most) ? true: false;");
                    break;
                default:
                    break;
            }
            switch (in_z)
            {
                case 1:
                    outtext("            g_status_Z = true;");
                    break;
                case 2:
                    outtext("            g_status_Z = ((", in_data, ".l & w_mask) == 0) ? true: false;");
                    break;
                case 4:
                    outtext("            if ((", in_data, ".l & w_mask) != 0) g_status_Z = false;");
                    break;
                default:
                    break;
            }
            switch (in_v)
            {
                case 0:
                    outtext("            g_status_V = false;");
                    break;
                case 2:
                    outtext("            g_status_V = ((SMC ^ RMC) & (DMC ^ RMC));");
                    break;
                case 3:
                    outtext("            g_status_V = ((SMC ^ DMC) & (DMC ^ RMC));");
                    break;
                default:
                    break;
            }
            switch (in_c)
            {
                case 0:
                    outtext("            g_status_C = false;");
                    break;
                case 2:
                    outtext("            g_status_C = ((SMC & DMC) | (!RMC & DMC) | (SMC & !RMC));");
                    break;
                case 3:
                    outtext("            g_status_C = ((SMC & !DMC) | (RMC & !DMC) | (SMC & RMC));");
                    break;
                default:
                    break;
            }
            switch (in_x)
            {
                case 2:
                    outtext("            g_status_X = g_status_C;");
                    break;
                default:
                    break;
            }
        }
    }
}
