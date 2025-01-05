using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_EXG_0()
        {
            g_reg_PC += 2;
            g_work_data.l = g_reg_data[g_op1].l;
            g_reg_data[g_op1].l = g_reg_data[g_op4].l;
            g_reg_data[g_op4].l = g_work_data.l;
           g_clock += 6;
        }
        private void analyse_EXG_1()
        {
            g_reg_PC += 2;
            g_work_data.l = g_reg_addr[g_op1].l;
            g_reg_addr[g_op1].l = g_reg_addr[g_op4].l;
            g_reg_addr[g_op4].l = g_work_data.l;
           g_clock += 6;
        }
        private void analyse_EXG_2()
        {
            g_reg_PC += 2;
            g_work_data.l = g_reg_data[g_op1].l;
            g_reg_data[g_op1].l = g_reg_addr[g_op4].l;
            g_reg_addr[g_op4].l = g_work_data.l;
           g_clock += 6;
        }
   }
}
