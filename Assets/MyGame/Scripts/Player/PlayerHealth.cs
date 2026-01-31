using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private PlayerAnimation playerAnimation;
    private PlayerLocomotion playerLocomotion;


    private void Start()
    {
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
        playerAnimation.OnHitStart += PlayerAnimation_OnHitStart;
        playerAnimation.OnHitEnd += PlayerAnimation_OnHitEnd;
    }

    private void PlayerAnimation_OnHitStart()
    {
        playerLocomotion.StopMove();
    }

    private void PlayerAnimation_OnHitEnd()
    {
        playerLocomotion.ResumeMove();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        playerAnimation.PlayHit();
    }
}
