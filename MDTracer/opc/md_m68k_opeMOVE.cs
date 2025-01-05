using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_MOVE_b()
        {
            int w_size = 0;
            int w_src = (g_op3 < 7) ? g_op3 : 7 + g_op4;
            int w_dest= (g_op2 < 7) ? g_op2 : 7 + g_op1;
            int w_clock = 0;
            switch(g_op)
            {
                case 1:
                    w_size = 0;
                    w_clock = MOVE_CLOCK[w_src, w_dest];
                    break;
                case 3: 
                    w_size = 1; 
                    w_clock = MOVE_CLOCK[w_src, w_dest];
                    break; 
                default: 
                    w_size = 2; 
                    w_clock = MOVE_CLOCK_L[w_src, w_dest];
                    break; 
            } 
            g_reg_PC += 2; 
            adressing_func_address(g_op3, g_op4, w_size); 
            g_work_data.l = (uint)adressing_func_read(g_op3, g_op4, w_size); 
            adressing_func_address(g_op2, g_op1, w_size); 
            adressing_func_write(g_op2, g_op1, w_size, g_work_data.l); 
            g_clock = w_clock;
            uint w_mask = MASKBIT[w_size];
            uint w_most = MOSTBIT[w_size];
            g_status_N = ((g_work_data.l & w_most) == w_most) ? true: false;
            g_status_Z = ((g_work_data.l & w_mask) == 0) ? true: false;
            g_status_V = false;
            g_status_C = false;
        }
        private void analyse_MOVE_w()
        {
            int w_size = 0;
            int w_src = (g_op3 < 7) ? g_op3 : 7 + g_op4;
            int w_dest= (g_op2 < 7) ? g_op2 : 7 + g_op1;
            int w_clock = 0;
            switch(g_op)
            {
                case 1:
                    w_size = 0;
                    w_clock = MOVE_CLOCK[w_src, w_dest];
                    break;
                case 3: 
                    w_size = 1; 
                    w_clock = MOVE_CLOCK[w_src, w_dest];
                    break; 
                default: 
                    w_size = 2; 
                    w_clock = MOVE_CLOCK_L[w_src, w_dest];
                    break; 
            } 
            g_reg_PC += 2; 
            adressing_func_address(g_op3, g_op4, w_size); 
            g_work_data.l = (uint)adressing_func_read(g_op3, g_op4, w_size); 
            adressing_func_address(g_op2, g_op1, w_size); 
            adressing_func_write(g_op2, g_op1, w_size, g_work_data.l); 
            g_clock = w_clock;
            uint w_mask = MASKBIT[w_size];
            uint w_most = MOSTBIT[w_size];
            g_status_N = ((g_work_data.l & w_most) == w_most) ? true: false;
            g_status_Z = ((g_work_data.l & w_mask) == 0) ? true: false;
            g_status_V = false;
            g_status_C = false;
        }
        private void analyse_MOVE_l()
        {
            int w_size = 0;
            int w_src = (g_op3 < 7) ? g_op3 : 7 + g_op4;
            int w_dest= (g_op2 < 7) ? g_op2 : 7 + g_op1;
            int w_clock = 0;
            switch(g_op)
            {
                case 1:
                    w_size = 0;
                    w_clock = MOVE_CLOCK[w_src, w_dest];
                    break;
                case 3: 
                    w_size = 1; 
                    w_clock = MOVE_CLOCK[w_src, w_dest];
                    break; 
                default: 
                    w_size = 2; 
                    w_clock = MOVE_CLOCK_L[w_src, w_dest];
                    break; 
            } 
            g_reg_PC += 2; 
            adressing_func_address(g_op3, g_op4, w_size); 
            g_work_data.l = (uint)adressing_func_read(g_op3, g_op4, w_size); 
            adressing_func_address(g_op2, g_op1, w_size); 
            adressing_func_write(g_op2, g_op1, w_size, g_work_data.l); 
            g_clock = w_clock;
            uint w_mask = MASKBIT[w_size];
            uint w_most = MOSTBIT[w_size];
            g_status_N = ((g_work_data.l & w_most) == w_most) ? true: false;
            g_status_Z = ((g_work_data.l & w_mask) == 0) ? true: false;
            g_status_V = false;
            g_status_C = false;
        }
   }
}
