using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_NBCD()
        {
            if(g_op3 <= 1) g_clock += 6; else g_clock += 9;
            g_reg_PC += 2;
            adressing_func_address(g_op3, g_op4, 0);
            g_work_data.b0 = (byte)adressing_func_read(g_op3, g_op4, 0);
            int wkekka1 = 10 - (g_work_data.b0 & 0xf);
            if (g_status_X == true) wkekka1 -= 1;
            if(wkekka1 < 10) g_status_C = true;
            else { g_status_C = false; wkekka1 = 0; }
            int wkekka2 = 10 - ((g_work_data.b0 >> 4) & 0xf);
            if (g_status_C == true) wkekka2 -= 1;
            if(wkekka2 < 10) g_status_C = true;
            else { g_status_C = false; wkekka2 = 0; }
            g_work_data.b0 = (byte)((wkekka2 << 4) + wkekka1);
            adressing_func_write(g_op3, g_op4, 0, g_work_data.b0);
            if (g_work_data.b0 != 0) g_status_Z = false;
        }
   }
}
