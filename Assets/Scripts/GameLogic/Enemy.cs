using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    Rigidbody rb;
    private bool isTouchPlayer = false;
    private bool isAlive = true;

    private float m_currSpeed = 0f;
    private float m_maxSpeed = 1f;
    [SerializeField] private float m_speed = 0.001f;
    private float m_speed_by_level;
    [SerializeField] private float m_lifes = 100f;

    [SerializeField] private float m_hitPower = 10f;

    [SerializeField] private Animator m_ZombieController;

    [SerializeField] private AudioSource m_attack_sound;
    [SerializeField] private AudioSource m_hitted_sound;
    [SerializeField] private AudioSource m_die_sound;

    [SerializeField] private NavMeshAgent m_nav_agent;


    private Manager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<Manager>();
        rb = GetComponent<Rigidbody>();
        m_ZombieController.SetFloat("Speed", m_currSpeed);
        m_nav_agent.SetDestination(Vector3.zero);
        m_speed_by_level = m_speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            // zombie is alive and didnt hit player, keep moving
            if (isTouchPlayer == false)
                keepGoToPlayer();

            if (m_lifes <= 0)
            {
                m_die_sound.Play();
                GetComponentInChildren<Animator>().SetTrigger("isDead");
                isAlive = false;
                manager.decNumOfZombies();
                Destroy(this.gameObject, 5);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<Player>() != null)
        {
            // Zombie hit player, start to attack
            isTouchPlayer = true;
            transform.LookAt(Vector3.zero);
            GetComponentInChildren<Animator>().SetTrigger("isTouchPlayer");
            StartCoroutine("attackPlayer", collision.transform);
        }
    }

    public void hitByBullet(float maxHit)
    {
        // Zombie hitted by bullet, dec life points
        m_hitted_sound.Play();
        float dist = Vector3.Distance(Vector3.zero, transform.position);
        float hitPower = Mathf.Abs(maxHit - dist);
        m_lifes -= hitPower;
    }

    IEnumerator attackPlayer(Transform player)
    {
        while(isAlive)
        {
            m_attack_sound.Play();
            player.GetComponent<Player>().hitByZombie(m_hitPower);
            yield return new WaitForSeconds(1.3f);
        }
    }

    public void add_level_speed(float level)
    {
        m_speed_by_level = m_speed + (level / 5000);
        //Debug.Log(m_speed_by_level);
    }

    private void keepGoToPlayer()
    {
        // inc speed to max speed
        if (m_currSpeed < m_maxSpeed)
        {
            m_nav_agent.speed = m_currSpeed;
            m_currSpeed += m_speed_by_level;
            m_ZombieController.SetFloat("Speed", m_currSpeed);
        }

        // move Zombie
        transform.position += transform.forward * m_currSpeed / 10f;
    }
}
