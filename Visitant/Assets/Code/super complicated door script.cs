using UnityEngine;

public class supercomplicateddoorscript : MonoBehaviour
{
    Animator animator;
    Collider2D collider;
    float opentime = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (opentime <= 0)
        {
            animator.SetBool("open", false);
            collider.isTrigger = false;
        }
        if (opentime < 2)
        {
            opentime -= Time.deltaTime;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetBool("open", true);
        opentime -= Time.deltaTime;
        collider.isTrigger = true;
    }
}
