using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceContainer : MonoBehaviour, Interactable
{
    public ItemType acceptsItem;
    public List<Sprite> sprites;

    private int amount = 0;
    private SpriteRenderer _spriteRenderer;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void Interact(PlayerController playerController)
    {
        if (playerController.HasItem() && playerController.getItemType() == acceptsItem && amount < sprites.Count - 1)
        {
            Item item = playerController.GiveItem();
            AddItem();
            Destroy(item.gameObject);
        }
    }

    public void AddItem()
    {
        amount++;
        _spriteRenderer.sprite = sprites[amount];
    }
}