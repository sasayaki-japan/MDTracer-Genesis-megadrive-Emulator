using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_MOVEFROMSR()
        {
            if(g_op3 <= 1) g_clock += 6; else g_clock += 9;
            g_reg_PC += 2;
            g_work_data.w = g_reg_SR;
            adressing_func_address(g_op3, g_op4, 1);
            adressing_func_write(g_op3, g_op4, 1, g_work_data.w);
        }
   }
}
