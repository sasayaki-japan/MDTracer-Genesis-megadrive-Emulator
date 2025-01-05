using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_EOR()
        {
            g_reg_PC += 2;
            int w_size = g_op2 & 0x03;
            if ((g_op2 & 0x04) == 0)
            {
                g_clock = (w_size == 2) ? 6 : 4;
                g_work_val1.l = read_g_reg_data(g_op1, w_size);
                adressing_func_address(g_op3, g_op4, w_size);
                g_work_val2.l = adressing_func_read(g_op3, g_op4, w_size);
                g_work_data.l = g_work_val1.l  ^  g_work_val2.l;
                write_g_reg_data(g_op1, w_size, g_work_data.l);
            }else{
                g_clock = (w_size == 2) ? 14 : 9;
                adressing_func_address(g_op3, g_op4, w_size);
                g_work_val1.l = adressing_func_read(g_op3, g_op4, w_size);
                g_work_val2.l = read_g_reg_data(g_op1, w_size);
                g_work_data.l = g_work_val1.l  ^  g_work_val2.l;
                adressing_func_write(g_op3, g_op4, w_size, g_work_data.l);
            }
            uint w_mask = MASKBIT[w_size];
            uint w_most = MOSTBIT[w_size];
            g_status_N = ((g_work_data.l & w_most) == w_most) ? true: false;
            g_status_Z = ((g_work_data.l & w_mask) == 0) ? true: false;
            g_status_V = false;
            g_status_C = false;
        }
   }
}
