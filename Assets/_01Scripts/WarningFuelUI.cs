using System;
using System.Collections;
using System.Collections.Generic;
using DataSystem;
using UnityEngine;
using UnityEngine.UI;

public class WarningFuelUI : MonoBehaviour
{
    [SerializeField] private Animator fuelWarninganimator;
    [SerializeField] private Image fuelImage;
    [SerializeField] private FloatValue totalFuel;
    [SerializeField] private float trasHold = 0.2f;
    private void OnEnable()
    {
        totalFuel.MyValueChanged += UpdateWarningSign;
    }

    private void OnDisable()
    {
        totalFuel.MyValueChanged -= UpdateWarningSign;
    }

    void UpdateWarningSign(float value)
    {
        if (value <= trasHold && !fuelWarninganimator.enabled)
        {
            fuelWarninganimator.enabled = true;
            fuelImage.enabled = true;
        }
    }
}
