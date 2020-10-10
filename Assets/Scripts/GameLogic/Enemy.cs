using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    private Manager manager;

    private bool attackMode = false;
    private bool isAlive = true;

    private float m_currSpeed = 0.45f;
    private float m_maxSpeed = 10f;

    [SerializeField] private float m_speed = 0.001f;
    [SerializeField] private float m_life_points = 2f;
    [SerializeField] private float m_hitPower = 10f;
    [SerializeField] private Animator m_ZombieController;
    [SerializeField] private AudioSource m_attack_sound;
    [SerializeField] private AudioSource m_hitted_sound;
    [SerializeField] private AudioSource m_die_sound;
    [SerializeField] private NavMeshAgent m_nav_agent;


    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<Manager>();
        m_nav_agent.SetDestination(Vector3.zero);
        setSpeed(m_currSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            // zombie is alive and didnt hit player, keep moving
            if (attackMode == false)
                keepGoToPlayer();

            if (m_life_points <= 0)
                die();
        }
    }

    private void setSpeed(float speed)
    {
        // set new speed for animation and navmash agent
        m_currSpeed = speed;
        m_nav_agent.speed = speed + 0.5f;
        m_ZombieController.SetFloat("Speed", speed);
    }


    private void keepGoToPlayer()
    {
        // dest of zombie from player (camera)
        float dist = Vector3.Distance(Vector3.zero, transform.position); 

        // if zombie is close to player start inc the speed
        if (dist <= 40)
        {
            if (m_currSpeed < m_maxSpeed)
                setSpeed(m_currSpeed + m_speed);
        }
    }


    private void die()
    {
        m_die_sound.Play();
        GetComponent<NavMeshAgent>().ResetPath(); // stop agent movement
        GetComponentInChildren<Animator>().SetTrigger("isDead"); // play die animation
        isAlive = false;
        manager.decNumOfZombies(); // update the manager to dec num of zombies
        Destroy(this.gameObject, 5);
    }


    private void OnCollisionEnter(Collision collision)
    {
        // if zombie enter to PlayerArea start attack
        if (collision.transform.name == "PlayerArea")
            StartCoroutine("attackPlayer");
    }


    IEnumerator attackPlayer()
    {
        // zombie is attacking
        GetComponentInChildren<Animator>().SetTrigger("Attack");
        attackMode = true;
        GetComponent<NavMeshAgent>().ResetPath(); // stop agent movement
        transform.LookAt(Vector3.zero); // look at camera

        // while zombie is alive hit player every 1.6 seconds
        while (isAlive)
        {
            m_attack_sound.Play();
            manager.playerHittedByZombie(m_hitPower);
            yield return new WaitForSeconds(1.6f);
        }
    }


    public void hitted()
    {
        // Zombie hitted, dec life points

        if (isAlive)
            m_life_points -= 1;

        m_hitted_sound.Play();

        //Debug.Log("HIT! rest: " + m_life_points);
    }


    public void incSpeedByLevel(float level)
    {
        //increases zombie speed by level to increase difficulty level
        setSpeed(m_currSpeed + (level / 500));
    }
}
