using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_DBcc()
        {
           g_clock += 12;
           g_reg_PC += 2;
           uint w_next_pc_work = (uint)(g_reg_PC +(short)md_main.g_md_bus.read16(g_reg_PC));
           g_reg_PC += 2;
            if(g_flag_chack[(g_opcode >> 8) & 0x0f]()) { }
            else {
                g_reg_data[g_op4].w -= 1;
                if((short)g_reg_data[g_op4].w != -1) g_reg_PC = w_next_pc_work;
            }
        }
   }
}
