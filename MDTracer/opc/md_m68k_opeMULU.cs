using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_MULU()
        {
            g_reg_PC += 2;
            adressing_func_address(g_op3, g_op4, 1);
            g_clock = 70;
            g_work_data.w = (ushort)adressing_func_read(g_op3, g_op4, 1);
            g_work_data.l = (uint)(g_work_data.w * g_reg_data[g_op1].w);
            g_reg_data[g_op1].l = g_work_data.l;
            uint w_mask = MASKBIT[2];
            uint w_most = MOSTBIT[2];
            g_status_N = ((g_work_data.l & w_most) == w_most) ? true: false;
            g_status_Z = ((g_work_data.l & w_mask) == 0) ? true: false;
            g_status_V = false;
            g_status_C = false;
        }
   }
}
