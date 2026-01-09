using UnityEngine;

public class playerhealthbar : MonoBehaviour
{
    public float damage = 0;
    float invincibilityTime = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        invincibilityTime -= Time.deltaTime;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CompareTag("canDamagePlayer") && invincibilityTime <= 0)
        {
            float healthDifference = damage / 100;
            transform.localScale = new Vector2(transform.localScale.x - healthDifference, transform.localScale.y);
            transform.position = new Vector2(transform.position.x - healthDifference, transform.position.y);
            invincibilityTime = 2;
        }
    }
}
