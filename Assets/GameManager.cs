
using System;
using DataSystem;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private IntValue playerLevel;

    public void OnGameSuccess()
    {
        playerLevel.MyValue++;
    }
}
