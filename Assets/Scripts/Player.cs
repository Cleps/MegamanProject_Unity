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

    bool isJumping = false;
    float jumpTime = 0f;
    bool canMove;

    [Header("Others")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Camera cam;

    float horizontal;
    Rigidbody2D rig;
    float ultimaDirecao;
    Animator anim;
    

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Move();

        Jump();

        anim.SetBool("jump", !isGrounded());
    }


    void Move()
    {

        horizontal = Input.GetAxisRaw("Horizontal") * (moveSpeed);
        rig.velocity = new Vector2(horizontal, rig.velocity.y);

        
        if (horizontal < 0)
        {

            ultimaDirecao = -1f;
        }
        else if (horizontal > 0)
        {

            ultimaDirecao = 1f;
        }
        GetComponent<SpriteRenderer>().flipX = (ultimaDirecao == -1f);

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




    bool isGrounded()
    {   
        return Physics2D.CircleCast(groundCheck.position, 0.1f, Vector2.down, 0.1f, groundLayer);
    }
}
