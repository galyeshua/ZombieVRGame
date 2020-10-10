using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private Manager manager;
    private GameUIManager UIManager;

    [SerializeField] private GameObject muzzleFlashPrefab;
    [SerializeField] private Transform barrelLocation;

    [SerializeField] private AudioSource m_no_ammo_sound;
    [SerializeField] private AudioSource m_shoot_sound;
    [SerializeField] private AudioSource m_reload_sound;

    [SerializeField] private float numOfBulletsPerLoad = 12f;

    private float currNumOfBullets;
    private bool inAction = false; // for detecting loading or shoting

    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<Manager>();
        UIManager = GameObject.Find("GameManager").GetComponent<GameUIManager>();

        setAmmoValue(numOfBulletsPerLoad); // update amount of ammo
    }

    public IEnumerator Shoot(Transform hit)
    {
        // befor act, check if game stil active and that gun is not in action
        if (manager.isGameActive() && inAction == false)
            // check if there are bullets
            if (currNumOfBullets > 0)
            {
                m_shoot_sound.Play();
                GetComponent<Animator>().SetTrigger("Fire"); // start animation of fire
                setAmmoValue(currNumOfBullets - 1); // dec ammo count by 1

                inAction = true; // gun in action until end of shoot
                yield return new WaitForSeconds(0.1f);

                if (hit != null) // check if the hit is enemy
                    if (hit.GetComponent<Enemy>() != null)
                        hit.GetComponent<Enemy>().hitted();
                
                yield return new WaitForSeconds(0.4f);
                inAction = false;
            }
            else // no ammo
            {
                m_no_ammo_sound.Play();
            }
                
    }

    private void setAmmoValue(float newValue)
    {
        // change amount of ammo and display it to the screen
        currNumOfBullets = newValue;
        UIManager.updateAmmoText(newValue);
    }


    IEnumerator Reload()
    {
        // load only if the are less then numOfBulletsPerLoad and if the game is active
        if (currNumOfBullets < numOfBulletsPerLoad)
            if (manager.isGameActive())
            {
                m_reload_sound.Play();
                GetComponent<Animator>().SetTrigger("Reload"); // start animation of Reload

                inAction = true; // gun in action until end of reload
                yield return new WaitForSeconds(3f);
                inAction = false;

                setAmmoValue(numOfBulletsPerLoad);
            }
    }

    void ShootBullet()
    {
        // create flash effect
        GameObject tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);
        Destroy(tempFlash, 0.5f);
    }
}
