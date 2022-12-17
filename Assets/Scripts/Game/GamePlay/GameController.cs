using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class DifficultyAtributtes
{
    public int extraStartingHazards;
    public int extraAddingHazards;
    public int bossWave;
    public int maxPowerUps;
    public int playerLifes;
    public float playerFireRate;
}
public class GameController : MonoBehaviour
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    public static Difficulty difficulty;

    public DifficultyAtributtes easy;
    public DifficultyAtributtes medium;
    public DifficultyAtributtes hard;

    private DifficultyAtributtes currentDif;

    [Header("Hazards")]
    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int hazardCount;
    public int addHazards;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    [Header("Bosses")]
    public int bossWave = 7;
    public Vector3 bossSpawnValues;
    public GameObject[] bosses;

    [Header("PowerUps")]
    public GameObject[] powers;
    public int maxPowersCount;

    [Header("Respawn")]
    public float flashingTime = 1f;
    private float tmpFT;

    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI waveText;
    [SerializeField]
    private Sprite[] _lifesSprites;
    [SerializeField]
    private Image _lifesImage;

    [SerializeField]
    private Sprite[] _powerUpsSprites;
    [SerializeField]
    private Image[] _powerUpImages;
    public int _powerUpsActive;

    private int score;
    private int wave;

    void Start()
    {
        difficulty = MainMenu.difficulty;

        highScoreText.text = "HighScore: " + PlayerPrefs.GetInt("HighScore" + difficulty, 0).ToString();


        score = 0;
        wave = 0;

        switch (difficulty)
        {
            case Difficulty.Easy:
                currentDif = easy;
                break;
            case Difficulty.Medium:
                currentDif = medium;
                break;
            case Difficulty.Hard:
                currentDif = hard;
                break;
        }

        hazardCount += currentDif.extraStartingHazards;
        addHazards += currentDif.extraAddingHazards;
        bossWave = currentDif.bossWave;
        maxPowersCount = currentDif.maxPowerUps;
        wave = MainMenu.wave;
        score = MainMenu.score;
        hazardCount += wave * addHazards;

        GameObject.Find("Player").GetComponent<PlayerController>().fireRate = currentDif.playerFireRate;
        if(MainMenu.lifes != -1)
        {
            GameObject.Find("Player").GetComponent<PlayerController>().health = MainMenu.lifes;
            ChangeLife(MainMenu.lifes);
        }
        else
        {
            GameObject.Find("Player").GetComponent<PlayerController>().health = currentDif.playerLifes;
            ChangeLife(currentDif.playerLifes);

        }
        hidePowerUp();
        UpdateWave();
        UpdateScore();
        StartCoroutine(SpawnWaves());
    }


    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            AddWave();

            if (wave % bossWave == 0) 
            {
                GameObject boss = bosses[Random.Range(0, bosses.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-bossSpawnValues.x, bossSpawnValues.x), bossSpawnValues.y, bossSpawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                GameObject bossClone = Instantiate(boss, spawnPosition, spawnRotation);

                yield return new WaitUntil(bossClone.GetComponent<Boss>().bossDestroyed);
            }
            else
            {
                int powerUpsNumb = Random.Range(0, maxPowersCount);
                float chance = .5f;
                for (int i = 0; i < hazardCount; i++)
                {
                    GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                    Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                    Quaternion spawnRotation = Quaternion.identity;
                    Instantiate(hazard, spawnPosition, spawnRotation);

                    yield return new WaitForSeconds(spawnWait);

                    if (Random.value <= chance && powerUpsNumb > 0)
                    {
                        GameObject powerUp = powers[Random.Range(0, powers.Length)];
                        Instantiate(powerUp, spawnPosition, spawnRotation);
                        powerUpsNumb--;
                    }
                    else
                    {
                        chance += .1f;
                    }
                }
            }

            yield return new WaitForSeconds(waveWait);

            hazardCount += addHazards;
        }
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void AddWave()
    {
        wave ++;
        UpdateWave();
    }

    void UpdateWave()
    {
        waveText.text = "Wave: " + wave;
    }

    public void GameOver()
    {
        StopAllCoroutines();
        if(score > PlayerPrefs.GetInt("HighScore"+difficulty, 0))
        {
            PlayerPrefs.SetInt("HighScore"+ difficulty, score);
            GameObject.Find("Canvas").GetComponent<EnableMenus>().highscoreBeat = true;
        }
        GameObject.Find("Canvas").GetComponent<EnableMenus>().score = score;
        GameObject.Find("Canvas").GetComponent<EnableMenus>().waves = wave;
        GameObject.Find("Canvas").GetComponent<EnableMenus>().gameOver = true;
        SaveSystem.SavePlayer(difficulty.ToString(), -1, wave - 1, GameObject.FindWithTag("Player").GetComponent<PlayerController>().health);

    }

    public void ChangeLife(int lifesCount)
    {
        switch (difficulty) {
            case Difficulty.Easy:
                _lifesImage.sprite = _lifesSprites[lifesCount];
                break;
            case Difficulty.Medium:
                _lifesImage.sprite = _lifesSprites[lifesCount];
                break;
            case Difficulty.Hard:
                _lifesImage.sprite = _lifesSprites[4+lifesCount];
                break;
        }
    }

    public void Respawn()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            tmpFT = flashingTime;
            StartCoroutine(FlashMesh(playerObject));
        }
    }

    public void showPowerUp(PowerUp.PowerUps powerUp, float duration) {

        switch (_powerUpsActive)
        {
            case 0:
                foreach(Image im in _powerUpImages)
                {
                    im.enabled = false;
                    im.sprite = null;
                }
                break;
            case 1:
                switch (powerUp)
                {
                    case PowerUp.PowerUps.DoubleTap:
                        _powerUpImages[0].gameObject.GetComponent<PowerUpUI>().duration = duration;
                        _powerUpImages[0].gameObject.GetComponent<PowerUpUI>().enabled = true;
                        _powerUpImages[0].sprite = _powerUpsSprites[0];
                        break;
                    case PowerUp.PowerUps.Shield:
                        _powerUpImages[0].gameObject.GetComponent<PowerUpUI>().duration = duration;
                        _powerUpImages[0].gameObject.GetComponent<PowerUpUI>().enabled = true;
                        _powerUpImages[0].sprite = _powerUpsSprites[1];
                        break;
                    case PowerUp.PowerUps.Twinshot:
                        _powerUpImages[0].gameObject.GetComponent<PowerUpUI>().duration = duration;
                        _powerUpImages[0].gameObject.GetComponent<PowerUpUI>().enabled = true;
                        _powerUpImages[0].sprite = _powerUpsSprites[2];
                        break;
                }
                _powerUpImages[0].enabled = true;
                break;
            case 2:
                switch (powerUp)
                {
                    case PowerUp.PowerUps.DoubleTap:
                        _powerUpImages[1].gameObject.GetComponent<PowerUpUI>().duration = duration;
                        _powerUpImages[1].gameObject.GetComponent<PowerUpUI>().enabled = true;
                        _powerUpImages[1].sprite = _powerUpsSprites[0];
                        break;
                    case PowerUp.PowerUps.Shield:
                        _powerUpImages[1].gameObject.GetComponent<PowerUpUI>().duration = duration;
                        _powerUpImages[1].gameObject.GetComponent<PowerUpUI>().enabled = true;
                        _powerUpImages[1].sprite = _powerUpsSprites[1];
                        break;
                    case PowerUp.PowerUps.Twinshot:
                        _powerUpImages[1].gameObject.GetComponent<PowerUpUI>().duration = duration;
                        _powerUpImages[1].gameObject.GetComponent<PowerUpUI>().enabled = true;
                        _powerUpImages[1].sprite = _powerUpsSprites[2];
                        break;
                }
                _powerUpImages[1].enabled = true;
                break;
            case 3:
                switch (powerUp)
                {
                    case PowerUp.PowerUps.DoubleTap:
                        _powerUpImages[2].gameObject.GetComponent<PowerUpUI>().duration = duration;
                        _powerUpImages[2].gameObject.GetComponent<PowerUpUI>().enabled = true;
                        _powerUpImages[2].sprite = _powerUpsSprites[0];
                        break;
                    case PowerUp.PowerUps.Shield:
                        _powerUpImages[2].gameObject.GetComponent<PowerUpUI>().duration = duration;
                        _powerUpImages[2].gameObject.GetComponent<PowerUpUI>().enabled = true;
                        _powerUpImages[2].sprite = _powerUpsSprites[1];
                        break;
                    case PowerUp.PowerUps.Twinshot:
                        _powerUpImages[2].gameObject.GetComponent<PowerUpUI>().duration = duration;
                        _powerUpImages[2].gameObject.GetComponent<PowerUpUI>().enabled = true;
                        _powerUpImages[2].sprite = _powerUpsSprites[2];
                        break;
                }
                _powerUpImages[2].enabled = true;
                break;
        }
    }

    public void hidePowerUp()
    {
        switch (_powerUpsActive)
        {
            case 0:
                foreach (Image im in _powerUpImages)
                {
                    im.enabled = false;
                    im.sprite = null;
                    _powerUpImages[0].gameObject.GetComponent<PowerUpUI>().enabled = false;

                }
                break;
            case 1:
                _powerUpImages[0].sprite = _powerUpImages[1].sprite;
                _powerUpImages[1].enabled = false;
                _powerUpImages[1].sprite = null;
                _powerUpImages[2].enabled = false;
                _powerUpImages[1].gameObject.GetComponent<PowerUpUI>().enabled = false;
                _powerUpImages[2].gameObject.GetComponent<PowerUpUI>().enabled = false;

                break;
            case 2:
                _powerUpImages[0].sprite = _powerUpImages[1].sprite;
                _powerUpImages[1].sprite = _powerUpImages[2].sprite;

                _powerUpImages[2].sprite = null;
                _powerUpImages[2].enabled = false;
                _powerUpImages[2].gameObject.GetComponent<PowerUpUI>().enabled = false;

                break;
        }
    }

    IEnumerator FlashMesh(GameObject playerObject)
    {
        playerObject.GetComponent<MeshCollider>().enabled = false;

        while (tmpFT > 0)
        {
            playerObject.transform.GetChild(0).gameObject.SetActive(false);
            yield return new WaitForSeconds(tmpFT);
            playerObject.transform.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(tmpFT);
            tmpFT -= 0.2f;
        }
        playerObject.GetComponent<MeshCollider>().enabled = true;
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(difficulty.ToString(), score, wave-1, GameObject.FindWithTag("Player").GetComponent<PlayerController>().health);
    }
}