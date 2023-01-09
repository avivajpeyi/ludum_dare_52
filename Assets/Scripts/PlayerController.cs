using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3;

    private Vector2 _moveInput;
    private bool _interactInput;

    private Vector2 _facingDir;
    private int health = 3;
    private bool isDead = false;


    public Rigidbody2D rig;
    public SpriteRenderer sr;

    public Sickle sickle;

    private bool _takingDamage = false;


    private void Update()
    {
        if (isDead)
            return;

        if (_moveInput.magnitude != 0.0f)
        {
            _facingDir = _moveInput.normalized;
            sr.flipX = _moveInput.x > 0;
        }

        if (_interactInput)
        {
            sickle.SwingSickle(_facingDir); // turns sickle off after swing
            _interactInput = false;
        }
    }


    private void FixedUpdate()
    {
        if (isDead)
            rig.velocity = Vector2.zero;
        else
            rig.velocity = _moveInput.normalized * moveSpeed;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (isDead)
            return;
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (isDead)
            return;
        if (context.phase == InputActionPhase.Performed)
        {
            _interactInput = true;
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