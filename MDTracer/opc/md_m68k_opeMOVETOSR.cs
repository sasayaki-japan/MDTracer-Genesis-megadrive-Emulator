using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_MOVETOSR()
        {
            g_clock += 12;
            g_reg_PC += 2;
            adressing_func_address(g_op3, g_op4, 1);
            g_work_data.w = (ushort)adressing_func_read(g_op3, g_op4, 1);
            g_reg_SR = g_work_data.w;
        }
   }
}
