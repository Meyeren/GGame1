using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class playerController : MonoBehaviour
{

    // Variabler for movement
    [SerializeField] public float speed = 1f;
    private float inputX;
    [SerializeField] public float jumpForce = 1f;

    // Variabel for rigidbody
    private Rigidbody2D rb;

    // Variabel for karakterdirektion
    [SerializeField] private bool facingRight = false;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool isGrounded = true;

    [SerializeField] private float fallTime = 1.5f;

    private bool isJumping;
    [SerializeField] private float jumpTime = 0.5f;
    private float jumpTimeCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);


        // Variablen inputX bliver sat til det rå input fra det horizontale knapper
        inputX = Input.GetAxisRaw("Horizontal");
        if (inputX > 0 && facingRight)
        {
            Flip();
        }
        if (inputX < 0 && !facingRight)
        {
            Flip();
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {

            rb.velocity = new Vector2(inputX, jumpForce);
            //rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isJumping = true;
            jumpTimeCounter = jumpTime;
        }
        if (Input.GetButtonDown("Jump") && isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimeCounter -= Time.deltaTime;
        }
        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
    }

    private void FixedUpdate()
    {
        // Rigidbody velocity sættes lig en ny vektor med inputtet og hastigheden speed
        rb.velocity = new Vector2(inputX * speed, rb.velocity.y);
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * (Physics2D.gravity * fallTime * Time.fixedDeltaTime);
        }
    }

    void Flip()
    {
        // Skalaen af karakteren gemmes i en vektor, hvorefter at denne skala ganges med -1 for at flippe karakteren
        Vector3 tempScale = transform.localScale;
        tempScale.x *= -1f;
        transform.localScale = tempScale;
        facingRight = !facingRight;
    }
}
