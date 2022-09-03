using UnityEngine;

public class AnimatorEnabler : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject animatedObject;
    private bool inProgress;
    public void EnableAnimator()
    {
        if (inProgress) return;
        inProgress = true;
        animatedObject.SetActive(true);
        anim.enabled = true;
        Invoke("DisableAnimator", anim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
    }

    void DisableAnimator()
    {
        anim.enabled = false;
        inProgress = false;
        animatedObject.SetActive(false);
    }
}
