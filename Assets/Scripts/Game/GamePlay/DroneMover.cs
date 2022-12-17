using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Drone_Boundary
{
    public float xEnd, zMin, zMax;
}

public class DroneMover : MonoBehaviour
{
    public Drone_Boundary boundary;
    public float zValue;
    public float transitionTime;

    public GameObject leftDrone, rightDrone;

    public bool moveBack=false;

    private Vector3 newPosL,newPosR;
    private Quaternion newQuatL, newQuatR;

    private Vector3 origPosL, origPosR;
    private Quaternion origQuatL, origQuatR;
    // Start is called before the first frame update
    void Start()
    {
        origPosL = leftDrone.transform.localPosition;
        origPosR = rightDrone.transform.localPosition;

        origQuatL = leftDrone.transform.localRotation;
        origQuatR = rightDrone.transform.localRotation;

        zValue = Random.Range(boundary.zMin, boundary.zMax);

        newPosL = new Vector3(-boundary.xEnd, transform.position.y, zValue);
        newPosR = new Vector3(boundary.xEnd, transform.position.y, zValue);

        newQuatL = Quaternion.Euler(0,90,0);
        newQuatR = Quaternion.Euler(0, -90, 0);

    }

    private void FixedUpdate()
    {
        if (!moveBack)
        {
            leftDrone.transform.localPosition = Vector3.Lerp(leftDrone.transform.localPosition, newPosL, transitionTime * Time.deltaTime);
            leftDrone.transform.localRotation = Quaternion.Slerp(leftDrone.transform.localRotation, newQuatL, transitionTime * Time.deltaTime);


            rightDrone.transform.localPosition = Vector3.Lerp(rightDrone.transform.localPosition, newPosR, transitionTime * Time.deltaTime);
            rightDrone.transform.localRotation = Quaternion.Slerp(rightDrone.transform.localRotation, newQuatR, transitionTime * Time.deltaTime);


            if (Mathf.Floor(leftDrone.transform.localPosition.x) == -boundary.xEnd && Mathf.Ceil(rightDrone.transform.localPosition.x) == boundary.xEnd)
            {
                GetComponent<DroneLaserActivation>().enabled = true;
            }
            else
            {
                GetComponent<DroneLaserActivation>().enabled = false;

            }

        }
        else
        {
            GetComponent<DroneLaserActivation>().enabled = false;

            leftDrone.transform.localPosition = Vector3.Lerp(leftDrone.transform.localPosition, origPosL, transitionTime * Time.deltaTime);
            leftDrone.transform.localRotation = Quaternion.Slerp(leftDrone.transform.localRotation, origQuatL, transitionTime * Time.deltaTime);


            rightDrone.transform.localPosition = Vector3.Lerp(rightDrone.transform.localPosition, origPosR, transitionTime * Time.deltaTime);
            rightDrone.transform.localRotation = Quaternion.Slerp(rightDrone.transform.localRotation, origQuatR, transitionTime * Time.deltaTime);

            if (Mathf.Ceil(leftDrone.transform.localPosition.x) > origPosL.x && Mathf.Floor(rightDrone.transform.localPosition.x) < origPosR.x)
            {
                Destroy(gameObject);
            }
        }

    }


}
