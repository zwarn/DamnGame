using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, Interactable
{
    public ItemType type;

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void Interact(PlayerController playerController)
    {
        if (!playerController.HasItem())
        {
            playerController.PickupItem(this);
        }
    }
}

public enum ItemType
{
    Stone,
    Brick,
}