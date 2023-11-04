using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    [Header("References")]
    public SwordAttack swordAttack;

    private Vector2 movementInput;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool canMove = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent < SpriteRenderer();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            UpdateMovement();
        }
    }

    private void UpdateMovement()
    {
        movementInput = GetMovementInput();

        bool success = TryMove(movementInput);

        if (!success)
        {
            success = TryMove(new Vector2(movementInput.x, 0));
        }

        if (!success)
        {
            success = TryMove(new Vector2(0, movementInput.y));
        }

        UpdateAnimator(success);
        UpdateSpriteDirection();
    }

    private Vector2 GetMovementInput()
    {
        return Keyboard.current.wKey.isPressed ? Vector2.up : Vector2.zero;
        // Add more input handling logic as needed (e.g., using the Input System).
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            return false; // Can't move if there's no direction to move in.
        }

        RaycastHit2D[] hits = new RaycastHit2D[1];
        int hitCount = rb.Cast(direction, movementFilter, hits, moveSpeed * Time.fixedDeltaTime + collisionOffset);

        if (hitCount == 0)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
        }

        return false;
    }

    private void UpdateAnimator(bool isMoving)
    {
        animator.SetBool("isMoving", isMoving);
    }

    private void UpdateSpriteDirection()
    {
        spriteRenderer.flipX = movementInput.x < 0;
    }

    public void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    public void OnFire()
    {
        animator.SetTrigger("swordAttack");
    }

    public void SwordAttack()
    {
        LockMovement();

        if (spriteRenderer.flipX)
        {
            swordAttack.AttackLeft();
        }
        else
        {
            swordAttack.AttackRight();
        }
    }

    public void EndSwordAttack()
    {
        UnlockMovement();
        swordAttack.StopAttack();
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }
}
