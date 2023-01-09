using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarecrow : MonoBehaviour
{
    int health = 100;

    void TakeDamage()
    {
        health -= 10;
    }
    
    void GameOverCheck()
    {
        if (health <= 0)
        {
            Debug.Log("Game Over");
            // Trigger GameOver event
            GameManager.instance.GameOver();
            Destroy(this);
        }
    }
    
    
    void Update()
    {
        GameOverCheck();
    }
    
}
