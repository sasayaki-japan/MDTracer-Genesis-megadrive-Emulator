using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_TAS()
        {
            g_reg_PC += 2;
            adressing_func_address(g_op3, g_op4, 0);
            g_work_val1.b0 = (byte)adressing_func_read(g_op3, g_op4, 0);
            g_work_val2.b0 = (byte)(g_work_val1.b0 | 0x80);
            adressing_func_write(g_op3, g_op4, 0, g_work_val2.b0);
            uint w_mask = MASKBIT[0];
            uint w_most = MOSTBIT[0];
            g_status_N = ((g_work_val1.l & w_most) == w_most) ? true: false;
            g_status_Z = ((g_work_val1.l & w_mask) == 0) ? true: false;
            g_status_V = false;
            g_status_C = false;
           g_clock += 4;
        }
   }
}
