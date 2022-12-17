using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float scaleTime;

    public float materialFillMin;
    public float materialFillMax;

    public float duration;

    public float startingFresnelPower;
    public float endFresnelPower;

    private Material material;
    private GameObject boss;
    private bool bossDefeated;
    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.FindWithTag("Boss");
        material = gameObject.GetComponentInChildren<Renderer>().material;
        StartCoroutine(Activation());
    }

    private void Update()
    {
        if (boss != null)
            bossDefeated = boss.GetComponent<Boss>().defeated;
    }

    IEnumerator Activation()
    {
        GetComponentInChildren<LaserSoundEffects>().PlayActivation();
        float currentTime = 0.0f;

        do
        {
            material.SetFloat("Vector1_8465849d1cde4f1c8e55d7a873831406", Mathf.Lerp(materialFillMin, materialFillMax, currentTime / scaleTime));
            currentTime += Time.deltaTime;
            yield return null;
            
            if(currentTime >= scaleTime / 2)
            {
                GetComponent<CapsuleCollider>().enabled = true;
            }
        } while (currentTime <= scaleTime);

        StartCoroutine(DeActivation());
    }

    IEnumerator DeActivationCondition()
    {

        float timePassed = 0f;
        while (true)
        {
            if (bossDefeated || timePassed >= duration)
            {
                break;
            }

            yield return null;
            timePassed += Time.deltaTime;
        }
    }

    IEnumerator DeActivation()
    {
        yield return StartCoroutine(DeActivationCondition());
        bool played = false;

        float currentTime = 0.0f;

        do
        {
            material.SetFloat("Vector1_fd82df3450254d97bbeee6c2b402d832", Mathf.Lerp(startingFresnelPower, endFresnelPower, currentTime / scaleTime));
            currentTime += Time.deltaTime;
            yield return null;

            if (currentTime >= scaleTime / 2)
            {
                GetComponent<CapsuleCollider>().enabled = false;
            }

            if (!played)
            {
                if (currentTime >= scaleTime / 5)
                {
                    GetComponentInChildren<LaserSoundEffects>().PlayShutDown();
                    played = true;
                }
            }

        } while (currentTime <= scaleTime);

        Destroy(gameObject.gameObject);
    }
}
