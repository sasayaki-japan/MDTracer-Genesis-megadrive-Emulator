using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_CMP()
        {
            g_reg_PC += 2;
            int w_size = g_op2 & 0x03;
                g_clock = (w_size == 2) ? 6 : 4;
                g_work_val1.l = read_g_reg_data(g_op1, w_size);
                adressing_func_address(g_op3, g_op4, w_size);
                g_work_val2.l = adressing_func_read(g_op3, g_op4, w_size);
                g_work_data.l = g_work_val1.l  -  g_work_val2.l;
            uint w_mask = MASKBIT[w_size];
            uint w_most = MOSTBIT[w_size];
            bool SMC = ((g_work_val2.l & w_most)) == 0 ? false : true;
            bool DMC = ((g_work_val1.l & w_most)) == 0 ? false : true;
            bool RMC = ((g_work_data.l & w_most)) == 0 ? false : true;
            g_status_N = ((g_work_data.l & w_most) == w_most) ? true: false;
            g_status_Z = ((g_work_data.l & w_mask) == 0) ? true: false;
            g_status_V = ((SMC ^ DMC) & (DMC ^ RMC));
            g_status_C = ((SMC & !DMC) | (RMC & !DMC) | (SMC & RMC));
        }
   }
}
