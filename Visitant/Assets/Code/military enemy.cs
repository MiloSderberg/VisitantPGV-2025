using UnityEngine;

public class militaryenemy : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    public GameObject player;
    public GameObject projectile;
    float idleTime = 4;
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
        rb.linearVelocityX = 2;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (player.transform.position.x < transform.position.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
        animator.SetFloat("state", 3);
        Instantiate(projectile, transform.position, Quaternion.identity);
    }
}
