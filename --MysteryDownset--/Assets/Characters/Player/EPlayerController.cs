using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float collisionOffset = 0.05f;
    [SerializeField]
    private ContactFilter2D movementFilter;

    [Header("References")]
    [SerializeField]
    private SwordAttack swordAttack;

    private Vector2 movementInput;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Animator animator;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    private bool canMove = true;

    private void Start()
    {
        InitializeReferences();
    }

    private void InitializeReferences()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent < SpriteRenderer();
        if (swordAttack == null)
        {
            Debug.LogError("SwordAttack reference is missing. Attach a SwordAttack script to the player GameObject.");
            canMove = false;
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        ReadInput();
        bool success = TryMove(movementInput);
        UpdateAnimation(success);
        UpdateSpriteDirection();
    }

    private void ReadInput()
    {
        movementInput = Keyboard.current.wKey.isPressed ? Vector2.up : Vector2.zero;
        // You can expand this method for more input handling.
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            return false; // Can't move if there's no direction to move in.
        }

        int count = rb.Cast(
            direction, movementFilter, castCollisions,
            moveSpeed * Time.fixedDeltaTime + collisionOffset);

        if (count == 0)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
        }
        return false;
    }

    private void UpdateAnimation(bool isMoving)
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
