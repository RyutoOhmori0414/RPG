using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutlineRenderPass : ScriptableRenderPass
{
    // デバッグ関係
    private const string ProfilerTag = nameof(OutlineRenderPass);
    private readonly ProfilingSampler _profilingSampler = new(ProfilerTag);
    
    ///<summary>レンダリングするタイミング</summary>
    private readonly RenderPassEvent _renderPassEvent;
    
    /// <summary>対象とするRenderQueue</summary>
    private readonly RenderQueueRange _renderQueueRange;

    /// <summary>Shader内のLightModeのTagを設定している</summary>
    private readonly ShaderTagId _shaderTagId = new(nameof(OutlineRenderPass).Replace("RenderPass", ""));

    private FilteringSettings _filteringSettings;
    
    public OutlineRenderPass()
    {
        _renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
        _renderQueueRange = RenderQueueRange.all;
        _filteringSettings = new FilteringSettings(_renderQueueRange);
        
        renderPassEvent = _renderPassEvent;
    }
    
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var cmd = CommandBufferPool.Get(ProfilerTag);
        using (new ProfilingScope(cmd, _profilingSampler))
        {
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            CommandBufferPool.Release(cmd);

            var drawingSettings =
                CreateDrawingSettings(_shaderTagId, ref renderingData, SortingCriteria.CommonTransparent);
            context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref _filteringSettings);
        }
    }
}
