using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Perdu (looser)");
        }

        if (collision.CompareTag("Enemy"))
        {
            
            StartCoroutine(WaitCollision());
        }
    }

    IEnumerator WaitCollision()
    {
        GetComponent<EnemyController>().StopMovement();
        yield return new WaitForSeconds(2);
        GetComponent<EnemyController>().StartMovement();
    }
}
