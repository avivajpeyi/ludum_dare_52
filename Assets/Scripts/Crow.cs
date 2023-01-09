using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Crow : MonoBehaviour
{
    private Vector2 startPosition;
    private Vector2 endPosition;
    private Vector2 direction;
    private float speed;
    

    private void Start()
    {
        speed = Random.Range(3.5f, 6.5f);
        startPosition = transform.position;
        Vector2 target = FindObjectOfType<Scarecrow>().transform.position;
        // get direction to target
        direction = (target - startPosition).normalized;
        endPosition = startPosition + direction * 100;
    }


    private void Update()
    {
        FlyTo(endPosition);
        FlipIfNecessary();
    }

    void FlyTo(Vector2 position)
    {
        transform.position = Vector2.MoveTowards(
            transform.position, position,
            speed * Time.deltaTime
        );
    }
    
    
    public float distanceFrom(Vector3 point)
    {
        return Vector3.Distance(point, transform.position);
    }

    private void FlipIfNecessary()
    {
        if (direction.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }


    public void Die()
    {
        Destroy(gameObject);
    }
}