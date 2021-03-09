using UnityEngine;
using UnityEngine.UI;

public class WorkProgress : MonoBehaviour
{
    public Image fill;

    public void SetFill(float amount)
    {
        fill.fillAmount = amount;
    }
}