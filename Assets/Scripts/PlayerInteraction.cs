using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Input _input = new Input();
    private Selector _selector;

    private void Awake()
    {
        _input.Play.Throw.performed += context => OnInteract();
        _selector = GetComponent<Selector>();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void OnInteract()
    {
        Interactable interactable = _selector.Selected();
    }
}