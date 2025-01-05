using SharpDX;
using SharpDX.Direct3D12;
using SharpDX.D3DCompiler;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace MDTracer
{

    internal partial class md_vdp
    {
        [StructLayout(LayoutKind.Sequential, Size = 256)]
        public struct VDP_REGISTER
        {
            public int display_xsize;
            public int display_ysize;
            public int scroll_xsize;
            public int scroll_xcell;
            public int scroll_mask;
            public int scrollw_xcell;
            public int vdp_reg_1_6_display;
            public int vdp_reg_2_scrolla;
            public int vdp_reg_4_scrollb;
            public int vdp_reg_3_windows;
            public uint vdp_reg_7_backcolor;
            public uint vdp_reg_12_3_shadow;
            public uint screenA_left;
            public uint screenA_right;
            public uint screenA_top;
            public uint screenA_bottom;
        };
        [StructLayout(LayoutKind.Sequential, Size = 4)]
        private struct VDP_LINE_SNAP
        {
            public int hscrollA;
            public int hscrollB;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = VSRAM_DATASIZE)]
            public int[] vscrollA;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = VSRAM_DATASIZE)]
            public int[] vscrollB;
            public int window_x_st;
            public int window_x_ed;
            public int sprite_rendrere_num;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_SPRITE)]
            public int[] sprite_left;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_SPRITE)]
            public int[] sprite_right;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_SPRITE)]
            public int[] sprite_top;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_SPRITE)]
            public int[] sprite_bottom;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_SPRITE)]
            public int[] sprite_xcell_size;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_SPRITE)]
            public int[] sprite_ycell_size;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_SPRITE)]
            public uint[] sprite_priority;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_SPRITE)]
            public uint[] sprite_palette;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_SPRITE)]
            public uint[] sprite_reverse;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_SPRITE)]
            public uint[] sprite_char;
        }
        private Device g_dx_device;
        private CommandQueue g_dx_CommandQueue;
        private DescriptorHeap g_dx_Heap;
        private CpuDescriptorHandle g_dx_HeapHandle;
        private int g_dx_HeapHandle_offset;
        private CommandAllocator g_dx_CommandAllocator;
        private GraphicsCommandList g_dx_CommandList;
        private int g_dx_FenceNum;
        private Fence g_dx_Fence;
        private AutoResetEvent g_dx_FenceEvent;
        private RootSignature g_dx_RootSignature;
        //---------------------------
        private PipelineState g_dx_PipelineState_screenb;
        private PipelineState g_dx_PipelineState_screena;
        private PipelineState g_dx_PipelineState_sprite;
        private PipelineState g_dx_PipelineState_window;
        private PipelineState g_dx_PipelineState_final;
        //---------------------------
        private Resource g_dx_vram_buffers;
        private Resource g_dx_color_buffers;
        private Resource g_dx_colorshadow_buffers;
        private Resource g_dx_color_highlight_buffers;
        private Resource g_dx_line_snap_buffers;
        private Resource g_dx_screen_buffers;
        private Resource g_dx_cmap_buffers;
        private Resource g_dx_primap_buffers;
        private Resource g_dx_shadowmap_buffers;
        private Resource g_dx_register_buffers;
        private Resource g_dx_vram_update_buffers;
        private Resource g_dx_color_update_buffers;
        private Resource g_dx_color_shadow_update_buffers;
        private Resource g_dx_color_highlight_update_buffers;
        private Resource g_dx_line_snap_update_buffers;
        private Resource g_dx_screen_download_buffers;
        //---------------------------
        private IntPtr g_dx_vram_update_buffers_ptr;
        private IntPtr g_dx_color_update_buffers_ptr;
        private IntPtr g_dx_color_shadow_update_buffers_ptr;
        private IntPtr g_dx_color_highlight_update_buffers_ptr;
        private IntPtr g_dx_line_snap_update_buffers_ptr;
        private IntPtr g_dx_screen_download_buffers_ptr;
        private IntPtr g_dx_register_buffers_ptr;
        //---------------------------
        public void dx_rendering_initialize()
        {
            //device
            g_dx_device = new Device(null, SharpDX.Direct3D.FeatureLevel.Level_11_0);
            g_dx_CommandQueue = g_dx_device.CreateCommandQueue(new CommandQueueDescription(CommandListType.Compute));
            g_dx_Heap = g_dx_device.CreateDescriptorHeap(new DescriptorHeapDescription()
            {
                DescriptorCount = 10,
                Type = DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView,
                Flags = DescriptorHeapFlags.ShaderVisible,
            });
            g_dx_HeapHandle = g_dx_Heap.CPUDescriptorHandleForHeapStart;
            g_dx_HeapHandle_offset = g_dx_device.GetDescriptorHandleIncrementSize(DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView);
            g_dx_CommandAllocator = g_dx_device.CreateCommandAllocator(CommandListType.Compute);
            g_dx_CommandList = g_dx_device.CreateCommandList(CommandListType.Compute, g_dx_CommandAllocator, null);
            g_dx_CommandList.Close();
            g_dx_Fence = g_dx_device.CreateFence(0, FenceFlags.None);
            g_dx_FenceNum = 1;
            g_dx_FenceEvent = new AutoResetEvent(false);

            //root signature
            var w_rootsignatureDesc = new RootSignatureDescription(RootSignatureFlags.None,
                new[]{ new RootParameter(ShaderVisibility.All,
                        new DescriptorRange()
                        {
                            RangeType = DescriptorRangeType.ShaderResourceView,
                            BaseShaderRegister = 0,
                            OffsetInDescriptorsFromTableStart = 0,
                            DescriptorCount = 5,
                        },
                        new DescriptorRange()
                        {
                            RangeType = DescriptorRangeType.UnorderedAccessView,
                            BaseShaderRegister = 0,
                            OffsetInDescriptorsFromTableStart = 5,
                            DescriptorCount = 4,
                        },
                        new DescriptorRange()
                        {
                            RangeType = DescriptorRangeType.ConstantBufferView,
                            BaseShaderRegister = 0,
                            OffsetInDescriptorsFromTableStart = 9,
                            DescriptorCount = 1,
                        }),
            });
            g_dx_RootSignature = g_dx_device.CreateRootSignature(w_rootsignatureDesc.Serialize());

            //pipeline state
            string w_hlsl_string;
            var w_assembly = Assembly.GetExecutingAssembly();
            using (Stream w_stream = w_assembly.GetManifestResourceStream("MDTracer.md_vdp_renderer_directx_update.hlsl"))
            {
                using (StreamReader w_reader = new StreamReader(w_stream))
                {
                    w_hlsl_string = w_reader.ReadToEnd();
                }
            }
            g_dx_PipelineState_screenb = CreatePipelineState(w_hlsl_string, "CS_SCREENB");
            g_dx_PipelineState_screena = CreatePipelineState(w_hlsl_string, "CS_SCREENA");
            g_dx_PipelineState_sprite = CreatePipelineState(w_hlsl_string, "CS_SPRITE");
            g_dx_PipelineState_window = CreatePipelineState(w_hlsl_string, "CS_WINDOW");
            g_dx_PipelineState_final = CreatePipelineState(w_hlsl_string, "CS_FINAL");

            //resorce
            g_dx_vram_buffers = CreateBufferResource_view_srv(VRAM_DATASIZE * 4, Utilities.SizeOf<uint>());
            g_dx_color_buffers = CreateBufferResource_view_srv(COLOR_MAX, Utilities.SizeOf<uint>());
            g_dx_colorshadow_buffers = CreateBufferResource_view_srv(COLOR_MAX, Utilities.SizeOf<uint>());
            g_dx_color_highlight_buffers = CreateBufferResource_view_srv(COLOR_MAX, Utilities.SizeOf<uint>());
            g_dx_line_snap_buffers = CreateBufferResource_view_srv(DISPLAY_YSIZE, Marshal.SizeOf(typeof(VDP_LINE_SNAP)));
            g_dx_screen_buffers = CreateBufferResource_view_uav(DISPLAY_BUFSIZE, Utilities.SizeOf<uint>());
            g_dx_cmap_buffers = CreateBufferResource_view_uav(DISPLAY_BUFSIZE, Utilities.SizeOf<uint>());
            g_dx_primap_buffers = CreateBufferResource_view_uav(DISPLAY_BUFSIZE, Utilities.SizeOf<uint>());
            g_dx_shadowmap_buffers = CreateBufferResource_view_uav(DISPLAY_BUFSIZE, Utilities.SizeOf<uint>());
            g_dx_register_buffers = CreateBufferResource_view_cbv(Marshal.SizeOf(typeof(VDP_REGISTER)));
            g_dx_register_buffers_ptr = g_dx_register_buffers.Map(0);
            //resorce(update)
            g_dx_vram_update_buffers = CreateBufferResource_update(VRAM_DATASIZE * 4, Utilities.SizeOf<uint>());
            g_dx_color_update_buffers = CreateBufferResource_update(COLOR_MAX, Utilities.SizeOf<uint>());
            g_dx_color_shadow_update_buffers = CreateBufferResource_update(COLOR_MAX, Utilities.SizeOf<uint>());
            g_dx_color_highlight_update_buffers = CreateBufferResource_update(COLOR_MAX, Utilities.SizeOf<uint>());
            g_dx_line_snap_update_buffers = CreateBufferResource_update(DISPLAY_YSIZE, Marshal.SizeOf(typeof(VDP_LINE_SNAP)));
            g_dx_screen_download_buffers = CreateBufferResource_update(DISPLAY_BUFSIZE, Utilities.SizeOf<uint>());
            g_dx_vram_update_buffers_ptr = g_dx_vram_update_buffers.Map(0);
            g_dx_color_update_buffers_ptr = g_dx_color_update_buffers.Map(0);
            g_dx_color_shadow_update_buffers_ptr = g_dx_color_shadow_update_buffers.Map(0);
            g_dx_color_highlight_update_buffers_ptr = g_dx_color_highlight_update_buffers.Map(0);
            g_dx_line_snap_update_buffers_ptr = g_dx_line_snap_update_buffers.Map(0);
            g_dx_screen_download_buffers_ptr = g_dx_screen_download_buffers.Map(0);
        }

        private void dx_frame_stack()
        {

            var w_cur_ptr1 = g_dx_vram_update_buffers_ptr;
            for (var i = 0; i < (VRAM_DATASIZE * 4); i++)
            {
                w_cur_ptr1 = Utilities.WriteAndPosition(w_cur_ptr1, ref g_snap_renderer_vram[i]);
            }
            var w_cur_ptr2 = g_dx_color_update_buffers_ptr;
            for (var i = 0; i < COLOR_MAX; i++)
            {
                w_cur_ptr2 = Utilities.WriteAndPosition(w_cur_ptr2, ref g_snap_color[i]);
            }
            var w_cur_ptr3 = g_dx_color_shadow_update_buffers_ptr;
            for (var i = 0; i < COLOR_MAX; i++)
            {
                w_cur_ptr3 = Utilities.WriteAndPosition(w_cur_ptr3, ref g_snap_color_shadow[i]);
            }
            var w_cur_ptr4 = g_dx_color_highlight_update_buffers_ptr;
            for (var i = 0; i < COLOR_MAX; i++)
            {
                w_cur_ptr4 = Utilities.WriteAndPosition(w_cur_ptr4, ref g_snap_color_highlight[i]);
            }

            var w_cur_ptr5 = g_dx_line_snap_update_buffers_ptr;
            for (var i = 0; i < DISPLAY_YSIZE; i++)
            {
                w_cur_ptr5 = Utilities.WriteAndPosition(w_cur_ptr5, ref g_snap_line_snap[i].hscrollA);
                w_cur_ptr5 = Utilities.WriteAndPosition(w_cur_ptr5, ref g_snap_line_snap[i].hscrollB);
                for (int j = 0; j < VSRAM_DATASIZE; j++) w_cur_ptr5 = Utilities.WriteAndPosition(w_cur_ptr5, ref g_snap_line_snap[i].vscrollA[j]);
                for (int j = 0; j < VSRAM_DATASIZE; j++) w_cur_ptr5 = Utilities.WriteAndPosition(w_cur_ptr5, ref g_snap_line_snap[i].vscrollB[j]);
                w_cur_ptr5 = Utilities.WriteAndPosition(w_cur_ptr5, ref g_snap_line_snap[i].window_x_st);
                w_cur_ptr5 = Utilities.WriteAndPosition(w_cur_ptr5, ref g_snap_line_snap[i].window_x_ed);
                w_cur_ptr5 = Utilities.WriteAndPosition(w_cur_ptr5, ref g_snap_line_snap[i].sprite_rendrere_num);
                for (int j = 0; j < MAX_SPRITE; j++) w_cur_ptr5 = Utilities.WriteAndPosition(w_cur_ptr5, ref g_snap_line_snap[i].sprite_left[j]);
                for (int j = 0; j < MAX_SPRITE; j++) w_cur_ptr5 = Utilities.WriteAndPosition(w_cur_ptr5, ref g_snap_line_snap[i].sprite_right[j]);
                for (int j = 0; j < MAX_SPRITE; j++) w_cur_ptr5 = Utilities.WriteAndPosition(w_cur_ptr5, ref g_snap_line_snap[i].sprite_top[j]);
                for (int j = 0; j < MAX_SPRITE; j++) w_cur_ptr5 = Utilities.WriteAndPosition(w_cur_ptr5, ref g_snap_line_snap[i].sprite_bottom[j]);
                for (int j = 0; j < MAX_SPRITE; j++) w_cur_ptr5 = Utilities.WriteAndPosition(w_cur_ptr5, ref g_snap_line_snap[i].sprite_xcell_size[j]);
                for (int j = 0; j < MAX_SPRITE; j++) w_cur_ptr5 = Utilities.WriteAndPosition(w_cur_ptr5, ref g_snap_line_snap[i].sprite_ycell_size[j]);
                for (int j = 0; j < MAX_SPRITE; j++) w_cur_ptr5 = Utilities.WriteAndPosition(w_cur_ptr5, ref g_snap_line_snap[i].sprite_priority[j]);
                for (int j = 0; j < MAX_SPRITE; j++) w_cur_ptr5 = Utilities.WriteAndPosition(w_cur_ptr5, ref g_snap_line_snap[i].sprite_palette[j]);
                for (int j = 0; j < MAX_SPRITE; j++) w_cur_ptr5 = Utilities.WriteAndPosition(w_cur_ptr5, ref g_snap_line_snap[i].sprite_reverse[j]);
                for (int j = 0; j < MAX_SPRITE; j++) w_cur_ptr5 = Utilities.WriteAndPosition(w_cur_ptr5, ref g_snap_line_snap[i].sprite_char[j]);
            }
        }
        private void dx_rendering()
        {
            Utilities.Write(g_dx_register_buffers_ptr, ref g_snap_register);
            //execute
            g_dx_CommandAllocator.Reset();
            g_dx_CommandList.Reset(g_dx_CommandAllocator, null);
            g_dx_CommandList.SetComputeRootSignature(g_dx_RootSignature);
            g_dx_CommandList.SetDescriptorHeaps(1, new[] { g_dx_Heap });
            g_dx_CommandList.SetComputeRootDescriptorTable(0, g_dx_Heap.GPUDescriptorHandleForHeapStart);
            g_dx_CommandList.CopyBufferRegion(g_dx_vram_buffers, 0, g_dx_vram_update_buffers, 0, VRAM_DATASIZE * 4 * Utilities.SizeOf<uint>());
            g_dx_CommandList.CopyBufferRegion(g_dx_color_buffers, 0, g_dx_color_update_buffers, 0, COLOR_MAX * Utilities.SizeOf<uint>());
            g_dx_CommandList.CopyBufferRegion(g_dx_colorshadow_buffers, 0, g_dx_color_shadow_update_buffers, 0, COLOR_MAX * Utilities.SizeOf<uint>());
            g_dx_CommandList.CopyBufferRegion(g_dx_color_highlight_buffers, 0, g_dx_color_highlight_update_buffers, 0, COLOR_MAX * Utilities.SizeOf<uint>());
            g_dx_CommandList.CopyBufferRegion(g_dx_line_snap_buffers, 0, g_dx_line_snap_update_buffers, 0, DISPLAY_YSIZE * Marshal.SizeOf(typeof(VDP_LINE_SNAP)));
            g_dx_CommandList.PipelineState = g_dx_PipelineState_screenb;
            g_dx_CommandList.Dispatch(g_display_ysize, 1, 1);
            g_dx_CommandList.ResourceBarrier(new ResourceTransitionBarrier(g_dx_cmap_buffers, ResourceStates.UnorderedAccess, ResourceStates.NonPixelShaderResource));
            g_dx_CommandList.ResourceBarrier(new ResourceTransitionBarrier(g_dx_cmap_buffers, ResourceStates.NonPixelShaderResource, ResourceStates.UnorderedAccess));
            g_dx_CommandList.PipelineState = g_dx_PipelineState_screena;
            g_dx_CommandList.Dispatch(g_display_ysize, 1, 1);
            g_dx_CommandList.ResourceBarrier(new ResourceTransitionBarrier(g_dx_cmap_buffers, ResourceStates.UnorderedAccess, ResourceStates.NonPixelShaderResource));
            g_dx_CommandList.ResourceBarrier(new ResourceTransitionBarrier(g_dx_cmap_buffers, ResourceStates.NonPixelShaderResource, ResourceStates.UnorderedAccess));
            g_dx_CommandList.PipelineState = g_dx_PipelineState_sprite;
            g_dx_CommandList.Dispatch(g_display_ysize, 1, 1);
            g_dx_CommandList.ResourceBarrier(new ResourceTransitionBarrier(g_dx_cmap_buffers, ResourceStates.UnorderedAccess, ResourceStates.NonPixelShaderResource));
            g_dx_CommandList.ResourceBarrier(new ResourceTransitionBarrier(g_dx_cmap_buffers, ResourceStates.NonPixelShaderResource, ResourceStates.UnorderedAccess));
            g_dx_CommandList.PipelineState = g_dx_PipelineState_window;
            g_dx_CommandList.Dispatch(g_display_ysize, 1, 1);
            g_dx_CommandList.ResourceBarrier(new ResourceTransitionBarrier(g_dx_cmap_buffers, ResourceStates.UnorderedAccess, ResourceStates.NonPixelShaderResource));
            g_dx_CommandList.ResourceBarrier(new ResourceTransitionBarrier(g_dx_cmap_buffers, ResourceStates.NonPixelShaderResource, ResourceStates.UnorderedAccess));
            g_dx_CommandList.PipelineState = g_dx_PipelineState_final;
            g_dx_CommandList.Dispatch(g_display_ysize, 1, 1);
            g_dx_CommandList.CopyBufferRegion(g_dx_screen_download_buffers, 0, g_dx_screen_buffers, 0, DISPLAY_BUFSIZE * Utilities.SizeOf<uint>());
            g_dx_CommandList.Close();
            g_dx_CommandQueue.ExecuteCommandList(g_dx_CommandList);
            g_dx_CommandQueue.Signal(g_dx_Fence, g_dx_FenceNum);
            g_dx_CommandQueue.Wait(g_dx_Fence, g_dx_FenceNum);
            var fence = g_dx_FenceNum;
            g_dx_CommandQueue.Signal(g_dx_Fence, fence);
            g_dx_FenceNum++;

            if (g_dx_Fence.CompletedValue < fence)
            {
                g_dx_Fence.SetEventOnCompletion(fence, g_dx_FenceEvent.SafeWaitHandle.DangerousGetHandle());
                g_dx_FenceEvent.WaitOne();
            }
        }
        private void dx_get_screen_data()
        {
            var currentComputeOutputBufferPtr = g_dx_screen_download_buffers_ptr;
            for (var i = 0; i < DISPLAY_BUFSIZE; i++)
            {
                currentComputeOutputBufferPtr = Utilities.ReadAndPosition(currentComputeOutputBufferPtr, ref g_game_screen[i]);
            }
        }
    }
}
