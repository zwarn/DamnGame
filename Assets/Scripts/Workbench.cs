using System;
using UnityEngine;

public class Workbench : MonoBehaviour, Interactable, Workable, HasItem
{
    public GameObject itemHolder;
    public ItemType from;
    public GameObject to;
    public Item _currentItem;

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void Work(PlayerController playerController)
    {
    }

    public void Interact(PlayerController playerController)
    {
        if (playerController.HasItem() && playerController.getItemType() == from && !HasItem())
        {
            Item item = playerController.GiveItem();
            PickupItem(item);
        }
    }

    public bool HasItem()
    {
        return _currentItem != null;
    }

    public ItemType getItemType()
    {
        if (_currentItem == null)
        {
            throw new InvalidOperationException("no item present");
        }

        return _currentItem.type;
    }

    public void PickupItem(Item item)
    {
        _currentItem = item;
        item.gameObject.transform.parent = itemHolder.transform;
        item.gameObject.transform.localPosition = Vector3.zero;
        item.GetComponent<Rigidbody2D>().isKinematic = true;
        item.GetComponent<Collider2D>().enabled = false;
    }

    public Item GiveItem()
    {
        Item item = _currentItem;
        _currentItem = null;
        item.transform.parent = null;
        item.GetComponent<Rigidbody2D>().isKinematic = false;
        item.GetComponent<Collider2D>().enabled = true;
        return item;
    }
}