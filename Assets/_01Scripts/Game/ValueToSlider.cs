using DataSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI.ProceduralImage;

public class ValueToSlider : MonoBehaviour
{
    [SerializeField] private UnityEvent OnvalueChanged;
    [SerializeField] private ProceduralImage fillImage;
    [SerializeField] private FloatValue Progression;

    private void OnEnable()
    {
        Progression.MyValueChanged += FillImage;
    }

    private void OnDisable()
    {
        Progression.MyValueChanged -= FillImage;
    }

    private void FillImage(float value)
    {
        fillImage.fillAmount = value;
        OnvalueChanged?.Invoke();
    }
}
