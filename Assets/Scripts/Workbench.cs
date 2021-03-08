using UnityEngine;

public class Workbench : MonoBehaviour, Interactable
{
    public ResourceContainer from;
    public ResourceContainer to;

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void Interact(PlayerController playerController)
    {
        if (from.CurrentAmount() > 0 && !to.isFull())
        {
            from.SubItem();
            to.AddItem();
        }
    }
}