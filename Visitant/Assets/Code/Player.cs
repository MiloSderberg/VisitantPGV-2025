using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Vector3 playerHalfSize;
    Vector2 pos;
    bool isCounting = false;
    Collider2D playerCollider;
    public LayerMask groundLayerMask;
    Vector2 direction;
    AudioSource audioSource;
    bool canWarp = true;
    bool moving = false;
    bool hasJumpedTwize = false;
    Camera cam;
    public float mapWidth;
    bool withinCamWorldX;
    bool withinCamWorldY;
    // Movement
    public float jumpPower;
    public float speed;
    public float speedLimit;
    public float friction;
    // Health
    public GameObject healthBar;
    public AudioClip hurtSound;
    float health = 100;
    public float invincibilityTime;
    // Dash
    public KeyCode dashButton;
    public float dashLength;
    public float dashSpeed;
    public float dashCooldown;
    float dashTimer;
    float dashTimer2;
    public GameObject dashPrefab;
    GameObject spawnedDash;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        playerHalfSize = sr.bounds.extents;
        audioSource = GetComponent<AudioSource>();
        playerCollider = GetComponent<Collider2D>();
        cam = Camera.main;
        dashTimer = dashCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        dashTimer -= Time.deltaTime;
        invincibilityTime -= Time.deltaTime;
        float camX = cam.transform.position.x;
        float camY = cam.transform.position.y;
        float camZ = cam.transform.position.z;
        float playerX = transform.position.x;
        float playerY = transform.position.y;
        float playerZ = transform.position.z;
        // Movement
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (direction.x < speedLimit) direction.x += speed;
            moving = true;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (direction.x > -speedLimit) direction.x -= speed;
            moving = true;
        }
        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) && isGrounded())
        {
            direction.y = jumpPower;
        }
        else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && isGrounded() != true && hasJumpedTwize == false)
        {
            direction.y = jumpPower;
            hasJumpedTwize = true;
        }
        else
        {
            direction.y = rb.linearVelocity.y;
        }
        if (Input.GetKey(dashButton) && dashTimer <= 0)
        {
            float camx = cam.ScreenToWorldPoint(Input.mousePosition).x;
            float camy = cam.ScreenToWorldPoint(Input.mousePosition).y;
            Vector3 cam2 = new Vector3(camx, camy, 0);
            direction = (cam2 - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float snappedAngle = Mathf.Round(angle / 180f) * 180f;
            float rad = snappedAngle * Mathf.Deg2Rad;
            Vector2 snappedDirection = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;

            if (snappedDirection.x > 0) pos = new Vector3((playerX + (playerHalfSize.x * 2)), playerY, playerZ);
            if (snappedDirection.x < 0) pos = new Vector3((playerX - (playerHalfSize.x * 2)), playerY, playerZ);
            spawnedDash = Instantiate(dashPrefab, pos, transform.rotation);
            Rigidbody2D rbD = spawnedDash.GetComponent<Rigidbody2D>();
            rbD.linearVelocity = snappedDirection * dashLength;
            isCounting = true;
            dashTimer2 = dashSpeed;
            dashTimer = dashCooldown;
            invincibilityTime += dashSpeed;
        }

        if (isCounting == true) dashTimer2 -= Time.deltaTime;
        if (isCounting == true && dashTimer2 <= 0)
        {
            transform.position = spawnedDash.transform.position;
            Destroy(spawnedDash);
            isCounting = false;
        }

        if (moving != true) direction.x = direction.x * friction;
        rb.linearVelocity = direction;
        moving = false;
        if (isGrounded() == true) hasJumpedTwize = false;

        // Dimension warper
        if (Input.GetKeyDown(KeyCode.E) && canWarp == true)
        {
            Vector2 dimPosition = transform.position;
            if (dimPosition.x > 0)
            {
                dimPosition.x = dimPosition.x - mapWidth;
                camX = camX - mapWidth;
            }
            else 
            { 
            dimPosition.x = dimPosition.x + mapWidth;
            camX = camX + mapWidth;
            }
            transform.position = dimPosition;
            cam.transform.position = new Vector3(camX, camY, camZ);
        }
        if (health <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
    private void FixedUpdate()
    {
        // Camera
        float camX = cam.transform.position.x;
        float camY = cam.transform.position.y;
        float camZ = cam.transform.position.z;

        Vector3 playerPos = transform.position;
        Vector3 cameraPos = cam.transform.position;
        Vector2 camMover = transform.position - cameraPos;
        camMover = camMover / 12.5f;
        if (withinCamWorldX == true) camX += camMover.x;
        if (withinCamWorldY == true) camY += camMover.y;
        cam.transform.position = new Vector3(camX, camY, -10);
    }

    // Health
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("canHealPlayer"))
        {
            if (health < 100)
            {
                health += 1f;
                healthBar.transform.localScale = new Vector2(healthBar.transform.localScale.x + 0.02f, healthBar.transform.localScale.y);
                healthBar.transform.position = new Vector3(healthBar.transform.position.x + 0.02f, healthBar.transform.position.y, healthBar.transform.position.z);
            }
        }
        if (collision.CompareTag("canDamagePlayer") && invincibilityTime <= 0)
        {
            float damage = collision.GetComponent<damageamount>().damage;
            health -= damage;
            healthBar.transform.localScale = new Vector2(healthBar.transform.localScale.x - damage / 50, healthBar.transform.localScale.y);
            healthBar.transform.position = new Vector3(healthBar.transform.position.x - damage / 50, healthBar.transform.position.y, healthBar.transform.position.z);
            invincibilityTime = 0.5f;
        }
        if (collision.CompareTag("withinCamWorldX")) withinCamWorldX = true;
        if (collision.CompareTag("withinCamWorldY")) withinCamWorldY = true;
        if (collision.CompareTag("noWarpZone")) canWarp = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("withinCamWorldX")) withinCamWorldX = false;
        if (collision.CompareTag("withinCamWorldY")) withinCamWorldY = false;
        if (collision.CompareTag("noWarpZone")) canWarp = true;
    }

    // Teacher made code, I just barley know how it works.
    bool isGrounded()
    {
        if (groundLayerMask == 0)
        {
            return true;
        }
        RaycastHit2D leftHit = Physics2D.Raycast(playerCollider.bounds.min, Vector2.down, 0.3f, groundLayerMask);
        RaycastHit2D rightHit = Physics2D.Raycast(new Vector2(playerCollider.bounds.max.x, playerCollider.bounds.min.y), Vector2.down, 0.3f, groundLayerMask);

        return leftHit || rightHit;
    }
}
