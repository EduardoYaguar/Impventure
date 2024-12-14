using System;
using NUnit.Framework.Internal;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    Rigidbody2D rb;
    CapsuleCollider2D touchingCol;
    Animator animator;
    public ContactFilter2D contactFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.2f;
    public float ceilingDistance = 0.05f;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];


    private bool _isGrounded = true;

    public bool IsGrounded {
        get
        {
            return _isGrounded;
        } private set
        {
            _isGrounded = value;
            animator.SetBool(AnimationStrings.isGrounded, value);
        }
    
    }
    private Boolean _isOnWall;
    public bool IsOnWall
    {
        get
        {
            return _isOnWall;
        }
        private set
        {
            _isOnWall = value;
            animator.SetBool(AnimationStrings.isOnWall, value);
        }

    }
    private Boolean _isOnCeiling;
    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    public bool IsOnCeiling
    {
        get
        {
            return _isOnCeiling;
        }
        private set
        {
            _isOnCeiling = value;
            animator.SetBool(AnimationStrings.isOnCeilign, value);
        }

    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
       IsGrounded = touchingCol.Cast(Vector2.down, contactFilter, groundHits, groundDistance) >0;
        IsOnWall = touchingCol.Cast(wallCheckDirection, contactFilter, wallHits, wallDistance) >0;
        IsOnCeiling = touchingCol.Cast(Vector2.up, contactFilter, ceilingHits, ceilingDistance) > 0;
    }
}
