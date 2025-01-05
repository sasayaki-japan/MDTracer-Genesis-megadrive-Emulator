using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_NOT_b()
        {
            if(g_op3 <= 1){
                g_clock += 4;
            }else{
                g_clock += 9;
            }
            g_reg_PC += 2;
            adressing_func_address(g_op3, g_op4, 0);
            g_work_data.l = adressing_func_read(g_op3, g_op4, 0);
            g_work_data.l = ~g_work_data.l;
            adressing_func_write(g_op3, g_op4, 0, g_work_data.l);
            uint w_mask = MASKBIT[g_op2];
            uint w_most = MOSTBIT[g_op2];
            g_status_N = ((g_work_data.l & w_most) == w_most) ? true: false;
            g_status_Z = ((g_work_data.l & w_mask) == 0) ? true: false;
            g_status_V = false;
            g_status_C = false;
        }
        private void analyse_NOT_w()
        {
            if(g_op3 <= 1){
                g_clock += 4;
            }else{
                g_clock += 9;
            }
            g_reg_PC += 2;
            adressing_func_address(g_op3, g_op4, 1);
            g_work_data.l = adressing_func_read(g_op3, g_op4, 1);
            g_work_data.l = ~g_work_data.l;
            adressing_func_write(g_op3, g_op4, 1, g_work_data.l);
            uint w_mask = MASKBIT[g_op2];
            uint w_most = MOSTBIT[g_op2];
            g_status_N = ((g_work_data.l & w_most) == w_most) ? true: false;
            g_status_Z = ((g_work_data.l & w_mask) == 0) ? true: false;
            g_status_V = false;
            g_status_C = false;
        }
        private void analyse_NOT_l()
        {
            if(g_op3 <= 1){
                g_clock += 6;
            }else{
                g_clock += 14;
            }
            g_reg_PC += 2;
            adressing_func_address(g_op3, g_op4, 2);
            g_work_data.l = adressing_func_read(g_op3, g_op4, 2);
            g_work_data.l = ~g_work_data.l;
            adressing_func_write(g_op3, g_op4, 2, g_work_data.l);
            uint w_mask = MASKBIT[g_op2];
            uint w_most = MOSTBIT[g_op2];
            g_status_N = ((g_work_data.l & w_most) == w_most) ? true: false;
            g_status_Z = ((g_work_data.l & w_mask) == 0) ? true: false;
            g_status_V = false;
            g_status_C = false;
        }
   }
}
