using System.Diagnostics;

namespace MDTracer
{
    internal partial class md_main
    {
        public static void initialize()
        {
            Process currentProcess = Process.GetCurrentProcess();
            int processId = currentProcess.Id;
            currentProcess.PriorityClass = ProcessPriorityClass.High;

            g_md_cartridge = new md_cartridge();
            g_md_bus = new md_bus();
            g_md_control = new md_control();
            g_md_io = new md_io();
            g_md_m68k = new md_m68k();
            g_md_z80 = new md_z80();
            g_md_vdp = new md_vdp();
            g_md_music = new md_music();
            
			g_form_setting = new Form_Setting();
            g_form_screenA = new Form_VDP_Screen();
            g_form_screenB = new Form_VDP_Screen();
            g_form_screenW = new Form_VDP_Screen();
            g_form_screenS = new Form_VDP_Screen();
            g_form_pattern = new Form_Pattern();
            g_form_pallete = new Form_Pallete();
            g_form_code_trace = new Form_Code_Trace();
            g_form_code = new Form_Code();
            g_form_io = new Form_IO();
            g_form_music = new Form_MUSIC();
            g_form_registry = new Form_Registry();
            g_form_flow = new Form_Flow();

            g_setting_name = new List<string>();
            g_setting_val = new List<string>();
            g_task_usage = 0;
        }
    }
}
