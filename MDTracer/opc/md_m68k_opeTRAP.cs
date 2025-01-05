using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_TRAP()
        {
            g_clock += 37;
            uint w_pc = g_reg_PC;
            g_reg_PC += 2;
            uint w_start_address = md_main.g_md_bus.read32((uint)(0x0080 + ((g_opcode & 0x0f) << 2)));
            stack_push32(g_reg_PC);
            md_main.g_form_code_trace.CPU_Trace_push(Form_Code_Trace.STACK_LIST_TYPE.TRAP, w_pc, w_start_address, g_reg_PC, g_reg_addr[7].l);
            stack_push16(g_reg_SR);
            g_reg_PC = w_start_address;
        }
   }
}
