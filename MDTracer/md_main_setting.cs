using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Xml.Linq;

namespace MDTracer
{
    internal partial class md_main
    {
        public static List<string> g_setting_name;
        public static List<string> g_setting_val;

        public static Configuration g_config;
        public static int g_tvmode_req;
        public static int g_gpu_req;

        public static void read_setting()
        {
            g_setting_name.Clear();
            g_setting_val.Clear();
            foreach (string key in System.Configuration.ConfigurationSettings.AppSettings.AllKeys)
            {
                g_setting_name.Add(key);
                g_setting_val.Add(System.Configuration.ConfigurationSettings.AppSettings[key]);
            }
            if(g_setting_name.Count == 0)
            {
                read_init();
                return;
            }
            for (int i = 0; i < g_setting_name.Count; i++)
            {
                string w_name = g_setting_name[i];
                string[] w_val = g_setting_val[i].Split(':');
                switch(w_name)
                {
                    case "key":
                        for (int j = 0; j < g_md_io.KEY_ALLCATION_NUM; j++)
                        {
                            g_md_io.g_key_allocation[j] = byte.Parse(w_val[j]);
                        }
                        break;
                    case "joyname":
                        g_md_io.g_joy_name = w_val[0];
                        break;
                    case "joy":
                        for (int j = 0; j < g_md_io.KEY_ALLCATION_NUM; j++)
                        {
                            g_md_io.g_joy_allocation[j] = byte.Parse(w_val[j]);
                        }
                        break;
                    case "screen_main":
                        Form_Main.g_screen_xpos = int.Parse(w_val[0]);
                        Form_Main.g_screen_ypos = int.Parse(w_val[1]);
                        Form_Main.g_screen_size_x = int.Parse(w_val[2]);
                        Form_Main.g_screen_size_y = int.Parse(w_val[3]);
                        break;
                    case "screenA":
                        g_screenA_enable = (w_val[0] == "1") ? true : false;
                        g_form_screenA.g_screen_xpos = int.Parse(w_val[1]);
                        g_form_screenA.g_screen_ypos = int.Parse(w_val[2]);
                        break;
                    case "screenB":
                        g_screenB_enable = (w_val[0] == "1") ? true : false;
                        g_form_screenB.g_screen_xpos = int.Parse(w_val[1]);
                        g_form_screenB.g_screen_ypos = int.Parse(w_val[2]);
                        break;
                    case "screenW":
                        g_screenW_enable = (w_val[0] == "1") ? true : false;
                        g_form_screenW.g_screen_xpos = int.Parse(w_val[1]);
                        g_form_screenW.g_screen_ypos = int.Parse(w_val[2]);
                        break;
                    case "screenS":
                        g_screenS_enable = (w_val[0] == "1") ? true : false;
                        g_form_screenS.g_screen_xpos = int.Parse(w_val[1]);
                        g_form_screenS.g_screen_ypos = int.Parse(w_val[2]);
                        break;
                    case "pattern":
                        g_pattern_enable = (w_val[0] == "1") ? true : false;
                        g_form_pattern.g_screen_xpos = int.Parse(w_val[1]);
                        g_form_pattern.g_screen_ypos = int.Parse(w_val[2]);
                        break;
                    case "pallete":
                        g_pallete_enable = (w_val[0] == "1") ? true : false;
                        g_form_pallete.g_screen_xpos = int.Parse(w_val[1]);
                        g_form_pallete.g_screen_ypos = int.Parse(w_val[2]);
                        break;
                    case "code":
                        g_code_enable = (w_val[0] == "1") ? true : false;
                        g_form_code.g_screen_xpos = int.Parse(w_val[1]);
                        g_form_code.g_screen_ypos = int.Parse(w_val[2]);
                        break;
                    case "io":
                        g_io_enable = (w_val[0] == "1") ? true : false;
                        g_form_io.g_screen_xpos = int.Parse(w_val[1]);
                        g_form_io.g_screen_ypos = int.Parse(w_val[2]);
                        break;
                    case "music":
                        g_music_enable = (w_val[0] == "1") ? true : false;
                        g_form_music.g_screen_xpos = int.Parse(w_val[1]);
                        g_form_music.g_screen_ypos = int.Parse(w_val[2]);
                        break;
                    case "registry":
                        g_registry_enable = (w_val[0] == "1") ? true : false;
                        g_form_registry.g_screen_xpos = int.Parse(w_val[1]);
                        g_form_registry.g_screen_ypos = int.Parse(w_val[2]);
                        break;
                    case "flow":
                        g_flow_enable = (w_val[0] == "1") ? true : false;
                        g_form_flow.g_screen_xpos = int.Parse(w_val[1]);
                        g_form_flow.g_screen_ypos = int.Parse(w_val[2]);
                        break;
                    case "trace_fsb":
                        g_trace_fsb = (w_val[0] == "1") ? true : false;
                        break;
                    case "trace_sip":
                        g_trace_sip = (w_val[0] == "1") ? true : false;
                        break;
                    case "music_chk":
                        for (int j = 0; j <= 10; j++)
                        {
                            g_md_music.g_master_chk[j] = (w_val[j] == "1") ? true : false;
                        }
                        break;
                    case "music_vol":
                        for (int j = 0; j <= 10; j++)
                        {
                            g_md_music.g_master_vol[j] = int.Parse(w_val[j]);
                        }
                        break;
                    case "vdp_tvmode":
                        g_tvmode_req = int.Parse(w_val[0]);
                        g_md_vdp.g_vdp_status_0_tvmode = (byte)g_tvmode_req;
                        break;
                    case "vdp_gpu":
                        g_gpu_req = int.Parse(w_val[0]);
                        g_md_vdp.rendering_gpu = (w_val[0] == "1") ? true : false;
                        break;
                    case "file0": Form_Main.g_file_name[0] = g_setting_val[i]; break;
                    case "file1": Form_Main.g_file_name[1] = g_setting_val[i]; break;
                    case "file2": Form_Main.g_file_name[2] = g_setting_val[i]; break;
                    case "file3": Form_Main.g_file_name[3] = g_setting_val[i]; break;
                    case "file4": Form_Main.g_file_name[4] = g_setting_val[i]; break;
                    case "file5": Form_Main.g_file_name[5] = g_setting_val[i]; break;
                    case "file6": Form_Main.g_file_name[6] = g_setting_val[i]; break;
                    case "file7": Form_Main.g_file_name[7] = g_setting_val[i]; break;
                    case "file8": Form_Main.g_file_name[8] = g_setting_val[i]; break;
                }
            }
        }
        public static void read_init()
        {
            g_md_io.g_key_allocation[0] = 49;
            g_md_io.g_key_allocation[1] = 50;
            g_md_io.g_key_allocation[2] = 51;
            g_md_io.g_key_allocation[3] = 35;
            g_md_io.g_key_allocation[4] = 17;
            g_md_io.g_key_allocation[5] = 31;
            g_md_io.g_key_allocation[6] = 30;
            g_md_io.g_key_allocation[7] = 32;
            for (int j = 0; j <= 10; j++)
            {
                md_main.g_md_music.g_master_chk[j] = true;
            }
            for (int j = 0; j <= 10; j++)
            {
                md_main.g_md_music.g_master_vol[j] = 100;
            }
            Form_Main.g_screen_size_x = 640;
            Form_Main.g_screen_size_y = 448;
        }
        public static void write_setting()
        {
            g_config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string w_val = "";
            for (int i = 0; i < g_md_io.KEY_ALLCATION_NUM; i++)
            {
                w_val += g_md_io.g_key_allocation[i].ToString();
                if (i < g_md_io.KEY_ALLCATION_NUM - 1)
                {
                    w_val += ":";
                }
            }
            setting_add("key", w_val);
           
            setting_add("joyname", g_md_io.g_joy_name);
            w_val = "";
            for (int i = 0; i < g_md_io.KEY_ALLCATION_NUM; i++)
            {
                w_val += g_md_io.g_joy_allocation[i].ToString();
                if (i < g_md_io.KEY_ALLCATION_NUM - 1)
                {
                    w_val += ":";
                }
            }
            setting_add("joy", w_val);

            w_val = Form_Main.g_screen_xpos
                + ":" + Form_Main.g_screen_ypos
                + ":" + Form_Main.g_screen_size_x
                + ":" + Form_Main.g_screen_size_y;
            setting_add("screen_main", w_val);
            w_val = ((md_main.g_screenA_enable == true) ? "1" : "0")
                + ":" + g_form_screenA.g_screen_xpos
                + ":" + g_form_screenA.g_screen_ypos;
            setting_add("screenA", w_val);
            w_val = ((md_main.g_screenB_enable == true) ? "1" : "0")
                + ":" + g_form_screenB.g_screen_xpos
                + ":" + g_form_screenB.g_screen_ypos;
            setting_add("screenB", w_val);
            w_val = ((md_main.g_screenW_enable == true) ? "1" : "0")
                + ":" + g_form_screenW.g_screen_xpos
                + ":" + g_form_screenW.g_screen_ypos;
            setting_add("screenW", w_val);
            w_val = ((md_main.g_screenS_enable == true) ? "1" : "0")
                + ":" + g_form_screenS.g_screen_xpos
                + ":" + g_form_screenS.g_screen_ypos;
            setting_add("screenS", w_val);
            w_val = ((md_main.g_pattern_enable == true) ? "1" : "0")
                + ":" + g_form_pattern.g_screen_xpos
                + ":" + g_form_pattern.g_screen_ypos;
            setting_add("pattern", w_val);
            w_val = ((md_main.g_pallete_enable == true) ? "1" : "0")
                + ":" + g_form_pallete.g_screen_xpos
                + ":" + g_form_pallete.g_screen_ypos;
            setting_add("pallete", w_val);
            w_val = ((md_main.g_code_enable == true) ? "1" : "0")
                + ":" + g_form_code.g_screen_xpos
                + ":" + g_form_code.g_screen_ypos;
            setting_add("code", w_val);
            w_val = ((md_main.g_io_enable == true) ? "1" : "0")
                + ":" + g_form_io.g_screen_xpos
                + ":" + g_form_io.g_screen_ypos;
            setting_add("io", w_val);
            w_val = ((md_main.g_music_enable == true) ? "1" : "0")
                + ":" + g_form_music.g_screen_xpos
                + ":" + g_form_music.g_screen_ypos;
            setting_add("music", w_val);
            w_val = ((md_main.g_registry_enable == true) ? "1" : "0")
                + ":" + g_form_registry.g_screen_xpos
                + ":" + g_form_registry.g_screen_ypos;
            setting_add("registry", w_val);
            w_val = ((md_main.g_flow_enable == true) ? "1" : "0")
                + ":" + g_form_flow.g_screen_xpos
                + ":" + g_form_flow.g_screen_ypos;
            setting_add("flow", w_val);

            w_val = ((md_main.g_trace_fsb == true) ? "1" : "0");
            setting_add("trace_fsb", w_val);

            w_val = ((md_main.g_trace_sip == true) ? "1" : "0");
            setting_add("trace_sip", w_val);

            w_val = "";
            for (int j = 0; j < 11; j++)
            {
                w_val += ((md_main.g_md_music.g_master_chk[j] == true) ? "1" : "0");
                if (j < 10)
                {
                    w_val += ":";
                }
            }
            setting_add("music_chk", w_val);
            w_val = "";
            for (int j = 0; j <= 10; j++)
            {
                w_val += md_main.g_md_music.g_master_vol[j].ToString();
                if (j < 10)
                {
                    w_val += ":";
                }
            }
            setting_add("music_vol", w_val);
            setting_add("vdp_tvmode", g_tvmode_req.ToString());
            setting_add("vdp_gpu", g_gpu_req.ToString());
            w_val = "";
            for (int i = 0; i < 9; i++)
            {
                setting_add("file" + i, Form_Main.g_file_name[i]);
            }
            g_config.Save();
        }
        public static void setting_add(string in_name, string in_val)
        {
            if (g_config.AppSettings.Settings.AllKeys.Contains(in_name))
            {
                g_config.AppSettings.Settings[in_name].Value = in_val;
            }
            else
            {
                g_config.AppSettings.Settings.Add(in_name, in_val);
            }
        }
    }
}
