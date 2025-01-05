using SharpDX;
using SharpDX.DXGI;
using SharpDX.Direct3D12;
using SharpDX.D3DCompiler;

namespace MDTracer
{
    internal partial class md_vdp
    {
        private SharpDX.Direct3D12.Resource CreateBufferResource_view_srv(int in_bufsize, int in_struct_size)
        {
            SharpDX.Direct3D12.Resource w_buffer;
            w_buffer = g_dx_device.CreateCommittedResource(
                new HeapProperties(HeapType.Default),
                HeapFlags.None,
                ResourceDescription.Buffer(in_bufsize * in_struct_size),
                ResourceStates.NonPixelShaderResource
            );
            var w_desc = new ShaderResourceViewDescription()
            {
                Format = Format.Unknown,
                Dimension = ShaderResourceViewDimension.Buffer,
                Shader4ComponentMapping = 0x1688,
                Buffer =
                    {
                        FirstElement = 0,
                        ElementCount = in_bufsize,
                        StructureByteStride = in_struct_size,
                        Flags = BufferShaderResourceViewFlags.None,
                    },
            };
            g_dx_device.CreateShaderResourceView(w_buffer, w_desc, g_dx_HeapHandle);
            g_dx_HeapHandle += g_dx_HeapHandle_offset;
            return w_buffer;
        }
        private SharpDX.Direct3D12.Resource CreateBufferResource_view_uav(int in_bufsize, int in_struct_size)
        {
            SharpDX.Direct3D12.Resource w_buffer;
            w_buffer = g_dx_device.CreateCommittedResource(
                new HeapProperties(HeapType.Default),
                HeapFlags.None,
                ResourceDescription.Buffer(in_bufsize * in_struct_size, ResourceFlags.AllowUnorderedAccess),
                ResourceStates.UnorderedAccess
            );
            var w_desc = new UnorderedAccessViewDescription()
            {
                Format = Format.Unknown,
                Dimension = UnorderedAccessViewDimension.Buffer,
                Buffer =
                    {
                        FirstElement = 0,
                        ElementCount = in_bufsize,
                        StructureByteStride = in_struct_size,
                        CounterOffsetInBytes = 0,
                        Flags = BufferUnorderedAccessViewFlags.None,
                    },
            };
            g_dx_device.CreateUnorderedAccessView(w_buffer, null, w_desc, g_dx_HeapHandle);
            g_dx_HeapHandle += g_dx_HeapHandle_offset;
            return w_buffer;
        }
        private SharpDX.Direct3D12.Resource CreateBufferResource_view_cbv(int in_bufsize)
        {
            SharpDX.Direct3D12.Resource w_buffer;
            w_buffer = g_dx_device.CreateCommittedResource(
                new HeapProperties(HeapType.Upload),
                HeapFlags.None,
                ResourceDescription.Buffer(in_bufsize),
                ResourceStates.GenericRead
                );
            var w_desc = new ConstantBufferViewDescription()
            {
                BufferLocation = w_buffer.GPUVirtualAddress,
                SizeInBytes = in_bufsize,
            };

            g_dx_device.CreateConstantBufferView(w_desc, g_dx_HeapHandle);
            g_dx_HeapHandle += g_dx_HeapHandle_offset;
            return w_buffer;
        }
        private SharpDX.Direct3D12.Resource CreateBufferResource_update(int in_bufsize, int in_struct_size)
        {
            SharpDX.Direct3D12.Resource w_buffer;
            w_buffer = g_dx_device.CreateCommittedResource(
                new HeapProperties(HeapType.Upload),
                HeapFlags.None,
                ResourceDescription.Buffer(in_bufsize * in_struct_size),
                ResourceStates.GenericRead
            );
            return w_buffer;
        }
        private PipelineState CreatePipelineState(string in_hlsl_string, string in_entrypoint)
        {
            SharpDX.Direct3D12.ShaderBytecode w_ShaderBytecode = new SharpDX.Direct3D12.ShaderBytecode(
                        SharpDX.D3DCompiler.ShaderBytecode.Compile(in_hlsl_string, in_entrypoint, "cs_5_0", ShaderFlags.Debug));
            var w_cpsDesc = new ComputePipelineStateDescription()
            {
                RootSignaturePointer = g_dx_RootSignature,
                ComputeShader = w_ShaderBytecode,
            };
            return g_dx_device.CreateComputePipelineState(w_cpsDesc);
        }
    }
}
