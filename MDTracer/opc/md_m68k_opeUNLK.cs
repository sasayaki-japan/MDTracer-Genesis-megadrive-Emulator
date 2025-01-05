using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_UNLK()
        {
            g_clock += 12;
            g_reg_PC += 2;
            g_reg_addr[7].l = g_reg_addr[g_op4].l;
            g_reg_addr[g_op4].l = stack_pop32();
        }
   }
}
