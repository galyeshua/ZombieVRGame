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

    [SerializeField] float m_maxBulletHit = 34f;

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
            Transform enemy = null;
            RaycastHit m_hit;

            Ray m_ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f)); // for pc
            //Ray m_ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 4f, Screen.height / 2f, 0f)); // for phone

            Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
            Debug.DrawRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f), forward, Color.green);

            //check if player hit EXIT or RELOAD buttons
            if (Physics.Raycast(m_ray, out m_hit))
            {
                if (m_hit.rigidbody != null)
                {
                    switch (m_hit.transform.tag)
                    {
                        case "AmmoBox":
                            //Reload
                            Debug.Log("Reload");
                            playerCanShoot = false;
                            cangeAmmoCount(numOfBulletsPerLoad);
                            break;

                        case "Enemy":
                            //enemy hitted
                            Debug.Log("Enemy");
                            enemy = m_hit.transform;
                            break;

                        case "UIButton":
                            //check button
                            playerCanShoot = false;
                            Debug.Log("UIButton");

                            if (m_hit.transform.name == "Exit")
                            {
                                Debug.Log("Exit");
                                // Exit Game
                            }
                            break;

                    }
                }
            }

            if (playerCanShoot)
                Shoot(enemy);
        }
    }


    private void Shoot(Transform hit)
    {
        if (currNumOfBullets > 0)
        {
            m_shoot_sound.Play();
            gun.Shoot();
            cangeAmmoCount(currNumOfBullets - 1); // dec ammo count by 1

            if (hit != null && hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().hitByBullet(m_maxBulletHit);
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
