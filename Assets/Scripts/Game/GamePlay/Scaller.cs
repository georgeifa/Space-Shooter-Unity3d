using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaller : MonoBehaviour
{
    public Vector3 startingScale;
    public Vector3 goalScale;
    public float scaleTime;

    private GameObject boss;
    private bool bossDefeated;
    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.FindWithTag("Boss");

        StartCoroutine(ScaleOverTime());
    }

    private void Update()
    {
        if (boss != null)
            bossDefeated = boss.GetComponent<Boss>().defeated;
    }

    IEnumerator ScaleOverTime()
    {

        float timePassed = 0f;
        while (true)
        {
            transform.localScale = Vector3.Lerp(startingScale, goalScale, timePassed / scaleTime);

            if (bossDefeated || timePassed >= scaleTime)
            {
                break;
            }
            yield return null;
            timePassed += Time.deltaTime;
        }
        if (GetComponent<Mover>() != null)
        {
            if(transform.localScale.x <= .7f)
            {
                Destroy(gameObject);
            }else
                GetComponent<Mover>().enabled = true;
        }

    }
}
