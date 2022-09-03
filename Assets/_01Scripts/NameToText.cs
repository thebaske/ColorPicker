using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameToText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    void OnEnable()
    {
        text.text = gameObject.name.ToString();
    }

   
}
