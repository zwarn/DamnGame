using UnityEngine;

public interface Workable
{
    GameObject GetGameObject();

    void Work(PlayerController playerController);
}