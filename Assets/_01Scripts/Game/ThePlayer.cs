using System;
using System.Collections;

using UnityEngine;
using DataSystem;
using UnityEngine.Events;
using Upgradables;

public class ThePlayer : MonoBehaviour, IPlayarable
{
    public ThePlayer GetPlayer()
    {
        return this;
    }
}
