using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace com.cngraphi.grp
{
    /// <summary>
    /// 渲染管线实例
    /// <para>作者：强辰</para>
    /// </summary>
    public class GraphiRenderPipeline : RenderPipeline
    {
        // 摄像机渲染器
        CameraRender renderer = new CameraRender();


        /// <summary>
        /// 每帧调用
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="cameras">摄像机组</param>
        protected override void Render(ScriptableRenderContext context, Camera[] cameras) { }


        /// <summary>
        /// 每帧调用
        /// <para>使用此方式是因为所有摄像机必须要求在每帧申请内存分配，所以第2个参数改为List列表方式。而数组方式只是保留了继承RenderPipeline类需要实现abstract的方法而已。</para>
        /// <para>这个改动在 unity 2022 版本可见。</para>
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="cameras">摄像机组</param>
        protected override void Render(ScriptableRenderContext context, List<Camera> cameras)
        {
            for (int i = 0; i < cameras.Count; i++)
            {
                renderer.Render(context, cameras[i]);
            }
        }
    }
}