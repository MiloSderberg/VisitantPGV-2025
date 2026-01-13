using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMover : MonoBehaviour
{
    Vector2 direction;
    bool idle = true;
    public GameObject player;
    Rigidbody2D rb;
    Animator am;
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        am = GetComponent<Animator>();
    }

    void Update()
    {
        if (idle == true)
        {
            if (am != null) am.Play("idle");
        }
        else
        {
            if (am != null) am.Play("walking");
        }
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
}
