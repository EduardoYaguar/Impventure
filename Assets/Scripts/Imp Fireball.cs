using UnityEngine;

public class ImpFireball : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 3f;
    private Vector2 fireballDirection;
    public float lifeTime = 2f;
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Destroy(gameObject, lifeTime);
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = fireballDirection * speed; 
    }
    public void setDirection(Vector2 direction)
    {
        fireballDirection = direction;
        if (direction.x < 0)
        {
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            return;
        }
        Destroy(gameObject);
    }
}
