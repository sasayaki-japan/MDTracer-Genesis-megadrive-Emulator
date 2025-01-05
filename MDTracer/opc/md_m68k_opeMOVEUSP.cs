using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_MOVEUSP_1()
        {
            g_clock += 4;
            g_reg_PC += 2;
            g_reg_addr_usp.l = g_reg_addr[g_op4].l;
        }
        private void analyse_MOVEUSP_2()
        {
            g_clock += 4;
            g_reg_PC += 2;
            g_reg_addr[g_op4].l = g_reg_addr_usp.l;
        }
   }
}
