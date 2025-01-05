using System.Runtime.InteropServices;
using System.Xml.Linq;
using System.Text;
using Microsoft.VisualBasic.Logging;
using System.Diagnostics;

namespace MDTracer
{
    //----------------------------------------------------------------
    //CPU : chips:Motorola MC68000
    //----------------------------------------------------------------
    internal partial class md_m68k
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct UNION_UINT
        {
            [FieldOffset(0)] public byte b0;
            [FieldOffset(1)] public byte b1;
            [FieldOffset(2)] public byte b2;
            [FieldOffset(3)] public byte b3;
            [FieldOffset(0)] public ushort w;
            [FieldOffset(2)] public ushort wup;
            [FieldOffset(0)] public uint l;
        }

        public uint g_reg_PC;
        public uint g_stack_top;
        public UNION_UINT[] g_reg_data;
        public UNION_UINT[] g_reg_addr;
        private UNION_UINT g_reg_addr_usp;
        public uint g_initial_PC;

        public bool g_status_T;
        public bool g_status_B1;
        public bool g_status_S;
        public bool g_status_B2;
        public bool g_status_B3;
        public int g_status_interrupt_mask;
        public bool g_status_B4;
        public bool g_status_B5;
        public bool g_status_B6;
        public bool g_status_X;
        public bool g_status_N;
        public bool g_status_Z;
        public bool g_status_V;
        public bool g_status_C;

        public bool g_interrupt_V_req;
        public bool g_interrupt_H_req;
        public bool g_interrupt_EXT_req;
        public bool g_interrupt_V_act;
        public bool g_interrupt_H_act;
        public bool g_interrupt_EXT_act;
        private bool g_68k_stop;

        private ushort g_opcode;
        private byte g_op;
        private byte g_op1;
        private byte g_op2;
        private byte g_op3;
        private byte g_op4;

        public struct OPINFO
        {
            public Action opcode;
            public string opname_org;
            public string opname;
            public string opname_out;
            public string format;
            public int opleng;
            public int datasize;
            public int memaccess;
        }
        public OPINFO[] g_opcode_info;

        public Int64 g_clock_total;
        public Int64 g_clock_now;
        private int g_clock;

        //----------------------------------------------------------------
        public md_m68k()
        {
            initialize();
            initialize2();
        }
        public void run(int in_clock)
        {
            g_clock_total += in_clock;
            while (g_clock_now < g_clock_total)
            {
                md_main.g_form_code_trace.CPU_Trace(g_reg_PC);

                interrupt_chk();
                g_clock = md_main.g_md_vdp.dma_status_update();
                if (g_clock == 0)
                {
                    g_opcode = read16(g_reg_PC);
                    g_op = (byte)(g_opcode >> 12);
                    g_op1 = (byte)((g_opcode >> 9) & 0x07);
                    g_op2 = (byte)((g_opcode >> 6) & 0x07);
                    g_op3 = (byte)((g_opcode >> 3) & 0x07);
                    g_op4 = (byte)(g_opcode & 0x07);

                    if (g_68k_stop == true)
                    {
                        g_clock_now = g_clock_total;
                        break;
                    }
                    g_opcode_info[g_opcode].opcode();
                }
                g_clock_now += g_clock;
            }
        }
        private void interrupt_chk()
        {
            if ((g_interrupt_H_req == true)
                && (g_status_interrupt_mask < 4)
                && (md_main.g_md_vdp.g_vdp_reg_0_4_hinterrupt == 1))
            {
                uint w_start_address = read32(0x0070);
                stack_push32(g_reg_PC);
                md_main.g_form_code_trace.CPU_Trace_push(Form_Code_Trace.STACK_LIST_TYPE.HINT, 0x0070, w_start_address, g_reg_PC, g_reg_addr[7].l);
                ushort w_data = g_reg_SR;
                stack_push16(w_data);
                g_reg_PC = w_start_address;
                g_status_interrupt_mask = 4;
                g_interrupt_H_req = false;
                g_interrupt_H_act = true;
                g_68k_stop = false;
            }
            else
            if ((g_interrupt_V_req == true)
                && (g_status_interrupt_mask < 6)
                && (md_main.g_md_vdp.g_vdp_reg_1_5_vinterrupt == 1)
                && (g_interrupt_H_act == false)
                )
            {
                uint w_start_address = read32(0x0078);
                stack_push32(g_reg_PC);
                md_main.g_form_code_trace.CPU_Trace_push(Form_Code_Trace.STACK_LIST_TYPE.VINT, 0x0078, w_start_address, g_reg_PC, g_reg_addr[7].l);
                ushort w_data = g_reg_SR;
                stack_push16(w_data);
                g_reg_PC = w_start_address;
                g_status_interrupt_mask = 6;
                g_interrupt_V_req = false;
                g_interrupt_V_act = true;
                g_68k_stop = false;
            }
            else
            if ((g_interrupt_EXT_req == true)
                && (g_status_interrupt_mask < 2))
            {
                uint w_start_address = read32(0x0068);
                stack_push32(g_reg_PC);
                md_main.g_form_code_trace.CPU_Trace_push(Form_Code_Trace.STACK_LIST_TYPE.EXT, 0x0068, w_start_address, g_reg_PC, g_reg_addr[7].l);
                ushort w_data = g_reg_SR;
                stack_push16(w_data);
                g_reg_PC = w_start_address;
                g_status_interrupt_mask = 2;
                g_interrupt_EXT_req = false;
                g_interrupt_EXT_act = true;
                g_68k_stop = false;
            }
        }
        private bool g_log = false;
        uint g_top;
        private uint[] log_trace = new uint[100];
        void traceout()
        {
            for (int i=98;i>=0;i--)
            {
                log_trace[i+1]= log_trace[i];
            }
            log_trace[0] = g_reg_PC;
        }
        void logout(string in_log)
        {
            if (g_log == true)
            {
                System.IO.File.AppendAllText("d:\\md_log.txt", in_log + Environment.NewLine);
            }
        }
        void logout2()
        {
            if (g_log == true)
            {
                using (FileStream fs = new FileStream("d:\\log2.txt", FileMode.Append, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: false))
                {
                    byte[] data = Encoding.UTF8.GetBytes(g_top.ToString("x6").ToString()
                        //+ "," + g_opcode.ToString("x4")

                        + "," + g_reg_addr[0].l.ToString("x8")
                        + "," + g_reg_addr[1].l.ToString("x8")
                        + "," + g_reg_addr[2].l.ToString("x8")
                        + "," + g_reg_addr[3].l.ToString("x8")
                        + "," + g_reg_addr[4].l.ToString("x8")
                        + "," + g_reg_addr[5].l.ToString("x8")
                        + "," + g_reg_addr[6].l.ToString("x8")
                        + "," + g_reg_addr[7].l.ToString("x8")
                        + "  ," + g_reg_data[0].l.ToString("x8")
                        + "," + g_reg_data[1].l.ToString("x8")
                        + "," + g_reg_data[2].l.ToString("x8")
                        + "," + g_reg_data[3].l.ToString("x8")
                        + "," + g_reg_data[4].l.ToString("x8")
                        + "," + g_reg_data[5].l.ToString("x8")
                        + "," + g_reg_data[6].l.ToString("x8")
                        + "," + g_reg_data[7].l.ToString("x8")
                        //+ "  ," + g_reg_SR.ToString("x4")



                        + Environment.NewLine);
                    fs.Write(data, 0, data.Length);
                }
            }
        }
    }
}
