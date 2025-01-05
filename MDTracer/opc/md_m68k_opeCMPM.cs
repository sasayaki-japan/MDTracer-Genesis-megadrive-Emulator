using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_CMPM_b()
        {
            g_clock += 12;
            g_reg_PC += 2;
            g_work_val1.l = md_main.g_md_bus.read8(g_reg_addr[g_op1].l);
            g_work_val2.l = md_main.g_md_bus.read8(g_reg_addr[g_op4].l);
            g_work_data.l = g_work_val1.l - g_work_val2.l;
            g_reg_addr[g_op1].l += 1;
            g_reg_addr[g_op4].l += 1;
            uint w_mask = MASKBIT[g_op2 & 0x03];
            uint w_most = MOSTBIT[g_op2 & 0x03];
            bool SMC = ((g_work_val2.l & w_most)) == 0 ? false : true;
            bool DMC = ((g_work_val1.l & w_most)) == 0 ? false : true;
            bool RMC = ((g_work_data.l & w_most)) == 0 ? false : true;
            g_status_N = ((g_work_data.l & w_most) == w_most) ? true: false;
            g_status_Z = ((g_work_data.l & w_mask) == 0) ? true: false;
            g_status_V = ((SMC ^ DMC) & (DMC ^ RMC));
            g_status_C = ((SMC & !DMC) | (RMC & !DMC) | (SMC & RMC));
        }
        private void analyse_CMPM_w()
        {
            g_clock += 12;
            g_reg_PC += 2;
            g_work_val1.l = md_main.g_md_bus.read16(g_reg_addr[g_op1].l);
            g_work_val2.l = md_main.g_md_bus.read16(g_reg_addr[g_op4].l);
            g_work_data.l = g_work_val1.l - g_work_val2.l;
            g_reg_addr[g_op1].l += 2;
            g_reg_addr[g_op4].l += 2;
            uint w_mask = MASKBIT[g_op2 & 0x03];
            uint w_most = MOSTBIT[g_op2 & 0x03];
            bool SMC = ((g_work_val2.l & w_most)) == 0 ? false : true;
            bool DMC = ((g_work_val1.l & w_most)) == 0 ? false : true;
            bool RMC = ((g_work_data.l & w_most)) == 0 ? false : true;
            g_status_N = ((g_work_data.l & w_most) == w_most) ? true: false;
            g_status_Z = ((g_work_data.l & w_mask) == 0) ? true: false;
            g_status_V = ((SMC ^ DMC) & (DMC ^ RMC));
            g_status_C = ((SMC & !DMC) | (RMC & !DMC) | (SMC & RMC));
        }
        private void analyse_CMPM_l()
        {
            g_clock += 20;
            g_reg_PC += 2;
            g_work_val1.l = md_main.g_md_bus.read32(g_reg_addr[g_op1].l);
            g_work_val2.l = md_main.g_md_bus.read32(g_reg_addr[g_op4].l);
            g_work_data.l = g_work_val1.l - g_work_val2.l;
            g_reg_addr[g_op1].l += 4;
            g_reg_addr[g_op4].l += 4;
            uint w_mask = MASKBIT[g_op2 & 0x03];
            uint w_most = MOSTBIT[g_op2 & 0x03];
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
