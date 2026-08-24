using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private PlayerHealth playerHealth;
    private bool gameOverShown = false;

    private void Update()
    {
        if (playerHealth.IsDead && !gameOverShown)
        {
            gameOverPanel.SetActive(true);
            gameOverShown = true;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
