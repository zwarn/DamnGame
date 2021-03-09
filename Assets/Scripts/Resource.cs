using System;
using UnityEngine;

public class Resource : MonoBehaviour, Workable
{
    public GameObject resource;
    public float workAmount;

    private float workDone;
    private WorkProgress _workProgress;


    private void Awake()
    {
        _workProgress = GetComponentInChildren<WorkProgress>();
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void Interact(PlayerController playerController)
    {
    }

    public void Work(PlayerController playerController)
    {
        workDone += Time.deltaTime;
        if (workDone >= workAmount)
        {
            workDone = 0;
            Item item = Instantiate(resource).GetComponent<Item>();
            playerController.PickupItem(item);
        }

        _workProgress.SetFill(workDone / workAmount);
    }
}