using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnOff_UI : MonoBehaviour
{
    [SerializeField] private GameObject toTurn;

    public void SwitchMe()
    {
        toTurn.gameObject.SetActive(!toTurn.gameObject.activeSelf);
    }
}
