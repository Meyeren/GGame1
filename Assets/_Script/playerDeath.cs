using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerDeath : MonoBehaviour
{
    // Navnet p� det lag, der bruges til d�dszone

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Tjek om objektet, spilleren kolliderer med, er p� "Death" lag
        if (collision.gameObject.layer == LayerMask.NameToLayer("Death"))
        {
            RestartScene(); // Genstart scenen, hvis spilleren rammer en d�d zone
            Debug.Log("Rskbit");
        }
    }

    // Metode til at genstarte scenen
    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Restart");
    }
}
