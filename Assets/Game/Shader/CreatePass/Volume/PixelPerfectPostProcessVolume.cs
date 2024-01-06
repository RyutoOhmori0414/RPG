using UnityEngine;
using UnityEngine.Rendering;

public class PixelPerfectPostProcessVolume : VolumeComponent
{
    [SerializeField, Tooltip("PixelPerfectのScale")]
    private FloatParameter _scale = new (1.0F);

    public float Scale => _scale.value;
}
