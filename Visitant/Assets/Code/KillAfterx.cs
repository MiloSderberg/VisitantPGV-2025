using UnityEngine;

public class KillAfterX : MonoBehaviour
{
    public float TimeToDeath;
    float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = TimeToDeath;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) Destroy(gameObject);
    }
}