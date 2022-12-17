using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeaponController : MonoBehaviour
{
    [Header("Shield")]
    public bool shielded;
    public GameObject ShieldObject;
    public float shieldDelay;
    public float shieldRate;

    [Header("Light Attacks")]
    public GameObject lightShot;
    public Transform[] lightShotSpawns;
    public float lightDelay;
    public float lightFireRate;

    [Header("Heavy Attacks")]
    public GameObject[] heavyShots;
    public Transform[] heavyShotSpawns;
    public Vector3 laserSpawnOffset;
    public Transform droneSpawnPoint;
    public float heavyDelay;
        public float heavyFireRate;


    private bool lightFinished;
    private bool heavyFinished;

    void Start()
    {
        Invoke("LightAttack", lightDelay);
        GetComponent<Boss>().enabled = true;
        Invoke("HeavyAttack", heavyDelay);
        InvokeRepeating("Shield", shieldDelay, shieldRate + ShieldObject.GetComponent<BossShield>().duration + ShieldObject.GetComponent<BossShield>().activationTime * 2);

    }

    void Shield()
    {
        GameObject shieldClone = Instantiate(ShieldObject, transform);

    }

    void LightAttack()
    {
        StartCoroutine(LightAttackShot());
    }

    IEnumerator LightAttackShot()
    {
        while (true)
        {
            lightFinished = false;

            switch (Random.Range(0, 6))
            {
                case 0:
                    StartCoroutine(LightPattern1());
                    break;
                case 1:
                    StartCoroutine(LightPattern2());
                    break;
                case 2:
                    StartCoroutine(LightPattern3());
                    break;
                case 3:
                    StartCoroutine(LightPattern4());
                    break;
                case 4:
                    StartCoroutine(LightPattern5());
                    break;
                case 5:
                    StartCoroutine(LightPattern6());
                    break;
            }

            yield return new WaitUntil(() => lightFinished);

            yield return new WaitForSeconds(lightFireRate);
        }
    }

    IEnumerator LightPattern1()
    {
        for (int i = 0; i < 2; i++)
        {
            foreach (Transform lightShotSpawn in lightShotSpawns)
            {
                Instantiate(lightShot, lightShotSpawn.position, lightShotSpawn.rotation);
            }
            yield return new WaitForSeconds(0.4f);
        }

        lightFinished = true;

    }

    IEnumerator LightPattern2()
    {
        foreach (Transform lightShotSpawn in lightShotSpawns)
        {
            Instantiate(lightShot, lightShotSpawn.position, lightShotSpawn.rotation);
            yield return new WaitForSeconds(0.2f);
        }

        lightFinished = true;
    }

    IEnumerator LightPattern3()
    {         
        for(int i =0;i<=4;i++)
        {

            if (i==0)
                Instantiate(lightShot, lightShotSpawns[4 - i].position, lightShotSpawns[4 - i].rotation);
            else
            {
                Instantiate(lightShot, lightShotSpawns[4 - i].position, lightShotSpawns[4 - i].rotation);
                Instantiate(lightShot, lightShotSpawns[4 + i].position, lightShotSpawns[4 + i].rotation);

            }
            yield return new WaitForSeconds(0.2f);

        }

        lightFinished = true;

    }

    IEnumerator LightPattern4()
    {
        int x = Random.Range(0, lightShotSpawns.Length);
        for (int i = 0; i <= 4; i++)
        {

            if (i == 0)
                Instantiate(lightShot, lightShotSpawns[4 - i].position, lightShotSpawns[x].rotation);
            else
            {
                Instantiate(lightShot, lightShotSpawns[4 - i].position, lightShotSpawns[x].rotation);
                Instantiate(lightShot, lightShotSpawns[4 + i].position, lightShotSpawns[x].rotation);

            }
            yield return new WaitForSeconds(0.1f);

        }

        lightFinished = true;

    }

    IEnumerator LightPattern5()
    {
        int x = Random.Range(0, lightShotSpawns.Length);

        foreach (Transform lightShotSpawn in lightShotSpawns)
        {
            Instantiate(lightShot, lightShotSpawn.position, lightShotSpawns[x].rotation);
            yield return new WaitForSeconds(0.2f);
        }

        lightFinished = true;

    }

    IEnumerator LightPattern6()
    {
        int x = Random.Range(0, lightShotSpawns.Length);

        for (int i = 0; i < 2; i++)
        {
            foreach (Transform lightShotSpawn in lightShotSpawns)
            {
                Instantiate(lightShot, lightShotSpawn.position, lightShotSpawns[x].rotation);
            }
            yield return new WaitForSeconds(0.4f);
        }

        lightFinished = true;

    }


    void HeavyAttack()
    {
        StartCoroutine(HeavyAttackShot());
    }

    IEnumerator HeavyAttackShot()
    {
        while (true)
        {
            heavyFinished = false;

            switch (Random.Range(0, 5))
            {
                case 0:
                    StartCoroutine(HeavyPattern1());
                    break;
                case 1:
                    StartCoroutine(HeavyPattern2());
                    break;
                case 2:
                    StartCoroutine(HeavyPattern3());
                    break;
                case 3:
                    StartCoroutine(HeavyPattern4());
                    break;
                case 4:
                    StartCoroutine(HeavyPattern5());
                    break;
            }

            yield return new WaitUntil(() => heavyFinished);

            yield return new WaitForSeconds(heavyFireRate);
        }
    }

    IEnumerator HeavyPattern1()
    {
        GameObject heavyBolt = heavyShots[0];
        GameObject clone = null;
        foreach (Transform heavyShotSpawn in heavyShotSpawns)
        {
            clone = Instantiate(heavyBolt, heavyShotSpawn.position, heavyShotSpawn.rotation);
        }


        yield return new WaitUntil(() => clone == null);

        heavyFinished = true;

    }

    IEnumerator HeavyPattern2()
    {
        GameObject heavyBolt = heavyShots[0];
        Transform heavyShotSpawn = heavyShotSpawns[Random.Range(0, heavyShotSpawns.Length)];
        GameObject clone = Instantiate(heavyBolt, heavyShotSpawn.position, heavyShotSpawn.rotation);



        yield return new WaitUntil(() => clone == null);

        heavyFinished = true;
    }

    IEnumerator HeavyPattern3()
    {

        GameObject heavyBolt = heavyShots[1];
        Transform heavyShotSpawn = heavyShotSpawns[Random.Range(0, heavyShotSpawns.Length)];
        GameObject clone =  Instantiate(heavyBolt, heavyShotSpawn.position + laserSpawnOffset, heavyBolt.transform.rotation);


        yield return new WaitUntil(() => clone == null);

        heavyFinished = true;

    }

    IEnumerator HeavyPattern4()
    {

        GameObject heavyBolt = heavyShots[1];
        GameObject clone = null;
        foreach (Transform heavyShotSpawn in heavyShotSpawns)
        {
            clone = Instantiate(heavyBolt, heavyShotSpawn.position + laserSpawnOffset, heavyBolt.transform.rotation);
        }


        yield return new WaitUntil(() => clone == null);

        heavyFinished = true;

    }


    IEnumerator HeavyPattern5()
    {
        GameObject drones = heavyShots[2];
        GameObject clone = Instantiate(drones, droneSpawnPoint.position, droneSpawnPoint.rotation);

        yield return new WaitUntil(() => clone == null);

        heavyFinished = true;

    }

    private void OnDisable()
    {
        StopAllCoroutines();
        CancelInvoke("Shield");
    }
}




