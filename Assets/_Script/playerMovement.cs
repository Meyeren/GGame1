using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class playerController : MonoBehaviour
{

    // Variabler for movement
    [SerializeField] public float speed = 1f;
    private float inputX;
    [SerializeField] public float jumpForce = 1f;

    // Variabel for rigidbody
    private Rigidbody2D rb;

    // Variabel for karakterdirektion
    [SerializeField] private bool facingRight = true;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    [SerializeField] private float fallTime = 1.5f;

    private bool isJumping;
    [SerializeField] private float jumpTime = 0.5f;
    private float jumpTimeCounter;

    //Wallslide
    private bool isWallSliding;
    [SerializeField] private float wallSlidingSpeed = 20f;
    private float pushAway = 2f;

    // Variabel for dash
    private bool canDash = true;
    private bool isDashing;
    private bool hasAirDashed = false;
    [SerializeField] private float dashingPower = 30f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;

    // Variable for wall jump
    private bool isWallJumping;
    private float wallJumpingDirection;
    [SerializeField] private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    [SerializeField] private float wallJumpingDuration = 0.4f;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(8f, 16f);


    //Nofriktion
    public PhysicsMaterial2D noFrictionMaterial;
    private PhysicsMaterial2D originalMaterial;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalMaterial = rb.sharedMaterial;

        // Skab et materiale uden friktion, hvis det ikke er sat
        if (noFrictionMaterial == null)
        {
            noFrictionMaterial = new PhysicsMaterial2D();
            noFrictionMaterial.friction = 0;
            noFrictionMaterial.bounciness = 0;
        }
    }


    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        //wall?
        WallSlide();
        WallJump();

        if (isWallJumping)
        {
            Flip();
        }

        //check for dashes brugt i luften
        if (isGrounded)
        {
            hasAirDashed = false;
            canDash = true;
        }

        // Variablen inputX bliver sat til det rå input fra det horizontale knapper
        inputX = Input.GetAxisRaw("Horizontal");
        if (inputX > 0 && !facingRight)
        {
            Flip();
        }
        if (inputX < 0 && facingRight)
        {
            Flip();
        }
        // idk det er en del af dashing ig
        if (isDashing)
        {
            return;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isJumping = true;
                jumpTimeCounter = jumpTime;
            }
            else if (isWallSliding || (isWallJumping && wallJumpingCounter > 0f))
            {
                rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
                isWallJumping = true;
                wallJumpingCounter = 0f;
                Invoke(nameof(StopWallJumping), wallJumpingDuration);
            }
        }

        if (Input.GetKeyDown(KeyCode.F) && canDash)
        {
            if (isGrounded || (!isGrounded && !hasAirDashed))
            {
                StartCoroutine(Dash());
            }
        }
        // Leftshift for at dashe
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            if (isGrounded || (!isGrounded && !hasAirDashed))
            {
                StartCoroutine(Dash());
            }
        }
    }

    private void FixedUpdate()
    {
        //walljump
        if (!isWallJumping && !isDashing)
        {
            rb.velocity = new Vector2(inputX * speed, rb.velocity.y);
        }

        // igen idk men det er en del af dashing
        if (isDashing)
        {
            return;
        }
        // Rigidbody velocity sættes lig en ny vektor med inputtet og hastigheden speed
        rb.velocity = new Vector2(inputX * speed, rb.velocity.y);
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * (Physics2D.gravity * fallTime * Time.fixedDeltaTime);
        }
    }

    //wall
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }
    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && Mathf.Abs(inputX) > 0)
        {
            rb.sharedMaterial = noFrictionMaterial;

            isWallSliding = true;
            rb.velocity = new Vector2(pushAway * -Mathf.Sign(inputX), -wallSlidingSpeed);
            Debug.Log("slide");
        }
        else
        {
            isWallSliding = false;
        }
    }

    //walljump
    private void WallJump()
    {
        if (isWallSliding)
        {
       
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;  
            wallJumpingCounter = wallJumpingTime;
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            
            if (transform.localScale.x != wallJumpingDirection)
            {
                Flip();
            }

            
            isWallSliding = false;

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    void Flip()
    {
        // Skalaen af karakteren gemmes i en vektor, hvorefter at denne skala ganges med -1 for at flippe karakteren
        Vector3 tempScale = transform.localScale;
        tempScale.x *= -1f;
        transform.localScale = tempScale;
        facingRight = !facingRight;
    }

    // big dash
    private IEnumerator Dash()
    {
        if (!isGrounded)
        {
            hasAirDashed = true;
        }
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
