using System.Collections.Generic;
using UnityEngine;

public class Upgradables_UI : MonoBehaviour
{
    [SerializeField] private UpgradeElementBuilder upgradableButtonPrefab;
    [SerializeField] private RectTransform upgradeBuilderHolder;
    [SerializeField] private Canvas myCanvas;
    [SerializeField] private float elementsDistance = 50f;
    private float upgradableSize = 100f;
    private List<UpgradeElementBuilder> uedes = new List<UpgradeElementBuilder>();
    public void ReceiveUpgradables(UpgradeData[] currentUpgradables)
    {
        upgradableSize = upgradableButtonPrefab.GetComponent<RectTransform>().sizeDelta.x;
        for (int i = 0; i < currentUpgradables.Length; i++)
        {
            Utility.DebugText($"Receiving {currentUpgradables[i].Name}");
            Vector3 calculatedPos = CalculateElementPosition(currentUpgradables.Length - 1, i);
            UpgradeElementBuilder ued = Instantiate(upgradableButtonPrefab,calculatedPos
                , Quaternion.identity, upgradeBuilderHolder);
            ued.ReceiveUpgradeDate(currentUpgradables[i]);
            ued.myRect.localPosition = new Vector3(calculatedPos.x, 0f, 0f);
            ued.OnClickedHappened += OnUpgradableClicked;
            uedes.Add(ued);
        }
    }
    private Vector3 CalculateElementPosition(int numberOfelements, int currentElement)
    {
        float screenWidth = numberOfelements * (elementsDistance + upgradableSize);
        float endXPointLeft = screenWidth /2f - 20f;
        Vector3 result = upgradeBuilderHolder.localPosition;
        float x = Utility.RemapValues(0, numberOfelements, -endXPointLeft, endXPointLeft, currentElement);
        result = new Vector3(x, 0, 0);
        return result;
    }

    public void Refresh()
    {
        OnUpgradableClicked();
    }
    private void OnUpgradableClicked()
    {
        for (int i = 0; i < uedes.Count; i++)
        {
            uedes[i].Refresh();
        }
    }
}
