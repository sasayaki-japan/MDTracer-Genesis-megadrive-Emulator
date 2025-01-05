using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_BSR_w()
        {
            g_clock += 20;
            uint w_pc = g_reg_PC;
            g_reg_PC += 2;
            uint w_start_address = (uint)(g_reg_PC + (short)md_main.g_md_bus.read16(g_reg_PC));
            stack_push32(g_reg_PC + 2);
            md_main.g_form_code_trace.CPU_Trace_push(Form_Code_Trace.STACK_LIST_TYPE.BSR, w_pc, w_start_address, g_reg_PC + 2, g_reg_addr[7].l);
            g_reg_PC = w_start_address;
        }
        private void analyse_BSR_b()
        {
            g_clock += 20;
            uint w_pc = g_reg_PC;
            g_reg_PC += 2;
            g_work_data.b0 = (byte)(g_opcode & 0x00ff);
            uint w_start_address = (uint)(g_reg_PC + (sbyte)g_work_data.b0);
            stack_push32(g_reg_PC);
            md_main.g_form_code_trace.CPU_Trace_push(Form_Code_Trace.STACK_LIST_TYPE.BSR, w_pc, w_start_address, g_reg_PC, g_reg_addr[7].l);
            g_reg_PC = w_start_address;
        }
   }
}
