using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EPlayerController : MonoBehaviour
{   
    public float moveSpeed = 1f;

    private float collisionOffset;

    public global::System.Single CollisionOffset { get => collisionOffset; set => collisionOffset = value; }

    public ContactFilter2D movementFilter;

    Vector2 movementInput;

    Rigidbody2D rb;

    Animator animator;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate() {

        if (movementInput != Vector2.zero) {

           bool succes = TryMove(movementInput);

           if (!succes) {
                succes = TryMove(new Vector2(movementInput.x, 0));
                
                if (!succes)
                {
                    succes = TryMove(new Vector2(0, movementInput.y));
                }
           }
            animator.SetBool("isMoving", succes);
        } else
        {
            animator.SetBool("isMoving", false);
        }
    
    }

    private bool TryMove(Vector2 direction)
    {
        int count = rb.Cast(
               direction, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime * collisionOffset);
        if (count == 0)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
        } else {
            return false;
        }

    }



}