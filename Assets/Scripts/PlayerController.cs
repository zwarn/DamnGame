using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Input _input;
    private Rigidbody2D _rigidbody;
    private Item _currentItem;
    private bool isWorking;

    public Selector selector;
    public float moveSpeed = 500;
    public float jumpPower = 500;
    public float throwPower = 500;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _input = new Input();
        _input.Play.Throw.performed += context => Throw();
        _input.Play.Jump.performed += context => Jump();
        _input.Play.Interact.performed += context => Interact();
        _input.Play.Work.started += context => BeginWork();
        _input.Play.Work.canceled += context => EndWork();
        selector = GetComponentInChildren<Selector>();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    void Update()
    {
        var vertical = _input.Play.Movement.ReadValue<float>();
        _rigidbody.AddForce(Vector2.right * (vertical * moveSpeed * Time.deltaTime));
        if (vertical > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (vertical < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        var interactable = selector.Selected();
        if (interactable != null && interactable is Workable workable && isWorking)
        {
            workable.Work(this);
        }
    }

    void Throw()
    {
        if (HasItem())
        {
            Item item = GiveItem();
            item.GetComponent<Rigidbody2D>()
                .AddForce((Vector2.up + transform.localScale.x * Vector2.right) * throwPower);
        }
    }

    void Jump()
    {
        _rigidbody.AddForce(Vector2.up * (jumpPower));
    }

    void Interact()
    {
        selector.Selected()?.Interact(this);
    }

    private void EndWork()
    {
        isWorking = false;
    }

    private void BeginWork()
    {
        isWorking = true;
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
        item.gameObject.transform.parent = selector.transform;
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