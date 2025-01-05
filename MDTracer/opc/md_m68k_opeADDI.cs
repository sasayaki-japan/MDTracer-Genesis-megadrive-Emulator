using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_ADDI()
        {
           if(g_op3 == 0) if(g_op2 == 2)g_clock = 16; else g_clock = 8;
                     else if(g_op2 == 2)g_clock = 22; else g_clock = 13;
            g_reg_PC += 2;
            switch (g_op2)
            {
                case 0: g_work_val2.l = (uint)(md_main.g_md_bus.read16(g_reg_PC) & 0x00ff); g_reg_PC += 2; break;
                case 1: g_work_val2.l = md_main.g_md_bus.read16(g_reg_PC); g_reg_PC += 2; break;
                default: g_work_val2.l = md_main.g_md_bus.read32(g_reg_PC); g_reg_PC += 4; break;
            }
            adressing_func_address(g_op3, g_op4, g_op2);
            g_work_val1.l = adressing_func_read(g_op3, g_op4, g_op2);
            g_work_data.l = g_work_val1.l  +  g_work_val2.l;
            adressing_func_write(g_op3, g_op4, g_op2, g_work_data.l);
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
