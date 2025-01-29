using static MDTracer.md_m68k;

namespace MDTracer
{
    public partial class Form_Registry : Form
    {
        public class ParamView
        {
            public string name { get; set; }
            public string value { get; set; }
        }
        private static List<ParamView> g_paramview_cpu;
        private static List<ParamView> g_paramview_vdp;
        public int g_screen_xpos;
        public int g_screen_ypos;

        public static string[] CPU_NAME = {
            "ProgramCounter",
            "register_D0",
            "register_D1",
            "register_D2",
            "register_D3",
            "register_D4",
            "register_D5",
            "register_D6",
            "register_D7",
            "register_A0",
            "register_A1",
            "register_A2",
            "register_A3",
            "register_A4",
            "register_A5",
            "register_A6",
            "register_A7",
            "status_T",
            "status_S",
            "status_interrupt",
            "status_X",
            "status_N",
            "status_Z",
            "status_V",
            "status_C"
        };
        public static string[] VDP_NAME = {
            "scanline",
            "status_9_empl",
            "status_8_full",
            "status_7_vinterrupt",
            "status_6_sprite",
            "status_5_collision",
            "status_4_frame",
            "status_3_vbrank",
            "status_2_hbrank",
            "status_1_dma",
            "status_0_tvmode",
            "hvcounter",
            "hvcounter_latched",
            "reg_0_4_hinterrupt",
            "reg_0_1_hvcounter",
            "reg_1_6_display",
            "reg_1_5_vinterrupt",
            "reg_1_4_dma",
            "reg_1_3_cellmode",
            "reg_2_scrolla",
            "reg_3_windows",
            "reg_4_scrollb",
            "reg_5_sprite",
            "reg_7_backcolor",
            "reg_10_hint",
            "reg_11_3_ext",
            "reg_11_2_vscroll",
            "reg_11_1_hscroll",
            "reg_12_7_cellmode1",
            "reg_12_3_shadow",
            "reg_12_2_interlacemode",
            "reg_12_0_cellmode2",
            "reg_13_hscroll",
            "reg_15_autoinc",
            "reg_16_5_scrollV",
            "reg_16_1_scrollH",
            "reg_17_7_windows",
            "reg_17_4_basspointer",
            "reg_18_7_windows",
            "reg_18_4_basspointer",
            "reg_19_dma_counter_low",
            "reg_20_dma_counter_high",
            "reg_21_dma_source_low",
            "reg_22_dma_source_mid",
            "reg_23_dma_mode",
            "reg_23_5_dma_high"
        };

        public class CallView
        {
            public string type { get; set; }
            public string caller { get; set; }
            public string call { get; set; }
        }
        public static List<CallView> g_paramview_call;
        //----------------------------------------------------------------
        //form
        //----------------------------------------------------------------
        public Form_Registry()
        {
            InitializeComponent();
            dataGridView_cpu.Font = new Font("Yu Gothic UI", 8);
            dataGridView_vdp.Font = new Font("Yu Gothic UI", 8);

            g_paramview_cpu = new List<ParamView>();
            for (int i = 0; i < CPU_NAME.Length; i++)
            {
                ParamView w_addval = new ParamView();
                w_addval.name = CPU_NAME[i];
                w_addval.value = "";
                g_paramview_cpu.Add(w_addval);
            }
            dataGridView_cpu.DataSource = g_paramview_cpu;

            g_paramview_vdp = new List<ParamView>();
            for (int i = 0; i < VDP_NAME.Length; i++)
            {
                ParamView w_addval = new ParamView();
                w_addval.name = VDP_NAME[i];
                w_addval.value = "";
                g_paramview_vdp.Add(w_addval);
            }
            dataGridView_vdp.DataSource = g_paramview_vdp;
                g_paramview_call = new List<CallView>(Form_Code_Trace.STACK_LIST_NUM);
                for (int i = 0; i < Form_Code_Trace.STACK_LIST_NUM; i++)
                {
                    g_paramview_call.Add(new CallView { type = "" });
                }
                dataGridView_call_stack.DataSource = g_paramview_call;
        }

