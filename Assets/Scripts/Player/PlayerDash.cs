using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("Dash")]
    public bool canDash = true;
    public bool isDashing;
    public float speedDash;
    public float timeDash;

    float currentTimeDash;
    float originalGravity;
    Player playerScript;
    Rigidbody2D rbPlayer;
    Animator anim;
    bool hasFunctionBeenCalled = false;
    bool airdash;


    void Start()
    {
        rbPlayer =GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerScript = FindObjectOfType<Player>();

        originalGravity = rbPlayer.gravityScale;
        currentTimeDash = timeDash;
        canDash = true;
        isDashing = false;
    }

    void Update()
    {
        if (timeDash <= 0)
        {
            if (!hasFunctionBeenCalled){
                zeraY();
                hasFunctionBeenCalled = true;
            }
            rbPlayer.gravityScale = originalGravity;
            isDashing = false;
            canDash = false;
            anim.SetBool("dash", false);
        }else{
            hasFunctionBeenCalled = false;
        }

        if (Input.GetMouseButton(1) && playerScript.isGrounded()) // air dash?
        {
            timeDash -= Time.deltaTime;
            if(canDash && !airdash){
                Dash();
            }

        }

        else if(Input.GetMouseButtonUp(1))
        {
            rbPlayer.gravityScale = originalGravity;
            canDash = true;
            isDashing = false;
            anim.SetBool("dash", false);
            timeDash = currentTimeDash;

            if (!hasFunctionBeenCalled){
                //zeraY();
                hasFunctionBeenCalled = true;
            }
        }

        if(playerScript.isGrounded() && airdash){
            airdash = false;
            hasFunctionBeenCalled = true;
        }
    

    }

    private void Dash()
    {
        
        isDashing = true;
        //rbPlayer.gravityScale = 0f;
        anim.SetBool("dash", true);

        rbPlayer.velocity = new Vector2(playerScript.transform.localScale.x * speedDash, 0f);// rbPlayer.velocity.y);
        //transform.position += new Vector3(playerScript.transform.localScale.x * speedDash, 0f, 0f);

        if (!playerScript.isGrounded() && isDashing){
            airdash = true;
        }else{
            airdash = false;
        }

    }

    private void zeraY(){
        rbPlayer.velocity = new Vector2(0f, rbPlayer.velocity.y);
    }


}
