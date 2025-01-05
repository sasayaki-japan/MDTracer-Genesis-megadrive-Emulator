using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_TRAPV()
        {
            g_reg_PC += 2;
            if(g_status_V == true) g_reg_PC = md_main.g_md_bus.read32(28);
            g_clock += 37;
        }
   }
}
