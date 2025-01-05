using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_ANDITOCCR_b()
        {
            g_clock += 20;
            g_reg_PC += 2;
            g_work_val2.b0 = (byte)(md_main.g_md_bus.read16(g_reg_PC) & 0x00ff);
            g_reg_PC += 2;
            g_work_val1.b0 = g_status_CCR;
            g_work_data.b0 = (byte)(g_work_val1.b0  &  g_work_val2.b0);
            g_status_CCR = g_work_data.b0;
        }
   }
}
