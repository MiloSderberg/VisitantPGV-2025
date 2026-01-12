using UnityEngine;

public class Fade : MonoBehaviour
{
    public float time;

    public float end;

    private void Update()
    {
        if (time >= end)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            time += Time.deltaTime;
        }
    }
}
