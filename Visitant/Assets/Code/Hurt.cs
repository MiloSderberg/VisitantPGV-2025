using UnityEngine;

public class Hurt : MonoBehaviour
{
    public float hurtAmount;
    public bool KillOnDamage;
    public bool Player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() && Player != true)
        {
            Player pHealth = collision.gameObject.GetComponent<Player>();
            pHealth.pHurt(hurtAmount);
        }
        if (collision.gameObject.GetComponent<Enemy>())
        {
            Enemy eHealth = collision.gameObject.GetComponent<Enemy>();
            eHealth.eHurt(hurtAmount);
            if (KillOnDamage == true)
            {
                Destroy(gameObject);
            }
        }
    }
}
