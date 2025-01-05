using System;
using static MDTracer.md_m68k;
namespace MDTracer
{
    internal partial class md_m68k
    {
        private void analyse_MOVEM_w_r2m()
        {
            g_reg_PC += 2;
            uint w_mask = md_main.g_md_bus.read16(g_reg_PC);
            g_reg_PC += 2;
            g_clock += 8;
            adressing_func_address(g_op3, g_op4, 1);
            uint wdata = g_analyze_address;
            if ((w_mask & 0x0001) != 0) { md_main.g_md_bus.write16(wdata, g_reg_data[0].w); wdata += 2; g_clock += 5;};
            if ((w_mask & 0x0002) != 0) { md_main.g_md_bus.write16(wdata, g_reg_data[1].w); wdata += 2; g_clock += 5;};
            if ((w_mask & 0x0004) != 0) { md_main.g_md_bus.write16(wdata, g_reg_data[2].w); wdata += 2; g_clock += 5;};
            if ((w_mask & 0x0008) != 0) { md_main.g_md_bus.write16(wdata, g_reg_data[3].w); wdata += 2; g_clock += 5;};
            if ((w_mask & 0x0010) != 0) { md_main.g_md_bus.write16(wdata, g_reg_data[4].w); wdata += 2; g_clock += 5;};
            if ((w_mask & 0x0020) != 0) { md_main.g_md_bus.write16(wdata, g_reg_data[5].w); wdata += 2; g_clock += 5;};
            if ((w_mask & 0x0040) != 0) { md_main.g_md_bus.write16(wdata, g_reg_data[6].w); wdata += 2; g_clock += 5;};
            if ((w_mask & 0x0080) != 0) { md_main.g_md_bus.write16(wdata, g_reg_data[7].w); wdata += 2; g_clock += 5;};
            if ((w_mask & 0x0100) != 0) { md_main.g_md_bus.write16(wdata, g_reg_addr[0].w); wdata += 2; g_clock += 5;};
            if ((w_mask & 0x0200) != 0) { md_main.g_md_bus.write16(wdata, g_reg_addr[1].w); wdata += 2; g_clock += 5;};
            if ((w_mask & 0x0400) != 0) { md_main.g_md_bus.write16(wdata, g_reg_addr[2].w); wdata += 2; g_clock += 5;};
            if ((w_mask & 0x0800) != 0) { md_main.g_md_bus.write16(wdata, g_reg_addr[3].w); wdata += 2; g_clock += 5;};
            if ((w_mask & 0x1000) != 0) { md_main.g_md_bus.write16(wdata, g_reg_addr[4].w); wdata += 2; g_clock += 5;};
            if ((w_mask & 0x2000) != 0) { md_main.g_md_bus.write16(wdata, g_reg_addr[5].w); wdata += 2; g_clock += 5;};
            if ((w_mask & 0x4000) != 0) { md_main.g_md_bus.write16(wdata, g_reg_addr[6].w); wdata += 2; g_clock += 5;};
            if ((w_mask & 0x8000) != 0) { md_main.g_md_bus.write16(wdata, g_reg_addr[7].w); wdata += 2; g_clock += 5;};
        }
        private void analyse_MOVEM_w_r2m_4()
        {
            g_reg_PC += 2;
            uint w_mask = md_main.g_md_bus.read16(g_reg_PC);
            g_reg_PC += 2;
            g_clock += 8;
            uint wdata = g_reg_addr[g_op4].l;
            if ((w_mask & 0x0001) != 0) { wdata -= 2; md_main.g_md_bus.write16(wdata, g_reg_addr[7].w); g_clock += 5;};
            if ((w_mask & 0x0002) != 0) { wdata -= 2; md_main.g_md_bus.write16(wdata, g_reg_addr[6].w); g_clock += 5;};
            if ((w_mask & 0x0004) != 0) { wdata -= 2; md_main.g_md_bus.write16(wdata, g_reg_addr[5].w); g_clock += 5;};
            if ((w_mask & 0x0008) != 0) { wdata -= 2; md_main.g_md_bus.write16(wdata, g_reg_addr[4].w); g_clock += 5;};
            if ((w_mask & 0x0010) != 0) { wdata -= 2; md_main.g_md_bus.write16(wdata, g_reg_addr[3].w); g_clock += 5;};
            if ((w_mask & 0x0020) != 0) { wdata -= 2; md_main.g_md_bus.write16(wdata, g_reg_addr[2].w); g_clock += 5;};
            if ((w_mask & 0x0040) != 0) { wdata -= 2; md_main.g_md_bus.write16(wdata, g_reg_addr[1].w); g_clock += 5;};
            if ((w_mask & 0x0080) != 0) { wdata -= 2; md_main.g_md_bus.write16(wdata, g_reg_addr[0].w); g_clock += 5;};
            if ((w_mask & 0x0100) != 0) { wdata -= 2; md_main.g_md_bus.write16(wdata, g_reg_data[7].w); g_clock += 5;};
            if ((w_mask & 0x0200) != 0) { wdata -= 2; md_main.g_md_bus.write16(wdata, g_reg_data[6].w); g_clock += 5;};
            if ((w_mask & 0x0400) != 0) { wdata -= 2; md_main.g_md_bus.write16(wdata, g_reg_data[5].w); g_clock += 5;};
            if ((w_mask & 0x0800) != 0) { wdata -= 2; md_main.g_md_bus.write16(wdata, g_reg_data[4].w); g_clock += 5;};
            if ((w_mask & 0x1000) != 0) { wdata -= 2; md_main.g_md_bus.write16(wdata, g_reg_data[3].w); g_clock += 5;};
            if ((w_mask & 0x2000) != 0) { wdata -= 2; md_main.g_md_bus.write16(wdata, g_reg_data[2].w); g_clock += 5;};
            if ((w_mask & 0x4000) != 0) { wdata -= 2; md_main.g_md_bus.write16(wdata, g_reg_data[1].w); g_clock += 5;};
            if ((w_mask & 0x8000) != 0) { wdata -= 2; md_main.g_md_bus.write16(wdata, g_reg_data[0].w); g_clock += 5;};
            g_reg_addr[g_op4].l = wdata;
        }
        private void analyse_MOVEM_w_m2r()
        {
            g_reg_PC += 2;
            uint w_mask = md_main.g_md_bus.read16(g_reg_PC);
            g_reg_PC += 2;
            g_clock += 12;
            adressing_func_address(g_op3, g_op4, 1);
            uint wdata = g_analyze_address;
            if ((w_mask & 0x0001) != 0) { g_reg_data[0].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x0002) != 0) { g_reg_data[1].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x0004) != 0) { g_reg_data[2].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x0008) != 0) { g_reg_data[3].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x0010) != 0) { g_reg_data[4].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x0020) != 0) { g_reg_data[5].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x0040) != 0) { g_reg_data[6].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x0080) != 0) { g_reg_data[7].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x0100) != 0) { g_reg_addr[0].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x0200) != 0) { g_reg_addr[1].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x0400) != 0) { g_reg_addr[2].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x0800) != 0) { g_reg_addr[3].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x1000) != 0) { g_reg_addr[4].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x2000) != 0) { g_reg_addr[5].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x4000) != 0) { g_reg_addr[6].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x8000) != 0) { g_reg_addr[7].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
        }
        private void analyse_MOVEM_w_m2r_3()
        {
            g_reg_PC += 2;
            uint w_mask = md_main.g_md_bus.read16(g_reg_PC);
            g_reg_PC += 2;
            g_clock += 12;
            uint wdata = g_reg_addr[g_op4].l;
            if ((w_mask & 0x0001) != 0) { g_reg_data[0].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x0002) != 0) { g_reg_data[1].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x0004) != 0) { g_reg_data[2].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x0008) != 0) { g_reg_data[3].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x0010) != 0) { g_reg_data[4].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x0020) != 0) { g_reg_data[5].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x0040) != 0) { g_reg_data[6].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x0080) != 0) { g_reg_data[7].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x0100) != 0) { g_reg_addr[0].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x0200) != 0) { g_reg_addr[1].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x0400) != 0) { g_reg_addr[2].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x0800) != 0) { g_reg_addr[3].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x1000) != 0) { g_reg_addr[4].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x2000) != 0) { g_reg_addr[5].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x4000) != 0) { g_reg_addr[6].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            if ((w_mask & 0x8000) != 0) { g_reg_addr[7].l = get_int_cast(md_main.g_md_bus.read16(wdata), 1); wdata += 2; g_clock += 4;};
            g_reg_addr[g_op4].l = wdata;
        }
        private void analyse_MOVEM_l_r2m()
        {
            g_reg_PC += 2;
            uint w_mask = md_main.g_md_bus.read16(g_reg_PC);
            g_reg_PC += 2;
            g_clock += 8;
            adressing_func_address(g_op3, g_op4, 2);
            uint wdata = g_analyze_address;
            if ((w_mask & 0x0001) != 0) { md_main.g_md_bus.write32(wdata, g_reg_data[0].l); wdata += 4; g_clock += 10;};
            if ((w_mask & 0x0002) != 0) { md_main.g_md_bus.write32(wdata, g_reg_data[1].l); wdata += 4; g_clock += 10;};
            if ((w_mask & 0x0004) != 0) { md_main.g_md_bus.write32(wdata, g_reg_data[2].l); wdata += 4; g_clock += 10;};
            if ((w_mask & 0x0008) != 0) { md_main.g_md_bus.write32(wdata, g_reg_data[3].l); wdata += 4; g_clock += 10;};
            if ((w_mask & 0x0010) != 0) { md_main.g_md_bus.write32(wdata, g_reg_data[4].l); wdata += 4; g_clock += 10;};
            if ((w_mask & 0x0020) != 0) { md_main.g_md_bus.write32(wdata, g_reg_data[5].l); wdata += 4; g_clock += 10;};
            if ((w_mask & 0x0040) != 0) { md_main.g_md_bus.write32(wdata, g_reg_data[6].l); wdata += 4; g_clock += 10;};
            if ((w_mask & 0x0080) != 0) { md_main.g_md_bus.write32(wdata, g_reg_data[7].l); wdata += 4; g_clock += 10;};
            if ((w_mask & 0x0100) != 0) { md_main.g_md_bus.write32(wdata, g_reg_addr[0].l); wdata += 4; g_clock += 10;};
            if ((w_mask & 0x0200) != 0) { md_main.g_md_bus.write32(wdata, g_reg_addr[1].l); wdata += 4; g_clock += 10;};
            if ((w_mask & 0x0400) != 0) { md_main.g_md_bus.write32(wdata, g_reg_addr[2].l); wdata += 4; g_clock += 10;};
            if ((w_mask & 0x0800) != 0) { md_main.g_md_bus.write32(wdata, g_reg_addr[3].l); wdata += 4; g_clock += 10;};
            if ((w_mask & 0x1000) != 0) { md_main.g_md_bus.write32(wdata, g_reg_addr[4].l); wdata += 4; g_clock += 10;};
            if ((w_mask & 0x2000) != 0) { md_main.g_md_bus.write32(wdata, g_reg_addr[5].l); wdata += 4; g_clock += 10;};
            if ((w_mask & 0x4000) != 0) { md_main.g_md_bus.write32(wdata, g_reg_addr[6].l); wdata += 4; g_clock += 10;};
            if ((w_mask & 0x8000) != 0) { md_main.g_md_bus.write32(wdata, g_reg_addr[7].l); wdata += 4; g_clock += 10;};
        }
        private void analyse_MOVEM_l_r2m_4()
        {
            g_reg_PC += 2;
            uint w_mask = md_main.g_md_bus.read16(g_reg_PC);
            g_reg_PC += 2;
            g_clock += 8;
            uint wdata = g_reg_addr[g_op4].l;
            if ((w_mask & 0x0001) != 0) { wdata -= 4; md_main.g_md_bus.write32(wdata, g_reg_addr[7].l); g_clock += 10;};
            if ((w_mask & 0x0002) != 0) { wdata -= 4; md_main.g_md_bus.write32(wdata, g_reg_addr[6].l); g_clock += 10;};
            if ((w_mask & 0x0004) != 0) { wdata -= 4; md_main.g_md_bus.write32(wdata, g_reg_addr[5].l); g_clock += 10;};
            if ((w_mask & 0x0008) != 0) { wdata -= 4; md_main.g_md_bus.write32(wdata, g_reg_addr[4].l); g_clock += 10;};
            if ((w_mask & 0x0010) != 0) { wdata -= 4; md_main.g_md_bus.write32(wdata, g_reg_addr[3].l); g_clock += 10;};
            if ((w_mask & 0x0020) != 0) { wdata -= 4; md_main.g_md_bus.write32(wdata, g_reg_addr[2].l); g_clock += 10;};
            if ((w_mask & 0x0040) != 0) { wdata -= 4; md_main.g_md_bus.write32(wdata, g_reg_addr[1].l); g_clock += 10;};
            if ((w_mask & 0x0080) != 0) { wdata -= 4; md_main.g_md_bus.write32(wdata, g_reg_addr[0].l); g_clock += 10;};
            if ((w_mask & 0x0100) != 0) { wdata -= 4; md_main.g_md_bus.write32(wdata, g_reg_data[7].l); g_clock += 10;};
            if ((w_mask & 0x0200) != 0) { wdata -= 4; md_main.g_md_bus.write32(wdata, g_reg_data[6].l); g_clock += 10;};
            if ((w_mask & 0x0400) != 0) { wdata -= 4; md_main.g_md_bus.write32(wdata, g_reg_data[5].l); g_clock += 10;};
            if ((w_mask & 0x0800) != 0) { wdata -= 4; md_main.g_md_bus.write32(wdata, g_reg_data[4].l); g_clock += 10;};
            if ((w_mask & 0x1000) != 0) { wdata -= 4; md_main.g_md_bus.write32(wdata, g_reg_data[3].l); g_clock += 10;};
            if ((w_mask & 0x2000) != 0) { wdata -= 4; md_main.g_md_bus.write32(wdata, g_reg_data[2].l); g_clock += 10;};
            if ((w_mask & 0x4000) != 0) { wdata -= 4; md_main.g_md_bus.write32(wdata, g_reg_data[1].l); g_clock += 10;};
            if ((w_mask & 0x8000) != 0) { wdata -= 4; md_main.g_md_bus.write32(wdata, g_reg_data[0].l); g_clock += 10;};
            g_reg_addr[g_op4].l = wdata;
        }
        private void analyse_MOVEM_l_m2r()
        {
            g_reg_PC += 2;
            uint w_mask = md_main.g_md_bus.read16(g_reg_PC);
            g_reg_PC += 2;
            g_clock += 12;
            adressing_func_address(g_op3, g_op4, 2);
            uint wdata = g_analyze_address;
            if ((w_mask & 0x0001) != 0) { g_reg_data[0].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x0002) != 0) { g_reg_data[1].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x0004) != 0) { g_reg_data[2].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x0008) != 0) { g_reg_data[3].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x0010) != 0) { g_reg_data[4].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x0020) != 0) { g_reg_data[5].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x0040) != 0) { g_reg_data[6].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x0080) != 0) { g_reg_data[7].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x0100) != 0) { g_reg_addr[0].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x0200) != 0) { g_reg_addr[1].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x0400) != 0) { g_reg_addr[2].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x0800) != 0) { g_reg_addr[3].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x1000) != 0) { g_reg_addr[4].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x2000) != 0) { g_reg_addr[5].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x4000) != 0) { g_reg_addr[6].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x8000) != 0) { g_reg_addr[7].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
        }
        private void analyse_MOVEM_l_m2r_3()
        {
            g_reg_PC += 2;
            uint w_mask = md_main.g_md_bus.read16(g_reg_PC);
            g_reg_PC += 2;
            g_clock += 12;
            uint wdata = g_reg_addr[g_op4].l;
            if ((w_mask & 0x0001) != 0) { g_reg_data[0].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x0002) != 0) { g_reg_data[1].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x0004) != 0) { g_reg_data[2].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x0008) != 0) { g_reg_data[3].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x0010) != 0) { g_reg_data[4].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x0020) != 0) { g_reg_data[5].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x0040) != 0) { g_reg_data[6].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x0080) != 0) { g_reg_data[7].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x0100) != 0) { g_reg_addr[0].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x0200) != 0) { g_reg_addr[1].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x0400) != 0) { g_reg_addr[2].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x0800) != 0) { g_reg_addr[3].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x1000) != 0) { g_reg_addr[4].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x2000) != 0) { g_reg_addr[5].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x4000) != 0) { g_reg_addr[6].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            if ((w_mask & 0x8000) != 0) { g_reg_addr[7].l = md_main.g_md_bus.read32(wdata); wdata += 4; g_clock += 8;};
            g_reg_addr[g_op4].l = wdata;
        }
   }
}
