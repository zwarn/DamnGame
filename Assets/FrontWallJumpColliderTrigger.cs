using UnityEngine;

public class FrontWallJumpColliderTrigger : MonoBehaviour
{
    public int isCollidingWallJumpFront;  // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
    private void OnTriggerEnter2D(Collider2D col)
    {
        isCollidingWallJumpFront++;  // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        isCollidingWallJumpFront--;  // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
    }
}
