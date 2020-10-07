using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject casingPrefab;
    [SerializeField] private GameObject muzzleFlashPrefab;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;
    [SerializeField] private float shotPower = 1000f;

    void Start()
    {
        if (barrelLocation == null)
            barrelLocation = transform;
    }

    public void Shoot()
    {
        GetComponent<Animator>().SetTrigger("Fire");
    }

    public void Reload()
    {

        //GameObject Magazine = GameObject.FindGameObjectWithTag("Magazine");
        //Debug.Log(Magazine.GetComponent<Animator>());
        //Magazine.GetComponent<Animator>().SetTrigger("Reload");
    }

    void ShootBullet()
    {
        //GameObject tempBullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
        //tempBullet.GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);
        GameObject tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);
        Destroy(tempFlash, 0.5f);
        //Destroy(tempBullet, 5f);
    }

}
