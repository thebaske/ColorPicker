using System.Collections;
using DataSystem;
using TMPro;
using UnityEngine;

public class WriteLevel : MonoBehaviour
{
    [SerializeField] private IntValue playerLevel;
    [SerializeField] private TMP_Text text;
    private IEnumerator Start()
    {
        playerLevel.MyValueChanged += UpdateValue;
        UpdateValue(playerLevel.MyValue);
        yield return new WaitForSeconds(0.1f);
        UpdateValue(playerLevel.MyValue);

    }

    private void OnDisable()
    {
        playerLevel.MyValueChanged -= UpdateValue;
    }

    void UpdateValue(int value)
    {
        text.text = (value + 1).ToString();
    }
}
