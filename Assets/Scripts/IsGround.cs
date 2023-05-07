using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGround : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    [SerializeField] private float jumpPower;
    [SerializeField] private Player player;

    private RaycastHit2D hit;
    private bool isGrounded;
    private Animator playerAnim;
    private Rigidbody2D playerRb;

    //private bool doubleJump;

    private void Awake()
    {
        playerAnim = player.GetComponent<Animator>();
        playerRb = player.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (player.isStart == false) return;

        hit = Physics2D.Raycast(transform.position, Vector2.down, 0.3f, layer);

        RaycastHit(hit);

        GetJump();

        // if(playerRb.velocity.y > .1f) playerAnim.SetTrigger("Jumping");
        // else if(playerRb.velocity.y < -.1f) playerAnim.SetTrigger("Falling");
        if (playerRb.velocity.y > .1f)
        {
            playerAnim.SetBool("Jumping", true);
            playerAnim.SetBool("Falling", false);
        }
        else if (playerRb.velocity.y < -.1f)
        {
            playerAnim.SetBool("Jumping", false);
            playerAnim.SetBool("Falling", true);
        }
    }

    private void RaycastHit(RaycastHit2D hit)
    {
        if (hit.collider)
        {
            isGrounded = true;
            playerAnim.SetBool("Jumping", false);
            playerAnim.SetBool("Falling", false);
            //doubleJump = false;
        }

        else isGrounded = false;
    }

    private void JumpFonc()
    {
        var rbVelocity = playerRb.velocity;

        playerRb.velocity = new Vector2(rbVelocity.x, rbVelocity.y + jumpPower);

        SoundManager.Instance.PlaySfx("Jump");
    }

    private void GetJump()
    {
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            SoundManager.Instance.PlaySfx("Jump");
            //doubleJump = true;
            JumpFonc();
        }

        /*if (Input.GetKeyDown(KeyCode.W) && !isGrounded && doubleJump) // Double Jump koymak istenirse
        {
            JumpFonc();
            doubleJump = false;
        }*/
    }
}