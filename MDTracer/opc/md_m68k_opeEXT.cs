using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_EXT2()
        {
            g_reg_PC += 2;
            if((g_reg_data[g_op4].b0 & 0x80) == 0){
                g_reg_data[g_op4].b1 = 0;
            }else{
                g_reg_data[g_op4].b1 = 0xff;
            }
            g_work_data.w = g_reg_data[g_op4].w;
            uint w_mask = MASKBIT[1];
            uint w_most = MOSTBIT[1];
            g_status_N = ((g_work_data.l & w_most) == w_most) ? true: false;
            g_status_Z = ((g_work_data.l & w_mask) == 0) ? true: false;
            g_status_V = false;
            g_status_C = false;
           g_clock += 4;
        }
        private void analyse_EXT3()
        {
            g_reg_PC += 2;
            if((g_reg_data[g_op4].w & 0x8000) == 0){
                g_reg_data[g_op4].wup = 0;
            }else{
                g_reg_data[g_op4].wup = 0xffff;
            }
            g_work_data.l = g_reg_data[g_op4].l;
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
