using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Input _input;
    private Rigidbody2D _rigidbody;

    public float moveSpeed = 200;
    public float jumpPower = 200;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _input = new Input();
        _input.Play.Throw.performed += context => Throw();
        _input.Play.Jump.performed += context => Jump();
        _input.Play.Interact.performed += context => Interact();
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
    }

    void Throw()
    {
    }

    void Jump()
    {
        _rigidbody.AddForce(Vector2.up * (jumpPower));
    }

    void Interact()
    {
    }
}