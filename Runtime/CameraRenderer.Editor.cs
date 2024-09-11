using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;

namespace com.cngraphi.grp
{
    public partial class CameraRender
    {
        #region 提前声明分部方法，此方法只在Editor环境下编译，发布时不会被编译

        /// <summary>
        /// 不支持着色器的绘制方式
        /// </summary>
        partial void DrawUnsupported();
        /// <summary>
        /// 绘制 Gizmos 标签及线框
        /// </summary>
        partial void DrawGizmos();
        /// <summary>
        /// 在 SceneView 视图下绘制UI
        /// </summary>
        partial void DrawUIInSceneView();
        /// <summary>
        /// 预修改缓冲名称
        /// </summary>
        partial void PreBufferName();

        #endregion




        #region 实现所有分部函数

#if UNITY_EDITOR

        static ShaderTagId[] legacyShaderTagIDs = {
        new ShaderTagId("Always"),
        new ShaderTagId("ForwardBase"),
        new ShaderTagId("PrepassBase"),
        new ShaderTagId("Vertex"),
        new ShaderTagId("VertexLMRGBM"),
        new ShaderTagId("VertexLM")
    };
        // 错误着色的渲染材质
        static Material errorMaterial = null;

        /// <summary>
        /// 缓冲数据分析在 Profile 中的名字
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