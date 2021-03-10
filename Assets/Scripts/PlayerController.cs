using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField]
    private float accelerationHorizontalGround;
    [SerializeField]
    private float decelerationHorizontalGround;
    [SerializeField]
    private float accelerationHorizontalAir;
    [SerializeField]
    private float decelerationHorizontalAir;
    [SerializeField]
    private float maxMovementSpeedHorizontal;
    [SerializeField]
    private float peakJumpReducedGravityMultiplier;
    [SerializeField]
    private float peakJumpReducedGravityAttackSpeed;
    [SerializeField]
    private float jumpCornerCorrectionInTiles;
    [SerializeField]
    private float dashForce;
    [SerializeField]
    private float dashCornerCorrectionInTiles;
    [SerializeField]
    private float jumpForce;
    
    
    
    
    private Input _input;
    private float _xInput;
    private Rigidbody2D _rigidbody;
    private Item _currentItem;
    private bool _isWorking;
    private int _isGrounded = 0;  // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic

    public Collider2D bottomEdgeCollider2D;
    public GameObject animationGameObject;
    public Selector selector;
    public float throwPower;

    private Vector2 _lastSpeed;
    public Vector2 speed;
    public Vector2 acceleration;

    private Animator _animator;
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.otherCollider == bottomEdgeCollider2D) {
            _isGrounded++;  // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
            _animator.SetInteger("isGrounded", _isGrounded);
            Debug.Log(_isGrounded);
        }
    }
    
    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.otherCollider == bottomEdgeCollider2D) {
            _isGrounded--;  // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
            _animator.SetInteger("isGrounded", _isGrounded);
            Debug.Log(_isGrounded);
        }
    }



    private void Awake()
    {
        _animator = animationGameObject.GetComponent<Animator> ();
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
        CheckInput();
        SetAnimator();

        var interactable = selector.Selected();
        if (interactable != null && interactable is Workable workable && _isWorking && !HasItem())
        {
            workable.Work(this);
        }
    }
    
    private void FixedUpdate()
    {
        ApplyMovement();
        MyDebug();
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
        _rigidbody.AddForce(Vector2.up * (jumpForce));
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
    
    private void CheckInput()
    {
        // get X Input
        var vertical = _input.Play.Movement.ReadValue<float>();
        
        // player holds left or right?
        if (Math.Abs(vertical) > 0.5)
        {
            // normalize controller input
            if (vertical < 0)
            {
                _xInput = -1;
            }
            else
            {
                _xInput = 1;
            }
            
            // flip player towards pressed direction 
            transform.localScale = new Vector3(-_xInput, 1, 1); 
        }
        else
        {
            _xInput = 0;
        }
    }

    private void SetAnimator()
    {
        _animator.SetFloat("hasXInput", Math.Abs(_xInput));
    }

    private void ApplyMovement()
    {
        if (_isGrounded > 0) // player is on ground
        {
            if (_xInput == 0)
            {
                // player stopping on ground // * Time.deltaTime
                // * (1 - (1 / (decelerationSpeedHorizontal * maxMovementSpeedHorizontal)))
                _rigidbody.AddForce(Vector2.right * ( -_rigidbody.velocity.x * Time.deltaTime * 3600 * (1-1/(decelerationHorizontalGround+1))));
            }
            else
            {
                // player accelerating on ground
                var maxFactor = Math.Abs((_xInput * maxMovementSpeedHorizontal - _rigidbody.velocity.x)) / maxMovementSpeedHorizontal;
                var force = maxFactor * accelerationHorizontalGround * maxMovementSpeedHorizontal;
                _rigidbody.AddForce(Vector2.right * (_xInput * force * Time.deltaTime));
            }
        }
        else // player is in air
        {
            if (_xInput == 0)
            {
                // player stopping on air // * Time.deltaTime
                // * (1 - (1 / (decelerationSpeedHorizontal * maxMovementSpeedHorizontal)))
                _rigidbody.AddForce(Vector2.right * ( -_rigidbody.velocity.x * Time.deltaTime * 3600 * (1-1/(decelerationHorizontalAir+1))));
            }
            else
            {
                // player accelerating on air
                var maxFactor = Math.Abs((_xInput * maxMovementSpeedHorizontal - _rigidbody.velocity.x)) / maxMovementSpeedHorizontal;
                var force = maxFactor * accelerationHorizontalAir * maxMovementSpeedHorizontal;
                _rigidbody.AddForce(Vector2.right * (_xInput * force * Time.deltaTime));
            }
        }
    }


    private void MyDebug()
    {
        _lastSpeed = speed;
        speed = _rigidbody.velocity;
        if (Math.Abs(speed.x) < 0.01)
        {
            speed.x = 0;
        }
        else
        {
            Debug.Log(speed.x);
        }

        if (Math.Abs(speed.y) < 0.01)
        {
            speed.y = 0;
        }
        acceleration = speed - _lastSpeed;
        if (Math.Abs(acceleration.x) < 0.01)
        {
            acceleration.x = 0;
        }
        if (Math.Abs(acceleration.y) < 0.01)
        {
            acceleration.y = 0;
        }
    }
}