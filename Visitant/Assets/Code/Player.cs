using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D playerCollider;
    public LayerMask groundLayerMask;
    Vector2 direction;
    AudioSource audioSource;
    public string gameOverSceneName;
    bool moving = false;
    bool hasJumpedTwize = false;
    Camera cam;
    public float mapWidth;
    // Movement
    public float jumpPower;
    public float speed;
    public float speedLimit;
    public float friction;
    // Health
    public GameObject healthBar;
    public AudioClip hurtSound;
    public float health;
    // Dash
    public KeyCode dashButton;
    public float dashLength;
    public float dashCooldown;
    float dashTimer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        playerCollider = GetComponent<Collider2D>();
        cam = Camera.main;
        dashTimer = dashCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        dashTimer -= Time.deltaTime;
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
        if (Input.GetKeyDown(dashButton) && dashTimer <= 0)
        {
            float camx = cam.ScreenToWorldPoint(Input.mousePosition).x;
            float camy = cam.ScreenToWorldPoint(Input.mousePosition).y;
            Vector3 cam2 = new Vector3(camx, camy, 0);
            direction = (cam2 - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float snappedAngle = Mathf.Round(angle / 180f) * 180f;
            float rad = snappedAngle * Mathf.Deg2Rad;
            Vector2 snappedDirection = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;

            /*
            if (snappedDirection.x > 0) snappedDirection.x += dashLength;
            else snappedDirection.x -= dashLength;
            direction = direction + snappedDirection;
            dashTimer = dashCooldown;
            */
        }
        if (moving != true) direction.x = direction.x * friction;
        rb.linearVelocity = direction;
        moving = false;
        if (isGrounded() == true) hasJumpedTwize = false;

        // Dimension warper
        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector2 dimPosition = transform.position;
            if (dimPosition.x > 0) dimPosition.x = dimPosition.x - mapWidth;
            else dimPosition.x = dimPosition.x + mapWidth;
            transform.position = dimPosition;
            float camX = cam.transform.position.x;
            float camY = cam.transform.position.y;
            float camZ = cam.transform.position.z;
            camX = camX * -1;
            cam.transform.position = new Vector3(camX, camY, camZ);
        }
    }

    // Health
    public void pHurt(float damage)
    {
        if (audioSource != null && hurtSound != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }
        health -= damage;
        if (health <= 0)
        {
            SceneManager.LoadScene(gameOverSceneName);
        }
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
