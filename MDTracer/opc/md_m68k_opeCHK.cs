using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_CHK()
        {
            g_clock += 43;
            g_reg_PC += 2;
            g_work_val1.w = g_reg_data[g_op1].w;
            adressing_func_address(g_op3, g_op4, 1);
            g_work_val2.w = (ushort)adressing_func_read(g_op3, g_op4, 1);
            if (g_work_val1.w < 0){ g_status_N = true; g_reg_PC = md_main.g_md_bus.read32(24); }
            else if (g_work_val2.w < g_work_val1.w) { g_status_N = false; g_reg_PC = md_main.g_md_bus.read32(24); }
        }
   }
}
