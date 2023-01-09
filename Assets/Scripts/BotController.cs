using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Farmer))]
public class BotController : MonoBehaviour
{
    public float crowDangerRadius = 3f;

    public CrowFactory crowFactory;
    public FieldManager fieldManager;

    private List<GameObject> plants, crows;
    private Dictionary<int, float> plantDistances, crowDistances;
    private Vector2 _moveInput;
    private bool _interactInput;
    private Farmer farmer;

    [SerializeField] private Vector2 targetPos = Vector2.negativeInfinity;
    [SerializeField] private float distToTarget;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(targetPos, Vector3.one * 0.2f);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (crowFactory == null)
        {
            crowFactory = FindObjectOfType<CrowFactory>();
        }

        if (fieldManager == null)
        {
            fieldManager = FindObjectOfType<FieldManager>();
        }

        farmer = GetComponent<Farmer>();
    }


    private void Update()
    {
        if (farmer.IsDead())
            return;

        ComputeMoveInput();
        farmer.UpdateMovement(_moveInput);

        ComputeInteractInput();
        if (_interactInput)
        {
            farmer.Interact();
            _interactInput = false;
        }
    }

    private void ExpensiveUpdate()
    {
        UpdateData();
        ComputeTargetPosition();
    }


    Dictionary<int, float> getDistances(List<GameObject> gos)
    {
        Vector3 myPos = transform.position;
        Dictionary<int, float> distances = new Dictionary<int, float>(gos.Count);
        for (int i = 0; i < gos.Count; i++)
        {
            distances[i] = Vector3.Distance(myPos, gos[i].transform.position);
        }

        return distances;
    }


    void UpdateData()
    {
        plants = fieldManager.getGrownPlantList();
        plantDistances = getDistances(plants);
        crows = crowFactory.getCrowList();
        crowDistances = getDistances(crows);
        distToTarget = Vector3.Distance(transform.position, targetPos);
    }

    public void ComputeTargetPosition()
    {
        // if crow within danger radius, try to avoid crow

        // move towards closest plant
    }


    public void ComputeMoveInput()
    {
        // using targetPos, determine the 'MoveInput' required to reach target
    }

    public void ComputeInteractInput()
    {
        if (distToTarget < 0.5f)
        {
            _interactInput = true;
        }
    }
}