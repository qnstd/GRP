using UnityEngine;
using UnityEngine.Rendering;

namespace com.cngraphi.grp
{
    /// <summary>
    /// 渲染管线配置
    /// <para>作者：强辰</para>
    /// </summary>
    [CreateAssetMenu(menuName = "Rendering/Graphi RenderPipeline Asset")]
    public class GraphiRenderPipelineAsset : RenderPipelineAsset
    {
        /// <summary>
        /// 创建渲染管道
        /// <para>提供给 unity 获取自定义渲染管道实例的入口</para>
        /// </summary>
        /// <returns></returns>
        protected override RenderPipeline CreatePipeline()
        {
            return new GraphiRenderPipeline();
        }
    }
}