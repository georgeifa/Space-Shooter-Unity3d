using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{

    public int health = 3;

    public float speed;
    public float tilt;
    public Boundary boundary;

    public GameObject shot;
    public Transform[] shotSpawns;
    public float fireRate;

    private float nextFire;

    [Header("Powerups")]

    public bool twinFire = false;
    public bool doubleTap = false;
    public bool shield = false;
    public GameObject shieldPrefab;
    private GameObject shieldClone;

    private GameObject canvas;
    private void Start()
    {
        canvas = GameObject.Find("Canvas");
    }
    void Update()
    {
        if (!canvas.GetComponent<EnableMenus>().gamePaused)
        {

            if (Input.GetButton("Fire1") && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                if (twinFire)
                {
                    Instantiate(shot, shotSpawns[1].position, shotSpawns[1].rotation);
                    Instantiate(shot, shotSpawns[2].position, shotSpawns[2].rotation);
                }
                else
                {
                    Instantiate(shot, shotSpawns[0].position, shotSpawns[0].rotation);
                }

                if (doubleTap)
                {
                    StartCoroutine(DoubleTapFire());
                }

                GetComponent<AudioSource>().Play();
            }
        }
    }

    

    public void CreateShield(float duration)
    {
        shieldClone = Instantiate(shieldPrefab, gameObject.transform);
        shieldClone.GetComponent<playerShield>().setDuration(duration);
    }

    public void DestroyShield()
    {
        shieldClone.GetComponent<playerShield>().Destroyed();
    }

    private IEnumerator DoubleTapFire()
    {
        yield return new WaitForSeconds(0.1f);

        if (twinFire)
        {
            Instantiate(shot, shotSpawns[1].position, shotSpawns[1].rotation);
            Instantiate(shot, shotSpawns[2].position, shotSpawns[2].rotation);
        }
        else
        {
            Instantiate(shot, shotSpawns[0].position, shotSpawns[0].rotation);
        }

        GetComponent<AudioSource>().Play();

    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        GetComponent<Rigidbody>().velocity = movement * speed;

        GetComponent<Rigidbody>().position = new Vector3
        (
            Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
        );

        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
    }
}
