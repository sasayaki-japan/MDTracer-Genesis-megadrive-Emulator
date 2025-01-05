using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_JSR()
        {
            g_clock += 14;
            uint w_pc = g_reg_PC;
            g_reg_PC += 2;
            adressing_func_address(g_op3, g_op4, 2);
            stack_push32(g_reg_PC);
            md_main.g_form_code_trace.CPU_Trace_push(Form_Code_Trace.STACK_LIST_TYPE.JSR, w_pc, g_analyze_address, g_reg_PC, g_reg_addr[7].l);
            g_reg_PC = g_analyze_address;
        }
   }
}
