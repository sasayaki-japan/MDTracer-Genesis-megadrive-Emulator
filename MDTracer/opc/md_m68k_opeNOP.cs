using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_NOP()
        {
            g_reg_PC += 2;
            g_clock += 4;
        }
   }
}
