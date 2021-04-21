using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EscapePod : MonoBehaviour
{
    Text healthText = null;
    private static int health = 3;
    const string gameOverScene = "Game Over";

    private void Awake()
    {
        healthText = GameObject.Find("Escape Pods Health Text").GetComponent<Text>();
        health = 3;
        SetHealthText();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        health--;
        Destroy(collision.gameObject);
        SetHealthText();
        CheckHealth();
    }

    private void SetHealthText()
    {
        healthText.text = health.ToString();
    }

    private void CheckHealth()
    {
        if(health <= 0)
        {
            GameObject.Find("Fade Image").GetComponent<FadeInOut>().FadeOut();
            StartCoroutine("GameOver");
        }
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(gameOverScene);
    }
}
