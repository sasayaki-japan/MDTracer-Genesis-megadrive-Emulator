using MDTracer;

namespace MDTracer
{
    public partial class Form_Setting : Form
    {
        //----------------------------------------------------------------
        //form
        //----------------------------------------------------------------
        public Form_Setting()
        {
            InitializeComponent();
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }

        //----------------------------------------------------------------
        //Event Handling: Screen Operations
        //----------------------------------------------------------------
        private void Form_Setting_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
        private void comboBox_videoformat_SelectedIndexChanged(object sender, EventArgs e)
        {
            md_main.g_tvmode_req = comboBox_videoformat.SelectedIndex;
            md_main.write_setting();
        }
        private void comboBox_rendering_SelectedIndexChanged(object sender, EventArgs e)
        {
            md_main.g_gpu_req = comboBox_rendering.SelectedIndex;
            md_main.write_setting();
        }
        private void checkBox_screenA_CheckedChanged(object sender, EventArgs e)
        {
            md_main.g_screenA_enable = checkBox_screenA.Checked;
            show_window();
            md_main.write_setting();
        }

        private void checkBox_screenB_CheckedChanged(object sender, EventArgs e)
        {
            md_main.g_screenB_enable = checkBox_screenB.Checked;
            show_window();
            md_main.write_setting();
        }

        private void checkBox_screenW_CheckedChanged(object sender, EventArgs e)
        {
            md_main.g_screenW_enable = checkBox_screenW.Checked;
            show_window();
            md_main.write_setting();
        }

        private void checkBox_screenS_CheckedChanged(object sender, EventArgs e)
        {
            md_main.g_screenS_enable = checkBox_screenS.Checked;
            show_window();
            md_main.write_setting();
        }

        private void checkBox_pattern_CheckedChanged(object sender, EventArgs e)
        {
            md_main.g_pattern_enable = checkBox_pattern.Checked;
            show_window();
            md_main.write_setting();
        }

        private void checkBox_pallete_CheckedChanged(object sender, EventArgs e)
        {
            md_main.g_pallete_enable = checkBox_pallete.Checked;
            show_window();
            md_main.write_setting();
        }

        private void checkBox_code_CheckedChanged(object sender, EventArgs e)
        {
            md_main.g_code_enable = checkBox_code.Checked;
            show_window();
            md_main.write_setting();
        }

        private void checkBox_io_CheckedChanged(object sender, EventArgs e)
        {
            md_main.g_io_enable = checkBox_io.Checked;
            show_window();
            md_main.write_setting();
        }

        private void checkBox_music_CheckedChanged(object sender, EventArgs e)
        {
            md_main.g_music_enable = checkBox_music.Checked;
            show_window();
            md_main.write_setting();
        }

        private void checkBox_register_CheckedChanged(object sender, EventArgs e)
        {
            md_main.g_registry_enable = checkBox_register.Checked;
            show_window();
            md_main.write_setting();
        }

        private void checkBox_flow_CheckedChanged(object sender, EventArgs e)
        {
            md_main.g_flow_enable = checkBox_flow.Checked;
            show_window();
            md_main.write_setting();
        }

        private void checkBox_fsb_CheckedChanged(object sender, EventArgs e)
        {
            md_main.g_trace_fsb = checkBox_fsb.Checked;
            md_main.write_setting();
        }

        private void checkBox_sip_CheckedChanged(object sender, EventArgs e)
        {
            md_main.g_trace_sip = checkBox_sip.Checked;
            md_main.write_setting();
        }
        //----------------------------------------------------------------
        //sub function
        //----------------------------------------------------------------
        public void update()
        {
            checkBox_screenA.Checked = md_main.g_screenA_enable;
            checkBox_screenB.Checked = md_main.g_screenB_enable;
            checkBox_screenW.Checked = md_main.g_screenW_enable;
            checkBox_screenS.Checked = md_main.g_screenS_enable;
            checkBox_pattern.Checked = md_main.g_pattern_enable;
            checkBox_pallete.Checked = md_main.g_pallete_enable;
            checkBox_code.Checked = md_main.g_code_enable;
            checkBox_io.Checked = md_main.g_io_enable;
            checkBox_music.Checked = md_main.g_music_enable;
            checkBox_register.Checked = md_main.g_registry_enable;
            checkBox_flow.Checked = md_main.g_flow_enable;
            checkBox_fsb.Checked = md_main.g_trace_fsb;
            checkBox_sip.Checked = md_main.g_trace_sip;
            comboBox_videoformat.SelectedIndex = md_main.g_md_vdp.g_vdp_status_0_tvmode;
            comboBox_rendering.SelectedIndex = (md_main.g_md_vdp.rendering_gpu == false) ? 0 : 1;
            show_window();
        }
        public void show_window()
        {
            if (md_main.g_screenA_enable == true) { md_main.g_form_screenA.Show(); } else { md_main.g_form_screenA.Hide(); }
            if (md_main.g_screenB_enable == true) { md_main.g_form_screenB.Show(); } else { md_main.g_form_screenB.Hide(); }
            if (md_main.g_screenW_enable == true) { md_main.g_form_screenW.Show(); } else { md_main.g_form_screenW.Hide(); }
            if (md_main.g_screenS_enable == true) { md_main.g_form_screenS.Show(); } else { md_main.g_form_screenS.Hide(); }
            if (md_main.g_pattern_enable == true) { md_main.g_form_pattern.Show(); } else { md_main.g_form_pattern.Hide(); }
            if (md_main.g_pallete_enable == true) { md_main.g_form_pallete.Show(); } else { md_main.g_form_pallete.Hide(); }
            if (md_main.g_code_enable == true) { md_main.g_form_code.Show(); } else { md_main.g_form_code.Hide(); }
            if (md_main.g_io_enable == true) { md_main.g_form_io.Show(); } else { md_main.g_form_io.Hide(); }
            if (md_main.g_music_enable == true) { md_main.g_form_music.Show(); } else { md_main.g_form_music.Hide(); }
            if (md_main.g_registry_enable == true) { md_main.g_form_registry.Show(); } else { md_main.g_form_registry.Hide(); }
            if (md_main.g_flow_enable == true) { md_main.g_form_flow.Show(); } else { md_main.g_form_flow.Hide(); }
        }
    }
}
