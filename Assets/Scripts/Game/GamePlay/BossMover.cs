using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMover : MonoBehaviour
{
    public float speed;
    public Vector3 battleLocation;

    private new Rigidbody rigidbody;
    private BossWeaponController bossWeaponController;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        bossWeaponController = GetComponent<BossWeaponController>();
        rigidbody.velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z <= battleLocation.z)
        {
            rigidbody.velocity = transform.forward * 0;
            bossWeaponController.enabled = true;
            this.enabled = false;
        }
    }
}
