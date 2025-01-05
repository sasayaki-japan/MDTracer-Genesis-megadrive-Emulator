using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_SBCD_0()
        {
           g_reg_PC += 2;
           g_work_val1.b0 = g_reg_data[g_op1].b0;
           g_work_val2.b0 = g_reg_data[g_op4].b0;
           g_clock += 6;
           int wkekka1 = (g_work_val1.b0 & 0xf) - (g_work_val2.b0 & 0xf);
           if (g_status_X == true) wkekka1 -= 1;
           if(wkekka1 < 0) { wkekka1 += 10; g_status_C = true; }
           else g_status_C = false;
           int wkekka2 = ((g_work_val1.b0 >> 4) & 0xf) - ((g_work_val2.b0 >> 4) & 0xf);
           if (g_status_C == true) wkekka2 -= 1;
           if(wkekka2 < 0) { wkekka2 += 10; g_status_C = true; }
           else g_status_C = false;
           g_work_data.b0 = (byte)((wkekka2 << 4) + wkekka1);
           g_reg_data[g_op1].b0 = g_work_data.b0;
           if (g_work_data.b0 != 0) g_status_Z = false;
        }
        private void analyse_SBCD_1()
        {
           g_reg_PC += 2;
           g_reg_addr[g_op1].l -= 1;
           g_work_val1.b0 = md_main.g_md_bus.read8(g_reg_addr[g_op1].l);
           g_reg_addr[g_op4].l -= 1;
           g_work_val2.b0 = md_main.g_md_bus.read8(g_reg_addr[g_op4].l);
           g_clock += 19;
           int wkekka1 = (g_work_val1.b0 & 0xf) - (g_work_val2.b0 & 0xf);
           if (g_status_X == true) wkekka1 -= 1;
           if(wkekka1 < 0) { wkekka1 += 10; g_status_C = true; }
           else g_status_C = false;
           int wkekka2 = ((g_work_val1.b0 >> 4) & 0xf) - ((g_work_val2.b0 >> 4) & 0xf);
           if (g_status_C == true) wkekka2 -= 1;
           if(wkekka2 < 0) { wkekka2 += 10; g_status_C = true; }
           else g_status_C = false;
           g_work_data.b0 = (byte)((wkekka2 << 4) + wkekka1);
           md_main.g_md_bus.write8(g_reg_addr[g_op1].l, g_work_data.b0);
           if (g_work_data.b0 != 0) g_status_Z = false;
        }
   }
}
