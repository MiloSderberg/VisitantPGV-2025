using UnityEngine;

public class militaryenemy : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    public GameObject player;
    public GameObject projectile;
    float idleTime = 4;
    float shotTime = 0.5f;
    float coolDownTime = 2;
    bool eyesOnTarget = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (eyesOnTarget == true)
        {
            rb.linearVelocityX = 0;
            coolDownTime -= Time.deltaTime;
            if (coolDownTime <= 0)
            {
                coolDownTime = 2;
            }
            if (coolDownTime <= 1)
            {
                animator.Play("idle");
            }
            if (coolDownTime > 1)
            {
                animator.Play("shooting");
                shotTime -= Time.deltaTime;
                if (shotTime <= 0)
                {
                    if (sr.flipX == true)
                    {
                        Instantiate(projectile, new Vector2(transform.position.x - 0.5f, transform.position.y + 1), Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(projectile, new Vector2(transform.position.x + 0.5f, transform.position.y + 1), Quaternion.identity);
                    }
                    shotTime = 0.5f;
                }
            }
        }
        else if (eyesOnTarget == false)
        {
            idleTime -= Time.deltaTime;
            if (idleTime <= 0)
            {
                sr.flipX = false;
                animator.Play("idle");
                idleTime = 4f;
            }
            else if (idleTime <= 0.5)
            {
                animator.Play("idle");
            }
            else if (idleTime <= 1.5)
            {
                animator.Play("walk");
                rb.linearVelocityX = -2;
            }
            else if (idleTime <= 2)
            {
                sr.flipX = true;
                animator.Play("idle");
            }
            else if (idleTime <= 2.5)
            {
                animator.Play("idle");
            }
            else if (idleTime <= 3.5)
            {
                animator.Play("walk");
                rb.linearVelocityX = 2;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (sr.flipX == true && player.transform.position.x < transform.position.x)
            {
                eyesOnTarget = true;
            }
            else if (sr.flipX == false && player.transform.position.x > transform.position.x)
            {
                eyesOnTarget = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            eyesOnTarget = false;
        }
    }
}
