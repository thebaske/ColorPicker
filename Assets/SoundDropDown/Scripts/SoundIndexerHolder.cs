using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundIndexerHolder
{
    public string Name;
    [HideInInspector]public int index;
    [Range(0f, 1f)]
    public float volume;

}