        //----------------------------------------------------------------
        //Event Handling: Screen Operations
        //----------------------------------------------------------------
        private void Form_Registry_FormClosing(object sender, FormClosingEventArgs e)
        {
            md_main.g_registry_enable = false;
            md_main.g_form_setting.update();
            md_main.write_setting();
            e.Cancel = true;
        }
        private void Form_Registry_ResizeEnd(object sender, EventArgs e)
        {
            var currentPosition = this.Location;
            g_screen_xpos = currentPosition.X;
            g_screen_ypos = currentPosition.Y;
            md_main.write_setting();
        }
        private void Form_Registry_Shown(object sender, EventArgs e)
        {
            this.Location = new System.Drawing.Point(g_screen_xpos, g_screen_ypos);
        }
        //----------------------------------------------------------------
        //Event Handling: Painting
        //----------------------------------------------------------------
        private void Form_Registry_Paint(object sender, PaintEventArgs e)
        {
            if (md_main.g_form_code_trace.g_cpu_pause == true)
            {
                g_paramview_cpu[0].value = md_main.g_md_m68k.g_reg_PC.ToString("x6");
                g_paramview_cpu[1].value = md_main.g_md_m68k.g_reg_data[0].l.ToString("x6");
                g_paramview_cpu[2].value = md_main.g_md_m68k.g_reg_data[1].l.ToString("x8");
                g_paramview_cpu[3].value = md_main.g_md_m68k.g_reg_data[2].l.ToString("x8");
                g_paramview_cpu[4].value = md_main.g_md_m68k.g_reg_data[3].l.ToString("x8");
                g_paramview_cpu[5].value = md_main.g_md_m68k.g_reg_data[4].l.ToString("x8");
                g_paramview_cpu[6].value = md_main.g_md_m68k.g_reg_data[5].l.ToString("x8");
                g_paramview_cpu[7].value = md_main.g_md_m68k.g_reg_data[6].l.ToString("x8");
                g_paramview_cpu[8].value = md_main.g_md_m68k.g_reg_data[7].l.ToString("x8");
                g_paramview_cpu[9].value = md_main.g_md_m68k.g_reg_addr[0].l.ToString("x8");
                g_paramview_cpu[10].value = md_main.g_md_m68k.g_reg_addr[1].l.ToString("x8");
                g_paramview_cpu[11].value = md_main.g_md_m68k.g_reg_addr[2].l.ToString("x8");
                g_paramview_cpu[12].value = md_main.g_md_m68k.g_reg_addr[3].l.ToString("x8");
                g_paramview_cpu[13].value = md_main.g_md_m68k.g_reg_addr[4].l.ToString("x8");
                g_paramview_cpu[14].value = md_main.g_md_m68k.g_reg_addr[5].l.ToString("x8");
                g_paramview_cpu[15].value = md_main.g_md_m68k.g_reg_addr[6].l.ToString("x8");
                g_paramview_cpu[16].value = md_main.g_md_m68k.g_reg_addr[7].l.ToString("x8");
                g_paramview_cpu[17].value = md_main.g_md_m68k.g_status_T.ToString();
                g_paramview_cpu[18].value = md_main.g_md_m68k.g_status_S.ToString();
                g_paramview_cpu[19].value = md_main.g_md_m68k.g_status_interrupt_mask.ToString();
                g_paramview_cpu[20].value = md_main.g_md_m68k.g_status_X.ToString();
                g_paramview_cpu[21].value = md_main.g_md_m68k.g_status_N.ToString();
                g_paramview_cpu[22].value = md_main.g_md_m68k.g_status_Z.ToString();
                g_paramview_cpu[23].value = md_main.g_md_m68k.g_status_V.ToString();
                g_paramview_cpu[24].value = md_main.g_md_m68k.g_status_C.ToString();

                g_paramview_vdp[0].value = md_main.g_md_vdp.g_scanline.ToString();
                g_paramview_vdp[1].value = md_main.g_md_vdp.g_vdp_status_9_empl.ToString();
                g_paramview_vdp[2].value = md_main.g_md_vdp.g_vdp_status_8_full.ToString();
                g_paramview_vdp[3].value = md_main.g_md_vdp.g_vdp_status_7_vinterrupt.ToString();
                g_paramview_vdp[4].value = md_main.g_md_vdp.g_vdp_status_6_sprite.ToString();
                g_paramview_vdp[5].value = md_main.g_md_vdp.g_vdp_status_5_collision.ToString();
                g_paramview_vdp[6].value = md_main.g_md_vdp.g_vdp_status_4_frame.ToString();
                g_paramview_vdp[7].value = md_main.g_md_vdp.g_vdp_status_3_vbrank.ToString();
                g_paramview_vdp[8].value = md_main.g_md_vdp.g_vdp_status_2_hbrank.ToString();
                g_paramview_vdp[9].value = md_main.g_md_vdp.g_vdp_status_1_dma.ToString();
                g_paramview_vdp[10].value = md_main.g_md_vdp.g_vdp_status_0_tvmode.ToString();
                g_paramview_vdp[11].value = md_main.g_md_vdp.g_vdp_c00008_hvcounter.ToString();
                g_paramview_vdp[12].value = md_main.g_md_vdp.g_vdp_c00008_hvcounter_latched.ToString();
                g_paramview_vdp[13].value = md_main.g_md_vdp.g_vdp_reg_0_4_hinterrupt.ToString();
                g_paramview_vdp[14].value = md_main.g_md_vdp.g_vdp_reg_0_1_hvcounter.ToString();
                g_paramview_vdp[15].value = md_main.g_md_vdp.g_vdp_reg_1_6_display.ToString();
                g_paramview_vdp[16].value = md_main.g_md_vdp.g_vdp_reg_1_5_vinterrupt.ToString();
                g_paramview_vdp[17].value = md_main.g_md_vdp.g_vdp_reg_1_4_dma.ToString();
                g_paramview_vdp[18].value = md_main.g_md_vdp.g_vdp_reg_1_3_cellmode.ToString();
                g_paramview_vdp[19].value = md_main.g_md_vdp.g_vdp_reg_2_scrolla.ToString("x4");
                g_paramview_vdp[20].value = md_main.g_md_vdp.g_vdp_reg_3_windows.ToString("x4");
                g_paramview_vdp[21].value = md_main.g_md_vdp.g_vdp_reg_4_scrollb.ToString("x4");
                g_paramview_vdp[22].value = md_main.g_md_vdp.g_vdp_reg_5_sprite.ToString("x4");
                g_paramview_vdp[23].value = md_main.g_md_vdp.g_vdp_reg_7_backcolor.ToString();
                g_paramview_vdp[24].value = md_main.g_md_vdp.g_vdp_reg_10_hint.ToString();
                g_paramview_vdp[25].value = md_main.g_md_vdp.g_vdp_reg_11_3_ext.ToString();
                g_paramview_vdp[26].value = md_main.g_md_vdp.g_vdp_reg_11_2_vscroll.ToString();
                g_paramview_vdp[27].value = md_main.g_md_vdp.g_vdp_reg_11_1_hscroll.ToString();
                g_paramview_vdp[28].value = md_main.g_md_vdp.g_vdp_reg_12_7_cellmode1.ToString();
                g_paramview_vdp[29].value = md_main.g_md_vdp.g_vdp_reg_12_3_shadow.ToString();
                g_paramview_vdp[30].value = md_main.g_md_vdp.g_vdp_reg_12_2_interlacemode.ToString();
                g_paramview_vdp[31].value = md_main.g_md_vdp.g_vdp_reg_12_0_cellmode2.ToString();
                g_paramview_vdp[32].value = md_main.g_md_vdp.g_vdp_reg_13_hscroll.ToString("x4");
                g_paramview_vdp[33].value = md_main.g_md_vdp.g_vdp_reg_15_autoinc.ToString();
                g_paramview_vdp[34].value = md_main.g_md_vdp.g_vdp_reg_16_5_scrollV.ToString();
                g_paramview_vdp[35].value = md_main.g_md_vdp.g_vdp_reg_16_1_scrollH.ToString();
                g_paramview_vdp[36].value = md_main.g_md_vdp.g_vdp_reg_17_7_windows.ToString();
                g_paramview_vdp[37].value = md_main.g_md_vdp.g_vdp_reg_17_4_basspointer.ToString();
                g_paramview_vdp[38].value = md_main.g_md_vdp.g_vdp_reg_18_7_windows.ToString();
                g_paramview_vdp[39].value = md_main.g_md_vdp.g_vdp_reg_18_4_basspointer.ToString();
                g_paramview_vdp[40].value = md_main.g_md_vdp.g_vdp_reg_19_dma_counter_low.ToString();
                g_paramview_vdp[41].value = md_main.g_md_vdp.g_vdp_reg_20_dma_counter_high.ToString();
                g_paramview_vdp[42].value = md_main.g_md_vdp.g_vdp_reg_21_dma_source_low.ToString();
                g_paramview_vdp[43].value = md_main.g_md_vdp.g_vdp_reg_22_dma_source_mid.ToString();
                g_paramview_vdp[44].value = md_main.g_md_vdp.g_vdp_reg_23_dma_mode.ToString();
                g_paramview_vdp[45].value = md_main.g_md_vdp.g_vdp_reg_23_5_dma_high.ToString();

                for (int i = 0; i < Form_Code_Trace.STACK_LIST_NUM; i++)
                {
                    g_paramview_call[i].caller = "";
                    g_paramview_call[i].call = "";
                }
                for (int i = 1; i <= md_main.g_form_code_trace.g_stack_cur; i++)
                {
                    Form_Code_Trace.STACK_LIST w_stack = md_main.g_form_code_trace.g_stack_list[i];
                    g_paramview_call[i - 1].type = md_main.g_form_code_trace.STACK_LIST_TYPE_STR[(int)w_stack.type];
                    g_paramview_call[i - 1].caller = w_stack.caller_address.ToString("X8");
                    g_paramview_call[i - 1].call = w_stack.start_address.ToString("X8");
                }

                dataGridView_cpu.Invalidate();
                dataGridView_vdp.Invalidate();
                dataGridView_call_stack.Invalidate();
            }
        }
    }
}
