using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpUI : MonoBehaviour
{

    public float duration;

    private float passed;

    public Image image;
    // Start is called before the first frame update
    void Start()
    {
        passed = 0;
        image.type = Image.Type.Filled;

    }

    // Update is called once per frame
    void Update()
    {
        passed += Time.deltaTime;

        image.fillAmount = 1 - (passed / duration);
    }

    private void OnDisable()
    {
        image.fillAmount = 1;
        passed = 0;
        duration = 0;
    }
}
