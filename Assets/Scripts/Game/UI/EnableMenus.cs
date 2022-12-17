using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class EnableMenus : MonoBehaviour
{
    public bool gamePaused = false;

    [Header("Player HUB")]
    public GameObject playerHUB;

    [Header("Help Menu")]
    public GameObject helpMenu;
    private Scrollbar scrollBar;
    public bool helpMenuActive = false;

    [Header("Pause Menu")]
    public GameObject pauseMenu;
    public GameObject settingMenu;
    public bool pauseMenuActive = false;

    [Header("Gameover Menu")]
    public GameObject ganeOverMenu;
    public GameObject highscoreText;
    public bool highscoreBeat = false;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI waveText;
    public int score;
    public int waves;
    public bool gameOver = false;
    public bool gameOverMenuActive = false;

    // Start is called before the first frame update
    void Start()
    {
        scrollBar = helpMenu.GetComponentInChildren<Scrollbar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                if (!helpMenuActive)
                {
                    if (pauseMenuActive)
                    {
                        DisablePauseMenu();
                    }
                    EnableHelpMenu();
                }
                else
                {
                    DisableHelpMenu();
                }
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                if (!pauseMenuActive)
                {
                    if (helpMenuActive)
                    {
                        DisableHelpMenu();
                    }
                    EnablePauseMenu();
                }
                else
                {
                    DisablePauseMenu();
                }
            }
        }
        else
        {
            if (!gameOverMenuActive)
            {
                GameOver();
            }
        }
    }


    public void EnableHelpMenu()
    {
        Pause();
        helpMenu.SetActive(true);
        scrollBar.value = 1f;
        helpMenuActive = true;
    }

    public void DisableHelpMenu()
    {
        Resume();
        helpMenu.SetActive(false);
        helpMenuActive = false;
    }

    public void EnablePauseMenu()
    {
        Pause();
        pauseMenu.SetActive(true);
        pauseMenuActive = true;
    }

    public void DisablePauseMenu()
    {
        Resume();
        DisableSettingsMenu();
        pauseMenu.SetActive(false);
        pauseMenuActive = false;
    }

    public void DisableSettingsMenu()
    {
        settingMenu.SetActive(false);

    }

    private void GameOver()
    {
        playerHUB.SetActive(false);
        if (highscoreBeat)
        {
            highscoreText.SetActive(true);
        }
        AudioListener.volume /= 3;

        Time.timeScale = 0.1f;
        ganeOverMenu.SetActive(true);
        scoreText.text = "Score: " + score;
        waveText.text = "Waves: " + waves;
        gameOverMenuActive = true;

    }

    private void Pause()
    {
        playerHUB.GetComponent<CanvasGroup>().alpha = 0f;
        gamePaused = true;
        AudioListener.volume /= 3; 
        Time.timeScale = 0f;
    }

    private void Resume()
    {
        AudioListener.volume *= 3;


        playerHUB.GetComponent<CanvasGroup>().alpha = 1f;

        gamePaused = false;
        Time.timeScale = 1f;
    }
}
