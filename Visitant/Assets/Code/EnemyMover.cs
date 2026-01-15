using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMover : MonoBehaviour
{
    Vector2 direction;
    bool idle = true;
    public GameObject player;
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator am;
    public float speed;
    bool attacking = false;
    float timer;
    float attackFor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        am = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (attacking == false)
        {
            if (idle == true)
            {
                if (am != null) am.Play("IDLE");
            }
            else
            {
                if (am != null) am.Play("WALK");
            }
        }
        else
        {
            if (timer > 0)
            {
                if (attackFor > 0)
                {
                    if (am != null) am.Play("ATTACK");
                    attackFor -= Time.deltaTime;
                }
                timer -= Time.deltaTime;
            }
        }
        if (attackFor <= 0) attacking = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            direction = (player.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float snappedAngle = Mathf.Round(angle / 180f) * 180f;
            float rad = snappedAngle * Mathf.Deg2Rad;
            Vector2 snappedDirection = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;

            if (snappedDirection.x < 0) sr.flipX = true;
            else sr.flipX = false;
            rb.linearVelocityX = snappedDirection.x * speed;
            idle = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            idle = true;
        }
    }

    public void Attack()
    {
        attacking = true;
        timer = 1.5f;
        attackFor = 0.25f;
    }
}
