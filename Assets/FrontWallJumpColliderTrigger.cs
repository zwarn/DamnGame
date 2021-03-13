using UnityEngine;

public class FrontWallJumpColliderTrigger : MonoBehaviour
{
    public int isCollidingWallJumpFront;  // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
    public float isCollidingWallJumpFrontFacingDirection;  // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
    private void OnTriggerEnter2D(Collider2D col)
    {
        isCollidingWallJumpFront++;  // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
        isCollidingWallJumpFrontFacingDirection = transform.parent.localScale.x;
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        isCollidingWallJumpFront--;  // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
        isCollidingWallJumpFrontFacingDirection = 0;
    }
}
