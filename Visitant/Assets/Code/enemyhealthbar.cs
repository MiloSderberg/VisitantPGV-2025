using UnityEngine;

public class enemyhealthbar : MonoBehaviour
{
    //To set up the healthbar, first drag in the PlayerHealthbar prefab and then drag in "RemainingHealth" into the field of "Healthbar"
    public GameObject healthbar;
    public float health = 100;
    public float damage = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("canDamageEnemy"))
        {
            float healthDifference = damage / 100;
            health -= damage;
            healthbar.transform.localScale = new Vector2(healthbar.transform.localScale.x - healthDifference, healthbar.transform.localScale.y);
            healthbar.transform.position = new Vector3(healthbar.transform.position.x - healthDifference, healthbar.transform.position.y, healthbar.transform.position.z);
        }
    }
}
