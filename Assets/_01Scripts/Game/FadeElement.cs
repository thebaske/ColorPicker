using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
namespace Feedbacks
{
    public class FadeElement : MonoBehaviour
    {
        [SerializeField]FeedbackAnimatorHolder animHolder;
        Image img;
        RawImage rawImg;
        TMP_Text text;
        Color startColor;
        Color fadedCOlor;


        bool fade;
        [SerializeField] AnimationCurve fadecurve;
        public float lerpValue;
        private void Start()
        {
            animHolder = GetComponentInParent<FeedbackAnimatorHolder>();
            animHolder.OnFeedBackRunning += Fade;
            img = GetComponent<Image>();
            rawImg = GetComponent<RawImage>();
            text = GetComponent<TMP_Text>();
            if (img != null)
            {
                startColor = img.color;
            }
            else if (rawImg != null)
            {
                startColor = rawImg.color;
            }
            else if (text != null)
            {
                startColor = text.color;
            }
            fadedCOlor = new Color(startColor.r, startColor.g, startColor.b, 0f);
            FullFade();
        }

        void Fade(float duration, float currentTime)
        {
            if (currentTime == duration)
            {
                FullOpacity();
            }
            lerpValue = fadecurve.Evaluate(Utility.RemapValues(duration, 0f, 0f, 1f, currentTime));
            Color usableColor = Color.Lerp(startColor, fadedCOlor, lerpValue);
            if (img != null)
            {
                img.color = usableColor;
            }
            else if (rawImg != null)
            {
                rawImg.color = usableColor;
            }
            else if (text != null)
            {
                text.color = usableColor;
            }
        }
        void FullFade()
        {
            if (img != null)
            {
                img.color = fadedCOlor;
            }
            else if (rawImg != null)
            {
                rawImg.color = fadedCOlor;
            }
            else if (text != null)
            {
                text.color = fadedCOlor;
            }
        }
        void FullOpacity()
        {
            if (img != null)
            {
                img.color = startColor;
            }
            else if (rawImg != null)
            {
                rawImg.color = startColor;
            }
            else if (text != null)
            {
                text.color = startColor;
            }
        }

    }
}
