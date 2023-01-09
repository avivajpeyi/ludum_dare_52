using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowAnimation : MonoBehaviour
{
    public Sprite[] crowSprites;

    public float frameRate = 0.2f;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        while (true)
        {
            for (int i = 0; i < crowSprites.Length; i++)
            {
                spriteRenderer.sprite = crowSprites[i];
                yield return new WaitForSeconds(frameRate);
            }
        }
    }
}