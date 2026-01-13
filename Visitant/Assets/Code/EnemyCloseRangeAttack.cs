using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyCloseRangeAttack : MonoBehaviour
{
    public float coolDown;
    public float range;
    public float stayFor;
    Vector2 direction;
    float timer;
    float stayTimer;
    Animator am;
    public GameObject damageHitbox;
    public GameObject player;
    GameObject attack;

    Vector3 pos;
    Vector3 snappedDirection;
    float rad;
    float snappedAngle;
    float angle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        am = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        direction = (player.transform.position - transform.position).normalized;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        snappedAngle = Mathf.Round(angle / 180f) * 180f;
        rad = snappedAngle * Mathf.Deg2Rad;
        snappedDirection = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
        pos = transform.position;

        if (attack != null)
        { 
            stayTimer -= Time.deltaTime;
            attack.transform.position = pos + (snappedDirection * range);
            if (stayTimer <= 0) Destroy(attack);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (timer <= 0)
            {
                attack = Instantiate(damageHitbox, pos, transform.rotation);
                stayTimer = stayFor;
                if (am != null) am.Play("Attack");
                timer = coolDown;
            }
        }
    }
}
