using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace com.cngraphi.grp
{
    /// <summary>
    /// ��Ⱦ����ʵ��
    /// <para>���ߣ�ǿ��</para>
    /// </summary>
    public class GraphiRenderPipeline : RenderPipeline
    {
        // �������Ⱦ��
        CameraRender renderer = new CameraRender();


        /// <summary>
        /// ÿ֡����
        /// </summary>
        /// <param name="context">������</param>
        /// <param name="cameras">�������</param>
        protected override void Render(ScriptableRenderContext context, Camera[] cameras) { }


        /// <summary>
        /// ÿ֡����
        /// <para>ʹ�ô˷�ʽ����Ϊ�������������Ҫ����ÿ֡�����ڴ���䣬���Ե�2��������ΪList�б�ʽ�������鷽ʽֻ�Ǳ����˼̳�RenderPipeline����Ҫʵ��abstract�ķ������ѡ�</para>
        /// <para>����Ķ��� unity 2022 �汾�ɼ���</para>
        /// </summary>
        /// <param name="context">������</param>
        /// <param name="cameras">�������</param>
        protected override void Render(ScriptableRenderContext context, List<Camera> cameras)
        {
            for (int i = 0; i < cameras.Count; i++)
            {
                renderer.Render(context, cameras[i]);
            }
        }
    }
}