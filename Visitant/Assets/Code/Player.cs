using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    SpriteRenderer sr2;
    Animator am;
    float idleTimer;
    bool isCounting = false;
    Collider2D playerCollider;
    public LayerMask groundLayerMask;
    public GameObject tutorial;
    Vector2 direction;
    AudioSource audioSource;
    bool canWarp = true;
    bool canWarpDash = true;
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
    float invincibilityTime;
    // Dash
    public KeyCode dashButton;
    public float dashLength;
    public float dashSpeed;
    public float dashCooldown;
    float dashTimer;
    float dashTimer2;
    public GameObject dashPrefab;
    GameObject spawnedDash;

    float sTimer;
    public TextMeshProUGUI speedrunTimer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        sr2 = dashPrefab.GetComponent<SpriteRenderer>();
        am = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerCollider = GetComponent<Collider2D>();
        cam = Camera.main;
        dashTimer = dashCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        dashTimer -= Time.deltaTime;
        sTimer += Time.deltaTime;
        invincibilityTime -= Time.deltaTime;
        float camX = cam.transform.position.x;
        float camY = cam.transform.position.y;
        float camZ = cam.transform.position.z;
        float playerX = transform.position.x;
        float playerY = transform.position.y;
        float playerZ = transform.position.z;
        // Movement
        if (rb.linearVelocity == new Vector2(0, 0))
        {
            am.Play("IDLE");
            idleTimer += Time.deltaTime;
        }
        else idleTimer = 0;
        if (idleTimer > 3.5) tutorial.SetActive(true);
        else tutorial.SetActive(false);
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            sr.flipX = false;
            if (isGrounded() == true)
            {
                am.Play("WALK");
            }
            if (direction.x < speedLimit) direction.x += speed;
            moving = true;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            sr.flipX = true;
            if (isGrounded() == true)
            {
                am.Play("WALK");
            }
            if (direction.x > -speedLimit) direction.x -= speed;
            moving = true;
        }
        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) && isGrounded())
        {
            am.Play("JUMP");
            direction.y = jumpPower;
        }
        else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && isGrounded() != true && hasJumpedTwize == false)
        {
            am.Play("JUMP");
            direction.y = jumpPower;
            hasJumpedTwize = true;
        }
        else
        {
            direction.y = rb.linearVelocity.y;
        }
        if (Input.GetKey(dashButton) && dashTimer <= 0)
        {
            am.Play("DASH");
            canWarpDash = false;
            if (sr.flipX == false)
            {
                sr2.flipX = true;
                spawnedDash = Instantiate(dashPrefab, new Vector3(playerX + 0.31125f, playerY, playerZ), Quaternion.Euler(0, 180, 0));
                Rigidbody2D rbD = spawnedDash.GetComponent<Rigidbody2D>();
                rbD.linearVelocityX = 1 * dashLength;
            }
            else
            {
                sr2.flipX = false;
                spawnedDash = Instantiate(dashPrefab, new Vector3(playerX - 0.31125f, playerY, playerZ), Quaternion.Euler(0, -180, 0));
                Rigidbody2D rbD = spawnedDash.GetComponent<Rigidbody2D>();
                rbD.linearVelocityX = -1 * dashLength;
            }

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
            canWarpDash = true;
            isCounting = false;
        }
        rb.linearVelocity = direction;
        moving = false;
        if (isGrounded() == true) hasJumpedTwize = false;

        // Dimension warper
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse1)) && canWarp == true && canWarpDash == true)
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
        if (Input.GetKey(KeyCode.T))
        {
            speedrunTimer.transform.position = new Vector3(0,0,0);
            int toSeconds = ((int)sTimer);
            speedrunTimer.text = toSeconds.ToString();
        }
        else speedrunTimer.transform.position = new Vector3(10000,10000,0);
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

        if (moving != true) direction.x = direction.x * friction;
        if (direction.x < 0.1 && direction.x > -0.1) direction.x = 0;
        print(direction.x);

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
        if (collision.CompareTag("canHealPlayer") && Input.GetKey(KeyCode.Q) && health < 100)
        {
            health += 1f;
            healthBar.transform.localScale = new Vector2(healthBar.transform.localScale.x + 0.04f, healthBar.transform.localScale.y);
            healthBar.transform.position = new Vector3(healthBar.transform.position.x + 0.04f, healthBar.transform.position.y, healthBar.transform.position.z);
        }
        if (collision.CompareTag("canDamagePlayer") && invincibilityTime <= 0)
        {
            audioSource.PlayOneShot(hurtSound);
            float damage = collision.GetComponent<damageamount>().damage;
            health -= damage;
            healthBar.transform.localScale = new Vector2(healthBar.transform.localScale.x - damage / 25, healthBar.transform.localScale.y);
            healthBar.transform.position = new Vector3(healthBar.transform.position.x - damage / 25, healthBar.transform.position.y, healthBar.transform.position.z);
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
