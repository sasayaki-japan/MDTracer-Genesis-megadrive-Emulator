using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_Bcc_w()
        {
            g_clock += 10;
            g_reg_PC += 2;
           uint w_next_pc_work = (uint)(g_reg_PC + (short)md_main.g_md_bus.read16(g_reg_PC));
           g_reg_PC += 2;
           if(g_flag_chack[(g_opcode >> 8) & 0x0f]()) g_reg_PC = w_next_pc_work;
        }
        private void analyse_Bcc_b()
        {
            g_clock += 10;
            g_reg_PC += 2;
           uint w_next_pc_work = (uint)(g_reg_PC + (sbyte)(g_opcode & 0x00ff));
           if(g_flag_chack[(g_opcode >> 8) & 0x0f]()) g_reg_PC = w_next_pc_work;
        }
   }
}
