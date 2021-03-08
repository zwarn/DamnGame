using UnityEngine;

public interface Interactable
{
    GameObject GetGameObject();

    void Interact(PlayerController playerController);
}