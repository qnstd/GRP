using UnityEngine;
using UnityEngine.Rendering;

namespace com.cngraphi.grp
{
    /// <summary>
    /// 相机的渲染
    /// <para>在渲染管道下，可支持多摄像机渲染，并且每个相机渲染都是独立的。</para>
    /// </summary>
    public partial class CameraRender
    {
        ScriptableRenderContext context;
        Camera camera;

        // Profiler , FrameDebugger 中显示信息
        const string bufferName = "Render Camera";
        // 命令缓冲
        CommandBuffer buffer = new CommandBuffer() { name = bufferName };

        // 裁剪结果
        CullingResults cullingResults;


        // 支持的着色器
        static ShaderTagId unlitShaderTagID = new ShaderTagId("SRPDefaultUnlit");



        /// <summary>
        /// 渲染
        /// </summary>
        /// <param name="context">引擎的上下文环境</param>
        /// <param name="camera">相机对象</param>
        public void Render(ScriptableRenderContext context, Camera camera)
        {
            this.context = context;
            this.camera = camera;

            PreBufferName();
            DrawUIInSceneView();

            // 裁剪失败，则返回
            if (!Cull()) { return; }

            Setup();
            DrawVisibleGeometry();
            DrawUnsupported();
            DrawGizmos();
            Submit();
        }


        /// <summary>
        /// 视锥体裁剪
        /// </summary>
        /// <returns></returns>
        private bool Cull()
        {
            if (camera.TryGetCullingParameters(out ScriptableCullingParameters cullingParams))
            {
                cullingResults = context.Cull(ref cullingParams);
                return true;
            }
            return false;
        }


        /// <summary>
        /// 在绘制之前进行相关初始化
        /// <para>数据准备、清理等操作</para>
        /// </summary>
        private void Setup()
        {
            // 提交摄像机相关属性
            context.SetupCameraProperties(camera);

            // 清理上一次的绘制，同时进行指令缓冲收集
            CameraClearFlags flags = camera.clearFlags;
            buffer.ClearRenderTarget
                (
                    flags <= CameraClearFlags.Depth,
                    flags == CameraClearFlags.Color,
                    (flags == CameraClearFlags.Color) ? camera.backgroundColor.linear : Color.clear
                );
            buffer.BeginSample(SampleName);
            ExcuteBuffer();
        }


        /// <summary>
        /// 执行缓冲命令及清理缓冲组
        /// </summary>
        private void ExcuteBuffer()
        {
            context.ExecuteCommandBuffer(buffer);
            buffer.Clear();
        }



        /// <summary>
        /// 绘制摄像机可见的所有网格
        /// </summary>
        private void DrawVisibleGeometry()
        {
            // 绘制不透明物体
            var sortSettings = new SortingSettings(camera)
            {
                criteria = SortingCriteria.CommonOpaque
            };
            var drawSettings = new DrawingSettings(unlitShaderTagID, sortSettings);
            var filterSettings = new FilteringSettings(RenderQueueRange.opaque);
            context.DrawRenderers(cullingResults, ref drawSettings, ref filterSettings);


            // 绘制天空盒
            context.DrawSkybox(camera);


            // 绘制透明物体
            sortSettings.criteria = SortingCriteria.CommonTransparent;
            drawSettings.sortingSettings = sortSettings;
            filterSettings.renderQueueRange = RenderQueueRange.transparent;
            context.DrawRenderers(cullingResults, ref drawSettings, ref filterSettings);
        }



        /// <summary>
        /// 提交所有渲染指令
        /// </summary>
        private void Submit()
        {
            buffer.EndSample(SampleName);
            ExcuteBuffer();
            context.Submit();
        }
    }
}