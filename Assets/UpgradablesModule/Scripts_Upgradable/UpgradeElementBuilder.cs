using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;


public class UpgradeElementBuilder : MonoBehaviour
{
    public UnityAction OnClickedHappened;
    [SerializeField] private Image elementIcon;
    [SerializeField] private TMP_Text upgradeName;
    [SerializeField] private TMP_Text upgradePrice;
    public RectTransform myRect;
    [SerializeField] private RectTransform lockedOverlay;
    [SerializeField] UpgradeData uData;
    [SerializeField] Animator anim;
    
    public void ReceiveUpgradeDate(UpgradeData ud)
    {
        uData = ud;
        Refresh();
        elementIcon.sprite = ud.image;
        // string extraInfoLevel = "";
        // string extraInfoValue = "";
        // if (uData.upgradable.writelevel)
        // {
        //     extraInfoLevel += "lvl. " + uData.upgradable.GetNextLevel();
        // }
        //
        // if (uData.upgradable.writeValue)
        // {
        //     extraInfoValue = "X" + uData.upgradable.GetIntValueAtLevel(uData.upgradable.GetNextLevel()).ToString();
        // }
        // upgradeName.text = ud.Name + " " + extraInfoLevel;
        // upgradePrice.text = Utility.NumberFormat(uData.upgradable.GetCurrentPrice());
        
        // ud.upgradable.OnUpgradeSuccessfull += 

    }

    public void Refresh()
    {
        string extraInfoLevel = "";
        string extraInfoValue = "";
        if (uData.upgradable.writelevel)
        {
            extraInfoLevel += "lvl. " + uData.upgradable.GetNextLevel();
        }

        if (uData.upgradable.writeValue)
        {
            extraInfoValue = "X" + uData.upgradable.GetIntValueAtLevel(uData.upgradable.GetNextLevel()).ToString();
        }
        upgradeName.text = uData.Name + " " + extraInfoLevel;
        upgradePrice.text = Utility.NumberFormat(uData.upgradable.GetCurrentPrice());
         lockedOverlay.gameObject.SetActive(!uData.upgradable.CanUpgrade());
         Utility.DebugText($"{uData.upgradable.name} - current level {uData.upgradable.GetNextLevel() - 1}, next level {uData.upgradable.GetNextLevel()}");
         if (!uData.upgradable.IsMaxedOut())
         {
             upgradePrice.text = Utility.NumberFormat(uData.upgradable.GetCurrentPrice());
         }
         else
         {
             upgradePrice.text = "MAXED OUT";
         }
    }
    public void Unavailable()
    {
        lockedOverlay.gameObject.SetActive(true);
    }
    public void OnClickedHandler()
    {
        uData.upgradable.TryUpgrade();
        anim.enabled = true;
        Invoke("DeactivateAnimator", anim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        Refresh();
        // OnClickedHappened?.Invoke();
    }

    void DeactivateAnimator()
    {
        anim.Play(0, 0, 0F);
        anim.enabled = false;
        lockedOverlay.localScale = Vector3.one;
    }
}
