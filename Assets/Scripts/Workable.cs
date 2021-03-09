using UnityEngine;

public interface Workable : Interactable
{
    GameObject GetGameObject();

    void Work(PlayerController playerController);
}