using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip hurtSound;
    public GameObject deathObject;
    public float health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void eHurt(float damage)
    {
        if (audioSource != null && hurtSound != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }
        health -= damage;
        if (health <= 0)
        {
            Instantiate(deathObject, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
