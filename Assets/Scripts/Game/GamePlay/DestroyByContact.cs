using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour
{
    public GameObject explosion;
    public GameObject playerExplosion;
    public int scoreValue;

    private GameController gameController;
    private PlayerController playerController;


    private void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerController = playerObject.GetComponent<PlayerController>();
        }
        if (playerController == null)
        {
            Debug.Log("Cannot find 'Player Controller' script");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("PowerUp") || other.CompareTag("EnemyBolt") || other.CompareTag("HeavyEnemy") || other.CompareTag("Boss"))
        {
            return;
        }

        if (CompareTag("EnemyBolt") && other.CompareTag("BoltPlayer"))
            return;

        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }

        if (other.tag == "Player")
        {
            if (playerController != null)
            {
                if (playerController.shield)
                {
                    playerController.DestroyShield();
                }
                else
                {
                    Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                    playerController.health -= 1;
                    gameController.ChangeLife(playerController.health);

                    if (playerController.health > 0)
                    {
                        gameController.Respawn();
                    }
                    else
                    {
                        gameController.GameOver();
                        Destroy(other.gameObject);
                    }

                }
            }

        }
        else
        {
            Destroy(other.gameObject);
        }

        gameController.AddScore(scoreValue);

        if(!CompareTag("HeavyEnemy"))
            Destroy(gameObject);
    }
}