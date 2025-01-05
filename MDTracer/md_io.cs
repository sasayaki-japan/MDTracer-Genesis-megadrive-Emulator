using Microsoft.VisualBasic.Devices;
using SharpDX.DirectInput;

namespace MDTracer
{
    //----------------------------------------------------------------
    //I/O Controller  : chips:315-5309
    //----------------------------------------------------------------
    internal partial class md_io
    {
        public byte g_io_a10001_7_mode;
        public byte g_io_a10001_6_vmod;
        public byte g_io_a10001_5_disk;
        public byte g_io_a10001_3_ver;

        public byte g_io_a10003_data1;
        public byte g_io_a10005_data2;
        public byte g_io_a10007_data3;
        public byte g_io_a10009_ctrl1;
        public byte g_io_a1000b_ctrl2;
        public byte g_io_a1000d_ctrl3;
        public byte g_io_a1000f_txdata1;
        public byte g_io_a10011_rxdata1;
        public byte g_io_a10013_s_ctrl1;
        public byte g_io_a10015_txdata2;
        public byte g_io_a10017_rxdata2;
        public byte g_io_a10019_s_ctrl1;
        public byte g_io_a1001b_txdata3;
        public byte g_io_a1001d_rxdata3;
        public byte g_io_a1001f_s_ctrl3;

        public const int JOY_STATUS_NUM = 50;
        public const int KEY_STATUS_NUM = 256;
        public int KEY_ALLCATION_NUM = 8;

        public byte[] g_joy_status;
        public byte[] g_key_status;

        private SharpDX.DirectInput.Keyboard g_keyboard;
        public List<SharpDX.DirectInput.Joystick> g_joy_device;
        public List<string> g_joy_name_list;
        public int g_joy_device_cur;
        public string g_joy_name;
        public int[] g_key_allocation;
        public int[] g_joy_allocation;
        int g_key_cur;
        //----------------------------------------------------------------
        public md_io()
        {
            g_io_a10001_7_mode = 1;
            g_io_a10001_6_vmod = 0;
            g_io_a10001_5_disk = 1;
            g_io_a10001_3_ver = 0;

            g_io_a10003_data1 = 0xff;
            g_io_a10005_data2 = 0xff;
            g_io_a10007_data3 = 0xff;

            g_joy_name_list = new List<string>();
            g_joy_device = new List<Joystick>();
            g_joy_status = new byte[JOY_STATUS_NUM];
            g_key_status = new byte[KEY_STATUS_NUM];
            g_joy_allocation = new int[KEY_ALLCATION_NUM];
            g_key_allocation = new int[KEY_ALLCATION_NUM];
        }

