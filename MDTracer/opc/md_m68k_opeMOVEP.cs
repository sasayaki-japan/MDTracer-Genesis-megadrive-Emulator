using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_MOVEP_4()
        {
            g_reg_PC += 2;
            ushort w_ext = md_main.g_md_bus.read16(g_reg_PC);
            g_reg_PC += 2;
            g_reg_data[g_op1].b1 = md_main.g_md_bus.read8(g_reg_addr[g_op4].l + w_ext);
            g_reg_data[g_op1].b0 = md_main.g_md_bus.read8(g_reg_addr[g_op4].l + w_ext + 2);
            g_clock = 16;
        }
        private void analyse_MOVEP_5()
        {
            g_reg_PC += 2;
            ushort w_ext = md_main.g_md_bus.read16(g_reg_PC);
            g_reg_PC += 2;
            g_reg_data[g_op1].b3 = md_main.g_md_bus.read8(g_reg_addr[g_op4].l + w_ext);
            g_reg_data[g_op1].b2 = md_main.g_md_bus.read8(g_reg_addr[g_op4].l + w_ext + 2);
            g_reg_data[g_op1].b1 = md_main.g_md_bus.read8(g_reg_addr[g_op4].l + w_ext + 4);
            g_reg_data[g_op1].b0 = md_main.g_md_bus.read8(g_reg_addr[g_op4].l + w_ext + 6);
            g_clock = 24;
        }
        private void analyse_MOVEP_6()
        {
            g_reg_PC += 2;
            ushort w_ext = md_main.g_md_bus.read16(g_reg_PC);
            g_reg_PC += 2;
            md_main.g_md_bus.write8(g_reg_addr[g_op4].l + w_ext, g_reg_data[g_op1].b1);
            md_main.g_md_bus.write8(g_reg_addr[g_op4].l + w_ext + 2, g_reg_data[g_op1].b0);
            g_clock = 18;
        }
        private void analyse_MOVEP_7()
        {
            g_reg_PC += 2;
            ushort w_ext = md_main.g_md_bus.read16(g_reg_PC);
            g_reg_PC += 2;
            md_main.g_md_bus.write8(g_reg_addr[g_op4].l + w_ext, g_reg_data[g_op1].b3);
            md_main.g_md_bus.write8(g_reg_addr[g_op4].l + w_ext + 2, g_reg_data[g_op1].b2);
            md_main.g_md_bus.write8(g_reg_addr[g_op4].l + w_ext + 4, g_reg_data[g_op1].b1);
            md_main.g_md_bus.write8(g_reg_addr[g_op4].l + w_ext + 6, g_reg_data[g_op1].b0);
            g_clock = 28;
        }
   }
}
