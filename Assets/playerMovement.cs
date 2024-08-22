using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{

    private RigidBody2D rb;
    public float speed = 3f;
    private float inputX;

    private float jumpForce = 16f;
    private bool isFacingRight = true;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool isGrounded;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<RigidBody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");

        //Vender karakteren den vej man bevæger sig
        if (inputX < 0 && isFacingRight)
        {
            flip()
        }
        if (inputX > 0 && !isFacingRight)
        {
            flip()
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 tempScale = transform.localScale;
        tempScale.x *= -1;
        transform.localScale = tempScale;
    }
