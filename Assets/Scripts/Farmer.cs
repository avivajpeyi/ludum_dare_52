using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Farmer : MonoBehaviour
{
    public float moveSpeed = 2.5f;
    private Vector2 _moveInput;
    private bool _interactInput;

    private Vector2 _facingDir;
    private int health = 3;
    private bool isDead = false;


    private SpriteRenderer sr;

    private Sickle sickle;

    private bool _takingDamage = false;
    private Rigidbody2D rig;
    
    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        sr = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        sickle = transform.Find("sickle").GetComponent<Sickle>();
    }


    public void Interact()
    {
        _interactInput = true;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void UpdateMovement(Vector2 movementInput)
    {
        _moveInput = movementInput;
    }

    

    private void Update()
    {
        if (isDead)
        {
            rig.velocity = Vector2.zero;
            return;
        }
        
         
        // handle movement
        rig.velocity = _moveInput.normalized * moveSpeed;
        if (_moveInput.magnitude != 0.0f)
        {
            _facingDir = _moveInput.normalized;
            sr.flipX = _moveInput.x > 0;
        }

        // handle sickle swing
        if (_interactInput)
        {
            sickle.SwingSickle(_facingDir); // turns sickle off after swing
            _interactInput = false;
        }
    }


    
    void TakeDamage()
    {
        if (!_takingDamage && !isDead)
        {
            health--;
            if (health <= 0)
            {
                GameManager.instance.GameOver();
                isDead = true;
            }

            GameManager.instance.UpdateHealthText(health);
            _takingDamage = true;
        }

        StartCoroutine(RecoverFromDamage());
    }

    private IEnumerator RecoverFromDamage()
    {
        yield return new WaitForSeconds(0.3f);
        _takingDamage = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Crow"))
        {
            TakeDamage();
        }
    } 
    
}
