using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_ADDQ()
        {
            int w_size = g_op2 & 0x03;
            if(w_size == 2)
            {
                if (g_op3 == 0) g_clock = 8; else g_clock = 14;
            }else{
                if (g_op3 == 0) g_clock = 4; else g_clock = 9;
            }
            g_reg_PC += 2;
            g_work_val2.l = (byte)((g_opcode >> 9) & 0x07);
            if(g_work_val2.l == 0) g_work_val2.l = 8;
            adressing_func_address(g_op3, g_op4, w_size);
            g_work_val1.l = adressing_func_read(g_op3, g_op4, w_size);
            g_work_data.l = g_work_val1.l + g_work_val2.l;
            adressing_func_write(g_op3, g_op4, w_size, g_work_data.l);
            if(g_op3 != 1){
            uint w_mask = MASKBIT[g_op2];
            uint w_most = MOSTBIT[g_op2];
            bool SMC = ((g_work_val2.l & w_most)) == 0 ? false : true;
            bool DMC = ((g_work_val1.l & w_most)) == 0 ? false : true;
            bool RMC = ((g_work_data.l & w_most)) == 0 ? false : true;
            g_status_N = ((g_work_data.l & w_most) == w_most) ? true: false;
            g_status_Z = ((g_work_data.l & w_mask) == 0) ? true: false;
            g_status_V = ((SMC ^ RMC) & (DMC ^ RMC));
            g_status_C = ((SMC & DMC) | (!RMC & DMC) | (SMC & !RMC));
            g_status_X = g_status_C;
            }
        }
   }
}
