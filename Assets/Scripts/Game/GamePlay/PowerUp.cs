using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    public GameObject pickUpEffect;
    public GameObject VFX;

    private GameController gameController;
    public enum PowerUps
    {
        None,
        Twinshot,
        DoubleTap,
        Shield
    };

    [Header("Stats")]
    public PowerUps power;

    public float duration = 4f;

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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickUp(other);
        }
    }

    private void PickUp(Collider player)
    {
        switch (power)
        {
            case PowerUps.Twinshot:
                if (!player.gameObject.GetComponent<PlayerController>().twinFire)
                {
                    Instantiate(pickUpEffect, transform.position, transform.rotation);

                    player.gameObject.GetComponent<PlayerController>().twinFire = true;

                    if (gameController != null)
                    {
                        gameController._powerUpsActive++;
                    }

                    GetComponent<AudioSource>().Play();
                    GetComponent<Collider>().enabled = false;
                    VFX.SetActive(false);
                }
                break;
            case PowerUps.DoubleTap:
                if (!player.gameObject.GetComponent<PlayerController>().doubleTap)
                {
                    Instantiate(pickUpEffect, transform.position, transform.rotation);

                    player.gameObject.GetComponent<PlayerController>().doubleTap = true;

                    if (gameController != null)
                    {
                        gameController._powerUpsActive++;
                    }

                    GetComponent<AudioSource>().Play();
                    GetComponent<Collider>().enabled = false;
                    VFX.SetActive(false);
                }
                break;
            case PowerUps.Shield:
                if (!player.gameObject.GetComponent<PlayerController>().shield)
                {

                    Instantiate(pickUpEffect, transform.position, transform.rotation);

                    player.gameObject.GetComponent<PlayerController>().CreateShield(duration);
                    player.gameObject.GetComponent<PlayerController>().shield = true;

                    if (gameController != null)
                    {
                        gameController._powerUpsActive++;
                    }

                    GetComponent<AudioSource>().Play();
                    GetComponent<Collider>().enabled = false;
                    VFX.SetActive(false);
                }
                break;

        }

        gameController.showPowerUp(power,duration);
        StartCoroutine(stopPowerUp(player));
    }

    IEnumerator stopPowerUp(Collider player)
    {
        switch (power)
        {
            case PowerUps.Twinshot:
                if (player.gameObject.GetComponent<PlayerController>().twinFire)
                {
                    yield return new WaitForSeconds(duration);
                    player.gameObject.GetComponent<PlayerController>().twinFire = false;

                    if (gameController != null)
                    {
                        gameController._powerUpsActive--;
                    }


                }
                break;
            case PowerUps.DoubleTap:
                if (player.gameObject.GetComponent<PlayerController>().doubleTap)
                {
                    yield return new WaitForSeconds(duration);
                    player.gameObject.GetComponent<PlayerController>().doubleTap = false;

                    if (gameController != null)
                    {
                        gameController._powerUpsActive--;
                    }
                }
                break;
            case PowerUps.Shield:
                if (player.gameObject.GetComponent<PlayerController>().shield)
                {
                    yield return new WaitUntil(() => !player.gameObject.GetComponent<PlayerController>().shield);

                    if (gameController != null)
                    {
                        gameController._powerUpsActive--;
                    }
                }
                break;
        }

        gameController.hidePowerUp();

        Destroy(gameObject);
    }


}
