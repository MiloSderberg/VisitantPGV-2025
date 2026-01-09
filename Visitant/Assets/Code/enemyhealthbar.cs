using UnityEngine;

public class enemyhealthbar : MonoBehaviour
{
    public float damage = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("canDamageEnemy"))
        {
            float healthDifference = damage / 100;
            transform.localScale = new Vector2(transform.localScale.x - healthDifference, transform.localScale.y);
            transform.position = new Vector2(transform.position.x - healthDifference, transform.position.y);
        }
    }
}
