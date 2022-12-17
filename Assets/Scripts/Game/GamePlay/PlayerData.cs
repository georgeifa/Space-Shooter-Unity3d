using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string difficulty;
    public int score;
    public int wave;
    public int lifes;

    public PlayerData(string difficulty,int score, int wave, int lifes)
    {
        this.difficulty = difficulty;
        this.score = score;
        this.wave = wave;
        this.lifes = lifes;
    }
}
