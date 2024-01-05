using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed;
    public float jumpSpeed;
    public float maxJumpTime;
    public float jump_height;
    public Vector2 wallJumpingPower = new Vector2(8f, 16f);
    public float wallJumpingDuration = 0.4f;
    public float wallJumpingTime = 0.2f;
    public float wallSlidingSpeed = 20;

    bool isJumping = false;
    float jumpTime = 0f;
    bool canMove;
    bool isWallSliding;

    bool isFacingRight = true;
    bool isWallJumping;
    float wallJumpingDirection;
    float wallJumpingCounter;
    
    

    [Header("Others")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform wallCheck;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] Camera cam;

    float horizontal;
    Rigidbody2D rig;
    float ultimaDirecao = 1;
    Animator anim;
    

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Jump();

        WallSlide();

        WallJump();

        anim.SetBool("jump", !isGrounded());
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {

        horizontal = Input.GetAxisRaw("Horizontal") * (moveSpeed);
        if (!isWallJumping)
            rig.velocity = new Vector2(horizontal, rig.velocity.y);

        
        if (horizontal < 0)
        {

            ultimaDirecao = -1f;
        }
        else if (horizontal > 0)
        {

            ultimaDirecao = 1f;
        }

        //GetComponent<SpriteRenderer>().flipX = (ultimaDirecao == -1f); // modifique aqui
        transform.localScale = new Vector3(ultimaDirecao, 1f, 1f);

        if (horizontal == 0){
            anim.SetBool("idle", true);
        }else{
            anim.SetBool("idle", false);
        }
    }


    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded())
            {
                isJumping = true;
                jumpTime = 0f;
            }
            else if (isJumping)
            {
                isJumping = false; // Impede pulos consecutivos enquanto no ar
            }
        }

        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTime < maxJumpTime)
            {
                // Se o botão estiver sendo mantido pressionado, pule alto; caso contrário, pule baixo
                float currentJumpSpeed = Input.GetButton("Jump") ? jumpSpeed : jumpSpeed / 2f;
                rig.velocity = new Vector2(rig.velocity.x, currentJumpSpeed + (currentJumpSpeed * jump_height / 100)); // porcentagem
                jumpTime += Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        else if (Input.GetButtonUp("Jump"))
        {
            isJumping = false; // Garante que o pulo pare quando o botão é solto
        }
    }

    void WallSlide()
    {
        anim.SetBool("inWall", isWallSliding);
        if (isWalled() && !isGrounded() && horizontal != 0)
        {
            isWallSliding = true;
            rig.velocity = new Vector2(rig.velocity.x, Mathf.Clamp(rig.velocity.y, wallSlidingSpeed, float.MinValue));
           
        }
        else
        {
            isWallSliding = false;
        }
    }


    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rig.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }


    bool isWalled()
    {
        return Physics2D.CircleCast(wallCheck.position, 0.1f, Vector2.down, 0.1f, wallLayer);
    }

    bool isGrounded()
    {   
        return Physics2D.CircleCast(groundCheck.position, 0.1f, Vector2.down, 0.1f, groundLayer);
    }
}
