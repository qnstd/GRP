using UnityEngine;
using UnityEngine.Rendering;

namespace com.cngraphi.grp
{
    /// <summary>
    /// ��Ⱦ��������
    /// <para>���ߣ�ǿ��</para>
    /// </summary>
    [CreateAssetMenu(menuName = "Rendering/Graphi RenderPipeline Asset")]
    public class GraphiRenderPipelineAsset : RenderPipelineAsset
    {
        /// <summary>
        /// ������Ⱦ�ܵ�
        /// <para>�ṩ�� unity ��ȡ�Զ�����Ⱦ�ܵ�ʵ�������</para>
        /// </summary>
        /// <returns></returns>
        protected override RenderPipeline CreatePipeline()
        {
            return new GraphiRenderPipeline();
        }
    }
}