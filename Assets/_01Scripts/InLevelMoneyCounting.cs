using System;
using System.Collections;
using System.Collections.Generic;
using DataSystem;
using TMPro;
using UnityEngine;

public class InLevelMoneyCounting : MonoBehaviour
{
    [SerializeField] private TMP_Text earnedMoneyText;
    [SerializeField] private IntValue inGameScore;
    private int startingValue;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.2f);
        startingValue = inGameScore.MyValue;
    }

    public void ShowScore()
    {
        earnedMoneyText.text = "+" + (inGameScore.MyValue - startingValue).ToString() + "$";
    }
}
