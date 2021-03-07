using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Selector : MonoBehaviour
{
    private readonly List<Interactable> _interactables = new List<Interactable>();

    private void OnCollisionEnter2D(Collision2D other)
    {
        var interactable = other.gameObject.GetComponent<Interactable>();
        if (interactable != null)
        {
            _interactables.Add(interactable);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        var interactable = other.gameObject.GetComponent<Interactable>();
        if (interactable != null)
        {
            _interactables.Add(interactable);
        }
    }

    private Interactable closest()
    {
        if (_interactables.Count == 0)
        {
            return null;
        }

        return _interactables.OrderBy(interactable =>
            (interactable.GetGameObject().transform.position - transform.position).magnitude).First();
    }

    public Interactable Selected()
    {
        return closest();
    }
}