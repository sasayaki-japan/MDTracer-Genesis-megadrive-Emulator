using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_EORITOSR_w()
        {
            g_clock += 20;
            g_reg_PC += 2;
            g_work_val2.w = md_main.g_md_bus.read16(g_reg_PC);
            g_reg_PC += 2;
            g_work_val1.w = g_reg_SR;
            g_work_data.w = (ushort)(g_work_val1.w  ^  g_work_val2.w);
            g_reg_SR = g_work_data.w;
        }
   }
}
