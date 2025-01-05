using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_ADDA()
        {
            g_reg_PC += 2;
            int w_size = (g_op2 >> 2) + 1;
            g_clock = (w_size == 1) ? 8 : 6;
            g_work_val1.l = g_reg_addr[g_op1].l;
            adressing_func_address(g_op3, g_op4, w_size);
            g_work_val2.l = adressing_func_read(g_op3, g_op4, w_size);
            g_work_val2.l = get_int_cast(g_work_val2.l, w_size);
            g_work_data.l = g_work_val1.l  +  g_work_val2.l;
            g_reg_addr[g_op1].l = g_work_data.l;
        }
   }
}
