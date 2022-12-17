using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static GameController.Difficulty difficulty;
    public static int wave;
    public static int score;
    public static int lifes;
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetDifficulty_Easy()
    {
        difficulty = GameController.Difficulty.Easy;
    }

    public void SetDifficulty_Medium()
    {
        difficulty = GameController.Difficulty.Medium;
    }

    public void SetDifficulty_Hard()
    {
        difficulty = GameController.Difficulty.Hard;
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        if (data != null)
        {
            if (data.score != -1)
            {
                score = data.score;
                wave = data.wave;
                switch (data.difficulty)
                {
                    case "Easy":
                        difficulty = GameController.Difficulty.Easy;
                        break;
                    case "Medium":
                        difficulty = GameController.Difficulty.Medium;
                        break;
                    case "Hard":
                        difficulty = GameController.Difficulty.Hard;
                        break;
                }
                lifes = data.lifes;
            }
            else
            {
                NewPlayer();
            }
        }
        else
        {
            NewPlayer();
        }
    }

    public void NewPlayer()
    {
        score = 0;
        wave = 0;
        lifes = -1;
    }
}
