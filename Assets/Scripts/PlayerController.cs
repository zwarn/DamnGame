using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool debugMode;
    [SerializeField] private float accelerationHorizontalGround;
    [SerializeField] private float decelerationHorizontalGround;
    [SerializeField] private float accelerationHorizontalAir;
    [SerializeField] private float decelerationHorizontalAir;
    [SerializeField] private float maxMovementSpeedHorizontal;
    [SerializeField] private int jumpCoyoteTimeInFrames;
    [SerializeField] private int jumpInputBufferInFrames;
    [SerializeField] private float peakJumpReducedGravityMultiplier;
    [SerializeField] private float peakJumpReducedGravityAttackSpeed;
    [SerializeField] private float jumpCornerCorrectionInTiles;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashCornerCorrectionInTiles;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpHoldBoost;
    [SerializeField] private float airFrictionX;
    [SerializeField] private float airFrictionY;
    [SerializeField] private float groundFrictionX;
    [SerializeField] private float slidingFrictionY;
    [SerializeField] private float jumpProhibitionframes;
    [SerializeField] private float wallJumpForce;
    
    // collider checks
    private int _isCollidingTop = 0; // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
    private int _isCollidingBottom = 0; // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
    private int _isCollidingFront = 0; // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
    private int _isCollidingBack = 0; // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic

    // player states
    private bool _isGrounded = false;
    private bool _isPushingFrontWall = false;
    private bool _isMovingX = false;
    private bool _canWallJumpFront = false;
    private bool _canWallJumpBack = false;
    private int _framesPassedSinceLastJumpInput = 0;
    private int _framesPassedSinceLastJumpPerformed = 0;
    private int _jumpProhibitionframesPassed = 0;
    private bool _jumpInputDown = false;
    private int _framesPassedSinceLastIsGrounded = 0;
    private int _facingDirection = -1;
    private bool _isHoldInputJumpBoosting = false;
    private bool _isHoldDirectionJumpBoosting = false;
    private int _isHoldDirectionJumpBoostingDirection = 0;
    

    //player inputs
    private Input _input;
    private float _inputX = 0;


    private Rigidbody2D _rigidbody;
    private Item _currentItem;
    private bool _isWorking;

    private FrontWallJumpColliderTrigger _frontWallJumpColliderTrigger;

    public GameObject animationGameObject;
    public Selector selector;
    public float throwPower;

    private Vector2 _lastSpeed;
    public Vector2 speed;
    public Vector2 acceleration;

    private Animator _animator;
    private EdgeCollider2D[] _edgeColliders;
    private EdgeCollider2D _topEdgeCollider2D;
    private EdgeCollider2D _bottomEdgeCollider2D;
    private EdgeCollider2D _leftEdgeCollider2D;
    private EdgeCollider2D _rightEdgeCollider2D;

    // debug
    private Renderer _debugRenderer;
    private SpriteRenderer _debugIsCollidingTopSpriteRenderer;
    private SpriteRenderer _debugIsCollidingBottomSpriteRenderer;
    private SpriteRenderer _debugIsCollidingFrontSpriteRenderer;
    private SpriteRenderer _debugIsCollidingBackSpriteRenderer;
    private static readonly int IsMovingX = Animator.StringToHash("isMovingX");
    private static readonly int IsGrounded = Animator.StringToHash("isGrounded");
    private static readonly int IsPushingFrontWall = Animator.StringToHash("isPushingFrontWall");


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider == _topEdgeCollider2D)
        {
            _isCollidingTop += 1; // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
        }
        else if (collision.otherCollider == _bottomEdgeCollider2D)
        {
            _isCollidingBottom += 1; // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
        }
        else if (collision.otherCollider == _leftEdgeCollider2D)
        {
            _isCollidingFront += 1; // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
        }
        else if (collision.otherCollider == _rightEdgeCollider2D)
        {
            _isCollidingBack += 1; // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.otherCollider == _topEdgeCollider2D)
        {
            _isCollidingTop -= 1; // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
        }
        else if (collision.otherCollider == _bottomEdgeCollider2D)
        {
            _isCollidingBottom -= 1; // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
        }
        else if (collision.otherCollider == _leftEdgeCollider2D)
        {
            _isCollidingFront -= 1; // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
        }
        else if (collision.otherCollider == _rightEdgeCollider2D)
        {
            _isCollidingBack -= 1; // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
        }
    }

    private void Test1()
    {
        Debug.Log("started");
    }
    private void Test2()
    {
        Debug.Log("performed");
    }
    private void Test3()
    {
        Debug.Log("canceled");
    }

    private void Awake()
    {
        _animator = animationGameObject.GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _input = new Input();
        _input.Play.Throw.performed += context => Throw();
        _input.Play.Jump.started += context => JumpDown();
        _input.Play.Jump.canceled += context => JumpUp();
        _input.Play.Interact.performed += context => Interact();
        _input.Play.Work.started += context => BeginWork();
        _input.Play.Work.canceled += context => EndWork();
        _input.Play.TEST.started += context => Test1();
        _input.Play.TEST.performed += context => Test2();
        _input.Play.TEST.canceled += context => Test3();
        selector = GetComponentInChildren<Selector>();
        _edgeColliders = GetComponents<EdgeCollider2D>();
        _topEdgeCollider2D = _edgeColliders[0];
        _bottomEdgeCollider2D = _edgeColliders[1];
        _leftEdgeCollider2D = _edgeColliders[2];
        _rightEdgeCollider2D = _edgeColliders[3];
        _frontWallJumpColliderTrigger = GetComponentInChildren<FrontWallJumpColliderTrigger>();

        var allSpriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        foreach (var spriteRenderer in allSpriteRenderer)
        {
            switch (spriteRenderer.gameObject.name)
            {
                case "Debug":
                    _debugRenderer = spriteRenderer.gameObject.GetComponent<Renderer>();
                    break;
                case "IsCollidingTop":
                    _debugIsCollidingTopSpriteRenderer = spriteRenderer;
                    break;
                case "IsCollidingBottom":
                    _debugIsCollidingBottomSpriteRenderer = spriteRenderer;
                    break;
                case "IsCollidingFront":
                    _debugIsCollidingFrontSpriteRenderer = spriteRenderer;
                    break;
                case "IsCollidingBack":
                    _debugIsCollidingBackSpriteRenderer = spriteRenderer;
                    break;
            }
        }
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
        SetPlayerStatus();
        SetAnimator();
        DebugUpdate();

        var interactable = selector.Selected();
        if (interactable != null && interactable is Workable workable && _isWorking && !HasItem())
        {
            workable.Work(this);
        }
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        DebugFixedUpdate();
        UpdateFrameBasedPlayerStates();
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

    void JumpDown()
    {
        _framesPassedSinceLastJumpInput = 0;
        _jumpInputDown = true;
    }

    void JumpUp()
    {
        _jumpInputDown = false;
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
                _inputX = -1;
            }
            else
            {
                _inputX = 1;
            }
        }
        else
        {
            _inputX = 0;
        }
    }


    private void SetPlayerStatus()
    {
        // flip player towards pressed direction 
        if (Math.Abs(_inputX) > 0)
        {
            _facingDirection = (int) (_inputX);
        }

        _isMovingX = Math.Abs(_inputX) > 0;
        _isGrounded = _isCollidingBottom > 0;
        if (_isGrounded)
        {
            _framesPassedSinceLastIsGrounded = 0;
        }

        _isPushingFrontWall = _isCollidingFront > 0 && _isMovingX;

        _canWallJumpFront = _frontWallJumpColliderTrigger.isCollidingWallJumpFront > 0;
    }

    private void SetAnimator()
    {
        // flip player towards pressed direction 
        if (Math.Abs(_inputX) > 0)
        {
            transform.localScale = new Vector3(-_facingDirection, 1, 1);
        }

        _animator.SetBool(IsMovingX, _isMovingX);
        _animator.SetBool(IsGrounded, _isGrounded);
        _animator.SetBool(IsPushingFrontWall, _isPushingFrontWall);
    }

    private void ApplyMovement()
    {
        if ( _jumpInputDown && _framesPassedSinceLastJumpInput < jumpInputBufferInFrames && _jumpProhibitionframesPassed >= jumpProhibitionframes )
        {
            // got jump input
            if (_framesPassedSinceLastIsGrounded < jumpCoyoteTimeInFrames)
            {
                // jump
                Debug.Log("jump " + _framesPassedSinceLastJumpPerformed);
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0); // remove remaining y speed
                _rigidbody.AddForce(Vector2.up * jumpForce); // new jump force
                _framesPassedSinceLastJumpPerformed = 0;
                _jumpProhibitionframesPassed = 0;
                _isHoldInputJumpBoosting = true;
                _isHoldDirectionJumpBoosting = true;
                if (_rigidbody.velocity.x > 0){
                    _isHoldDirectionJumpBoostingDirection = 1;
                } else {
                    _isHoldDirectionJumpBoostingDirection = -1;
                }

            }
            else if (_canWallJumpFront)
            {
                // wall jump
                Debug.Log("jump " + _framesPassedSinceLastJumpPerformed);
                _rigidbody.velocity = new Vector2(0, Math.Max(_rigidbody.velocity.y, 0)); // remove remaining x speed
                var force = wallJumpForce * 0.7f;
                _rigidbody.AddForce(new Vector2(-_facingDirection * force, force)); // new jump force
                _framesPassedSinceLastJumpPerformed = 0;
                _jumpProhibitionframesPassed = 0;
            }
        }

        // hold input jump boost
        if (_jumpInputDown && _isHoldInputJumpBoosting && _rigidbody.velocity.y >= 0)
        {
            _rigidbody.AddForce(Vector2.up * (_rigidbody.velocity.y * (jumpHoldBoost + airFrictionY)));
        }
        else
        {
            _isHoldInputJumpBoosting = false;
        }
        
        // hold direction jump boost
        if (_isHoldDirectionJumpBoosting && _inputX / _isHoldDirectionJumpBoostingDirection > 0 && !_isGrounded)
        {
            _rigidbody.AddForce(Vector2.right * (-_rigidbody.velocity.x * airFrictionX));
        }
        else
        {
            _isHoldDirectionJumpBoosting = false;
        }

        // air resistance y
        _rigidbody.AddForce(Vector2.up * (-_rigidbody.velocity.y * airFrictionY));

        // air resistance x
        _rigidbody.AddForce(Vector2.right * (-_rigidbody.velocity.x * airFrictionX));
        


        if (_isGrounded) // player is on ground
        {
            if (_isMovingX)
            {
                // player is moving on ground
                var maxFactor = Math.Abs(
                    _inputX * maxMovementSpeedHorizontal - _rigidbody.velocity.x
                ) / maxMovementSpeedHorizontal;
                var force = maxFactor * accelerationHorizontalGround * maxMovementSpeedHorizontal;
                _rigidbody.AddForce(Vector2.right * (_inputX * force * Time.deltaTime));
            }
            else
            {
                // player is stopping on ground
                _rigidbody.AddForce(Vector2.right * (-_rigidbody.velocity.x * Time.deltaTime * 3600 *
                                                     (1 - 1 / (decelerationHorizontalGround + 1))));
            }
        }
        else // player is in air
        {
            if (_isMovingX)
            {
                // player is moving in air
                var maxFactor = Math.Abs((_inputX * maxMovementSpeedHorizontal - _rigidbody.velocity.x)) /
                                maxMovementSpeedHorizontal;
                var force = maxFactor * accelerationHorizontalAir * maxMovementSpeedHorizontal;
                _rigidbody.AddForce(Vector2.right * (_inputX * force * Time.deltaTime));
            }
            else
            {
                // player is stopping in air
                _rigidbody.AddForce(Vector2.right * (-_rigidbody.velocity.x * Time.deltaTime * 3600 *
                                                     (1 - 1 / (decelerationHorizontalAir + 1))));
            }
        }
    }

    private void UpdateFrameBasedPlayerStates()
    {
        _framesPassedSinceLastIsGrounded += 1;
        _framesPassedSinceLastJumpInput += 1;
        _framesPassedSinceLastJumpPerformed += 1;
        _jumpProhibitionframesPassed += 1;
    }

    private void DebugUpdate()
    {
        // debug collider states
        if (debugMode)
        {
            _debugRenderer.enabled = true;
            _debugIsCollidingTopSpriteRenderer.enabled = _isCollidingTop > 0;
            _debugIsCollidingBottomSpriteRenderer.enabled = _isCollidingBottom > 0;
            _debugIsCollidingFrontSpriteRenderer.enabled = _isCollidingFront > 0;
            _debugIsCollidingBackSpriteRenderer.enabled = _isCollidingBack > 0;
        }
        else
        {
            _debugRenderer.enabled = false;
        }
    }


    private void DebugFixedUpdate()
    {
        _lastSpeed = speed;
        speed = _rigidbody.velocity;
        if (Math.Abs(speed.x) < 0.01)
        {
            speed.x = 0;
        }
        else
        {
            // Debug.Log(speed.x);
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