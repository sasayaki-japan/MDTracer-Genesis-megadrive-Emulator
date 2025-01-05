using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_Scc()
        {
            g_reg_PC += 2;
            if(g_flag_chack[(g_opcode >> 8) & 0x0f]()){
                g_work_data.b0 = 0xff;
                if(g_op3 <= 1) g_clock += 6; else g_clock += 9;
            }else{
                g_work_data.b0 = 0;
                if(g_op3 <= 1) g_clock += 4; else g_clock += 9;
            };
            adressing_func_address(g_op3, g_op4, 0);
            adressing_func_write(g_op3, g_op4, 0, g_work_data.b0);
        }
   }
}
