using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textCurrentScore;
    [SerializeField]
    private TextMeshProUGUI textHighScore;
    [SerializeField]
    private GameObject panelGameOver;
    public void UpdateCurrentScore(int score)
    {
        textCurrentScore.text = score.ToString();
    }
    public void UpdateHighScore(int score)
    {
        textHighScore.text = score.ToString();
    }
    public void OnClickGoToMain()
    {
        SceneManager.LoadScene("Main");
    }
    public void OnClickRestart()
    {
        SceneManager.LoadScene("02");
    }
    public void OnGameOver()
    {
        panelGameOver.SetActive(true);
    }
}
