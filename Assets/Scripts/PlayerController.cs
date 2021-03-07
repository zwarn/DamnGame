using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Input _input = new Input();

    private void Awake()
    {
        _input.Play.Throw.performed += context => Throw();
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
        var delta = _input.Play.Movement.ReadValue<Vector2>();
    }

    void Throw()
    {
    }
}