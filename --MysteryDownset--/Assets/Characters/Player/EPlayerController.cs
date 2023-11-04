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
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        
        if (movementInput != Vector2.zero) {

            int count = rb.Cast(
                movementInput, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime * collisionOffset);
            if (count == 0) {
                rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
            }
        
        } 
    
    }

}