using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_DIVU()
        {
            g_reg_PC += 2;
            adressing_func_address(g_op3, g_op4, 1);
            g_clock = 140;
            g_work_data.w = (ushort)adressing_func_read(g_op3, g_op4, 1);
            if (g_work_data.w == 0) { g_reg_PC = md_main.g_md_bus.read32(20); return; }
            g_work_val1.l = (uint)(g_reg_data[g_op1].l / g_work_data.w);
            if ((uint)g_work_val1.l > 0xffff) { g_status_V = true; }
            else {
                g_status_V = false;
                g_work_val2.l = (uint)(g_reg_data[g_op1].l % g_work_data.w);
                g_work_data.w = g_work_val1.w;
                g_work_data.wup = g_work_val2.w;
                g_reg_data[g_op1].l = g_work_data.l;
                if((g_work_val1.w & 0x8000) == 0x8000) g_status_N = true; else g_status_N = false;
                if(g_work_val1.w == 0) g_status_Z = true; else g_status_Z = false;
            }
            g_status_C = false;
        }
   }
}
