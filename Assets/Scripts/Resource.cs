using UnityEngine;

public class Resource : MonoBehaviour, Interactable
{
    public GameObject resource;

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void Interact(PlayerController playerController)
    {
        if (!playerController.HasItem())
        {
            Item item = Instantiate(resource).GetComponent<Item>();
            playerController.PickupItem(item);
        }
    }
}