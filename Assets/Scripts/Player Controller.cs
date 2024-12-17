using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class PlayerController : MonoBehaviour
{
    Vector2 moveInput;
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float airWalkSpeed = 3f;
    Rigidbody2D rb;
    Animator animator;
    TouchingDirections touchingDirections;
    public float jumpImpulse = 5.5f;
    public GameObject ImpFireballPrefab;
    private float lastFireball = 3f;

    private float fireballCooldown = 0.25f; // Tiempo base entre disparos
    private bool isFireRateBoosted = false; // Indica si el disparo está potenciado

    public float currentMoveSpeed 
    {
        get {
            if (IsMoving && !touchingDirections.IsOnWall)
            {
                if (touchingDirections.IsGrounded)
                {
                    if (IsRunning)
                    {
                        return runSpeed;
                    }
                    else
                    {
                        return walkSpeed;
                    }
                }
                else
                {
                    return airWalkSpeed;
                }
            }
            else {
                return 0;
            }
        }
    }

    private bool _isMoving = false;
    public bool IsMoving { get 
        { 
            return _isMoving;
        } private set 
        { 
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }
    private bool _isRunning = false;

    public bool IsRunning
    {
        get { return _isRunning; }
        set { 
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }
    
    public bool _isFacingRight = true;
    

    public bool IsFacingRight { get { return _isFacingRight; } set 
        {
            if (_isFacingRight !=value) 
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value; 
        
        } 
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * currentMoveSpeed, rb.linearVelocity.y);
        animator.SetFloat(AnimationStrings.yVelocity, rb.linearVelocity.y);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);

    }
    public void OnJump(InputAction.CallbackContext context)
        
    {   
        if (context.started && touchingDirections.IsGrounded)
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpulse);
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight) 
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight= false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }else if (context.canceled)
        {
            IsRunning = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeadZone"))
        {
            enabled = false;

            Invoke("RestartScene", 0.5f);
        }
        if (collision.CompareTag("Enemy"))
        {
            enabled = false;

            Invoke("RestartScene", 0.5f);
        }else if (collision.CompareTag("Cherry"))
        {
            // Activar el efecto de velocidad de disparo aumentada
            if (!isFireRateBoosted)
            {
                StartCoroutine(DoubleFireRateForSeconds(10f));
            }
            Destroy(collision.gameObject); // Opcional: destruir el objeto "Cherry"
        }else if (collision.CompareTag("Door"))
        {
            ChangeScene(); // Cambiar a una nueva escena
        }
    }

    private void ChangeScene()
    {
        // Cambia el nombre "NextSceneName" por el nombre de la escena a la que quieres cambiar
        SceneManager.LoadScene(1);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started && Time.time > lastFireball + 0.25f)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
            lastFireball = Time.time;

            Vector3 direction;
            if (IsFacingRight)
            {
                direction = Vector2.right;
            }
            else { direction = Vector2.left; }

            GameObject fireball = Instantiate(ImpFireballPrefab, transform.position + direction * 0.1f, Quaternion.identity);
            fireball.GetComponent<ImpFireball>().setDirection(direction);
            

        }
    }
    private System.Collections.IEnumerator DoubleFireRateForSeconds(float duration)
    {
        isFireRateBoosted = true;
        fireballCooldown /= 2; // Duplicar la velocidad de disparo
        yield return new WaitForSeconds(duration);
        fireballCooldown *= 2; // Restaurar la velocidad normal
        isFireRateBoosted = false;
    }
}
