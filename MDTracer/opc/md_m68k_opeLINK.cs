using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_LINK()
        {
            g_clock += 18;
            g_reg_PC += 2;
            short w_ext_disp = (short)md_main.g_md_bus.read16(g_reg_PC);
            g_reg_PC += 2;
            stack_push32(g_reg_addr[g_op4].l);
            g_reg_addr[g_op4].l = g_reg_addr[7].l;
            g_reg_addr[7].l = (uint)(g_reg_addr[7].l + w_ext_disp);
        }
   }
}
