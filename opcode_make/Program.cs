using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
namespace opcode_make
{
    internal partial class Program
    {
        static string[] SIZE_STR = { "b0", "w", "l" };
        static string[] SIZE_STR2 = { "b", "w", "l" };
        static string[] SIZE_STR3 = { ".b", ".w", ".l" };
        static int[] DATA_LENG = { 1, 2, 4 };
        static int[] DATA_BIT = { 8, 16, 32};
        private static string[] MOSTBIT = new string[3] { "0x80", "0x8000", "0x80000000" };
        public static string g_outtext_tab = "";
        public static string g_op_name;
        public struct Opinfo
        {
            public string funcname;
            public string opname;
            public int type;
            public int op1;
            public int op2;
            public int op3;
            public int op4;
            public int opleng;
            public int datasize;
            public bool op_size_mark;
            public Opinfo(string in_funcname, string in_opname
                , int in_type, int in_op1, int in_op2, int in_op3, int in_op4
                , int in_opleng, int in_datasize, bool in_op_size_mark)
            {
                funcname = in_funcname;
                opname = in_opname;
                type = in_type;
                op1 = in_op1;
                op2 = in_op2;
                op3 = in_op3;
                op4 = in_op4;
                opleng = in_opleng;
                datasize = in_datasize;
                op_size_mark = in_op_size_mark;
            }
        }
        static List<Opinfo> g_opinfo;

        static void Main(string[] args)
        {
            g_opinfo = new List<Opinfo>();

            phase1();
            phase2();
        }
    }
}
