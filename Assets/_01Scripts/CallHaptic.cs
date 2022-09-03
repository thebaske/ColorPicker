using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lofelt.NiceVibrations;
public class CallHaptic : MonoBehaviour
{
    public static CallHaptic _aInstance { get; private set; }
    float nextHaptic;
    private void Awake()
    {
        _aInstance = this;
    }
    public void HapticMedium()
    {
        if (Time.time > nextHaptic)
        {
            nextHaptic = Time.time + 0.1f;
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
        }
    }
    public void HapticLight()
    {
        if (Time.time > nextHaptic)
        {
            nextHaptic = Time.time + 0.1f;
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
        }
    }

    public void ManualHaptic(HapticPatterns.PresetType type)
    {
        HapticPatterns.PlayPreset(type);
    }
}