        //----------------------------------------------------------------
        //read
        //----------------------------------------------------------------
        public byte read8(uint in_address)
        {
            byte w_out = 0; 
            if (in_address == 0xa10001)
            {
                if (md_main.g_md_cartridge.g_country.Contains('J'))
                {
                    g_io_a10001_7_mode = 0;
                }
                else
                {
                    g_io_a10001_7_mode = 1;
                }
                w_out  = (byte)(g_io_a10001_7_mode << 7);
                w_out |= (byte)(g_io_a10001_6_vmod << 6);
                w_out |= (byte)(g_io_a10001_5_disk << 5);
                w_out |= g_io_a10001_3_ver;
            }
            else if (in_address == 0xa10003)
            {
                if ((g_io_a10003_data1 & 0x40) == 0)
                {
                    w_out = 0x33;
                    if (g_key_status[g_key_allocation[3]] == 1) w_out &= 0xdf;  //START
                    if (g_key_status[g_key_allocation[0]] == 1) w_out &= 0xef;  //A
                    if (g_key_status[g_key_allocation[5]] == 1) w_out &= 0xfd;  //DOWN
                    if (g_key_status[g_key_allocation[4]] == 1) w_out &= 0xfe;  //UP
                    if (g_joy_status[g_joy_allocation[3]] == 1) w_out &= 0xdf;  //START
                    if (g_joy_status[g_joy_allocation[0]] == 1) w_out &= 0xef;  //A
                    if (g_joy_status[g_joy_allocation[5]] == 1) w_out &= 0xfd;  //DOWN
                    if (g_joy_status[g_joy_allocation[4]] == 1) w_out &= 0xfe;  //UP
                }
                else
                {
                    w_out = 0x7f;
                    if (g_key_status[g_key_allocation[2]] == 1) w_out &= 0xdf;  //C
                    if (g_key_status[g_key_allocation[1]] == 1) w_out &= 0xef;  //B
                    if (g_key_status[g_key_allocation[7]] == 1) w_out &= 0xf7;  //RIGHT
                    if (g_key_status[g_key_allocation[6]] == 1) w_out &= 0xfb;  //LEFT
                    if (g_key_status[g_key_allocation[5]] == 1) w_out &= 0xfd;  //DOWN
                    if (g_key_status[g_key_allocation[4]] == 1) w_out &= 0xfe;  //UP
                    if (g_joy_status[g_joy_allocation[2]] == 1) w_out &= 0xdf;  //C
                    if (g_joy_status[g_joy_allocation[1]] == 1) w_out &= 0xef;  //B
                    if (g_joy_status[g_joy_allocation[7]] == 1) w_out &= 0xf7;  //RIGHT
                    if (g_joy_status[g_joy_allocation[6]] == 1) w_out &= 0xfb;  //LEFT
                    if (g_joy_status[g_joy_allocation[5]] == 1) w_out &= 0xfd;  //DOWN
                    if (g_joy_status[g_joy_allocation[4]] == 1) w_out &= 0xfe;  //UP

                }
            }
            else if (in_address == 0xa10005) w_out = g_io_a10005_data2;
            else if (in_address == 0xa10007) w_out = g_io_a10007_data3;
            else if (in_address == 0xa10009) w_out = g_io_a10009_ctrl1;
            else if (in_address == 0xa1000b) w_out = g_io_a1000b_ctrl2;
            else if (in_address == 0xa1000d) w_out = g_io_a1000d_ctrl3;
            else if (in_address == 0xa1000f) w_out = g_io_a1000f_txdata1;
            else if (in_address == 0xa10011) w_out = g_io_a10011_rxdata1;
            else if (in_address == 0xa10013) w_out = g_io_a10013_s_ctrl1;
            else if (in_address == 0xa10015) w_out = g_io_a10015_txdata2;
            else if (in_address == 0xa10017) w_out = g_io_a10017_rxdata2;
            else if (in_address == 0xa10019) w_out = g_io_a10019_s_ctrl1;
            else if (in_address == 0xa1001b) w_out = g_io_a1001b_txdata3;
            else if (in_address == 0xa1001d) w_out = g_io_a1001d_rxdata3;
            else if (in_address == 0xa1001f) w_out = g_io_a1001f_s_ctrl3;
            else
            {
                MessageBox.Show("md_io.read8", "error");
            }

            return w_out;
        }
        public ushort read16(uint in_address)
        {
            return read8(in_address + 1);
        }
        public uint read32(uint in_address)
        {
            ushort w_data = (ushort)((read8(in_address + 1) << 8)
                                    + read8(in_address + 3));
            return w_data;
        }
        //----------------------------------------------------------------
        //write
        //----------------------------------------------------------------
        public void write8(uint in_address, byte in_data)
        {
            if (in_address == 0xa10003) g_io_a10003_data1 = in_data;
            else if (in_address == 0xa10005) g_io_a10005_data2 = in_data;
            else if (in_address == 0xa10007) g_io_a10007_data3 = in_data;
            else if (in_address == 0xa10009) g_io_a10009_ctrl1 = in_data;
            else if (in_address == 0xa1000b) g_io_a1000b_ctrl2 = in_data;
            else if (in_address == 0xa1000d) g_io_a1000d_ctrl3 = in_data;
            else if (in_address == 0xa1000f) g_io_a1000f_txdata1 = in_data;
            else if (in_address == 0xa10013) g_io_a10013_s_ctrl1 = in_data;
            else if (in_address == 0xa10015) g_io_a10015_txdata2 = in_data;
            else if (in_address == 0xa10019) g_io_a10019_s_ctrl1 = in_data;
            else if (in_address == 0xa1001b) g_io_a1001b_txdata3 = in_data;
            else if (in_address == 0xa1001f) g_io_a1001f_s_ctrl3 = in_data;
            else
            {
                //MessageBox.Show("md_io.write8", "error");
            }
        }
        public void write16(uint in_address, ushort in_data)
        {
            MessageBox.Show("md_io.write16", "error");
        }
    }
}
