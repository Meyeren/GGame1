using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platFall : MonoBehaviour
{

    public GameObject FP;

    Rigidbody2D rb;

    private void Start()
    {
        // Få fat i Rigidbody2D fra platformen
        rb = FP.GetComponent<Rigidbody2D>();
        // Sørg for, at platformen ikke bevæger sig i starten
        rb.isKinematic = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Tjek om objektet der rammer er den lodrette platform
        if (collision.gameObject.tag == "DeathPlat")
        {
            // Aktivér Rigidbody ved at gøre den ikke-kinematisk
            rb.isKinematic = false;
            // Hvis du har brug for at tilføje tyngdekraft, sæt Gravity Scale
            rb.gravityScale = 1;
        }
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "platFormDestroy")
        {
            Destroy(gameObject);
        }
    }
}

