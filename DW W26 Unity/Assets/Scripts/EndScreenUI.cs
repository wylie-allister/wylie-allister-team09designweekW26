using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenUI : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;

    [SerializeField] private string gameplaySceneName = "Dual Monitor Scene";
    [SerializeField] private string menuSceneName = "Team Select";

    private void Start()
    {
        if (titleText == null) return;

        if (GameManager.LastWinner == GameManager.Winner.Bunnies)
            titleText.text = "BUNNIES WIN!";
        else if (GameManager.LastWinner == GameManager.Winner.Foxes)
            titleText.text = "FOXES WIN!";
        else
            titleText.text = "GAME OVER";
    }

    public void Restart()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
