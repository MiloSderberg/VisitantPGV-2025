using UnityEngine;
using UnityEngine.UIElements;

public class Attacks : MonoBehaviour
{
    Camera cam;
    Vector2 direction;

    public KeyCode fireButton;
    public GameObject bulletPrefab;
    public float bulletVelocity;
    public float gunCoolDown;
    float gunTimer = 0;

    public GameObject swordPrefab;
    public float swordCoolDown;
    float swordTimer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Direction calculator
        float camx = cam.ScreenToWorldPoint(Input.mousePosition).x;
        float camy = cam.ScreenToWorldPoint(Input.mousePosition).y;
        Vector3 cam2 = new Vector3(camx, camy, 0);
        direction = (cam2 - transform.position).normalized;
        Vector2 snappedDirection = Snap32Direction(direction);
        transform.up = snappedDirection;

        // Gun code 
        gunTimer -= Time.deltaTime;
        if ((fireButton == KeyCode.None || Input.GetKey(fireButton)) && gunTimer <= 0)
        {
            Vector2 pos = transform.position;
            GameObject spawnedBullet = Instantiate(bulletPrefab, pos, transform.rotation);

            Rigidbody2D rb = spawnedBullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = snappedDirection * bulletVelocity;

            gunTimer = gunCoolDown;
        }

        // Sword code
        swordTimer -= Time.deltaTime;
        if (Input.GetKey(KeyCode.Mouse0) && swordTimer <= 0)
        {

            swordTimer = swordCoolDown;
        }
    }

    // ChatGPT snapping code, as I've spend way to much time on this issue.
    Vector2 Snap32Direction(Vector2 direction)
    {
        // Convert direction → angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Snap angle to nearest 11.25°
        float snappedAngle = Mathf.Round(angle / 11.25f) * 11.25f;

        // Convert back to unit vector
        float rad = snappedAngle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
    }
}

