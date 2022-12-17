using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerShield : MonoBehaviour
{

    public AudioSource source;
    public AudioClip activationClip;
    public AudioClip deActivationClip;

    public float scaleTime;

    public float materialFillMin;
    public float materialFillMax;

    private float duration;

    private Material material;

    public GameObject shieldDestroyEffect;

    void Start()
    {
        material = gameObject.GetComponentInChildren<Renderer>().material;
        StartCoroutine(Activation());
    }

    public void setDuration(float duration)
    {
        this.duration = duration;
    }

    IEnumerator Activation()
    {
        PlayActivation();
        float currentTime = 0.0f;

        do
        {
            material.SetFloat("Vector1_8465849d1cde4f1c8e55d7a873831406", Mathf.Lerp(materialFillMin, materialFillMax, currentTime / scaleTime));
            currentTime += Time.deltaTime;
            yield return null;

        } while (currentTime <= scaleTime);


        StartCoroutine(DeActivation());
    }


    IEnumerator DeActivation()
    {
        yield return new WaitForSeconds(duration);
        PlayShutDown();

        float currentTime = 0.0f;

        do
        {
            material.SetFloat("Vector1_8465849d1cde4f1c8e55d7a873831406", Mathf.Lerp(materialFillMax, materialFillMin, currentTime / scaleTime));
            currentTime += Time.deltaTime;
            yield return null;

        } while (currentTime <= scaleTime);

        Destroy(gameObject);
    }

    public void PlayActivation()
    {
        source.clip = activationClip;
        source.Play();
    }

    public void PlayShutDown()
    {
        source.clip = deActivationClip;
        source.Play();
    }

    public void Destroyed()
    {
        StopAllCoroutines();
        Instantiate(shieldDestroyEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if(GameObject.FindWithTag("Player") != null)
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().shield = false;
    }
}
