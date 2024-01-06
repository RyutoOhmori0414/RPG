using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AfterTransparentEffectRendererFeature : ScriptableRendererFeature
{
    private AfterTransparentEffectRenderPass _afterTransparentEffectRenderPass = default;
    
    public override void Create()
    {
        _afterTransparentEffectRenderPass = new();
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        _afterTransparentEffectRenderPass.Setup(renderer);
        renderer.EnqueuePass(_afterTransparentEffectRenderPass);
    }
}
