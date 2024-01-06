using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelPerfectRendererFeature : ScriptableRendererFeature
{
    [SerializeField]
    private bool _applyToSceneView = true;

    private Shader _shader;

    private Material _mat;

    private PixelPerfectPostprocessRenderPass _pass;
    
    public override void Create()
    {
        _shader = Shader.Find("Hidden/PixelPerfectPostprocess");
        _mat = CoreUtils.CreateEngineMaterial(_shader);
        _pass = new PixelPerfectPostprocessRenderPass(_applyToSceneView, _mat);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_pass);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        _pass.ConfigureInput(ScriptableRenderPassInput.Color);
        _pass.SetTarget(renderer.cameraColorTargetHandle);
    }

    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(_mat);
    }
}
