using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class BossShield : MonoBehaviour
{
    private BossHealthBar healthBar;

    public int shieldHealth;
    public int damageTaken;
    public int duration;

    public GameObject damageEffect;
    public GameObject breakEffect;

    public float activationTime;
    public float startingMaterialFill;
    public float endMaterialFill;

    private Material material;
    private GameObject boss;

    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.FindWithTag("Boss");
        healthBar = GameObject.FindWithTag("HealthBar").GetComponent<BossHealthBar>();
        material = gameObject.GetComponentInChildren<Renderer>().material;
        StartCoroutine(Activation());
    }

    IEnumerator Activation()
    {
        GetComponent<BossShieldSoundEffects>().PlayActivation();
        float currentTime = 0.0f;

        do
        {
            material.SetFloat("Vector1_8465849d1cde4f1c8e55d7a873831406", Mathf.Lerp(startingMaterialFill, endMaterialFill, currentTime / activationTime));
            currentTime += Time.deltaTime;
            yield return null;

            if (currentTime >= activationTime / 2)
            {             
                gameObject.GetComponentInParent<BossWeaponController>().shielded = true;
                GetComponent<MeshCollider>().enabled = true;
                healthBar.healthToShield(shieldHealth);
            }
        } while (currentTime <= activationTime);




        StartCoroutine(DeActivation());
    }

    IEnumerator DeActivation()
    {

        yield return new WaitForSeconds(duration);

        GetComponent<BossShieldSoundEffects>().PlayShutDown();

        float currentTime = 0.0f;

        do
        {
            material.SetFloat("Vector1_8465849d1cde4f1c8e55d7a873831406", Mathf.Lerp(endMaterialFill, startingMaterialFill, currentTime / activationTime));
            currentTime += Time.deltaTime;
            yield return null;

            if (currentTime >= activationTime / 2)
            {
                gameObject.GetComponentInParent<BossWeaponController>().shielded = false;
                GetComponent<MeshCollider>().enabled = false;
            }

        } while (currentTime <= activationTime);

        resetHealthBar();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BoltPlayer"))
        {
            shieldHealth -= damageTaken;
            healthBar.SetShield(shieldHealth);
            if (shieldHealth > 0)
            {
                damageShield(other);
            }
            else
            {
                breakShield();
            }
            Destroy(other.gameObject);

        }
    }

    private void damageShield(Collider other)
    {
        Instantiate(damageEffect, other.transform.position, other.transform.rotation);
    }

    private void breakShield()
    {
        gameObject.GetComponentInParent<BossWeaponController>().shielded = false;
        Instantiate(breakEffect, gameObject.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().rotation);
        CameraShaker.Instance.ShakeOnce(7f,7f,.1f, 1f);
        resetHealthBar();
        Destroy(gameObject);


    }

    private void resetHealthBar()
    {
        healthBar.ShieldToHealth(boss.GetComponent<Boss>().BossMaxHealth, boss.GetComponent<Boss>().BossCurrentHealth);
    }
}
