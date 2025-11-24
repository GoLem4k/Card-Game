using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultManager : MonoBehaviour
{
    public GameObject panel;
    public int record;
    public IntToTxtSaver saver;

    public TextMeshProUGUI currentScore;
    public TextMeshProUGUI recordScore;

    private void Start()
    {
        record = saver.LoadInt();
    }

    public void CheckResults(int score)
    {
        if (score > record)
        {
            record = score;
            saver.SaveInt(record);
        }
    }

    public void ShowResults(int score)
    {
        CheckResults(score);
        currentScore.text = "ВАШ СЧЕТ: " + score;
        recordScore.text = "РЕКОРД: " + record;
        panel.SetActive(true);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}