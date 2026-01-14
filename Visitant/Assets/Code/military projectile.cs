using UnityEngine;

public class militaryprojectile : MonoBehaviour
{
    Rigidbody2D rb;
    bool destroy = false;
    float timer = 0.25f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        rb.linearVelocity = (player.transform.position - transform.position).normalized * 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (destroy == true)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("wall"))
        {
            destroy = true;
        }
    }
}
