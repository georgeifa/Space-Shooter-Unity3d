using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneLaserActivation : MonoBehaviour
{
    public GameObject droneLaser;
    // Start is called before the first frame update

    private GameObject clone;
    void Start()
    {
        droneLaser.transform.localPosition = new Vector3(droneLaser.transform.localPosition.x, droneLaser.transform.localPosition.y, GetComponent<DroneMover>().zValue);
        clone = Instantiate(droneLaser,gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (clone == null)
        {
            GetComponent<DroneMover>().moveBack = true;
        }
    }
}
