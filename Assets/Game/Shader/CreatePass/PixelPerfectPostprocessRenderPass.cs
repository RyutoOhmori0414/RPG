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
    private RTHandle _copiedColor;
    private RenderTargetIdentifier _cameraColorTarget;
    
    private PixelPerfectPostProcessVolume _volume;

    private bool _requiresColor;
    private bool _isBeforeTransparent;

    public PixelPerfectPostprocessRenderPass(bool applyToSceneView, Material mat)
    {
        if (!mat) return;
        
        _applyToSceneView = applyToSceneView;
        _profilingSampler = new ProfilingSampler(ProfilingSamplerName);
        _mat = mat;
    }

    public void SetTarget(RTHandle handle)
    {
        _colorTarget = handle;
    }

    public void Setup(bool requireColor, bool isBeforeTransparent, in RenderingData renderingData)
    {
        _requiresColor = requireColor;
        _isBeforeTransparent = isBeforeTransparent;
        
        var colorCopyDescriptor = renderingData.cameraData.cameraTargetDescriptor;
        colorCopyDescriptor.depthBufferBits = (int) DepthBits.None;
        RenderingUtils.ReAllocateIfNeeded(ref _copiedColor, colorCopyDescriptor, name: "_PixelPerfectColorCopy");
        
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
            (!_applyToSceneView && renderingData.cameraData.cameraType == CameraType.SceneView) || !_volume.IsActive()) return;

        var cmd = CommandBufferPool.Get(ProfilingSamplerName);
        var cameraData = renderingData.cameraData;

        _colorTarget = renderingData.cameraData.renderer.cameraColorTargetHandle;
        
        using (new ProfilingScope(cmd, _profilingSampler))
        {
            _mat.SetFloat(_scalePropertyId, _volume.Scale);
            
            Blitter.BlitCameraTexture(cmd, _colorTarget, _colorTarget, _mat, 0);
        }
        
        CoreUtils.SetRenderTarget(cmd, _colorTarget);
        //CoreUtils.DrawFullScreen(cmd, _mat);
        
        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }

    public void Dispose()
    {
        _copiedColor.Release();
    }
}
