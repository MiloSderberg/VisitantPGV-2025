using UnityEngine;
using UnityEngine.SceneManagement;

public class playerhealth : MonoBehaviour
{
    //To set up the healthbar, first drag in the PlayerHealthbar prefab and then drag in "RemainingHealth" into the field of "Healthbar"
    public GameObject healthbar;
    float invincibilityTime = 0.5f;
    float health = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        invincibilityTime -= Time.deltaTime;
//        if (health <= 0)
        {
//            SceneManager.LoadScene("GameOver");
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("canHealPlayer"))
        {
            if (health < 100)
            {
                health += 1f;
                healthbar.transform.localScale = new Vector2(healthbar.transform.localScale.x + 0.02f, healthbar.transform.localScale.y);
                healthbar.transform.position = new Vector3(healthbar.transform.position.x + 0.02f, healthbar.transform.position.y, healthbar.transform.position.z);
            }
        }
        if (collision.CompareTag("canDamagePlayer") && invincibilityTime <= 0)
        {
            float damage = collision.GetComponent<damageamount>().damage;
            health -= damage;
            healthbar.transform.localScale = new Vector2(healthbar.transform.localScale.x - damage / 50, healthbar.transform.localScale.y);
            healthbar.transform.position = new Vector3(healthbar.transform.position.x - damage / 50, healthbar.transform.position.y, healthbar.transform.position.z);
            invincibilityTime = 0.5f;
        }
    }
}
