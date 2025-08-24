using UnityEngine;


public class PlayerAnimationController : MonoBehaviour
{

    private Animator animator;

    private void Awake()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();
    }

    public void TriggerDanceAnimation(string dance)
    {
        switch (dance)
        {
            case "MOON STROLL":
                animator.Play(dance, 0, 0f);
                break;
            case "NATURE'S CALL":
                animator.Play("Nature's Call", 0, 0f);
                break;
            case "PAW SHAKE":
                animator.Play("Paw Shake", 0, 0f);
                break;
            case "BELLY DANCE":{}
                animator.Play("Belly Dance", 0, 0f);
                break;
            case "NOSE-KICK":
                animator.Play("Nose Kick", 0, 0f);
                break;
            case "SLEDGING":
                animator.Play("Sledging", 0, 0f);
                break;
            default:
                animator.Play("Idle 2");
                break;
        }
    }
}