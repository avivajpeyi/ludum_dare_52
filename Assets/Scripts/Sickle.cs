using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sickle : MonoBehaviour
{
    public float startAngle = -30;
    public float endAngle = -150;
    public float swingTime = 0.3f;

    private Vector3 rootPoint = Vector3.zero;
    private bool _isSwinging = false;

    private CapsuleCollider2D myCol;

    private Vector3 initialPos;
    private Quaternion initialRot;

    private void Start()
    {
        myCol = GetComponent<CapsuleCollider2D>();
        initialPos = transform.localPosition;
        initialRot = transform.localRotation;
        Reset();
    }


    private void Reset()
    {
        myCol.enabled = false;
        transform.localPosition = initialPos;
        transform.rotation = initialRot;
    }

    public void Update()
    {
        if (_isSwinging)
            transform.position = transform.root.position + rootPoint;
        else
            Reset();
    }


    public void SwingSickle(Vector2 swingPoint)
    {
        if (_isSwinging)
            return;

        myCol.enabled = true;
        _isSwinging = true;
        this.rootPoint = swingPoint / 2f;
        transform.localRotation = Quaternion.Euler(0, 0,
            Mathf.Atan2(swingPoint.y, swingPoint.x) * Mathf.Rad2Deg);

        StartCoroutine(RotateSickle90());
    }

    IEnumerator RotateSickle90()
    {
        Vector3 angleStart = Quaternion.Euler(0, 0, startAngle).eulerAngles;
        Vector3 angleEnd = Quaternion.Euler(0, 0, endAngle).eulerAngles;

        // var baseAngle = transform.rotation;
        var fromAngle = Quaternion.Euler(transform.eulerAngles + angleStart);
        var toAngle = Quaternion.Euler(transform.eulerAngles + angleEnd);
        for (var t = 0f; t < 1; t += Time.deltaTime / swingTime)
        {
            transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
            yield return null;
        }

        _isSwinging = false;
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        FieldTile tile = col.GetComponent<FieldTile>();
        if (tile != null)
        {
            tile.Interact();
        }
    }
}