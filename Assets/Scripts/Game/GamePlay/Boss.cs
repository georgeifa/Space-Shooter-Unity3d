using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;


[System.Serializable]
public class effectBoundaries
{
    public float xMin, xMax, yMin, yMax, zMin, zMax;
}

public class Boss : MonoBehaviour
{
    public GameObject healthBarOBJ;
    private BossHealthBar healthBar;

    public int BossMaxHealth;
    public int BossCurrentHealth;
    public int damageTaken;

    public int scorePoints;

    public GameObject damageEffect;
    public GameObject defeatedEffect;
    public GameObject explosionEffect;
    public int repeatEffect;
    public effectBoundaries boundaries;

    public bool defeated;
    private bool destroyed;
    private void Start()
    {

        healthBarOBJ = GameObject.FindGameObjectWithTag("HealthBar");

        BossCurrentHealth = BossMaxHealth;
        healthBar = healthBarOBJ.GetComponent<BossHealthBar>();


        healthBar.ActivateHealthBar();
        healthBar.SetMaxHealth(BossMaxHealth);
        GetComponent<MeshCollider>().enabled = true;   
    }

    public bool bossDestroyed()
    {
        return destroyed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BoltPlayer"))
        {

            if (!GetComponent<BossWeaponController>().shielded)
            {
                BossCurrentHealth -= damageTaken;
                healthBar.SetHealth(BossCurrentHealth);
                if(BossCurrentHealth > 0)
                {
                    Instantiate(damageEffect,other.transform.position,other.transform.rotation);
                }
                else
                {
                    StartCoroutine(Defeat());
                }
                Destroy(other.gameObject);
            }
        }
    }

    IEnumerator Defeat()
    {
        GetComponent<BossWeaponController>().enabled = false;
        GetComponent<MeshCollider>().enabled = false;
        healthBar.GetComponent<Animator>().SetBool("bossDefeated", true);
        healthBar.GetComponent<Animator>().SetBool("bossReady", false);

        defeated = true;
        for (int i = 0; i <= repeatEffect; i++)
        {
            Instantiate(defeatedEffect
                , new Vector3(Random.Range(boundaries.xMin, boundaries.xMax), Random.Range(boundaries.yMin, boundaries.yMax), Random.Range(boundaries.zMin, boundaries.zMax))
                , transform.rotation);

            yield return new WaitForSeconds(0.5f);

            if(i == repeatEffect - 2)
            {
                healthBar.DeActivateHealthBar();

            }
        }


        yield return new WaitUntil(() => !defeatedEffect.activeInHierarchy);


        Instantiate(explosionEffect,
            Vector3.Lerp(new Vector3(boundaries.xMin, boundaries.yMin, boundaries.zMin), new Vector3(boundaries.xMax, boundaries.yMax, boundaries.zMax), 0.5f),
            transform.rotation);
        CameraShaker.Instance.ShakeOnce(15f, 15f, .1f, 1f);

        healthBar.GetComponent<Animator>().SetBool("bossDefeated", false);

        destroyed = true;
        GameObject.FindWithTag("GameController").GetComponent<GameController>().AddScore(scorePoints);

        Destroy(gameObject);
    }
}
