using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_CLR_b()
        {
            if(g_op3 <= 1){
                g_clock += 4;
            }else{
                g_clock += 9;
            }
            g_reg_PC += 2;
            adressing_func_address(g_op3, g_op4, 0);
            adressing_func_write(g_op3, g_op4, 0, 0);
            g_status_N = false;
            g_status_Z = true;
            g_status_V = false;
            g_status_C = false;
        }
        private void analyse_CLR_w()
        {
            if(g_op3 <= 1){
                g_clock += 4;
            }else{
                g_clock += 9;
            }
            g_reg_PC += 2;
            adressing_func_address(g_op3, g_op4, 1);
            adressing_func_write(g_op3, g_op4, 1, 0);
            g_status_N = false;
            g_status_Z = true;
            g_status_V = false;
            g_status_C = false;
        }
        private void analyse_CLR_l()
        {
            if(g_op3 <= 1){
                g_clock += 6;
            }else{
                g_clock += 14;
            }
            g_reg_PC += 2;
            adressing_func_address(g_op3, g_op4, 2);
            adressing_func_write(g_op3, g_op4, 2, 0);
            g_status_N = false;
            g_status_Z = true;
            g_status_V = false;
            g_status_C = false;
        }
   }
}
