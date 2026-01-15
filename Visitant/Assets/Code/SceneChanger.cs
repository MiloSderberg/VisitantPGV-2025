using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public string SceneName;

    private void Start()
    {
        Button btn = GetComponent<Button>();

        if (btn != null)
        {
            btn.onClick.AddListener(() =>
            {
                ChangeSceneTo();
            });
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            ChangeSceneTo();
        }
    }

    public void ChangeSceneTo()
    {
        SceneManager.LoadScene(SceneName);
    }
}