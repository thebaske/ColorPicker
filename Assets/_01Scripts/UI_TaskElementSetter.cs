using TMPro;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

public class UI_TaskElementSetter : MonoBehaviour
{
    [SerializeField] ProceduralImage pImage;
    [SerializeField] private TMP_Text amountTxt;

    public RectTransform myRect;
    public void SetMeUp(Material mat)
    {
        pImage.color = mat.color;
        
    }
    void TrackGoalAmount(int value)
    {
    }
}
