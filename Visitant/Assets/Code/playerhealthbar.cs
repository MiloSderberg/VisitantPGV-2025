using UnityEngine;
using UnityEngine.SceneManagement;

public class playerhealth : MonoBehaviour
{
    //To set up the healthbar, first drag in the PlayerHealthbar prefab and then drag in "RemainingHealth" into the field of "Healthbar"
    public GameObject healthbar;
    public float health = 100;
    public float damage = 0;
    float invincibilityTime = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        invincibilityTime -= Time.deltaTime;
        if (health == 0)
        {
            SceneManager.LoadScene(0);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("canHealPlayer"))
        {
            if (health < 100)
            {
                health += 5;
                float healthDifference = 0.05f;
                healthbar.transform.localScale = new Vector2(healthbar.transform.localScale.x + healthDifference, healthbar.transform.localScale.y);
                healthbar.transform.position = new Vector3(healthbar.transform.position.x + healthDifference, healthbar.transform.position.y, healthbar.transform.position.z);
            }
        }
        if (collision.CompareTag("canDamagePlayer") && invincibilityTime <= 0)
        {
            print("hej");
            health -= damage;
            float healthDifference = damage / 100;
            healthbar.transform.localScale = new Vector2(healthbar.transform.localScale.x - healthDifference, healthbar.transform.localScale.y);
            healthbar.transform.position = new Vector3(healthbar.transform.position.x - healthDifference, healthbar.transform.position.y, healthbar.transform.position.z);
            invincibilityTime = 0.5f;
        }
    }
}
