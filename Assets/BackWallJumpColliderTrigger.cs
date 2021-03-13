using UnityEngine;

public class BackWallJumpColliderTrigger : MonoBehaviour
{
    public int isCollidingWallJumpBack;  // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
    public float isCollidingWallJumpBackFacingDirection;  // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
    private void OnTriggerEnter2D(Collider2D col)
    {
        isCollidingWallJumpBack++;  // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
        isCollidingWallJumpBackFacingDirection = transform.parent.localScale.x;
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        isCollidingWallJumpBack--;  // can't use bool as OnCollisionEnter2D/Exit2d order is undeterministic
        isCollidingWallJumpBackFacingDirection = 0;
    }
}