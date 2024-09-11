using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;

namespace com.cngraphi.grp
{
    public partial class CameraRender
    {
        #region ��ǰ�����ֲ��������˷���ֻ��Editor�����±��룬����ʱ���ᱻ����

        /// <summary>
        /// ��֧����ɫ���Ļ��Ʒ�ʽ
        /// </summary>
        partial void DrawUnsupported();
        /// <summary>
        /// ���� Gizmos ��ǩ���߿�
        /// </summary>
        partial void DrawGizmos();
        /// <summary>
        /// �� SceneView ��ͼ�»���UI
        /// </summary>
        partial void DrawUIInSceneView();
        /// <summary>
        /// Ԥ�޸Ļ�������
        /// </summary>
        partial void PreBufferName();

        #endregion




        #region ʵ�����зֲ�����

#if UNITY_EDITOR

        static ShaderTagId[] legacyShaderTagIDs = {
        new ShaderTagId("Always"),
        new ShaderTagId("ForwardBase"),
        new ShaderTagId("PrepassBase"),
        new ShaderTagId("Vertex"),
        new ShaderTagId("VertexLMRGBM"),
        new ShaderTagId("VertexLM")
    };
        // ������ɫ����Ⱦ����
        static Material errorMaterial = null;

        /// <summary>
        /// �������ݷ����� Profile �е�����
        /// </summary>
        string SampleName { get; set; }


        partial void DrawUnsupported()
        {
            if (errorMaterial == null)
                errorMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));

            var drawSettings = new DrawingSettings(legacyShaderTagIDs[0], new SortingSettings(camera));
            for (int i = 0; i < legacyShaderTagIDs.Length; i++)
                drawSettings.SetShaderPassName(i, legacyShaderTagIDs[i]);
            drawSettings.overrideMaterial = errorMaterial;

            var filterSettings = FilteringSettings.defaultValue;

            context.DrawRenderers(cullingResults, ref drawSettings, ref filterSettings);
        }


        partial void DrawGizmos()
        {
            if (Handles.ShouldRenderGizmos())
            {
                context.DrawGizmos(camera, GizmoSubset.PostImageEffects);
                context.DrawGizmos(camera, GizmoSubset.PreImageEffects);
            }
        }


        partial void DrawUIInSceneView()
        {
            if (camera.cameraType == CameraType.SceneView)
            {
                ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);
            }
        }


        partial void PreBufferName()
        {
            Profiler.BeginSample("Editor Only");
            buffer.name = SampleName = camera.name;
            Profiler.EndSample();
        }

#else
    string SampleName => bufferName;
#endif

        #endregion
    }
}