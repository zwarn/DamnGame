using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ResourceInput : ResourceContainer
{
    public ItemType type;

    public override void Interact(PlayerController playerController)
    {
        if (playerController.HasItem() && playerController.getItemType() == type && !isFull())
        {
            Item item = playerController.GiveItem();
            AddItem();
            Destroy(item.gameObject);
        }
    }
}