using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Input _input;
    private Rigidbody2D _rigidbody;
    private Item _currentItem;

    public Selector Selector;
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
        Selector = GetComponentInChildren<Selector>();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
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
        Selector.Selected()?.Interact(this);
    }

    public bool HasItem()
    {
        return _currentItem != null;
    }

    public void PickupItem(Item item)
    {
        _currentItem = item;
        item.gameObject.transform.parent = Selector.transform;
        item.gameObject.transform.localPosition = Vector3.zero;
        item.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    public Item GiveItem()
    {
        Item item = _currentItem;
        _currentItem = null;
        item.transform.parent = null;
        item.GetComponent<Rigidbody2D>().isKinematic = false;
        return item;
    }
}