using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ResourceOutput : ResourceContainer
{
    public GameObject resource;

    public override void Interact(PlayerController playerController)
    {
        if (!playerController.HasItem() && CurrentAmount() > 0)
        {
            SubItem();
            Item item = Instantiate(resource).GetComponent<Item>();
            playerController.PickupItem(item);
        }
    }
}