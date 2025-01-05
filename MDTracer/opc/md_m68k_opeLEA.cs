using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_LEA()
        {
            g_reg_PC += 2;
            adressing_func_address(g_op3, g_op4, 2);
            g_reg_addr[g_op1].l = g_analyze_address;
        }
   }
}
