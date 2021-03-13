using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool debugMode;
    [SerializeField] private float maxMovementSpeedX;
    [SerializeField] private float accelerationXGround;
    [SerializeField] private float decelerationXGround;
    [SerializeField] private float decelerationXGroundWithDirectionBoost;
    [SerializeField] private float accelerationXAir;
    [SerializeField] private float decelerationXAir;
    [SerializeField] private float decelerationXAirWithDirectionBoost;
    [SerializeField] private float targetSpeedOvershotPreventionRange;
    [SerializeField] private float jumpForce;
    [SerializeField] private float wallJumpForce;
    [SerializeField] private float wallJumpVelocityYReduction;
    [SerializeField] private float jumpHoldGravityReduction;
    [SerializeField] private int jumpCoyoteTimeInTicks;
    [SerializeField] private int jumpInputBufferInTicks;
    [SerializeField] private float peakJumpReducedGravityMultiplier;
    [SerializeField] private float peakJumpReducedGravityMultiplierVariance;
    [SerializeField] private float jumpProhibitionTicks;
    [SerializeField] private float jumpCornerCorrectionInTiles;
    [SerializeField] private float gravity;
    [SerializeField] private float airFrictionY;
    [SerializeField] private float slidingFrictionY;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashCornerCorrectionInTiles;


    // collider checks
    private int _isCollidingTop = 0; // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
    private int _isCollidingBottom = 0; // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
    private int _isCollidingBack = 0; // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
    private int _isCollidingFront = 0; // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic

    // game status
    private int _gameTick = 0;


    // player status
    private bool _isGrounded = false;
    private bool _isPushingFrontWall = false;
    private bool _isMovingX = false;
    private bool _canWallJumpFront = false;
    private bool _canWallJumpBack = false;
    private float _canWallJumpFrontDirectiom;
    private float _canWallJumpBackDirectiom;
    private int _ticksPassedSinceLastJumpInput = 0;
    private int _ticksPassedSinceLastJumpPerformed = 0;
    private int _jumpProhibitionTicksPassed = 0;
    private bool _jumpInputDown = false;
    private bool _jumpPerformed = true;
    private int _ticksPassedSinceLastIsGrounded = 0;
    private int _facingDirection = -1;
    private bool _isHoldInputJumpBoosting = false;

    //player inputs
    private Input _input;
    private float _inputX = 0;


    private Rigidbody2D _rigidbody;
    private Item _currentItem;
    private bool _isWorking;

    private FrontWallJumpColliderTrigger _frontWallJumpColliderTrigger;
    private BackWallJumpColliderTrigger _backWallJumpColliderTrigger;

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
    private EdgeCollider2D _backEdgeCollider2D;
    private EdgeCollider2D _frontEdgeCollider2D;

    // debug
    private Renderer _debugRenderer;
    private SpriteRenderer _debugIsCollidingTopSpriteRenderer;
    private SpriteRenderer _debugIsCollidingBottomSpriteRenderer;
    private SpriteRenderer _debugIsCollidingFrontSpriteRenderer;
    private SpriteRenderer _debugIsCollidingBackSpriteRenderer;
    private static readonly int IsMovingX = Animator.StringToHash("isMovingX");
    private static readonly int IsGrounded = Animator.StringToHash("isGrounded");
    private static readonly int IsPushingFrontWall = Animator.StringToHash("isPushingFrontWall");

    private const float NoVelocityTolerance = 0.01f;

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
        else if (collision.otherCollider == _backEdgeCollider2D)
        {
            _isCollidingBack += 1; // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
        }
        else if (collision.otherCollider == _frontEdgeCollider2D)
        {
            _isCollidingFront += 1; // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
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
        else if (collision.otherCollider == _backEdgeCollider2D)
        {
            _isCollidingBack -= 1; // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
        }
        else if (collision.otherCollider == _frontEdgeCollider2D)
        {
            _isCollidingFront -= 1; // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
        }
    }

    private void Test1()
    {
        _rigidbody.AddForce(new Vector2(_inputX * 2500, 2800));
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
        _frontEdgeCollider2D = _edgeColliders[2];
        _backEdgeCollider2D = _edgeColliders[3];
        _frontWallJumpColliderTrigger = GetComponentInChildren<FrontWallJumpColliderTrigger>();
        _backWallJumpColliderTrigger = GetComponentInChildren<BackWallJumpColliderTrigger>();

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
        _ticksPassedSinceLastJumpInput = 0;
        _jumpInputDown = true;
        _jumpPerformed = false;
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
            // flip player towards pressed direction 
            _facingDirection = (int) (_inputX);
            transform.localScale = new Vector3(_facingDirection, 1, 1);
        }
        else
        {
            _inputX = 0;
        }
    }


    private void SetPlayerStatus()
    {
        _isMovingX = Math.Abs(_inputX) > 0;
        _isGrounded = _isCollidingBottom > 0 && Math.Abs(_rigidbody.velocity.y) <= NoVelocityTolerance;
        if (_isGrounded)
        {
            _ticksPassedSinceLastIsGrounded = 0;
        }
        _isPushingFrontWall = _isCollidingFront > 0 && _isMovingX;

        _canWallJumpFront = _frontWallJumpColliderTrigger.isCollidingWallJumpFront > 0;
        _canWallJumpFrontDirectiom = -_frontWallJumpColliderTrigger.isCollidingWallJumpFrontFacingDirection;
        _canWallJumpBack = _backWallJumpColliderTrigger.isCollidingWallJumpBack > 0;
        _canWallJumpBackDirectiom = _backWallJumpColliderTrigger.isCollidingWallJumpBackFacingDirection;
    }

    private void SetAnimator()
    {
        _animator.SetBool(IsMovingX, _isMovingX);
        _animator.SetBool(IsGrounded, _isGrounded);
        _animator.SetBool(IsPushingFrontWall, _isPushingFrontWall);
    }

    private void ApplyMovement()
    {
        // jumping
        if (
            _jumpInputDown // jump input is held down
            && _ticksPassedSinceLastJumpInput <= jumpInputBufferInTicks // jump down was input within last x ticks
            && !_jumpPerformed // jump was not already executed
            && _ticksPassedSinceLastJumpPerformed >
            jumpProhibitionTicks // minimum ticks in between two jumps has passed
        )
        {
            // jump was input
            if (
                _ticksPassedSinceLastIsGrounded < jumpCoyoteTimeInTicks // coyote-time: was grounded within last x ticks
            )
            {
                // normal jump
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0); // remove remaining y speed
                _rigidbody.AddForce(Vector2.up * jumpForce); // new jump force
                _isGrounded = false;
                _ticksPassedSinceLastJumpPerformed = 0;
                _isHoldInputJumpBoosting = true;
            }
            else if (_canWallJumpFront || _canWallJumpBack)
            {
                // wall jump
                _rigidbody.velocity = new Vector2(
                    // remove remaining x speed
                    0,
                    // cancel negative y speed and reduce positive by factor wallJumpVelocityYReduction
                    Math.Max(_rigidbody.velocity.y * (1 - wallJumpVelocityYReduction), 0)
                );
                var direction = _canWallJumpBack ? _canWallJumpBackDirectiom : _canWallJumpFrontDirectiom;
                Debug.Log(_canWallJumpFront + " " + _canWallJumpBack + " " + direction + " " + _gameTick);
                _rigidbody.AddForce(new Vector2(direction * wallJumpForce * 0.7f,
                    wallJumpForce)); // new jump force
                _ticksPassedSinceLastJumpPerformed = 0;
                _isHoldInputJumpBoosting = true;
            }
        }

        // gravity y
        _rigidbody.AddForce(Vector2.down * gravity);

        // air resistance y
        _rigidbody.AddForce(Vector2.up * (-_rigidbody.velocity.y * airFrictionY));
        
        if (_isPushingFrontWall && _rigidbody.velocity.y < 0)
        {
            _rigidbody.AddForce(Vector2.up * (-_rigidbody.velocity.y * slidingFrictionY));
        }

        // hold input jump boost
        if (_jumpInputDown && _isHoldInputJumpBoosting && _rigidbody.velocity.y >= 0)
        {
            _rigidbody.AddForce(Vector2.up *
                                ((_rigidbody.velocity.y * airFrictionY + gravity) * jumpHoldGravityReduction));
        }
        else
        {
            _isHoldInputJumpBoosting = false;
        }


        float xAccelerationForce;
        float deltaToTargetSpeed;
        if (_isGrounded)
        {
            if (_inputX > 0)
            {
                if (_rigidbody.velocity.x >= maxMovementSpeedX)
                {
                    // decelerate slow towards -x
                    xAccelerationForce = -decelerationXGroundWithDirectionBoost;
                }
                else if (_rigidbody.velocity.x < 0)
                {
                    // decelerate fast towards +x
                    xAccelerationForce = decelerationXGround;
                }
                else // (0 <= _rigidbody.velocity.x < maxMovementSpeedHorizontal)
                {
                    // accelerate towards +x
                    xAccelerationForce = accelerationXGround;
                }

                deltaToTargetSpeed = _rigidbody.velocity.x - maxMovementSpeedX;
            }
            else if (_inputX < 0)
            {
                if (_rigidbody.velocity.x <= -maxMovementSpeedX)
                {
                    // decelerate slow towards +x
                    xAccelerationForce = decelerationXGroundWithDirectionBoost;
                }
                else if (_rigidbody.velocity.x > 0)
                {
                    // decelerate fast towards -x
                    xAccelerationForce = -decelerationXGround;
                }
                else // (0 >= _rigidbody.velocity.x > maxMovementSpeedHorizontal)
                {
                    // accelerate towards -x
                    xAccelerationForce = -accelerationXGround;
                }

                deltaToTargetSpeed = _rigidbody.velocity.x + maxMovementSpeedX;
            }
            else
            {
                // _inputX == 0
                if (_rigidbody.velocity.x > 0)
                {
                    // decelerate fast towards -x
                    xAccelerationForce = -decelerationXGround;
                }
                else // (0 >= _rigidbody.velocity.x > maxMovementSpeedHorizontal)
                {
                    // decelerate fast towards +x
                    xAccelerationForce = decelerationXGround;
                }

                deltaToTargetSpeed = _rigidbody.velocity.x;
            }
        }
        else
        {
            if (_inputX > 0)
            {
                if (_rigidbody.velocity.x >= maxMovementSpeedX)
                {
                    // decelerate slow towards -x
                    xAccelerationForce = -decelerationXAirWithDirectionBoost;
                }
                else if (_rigidbody.velocity.x < 0)
                {
                    // decelerate fast towards +x
                    xAccelerationForce = decelerationXAir;
                }
                else // (0 <= _rigidbody.velocity.x < maxMovementSpeedHorizontal)
                {
                    // accelerate towards +x
                    xAccelerationForce = accelerationXAir;
                }

                deltaToTargetSpeed = _rigidbody.velocity.x - maxMovementSpeedX;
            }
            else if (_inputX < 0)
            {
                if (_rigidbody.velocity.x <= -maxMovementSpeedX)
                {
                    // decelerate slow towards +x
                    xAccelerationForce = decelerationXAirWithDirectionBoost;
                }
                else if (_rigidbody.velocity.x > 0)
                {
                    // decelerate fast towards -x
                    xAccelerationForce = -decelerationXAir;
                }
                else // (0 >= _rigidbody.velocity.x > maxMovementSpeedHorizontal)
                {
                    // accelerate towards -x
                    xAccelerationForce = -accelerationXAir;
                }

                deltaToTargetSpeed = _rigidbody.velocity.x + maxMovementSpeedX;
            }
            else
            {
                // _inputX == 0
                if (_rigidbody.velocity.x > 0)
                {
                    // decelerate fast towards -x
                    xAccelerationForce = -decelerationXAir;
                }
                else // (0 >= _rigidbody.velocity.x > maxMovementSpeedHorizontal)
                {
                    // decelerate fast towards +x
                    xAccelerationForce = decelerationXAir;
                }

                deltaToTargetSpeed = _rigidbody.velocity.x;
            }
        }

        // force reduction to prevent overshooting target speed
        if (Math.Abs(deltaToTargetSpeed) < targetSpeedOvershotPreventionRange)
        {
            xAccelerationForce = xAccelerationForce * Math.Abs(deltaToTargetSpeed) / targetSpeedOvershotPreventionRange;
        }

        _rigidbody.AddForce(Vector2.right * xAccelerationForce);
    }

    private void UpdateFrameBasedPlayerStates()
    {
        _ticksPassedSinceLastIsGrounded += 1;
        _ticksPassedSinceLastJumpInput += 1;
        _ticksPassedSinceLastJumpPerformed += 1;
        _jumpProhibitionTicksPassed += 1;
        _gameTick++;
        _gameTick += 1;
    }

    private void DebugUpdate()
    {
        // debug collider states
        if (debugMode)
        {
            _debugRenderer.enabled = true;
            _debugIsCollidingTopSpriteRenderer.enabled = _isCollidingTop > 0;
            _debugIsCollidingBottomSpriteRenderer.enabled = _isCollidingBottom > 0;
            _debugIsCollidingFrontSpriteRenderer.enabled = _isCollidingBack > 0;
            _debugIsCollidingBackSpriteRenderer.enabled = _isCollidingFront > 0;
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
        acceleration = speed - _lastSpeed;
        if (Math.Abs(acceleration.x) < 0.01)
        {
            acceleration.x = 0;
        }

        if (Math.Abs(acceleration.y) < 0.01)
        {
            acceleration.y = 0;
        }

        speed.x = (float) Math.Round(speed.x);
        speed.y = (float) Math.Round(speed.y);
    }
}
