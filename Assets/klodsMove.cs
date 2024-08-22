using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class klodsMove : MonoBehaviour
{
    [SerializeField] public float klodsSpeed = 1f;

    Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(klodsSpeed, rb.velocity.y);   
    }
}
