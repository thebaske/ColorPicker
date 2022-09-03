using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DataSystem;
[System.Serializable]

public class WriteUI_INT : MonoBehaviour
{
    private TMP_Text myTextField;
    public bool animateValueChage;
    public bool usePrefix;
    public bool useSufix;
    public string prefix;
    public string suFix;
    public IntValue intValue;

    [SerializeField] UnityEvent OnValueChanged;
    public bool autSubscribe;
    private int oldValue;
    private void OnEnable()
    {
        if (myTextField != null)
        {
            if (autSubscribe)
            {
                ReadValueOnChange();
                intValue.MyValueChanged += MyValueChangedListenerInt;
            }
        }
        else
        {
            myTextField = GetComponent<TMP_Text>();
            if (myTextField != null)
            {
                if (autSubscribe)
                {
                    ReadValueOnChange();
                    intValue.MyValueChanged += MyValueChangedListenerInt;
                }
            }
            else
            {
                Utility.DebugText($"TextField not assigned {gameObject.name}");
            }

        }
    }

    public void SetMeUp()
    {
        ReadValueOnChange();
        intValue.MyValueChanged += MyValueChangedListenerInt;
        
    }
    [ContextMenu("TestMethod")]
    public void TestValue()
    {
        intValue.MyValue += 100;
    }
    public void ReadValueOnChange()
    {   
        StartCoroutine(ValueUpdate(intValue.Value));
    }
    private void MyValueChangedListenerInt(int value)
    {
        OnValueChanged?.Invoke();
     
        if (!animateValueChage)
        {  
            SetText(prefix, Utility.NumberFormat(intValue.MyValue), suFix);
        }
        else
        {
            StartCoroutine(ValueUpdate(value));
        }
    }
    private void SetText(string prefix, string value, string suffix)
    {
        myTextField.text = prefix + value + suffix;
    }

    
    private void OnDisable()
    {   
           intValue.MyValueChanged -= MyValueChangedListenerInt;
    }
    public IEnumerator ValueUpdate(int newValue)
    {
        float counter = 0.5f;
        while (counter > 0)
        {
            counter -= Time.deltaTime;
            SetText(prefix,  Utility.NumberFormat((int)Utility.RemapValues(1.5f, 0, oldValue, newValue, counter)), suFix);
            yield return null;
        }
        oldValue = newValue;
    }
}
