using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    Animator animator;
    [SerializeField]
    bool fadeInOnAwake = true;
    const string fadeIn = "FadeIn";
    const string fadeOut = "FadeOut";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (fadeInOnAwake)
        {
            FadeIn();
        }
    }

    public void FadeIn()
    {
        animator.SetBool(fadeIn, true);
    }

    public void FadeOut()
    {
        animator.SetBool(fadeOut, true);
    }

    public void RefreshBools()
    {
        animator.SetBool(fadeIn, false);
        animator.SetBool(fadeOut, false);
    }
}
