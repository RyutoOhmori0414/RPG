using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelPerfectPostprocessRenderPass : ScriptableRenderPass
{
    private const string ProfilingSamplerName = "PixelPerfectPostSrcToDst";

    private readonly bool _applyToSceneView;
    private readonly Material _mat;
    private readonly ProfilingSampler _profilingSampler;
    private readonly int _scalePropertyId = Shader.PropertyToID("_Scale");

    private RTHandle _colorTarget;
    private RenderTargetIdentifier _cameraColorTarget;
    
    private PixelPerfectPostProcessVolume _volume;

    public PixelPerfectPostprocessRenderPass(bool applyToSceneView, Material mat)
    {
        if (!mat) return;
        
        renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

        _applyToSceneView = applyToSceneView;
        _profilingSampler = new ProfilingSampler(ProfilingSamplerName);
        _mat = mat;
    }

    public void SetTarget(RTHandle handle)
    {
        _colorTarget = handle;
        
        var stack = VolumeManager.instance.stack;
        _volume = stack.GetComponent<PixelPerfectPostProcessVolume>();
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        if (_colorTarget == null) return;
        ConfigureTarget(_colorTarget);
    }
    
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (!_mat || !renderingData.cameraData.postProcessEnabled ||
            (!_applyToSceneView && renderingData.cameraData.cameraType == CameraType.SceneView)) return;

        var cmd = CommandBufferPool.Get(ProfilingSamplerName);

        using (new ProfilingScope(cmd, _profilingSampler))
        {
            _mat.SetFloat(_scalePropertyId, _volume.Scale);
            
            Blitter.BlitCameraTexture(cmd, _colorTarget, _colorTarget, _mat, 0);
        }
        
        Debug.Log("Tes");
        
        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }
}
