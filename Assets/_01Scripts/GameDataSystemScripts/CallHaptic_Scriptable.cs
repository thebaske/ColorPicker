using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="callHaptic", menuName = "Haptic")]
public class CallHaptic_Scriptable : ScriptableObject
{
    
    public void HapticMedium()
    {
        CallHaptic._aInstance.HapticMedium();
    }
    public void HapticLight()
    {
        CallHaptic._aInstance.HapticLight();
    }
}
