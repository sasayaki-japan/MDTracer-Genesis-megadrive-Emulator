using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_BTST_dynamic_long()
        {
            g_clock += 6;
            g_reg_PC += 2;
            int w_bit = g_reg_data[g_op1].b0;
            w_bit = w_bit & 0x1f;
            g_work_data.l = adressing_func_read(0, g_op4, 2);
            g_status_Z = ((g_work_data.l & BITHIT[w_bit]) == 0);
        }
        private void analyse_BTST_dynamic_byte()
        {
            g_clock += 4;
            g_reg_PC += 2;
            int w_bit = g_reg_data[g_op1].b0;
            w_bit = w_bit & 0x07;
            adressing_func_address(g_op3, g_op4, 0);
            g_work_data.b0 = (byte)adressing_func_read(g_op3, g_op4, 0);
            g_status_Z = ((g_work_data.b0 & BITHIT[w_bit]) == 0);
        }
        private void analyse_BTST_static_long()
        {
            g_clock += 10;
            g_reg_PC += 2;
            int w_bit = md_main.g_md_bus.read16(g_reg_PC);
            g_reg_PC += 2;
            w_bit = w_bit & 0x1f;
            g_work_data.l = adressing_func_read(0, g_op4, 2);
            g_status_Z = ((g_work_data.l & BITHIT[w_bit]) == 0);
        }
        private void analyse_BTST_static_byte()
        {
            g_clock += 8;
            g_reg_PC += 2;
            int w_bit = md_main.g_md_bus.read16(g_reg_PC);
            g_reg_PC += 2;
            w_bit = w_bit & 0x07;
            adressing_func_address(g_op3, g_op4, 0);
            g_work_data.b0 = (byte)adressing_func_read(g_op3, g_op4, 0);
            g_status_Z = ((g_work_data.b0 & BITHIT[w_bit]) == 0);
        }
   }
}
