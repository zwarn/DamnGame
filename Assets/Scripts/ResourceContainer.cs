using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class ResourceContainer : MonoBehaviour, Interactable
{
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

    public abstract void Interact(PlayerController playerController);

    public void AddItem()
    {
        amount++;
        _spriteRenderer.sprite = sprites[amount];
    }

    public void SubItem()
    {
        amount--;
        _spriteRenderer.sprite = sprites[amount];
    }

    public int CurrentAmount()
    {
        return amount;
    }

    public int MaxAmount()
    {
        return sprites.Count - 1;
    }

    public bool isFull()
    {
        return CurrentAmount() >= MaxAmount();
    }
}