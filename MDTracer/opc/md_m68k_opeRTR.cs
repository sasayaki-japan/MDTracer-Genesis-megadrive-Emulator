using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_RTR()
        {
            g_clock += 20;
            uint w_pc = g_reg_PC;
            g_reg_SR = stack_pop16();
            g_reg_PC = stack_pop32();
            md_main.g_form_code_trace.CPU_Trace_pop(g_reg_PC, w_pc, g_reg_addr[7].l);
        }
   }
}
