using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;
using TMPro;

public class PopupValueDisplayHandler : MonoBehaviour
{
    [SerializeField] TMP_Text popupValueText;
    [SerializeField] ProceduralImage icon;
    public RectTransform myRect;
    [SerializeField]Animator popupAnimator;
    //icons for money, scraps, damage/headshot
    public Sprite[] valueIcons;


    public void SetPopup(int value)
    {
        //popupValueText.color = CalculateColor(type);
        popupValueText.text = "+" + value.ToString();
        popupAnimator.enabled = true;
        Invoke("DisableAnimator", popupAnimator.GetCurrentAnimatorStateInfo(0).length);
    }

    public void SetPopup(string value)
    {
        popupValueText.text = value;
        popupAnimator.enabled = true;
        Invoke("DisableAnimator", popupAnimator.GetCurrentAnimatorStateInfo(0).length);
    }
    public void SetPopup(string prefix, int value, string suffix, Color color)
    {
        //popupValueText.color = CalculateColor(type);
        popupValueText.text = prefix + value.ToString() + suffix;
        popupValueText.color = color;
        popupAnimator.enabled = true;
        Invoke("DisableAnimator", popupAnimator.GetCurrentAnimatorStateInfo(0).length);
    }

    void DisableAnimator()
    {
        popupAnimator.enabled = false;
        popupValueText.text = "";
    }


    public Color CalculateColor(int type)
    {
        Color result;
        switch (type)
        {
            case 0:
                result = Color.yellow;
                break;
            case 1:
                result = Color.black;
                break;
            default:
                result = Color.white;
                break;
        }
        return result;
    }
}
