using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_RTE()
        {
            g_clock += 20;
            uint w_pc = g_reg_PC;
            md_main.g_md_vdp.g_vdp_status_7_vinterrupt = 0;
            if (md_main.g_md_m68k.g_interrupt_H_act == true) md_main.g_md_m68k.g_interrupt_H_act = false;
            else if (md_main.g_md_m68k.g_interrupt_V_act == true) md_main.g_md_m68k.g_interrupt_V_act = false;
            else md_main.g_md_m68k.g_interrupt_EXT_act = false;
            g_reg_SR = stack_pop16();
            g_reg_PC = stack_pop32();
            md_main.g_form_code_trace.CPU_Trace_pop(g_reg_PC, w_pc, g_reg_addr[7].l);
        }
   }
}
