using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_MOVEA_w()
        {
            if((g_op2 <= 1)&&(g_op3 <=1)) g_clock += 4; else g_clock += 5;
            g_reg_PC += 2;
            adressing_func_address(g_op3, g_op4, 1);
            g_work_data.l = adressing_func_read(g_op3, g_op4, 1);
            g_reg_addr[g_op1].l = get_int_cast(g_work_data.w, 1);
        }
        private void analyse_MOVEA_l()
        {
            if((g_op2 <= 1)&&(g_op3 <=1)) g_clock += 4; else g_clock += 5;
            g_reg_PC += 2;
            adressing_func_address(g_op3, g_op4, 2);
            g_work_data.l = adressing_func_read(g_op3, g_op4, 2);
            g_reg_addr[g_op1].l = g_work_data.l;
        }
   }
}
