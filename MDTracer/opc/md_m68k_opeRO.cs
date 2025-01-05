using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_RO_reg()
        {
            int w_size = g_op2 & 0x03;
            int w_ir = g_op3 & 0x04;
            int w_dr = g_op2 & 0x04;
            if (w_size == 2) g_clock = 8; else g_clock = 6;
            g_reg_PC += 2;
            uint wcnt = 0;
            if(w_ir == 0)
            {
                wcnt = g_op1;
                if(wcnt == 0) wcnt = 8;
            }else{
                wcnt = g_reg_data[g_op1].l & 0x3f;
            }
            g_work_data.l = read_g_reg_data(g_op4, w_size);
            g_status_V = false;
            g_status_C = false;
            if(w_dr == 0)
            {
                for (int i = 0; i < wcnt; i++)
                {
                    g_clock += 2;
                    g_status_C = ((g_work_data.l & 0x01) == 0x01);
                    g_work_data.l = (g_work_data.l >> 1);
                    if (g_status_C == true) {
                        g_work_data.l = (g_work_data.l | MOSTBIT[w_size]);
                    }
                }
            }else{
                for (int i = 0; i < wcnt; i++)
                {
                    g_clock += 2;
                    g_status_C = ((g_work_data.l & MOSTBIT[w_size]) != 0);
                    g_work_data.l = (uint)(g_work_data.l << 1);
                    if (g_status_C == true) {
                        g_work_data.l = (uint)(g_work_data.l | 0x01);
                    }
                }
            }
            if(w_ir != 0) g_clock += 2;
            write_g_reg_data(g_op4, w_size, g_work_data.l);
            uint w_mask = MASKBIT[g_op2 & 0x03];
            uint w_most = MOSTBIT[g_op2 & 0x03];
            g_status_N = ((g_work_data.l & w_most) == w_most) ? true: false;
            g_status_Z = ((g_work_data.l & w_mask) == 0) ? true: false;
        }
        private void analyse_RO_mem()
        {
            int w_size = 1;
            int w_dr = g_op2 & 0x04;
            g_clock = 9;
            g_reg_PC += 2;
            uint wcnt = 1;
            adressing_func_address(g_op3, g_op4, 1);
            g_work_data.w = (ushort)adressing_func_read(g_op3, g_op4, 1);
            g_status_V = false;
            g_status_C = false;
            if(w_dr == 0)
            {
                for (int i = 0; i < wcnt; i++)
                {
                    g_clock += 2;
                    g_status_C = ((g_work_data.l & 0x01) == 0x01);
                    g_work_data.l = (g_work_data.l >> 1);
                    if (g_status_C == true) {
                        g_work_data.l = (g_work_data.l | MOSTBIT[w_size]);
                    }
                }
            }else{
                for (int i = 0; i < wcnt; i++)
                {
                    g_clock += 2;
                    g_status_C = ((g_work_data.l & MOSTBIT[w_size]) != 0);
                    g_work_data.l = (uint)(g_work_data.l << 1);
                    if (g_status_C == true) {
                        g_work_data.l = (uint)(g_work_data.l | 0x01);
                    }
                }
            }
            adressing_func_write(g_op3, g_op4, 1, g_work_data.w);
            uint w_mask = MASKBIT[1];
            uint w_most = MOSTBIT[1];
            g_status_N = ((g_work_data.l & w_most) == w_most) ? true: false;
            g_status_Z = ((g_work_data.l & w_mask) == 0) ? true: false;
        }
   }
}
