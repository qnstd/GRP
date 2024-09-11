using UnityEngine;
using UnityEngine.Rendering;

namespace com.cngraphi.grp
{
    /// <summary>
    /// �������Ⱦ
    /// <para>����Ⱦ�ܵ��£���֧�ֶ��������Ⱦ������ÿ�������Ⱦ���Ƕ����ġ�</para>
    /// </summary>
    public partial class CameraRender
    {
        ScriptableRenderContext context;
        Camera camera;

        // Profiler , FrameDebugger ����ʾ��Ϣ
        const string bufferName = "Render Camera";
        // �����
        CommandBuffer buffer = new CommandBuffer() { name = bufferName };

        // �ü����
        CullingResults cullingResults;


        // ֧�ֵ���ɫ��
        static ShaderTagId unlitShaderTagID = new ShaderTagId("SRPDefaultUnlit");



        /// <summary>
        /// ��Ⱦ
        /// </summary>
        /// <param name="context">����������Ļ���</param>
        /// <param name="camera">�������</param>
        public void Render(ScriptableRenderContext context, Camera camera)
        {
            this.context = context;
            this.camera = camera;

            PreBufferName();
            DrawUIInSceneView();

            // �ü�ʧ�ܣ��򷵻�
            if (!Cull()) { return; }

            Setup();
            DrawVisibleGeometry();
            DrawUnsupported();
            DrawGizmos();
            Submit();
        }


        /// <summary>
        /// ��׶��ü�
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
        /// �ڻ���֮ǰ������س�ʼ��
        /// <para>����׼��������Ȳ���</para>
        /// </summary>
        private void Setup()
        {
            // �ύ������������
            context.SetupCameraProperties(camera);

            // ������һ�εĻ��ƣ�ͬʱ����ָ����ռ�
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
        /// ִ�л��������������
        /// </summary>
        private void ExcuteBuffer()
        {
            context.ExecuteCommandBuffer(buffer);
            buffer.Clear();
        }



        /// <summary>
        /// ����������ɼ�����������
        /// </summary>
        private void DrawVisibleGeometry()
        {
            // ���Ʋ�͸������
            var sortSettings = new SortingSettings(camera)
            {
                criteria = SortingCriteria.CommonOpaque
            };
            var drawSettings = new DrawingSettings(unlitShaderTagID, sortSettings);
            var filterSettings = new FilteringSettings(RenderQueueRange.opaque);
            context.DrawRenderers(cullingResults, ref drawSettings, ref filterSettings);


            // ������պ�
            context.DrawSkybox(camera);


            // ����͸������
            sortSettings.criteria = SortingCriteria.CommonTransparent;
            drawSettings.sortingSettings = sortSettings;
            filterSettings.renderQueueRange = RenderQueueRange.transparent;
            context.DrawRenderers(cullingResults, ref drawSettings, ref filterSettings);
        }



        /// <summary>
        /// �ύ������Ⱦָ��
        /// </summary>
        private void Submit()
        {
            buffer.EndSample(SampleName);
            ExcuteBuffer();
            context.Submit();
        }
    }
}