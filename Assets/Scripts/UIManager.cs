using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject titlePanel;
    [SerializeField] GameObject inGamePanel;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject gamePanel;
    [SerializeField] GameObject howToPlayPanel;
    [SerializeField] GameObject winPanel;
    // Start is called before the first frame update
    void Start()
    {
        ReturnToTitle();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            pausePanel.SetActive(true);
        }
    }

    public void StartGame() {
        titlePanel.SetActive(false);
        inGamePanel.SetActive(true);
        gamePanel.SetActive(true);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void PauseGame() {
        inGamePanel.SetActive(false);
        gamePanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void ResumeGame() {
        pausePanel.SetActive(false);
        gamePanel.SetActive(true);
        inGamePanel.SetActive(true);
    }

    public void ReturnToTitle() {
        titlePanel.SetActive(true);
        inGamePanel.SetActive(false);
        pausePanel.SetActive(false);
        gamePanel.SetActive(false);
        howToPlayPanel.SetActive(false);
    }

    public void HowToPlay() {
        howToPlayPanel.SetActive(true);
        titlePanel.SetActive(false);
        inGamePanel.SetActive(false);
        pausePanel.SetActive(false);
        gamePanel.SetActive(false);
    }

    public void WinGame() {
        titlePanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        inGamePanel.SetActive(false);
        pausePanel.SetActive(false);
        gamePanel.SetActive(false);
        winPanel.SetActive(true);
    }
}
