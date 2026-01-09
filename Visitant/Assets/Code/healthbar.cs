using UnityEngine;

public class healthbar : MonoBehaviour
{
    bool ifHit = false;
    public float damage = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            float healthDifference = damage / 100;
            transform.localScale = new Vector2(transform.localScale.x - healthDifference, transform.localScale.y);
            transform.position = new Vector2(transform.position.x - healthDifference / 2, transform.position.y);
        }
    }
}
