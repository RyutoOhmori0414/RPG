using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class PixelPerfectRendererFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public enum InjectPoint
    {
        BeforeRenderingTransparent = RenderPassEvent.BeforeRenderingTransparents,
        AfterRenderingTransparent = RenderPassEvent.AfterRenderingTransparents,
        BeforeRenderingPostProcessing = RenderPassEvent.BeforeRenderingPostProcessing,
        AfterRenderingPostProcessing = RenderPassEvent.AfterRenderingPostProcessing
    }
    
    [SerializeField]
    private bool _applyToSceneView = true;

    [SerializeField]
    private InjectPoint _injectionPoint = InjectPoint.BeforeRenderingPostProcessing;

    [FormerlySerializedAs("requirement")] [SerializeField]
    private ScriptableRenderPassInput _requirement = ScriptableRenderPassInput.Color;
    
    
    private Shader _shader;
    private Material _mat;
    private PixelPerfectPostprocessRenderPass _pass;

    private bool _requireColor;
    private bool _injectedBeforeTransparent;
    
    public override void Create()
    {
        _shader = Shader.Find("Hidden/PixelPerfectPostprocess");
        _mat = CoreUtils.CreateEngineMaterial(_shader);
        _pass = new PixelPerfectPostprocessRenderPass(_applyToSceneView, _mat);
        _pass.renderPassEvent = (RenderPassEvent)_injectionPoint;

        var modifiedRequirements = _requirement;
        _requireColor = (_requirement & ScriptableRenderPassInput.Color) != 0;
        _injectedBeforeTransparent = _injectionPoint <= InjectPoint.BeforeRenderingTransparent;

        if (_requireColor && !_injectedBeforeTransparent)
        {
            modifiedRequirements ^= ScriptableRenderPassInput.Color;
        }
        
        _pass.ConfigureInput(modifiedRequirements);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (_mat == null) return;
        
        _pass.Setup(_requireColor, _injectedBeforeTransparent, renderingData);
        
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
