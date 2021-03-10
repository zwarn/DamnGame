using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Input _input;
    private Rigidbody2D _rigidbody;
    private Item _currentItem;
    private bool _isWorking;
    public int isGrounded = 0;  // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic

    public Collider2D bottomEdgeCollider2D;
    public Animator animator;
    public Selector selector;
    public float moveSpeed;
    public float jumpPower;
    public float throwPower;
    
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.otherCollider == bottomEdgeCollider2D) {
            isGrounded++;  // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
            animator.SetInteger("isGrounded", isGrounded);
            Debug.Log(isGrounded);
        }
    }
    
    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.otherCollider == bottomEdgeCollider2D) {
            isGrounded--;  // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
            animator.SetInteger("isGrounded", isGrounded);
            Debug.Log(isGrounded);
        }
    }



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
        if (Math.Abs(vertical) > 0.5)
        {
            // player holds left or right

            // normalize controller input
            if (vertical < 0)
            {
                vertical = -1;
            }
            else
            {
                vertical = 1;
            }

            transform.localScale = new Vector3(-vertical, 1, 1); // flip player towards pressed direction 
            _rigidbody.AddForce(Vector2.right * (vertical * moveSpeed * Time.deltaTime));
            animator.SetFloat("Speed", 1);
        }
        else
        {
            // no input or controller input too low
            animator.SetFloat("Speed", 0);
        }


        var interactable = selector.Selected();
        if (interactable != null && interactable is Workable workable && _isWorking && !HasItem())
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
        _isWorking = false;
    }

    private void BeginWork()
    {
        _isWorking = true;
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