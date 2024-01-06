using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AfterTransparentEffectRenderPass : ScriptableRenderPass
{
    // デバッグ関係
    private const string ProfilerTag = nameof(AfterTransparentEffectRenderPass);
    private readonly ProfilingSampler _profilingSampler = new(ProfilerTag);
    
    ///<summary>レンダリングするタイミング</summary>
    private readonly RenderPassEvent _renderPassEvent;
    
    /// <summary>対象とするRenderQueue</summary>
    private readonly RenderQueueRange _renderQueueRange;

    /// <summary>Shader内のLightModeのTagを設定している</summary>
    private readonly ShaderTagId _shaderTagId = new(nameof(AfterTransparentEffectRenderPass).Replace("RenderPass", ""));

    private readonly int _customGrabTextureId = Shader.PropertyToID("_CustomGrabTex");
    
    /// <summary>カメラ</summary>
    private ScriptableRenderer _camera = default;
    
    private FilteringSettings _filteringSettings;
    
    public AfterTransparentEffectRenderPass()
    {
        _renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        _renderQueueRange = RenderQueueRange.all;
        _filteringSettings = new FilteringSettings(_renderQueueRange);
        
        renderPassEvent = _renderPassEvent;
    }

    public void Setup(ScriptableRenderer camera)
    {
        _camera = camera;
    }
    
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var cmd = CommandBufferPool.Get(ProfilerTag);
        using (new ProfilingScope(cmd, _profilingSampler))
        {
            context.ExecuteCommandBuffer(cmd);
            cmd.SetGlobalTexture(_customGrabTextureId, _camera.cameraColorTargetHandle);

            var drawingSettings =
                CreateDrawingSettings(_shaderTagId, ref renderingData, SortingCriteria.CommonTransparent);
            context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref _filteringSettings);
        }
        
        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }
}
