using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_MOVEQ()
        {
            g_reg_PC += 2;
            g_work_data.l = get_int_cast((byte)(g_opcode & 0x00ff), 0);
            g_reg_data[g_op1].l = g_work_data.l;
            uint w_mask = MASKBIT[2];
            uint w_most = MOSTBIT[2];
            g_status_N = ((g_work_data.l & w_most) == w_most) ? true: false;
            g_status_Z = ((g_work_data.l & w_mask) == 0) ? true: false;
            g_status_V = false;
            g_status_C = false;
            g_clock += 4;
        }
   }
}
