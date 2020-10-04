using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    SimpleShoot gun;
    private float m_lifes = 100f;
    [SerializeField] private Text m_lifeText;
    [SerializeField] private Text m_ammoText;
    [SerializeField] private float numOfBulletsPerLoad = 7f;
    private float currNumOfBullets;
    private Manager manager;

    [SerializeField] private AudioSource m_no_ammo_sound;
    [SerializeField] private AudioSource m_shoot_sound;
    [SerializeField] private AudioSource m_reload_ammo_sound;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<Manager>();
        gun = GetComponentInChildren<SimpleShoot>();
        cangeAmmoCount(numOfBulletsPerLoad);
    }

    // Update is called once per frame
    void Update()
    {
        checkInputs();

        if (m_lifes < 0)
            manager.gameOver();
    }

    private void checkInputs()
    {
        if (Input.anyKeyDown)
        {
            bool playerCanShoot = true;
            RaycastHit m_hit;
            Ray m_ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

            //check if player hit EXIT or RELOAD buttons
            if (Physics.Raycast(m_ray, out m_hit))
            {
                if (m_hit.rigidbody != null)
                {
                    switch (m_hit.transform.name)
                    {
                        case "Reload":
                            playerCanShoot = false;
                            cangeAmmoCount(numOfBulletsPerLoad);
                            break;

                        case "Exit":
                            playerCanShoot = false;
                            //Exit Game
                            break;
                    }
                }
            }

            if (playerCanShoot)
                Shoot();
        }
    }


    private void Shoot()
    {
        if (currNumOfBullets > 0)
        {
            m_shoot_sound.Play();
            gun.Shoot();
            cangeAmmoCount(currNumOfBullets - 1); // dec ammo count by 1
        }
        else
        {
            m_no_ammo_sound.Play();
        }
    }

    private void cangeAmmoCount(float n)
    {
        // if n is full ammo amount start reload animation
        if(n == numOfBulletsPerLoad)
        {
            //m_reload_ammo_sound.Play();
            gun.Reload();
        }

        currNumOfBullets = n;
        m_ammoText.text = "" + currNumOfBullets;
    }

    public void hitByZombie(float hitPower)
    {
        m_lifes -= hitPower;
        m_lifeText.text = "" + (int)m_lifes;
    }


}
