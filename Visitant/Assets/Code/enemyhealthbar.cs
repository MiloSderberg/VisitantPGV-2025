using UnityEngine;
using UnityEngine.SceneManagement;

public class enemyhealthbar : MonoBehaviour
{
    //To set up the healthbar, first drag in the PlayerHealthbar prefab and then drag in "RemainingHealth" into the field of "Healthbar"
    public GameObject healthbar;
    public float health = 200;
    public float damage = 0;
    float damagemult;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("canDamageEnemy"))
        {
            float damage = collision.GetComponent<damageamount>().damage;
            health -= damage * 2;
            healthbar.transform.localScale = new Vector2(healthbar.transform.localScale.x - damage / 50, healthbar.transform.localScale.y);
            healthbar.transform.position = new Vector3(healthbar.transform.position.x - damage / 50, healthbar.transform.position.y, healthbar.transform.position.z);
        }
    }
}
