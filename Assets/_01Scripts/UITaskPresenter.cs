using System.Collections;
using UnityEngine;

public class UITaskPresenter : MonoBehaviour
{
    [SerializeField] private UI_TaskElementSetter teSetter;
    [SerializeField] private RectTransform teSetterParent;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 3; i++)
        {
            UI_TaskElementSetter tes = Instantiate(teSetter, Vector3.zero,
                Quaternion.identity, teSetterParent);
            tes.myRect.localPosition = Utility.CalculateElementPosition(3, i, 15f, 100);
            // tes.SetMeUp(Color.blue, Color.white);

        }
    }

}