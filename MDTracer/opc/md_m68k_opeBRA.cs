using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_BRA_w()
        {
           g_clock += 10;
           g_reg_PC += 2;
           g_reg_PC = (uint)(g_reg_PC + (short)md_main.g_md_bus.read16(g_reg_PC));
        }
        private void analyse_BRA_b()
        {
           g_clock += 10;
           g_reg_PC += 2;
           g_work_data.b0 = (byte)(g_opcode & 0x00ff);
           g_reg_PC = (uint)(g_reg_PC + (sbyte)g_work_data.b0);
        }
   }
}
