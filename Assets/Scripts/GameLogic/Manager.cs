using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Manager : MonoBehaviour
{
    private GameUIManager UIManager;

    [SerializeField] private GameObject m_zombieEasyRef;
    [SerializeField] private GameObject m_zombieHardRef;
    [SerializeField] private List<GameObject> m_currentLevelEnemies;

    private bool gameIsActive = false;
    private float level = 1f;
    private float totalNumOfZombies;
    private float m_player_lifes = 100f;

    // Start is called before the first frame update
    void Start()
    {
        UIManager = GetComponent<GameUIManager>();
        UIManager.updateLifeText(m_player_lifes); // init player lifes
        StartCoroutine("createEnemies"); // start creating enemies
        gameIsActive = true; // game is active
    }

    private void Update()
    {
        // if  game is active and player is die, end the game
        if (gameIsActive)
            if (m_player_lifes < 0)
            {
                StopCoroutine("createEnemies");
                gameIsActive = false;
                UIManager.showGameOverPanel(level);
            }
    }


    IEnumerator createEnemies()
    {
        // display text level
        UIManager.switchLevelTexts(true); // display level on big text
        UIManager.updateLevelText(level);
        yield return new WaitForSeconds(2.5f);
        UIManager.switchLevelTexts(false); // display level on small text

        // create list of zombies (m_currentLevelEnemies) and update  the amount (totalNumOfZombies)
        createListOfZombies();

        // create the zombies
        while (m_currentLevelEnemies.Count > 0)
        {
            int zombieIndex = Random.Range(0, m_currentLevelEnemies.Count);
            GameObject zombieRef = m_currentLevelEnemies[zombieIndex]; // select random zombie
            createZombie(zombieRef); // create the zombie
            m_currentLevelEnemies.RemoveAt(zombieIndex); // remove the zombie from the list

            // wait between creations
            if (level <= 3) // easy for level 1 - 2
                yield return new WaitForSeconds(1f);
            else 
                yield return new WaitForSeconds(Random.Range(0.6f, 1.6f));
        }

        // wait while there are zombies alive
        bool roundIsActive = true;
        while (roundIsActive)
        {
            if (totalNumOfZombies > 0)
                yield return new WaitForSeconds(1f);
            else
                roundIsActive = false;
        }

        // add bonus lifes to the player
        if (m_player_lifes <= 95)
        {
            StartCoroutine(UIManager.addBonusLife(m_player_lifes, 5f));
            m_player_lifes += 5;
        }  

        level += 1; // inc level
        StartCoroutine("createEnemies"); // next level
    }

    private void createListOfZombies()
    {
        // add to list and update 
        float numOfZombiesEasy = (int)(2 * level);
        float numOfZombiesHard = (int)(((numOfZombiesEasy + 1) / 3) + (level / 2));
        //Debug.Log("Easy: " + numOfZombiesEasy + ", " + "Hard: " + numOfZombiesHard);

        // insert zombies to the list
        for (int i = 0; i < numOfZombiesEasy; i++)
            m_currentLevelEnemies.Add(m_zombieEasyRef);
        for (int i = 0; i < numOfZombiesHard; i++)
            m_currentLevelEnemies.Add(m_zombieHardRef);

        // update totalNumOfZombies
        totalNumOfZombies = numOfZombiesEasy + numOfZombiesHard;
    }


    public void playerHittedByZombie(float hitPower)
    {
        // dec life of player
        m_player_lifes -= hitPower; // update life
        UIManager.updateLifeText(m_player_lifes); // display it to the screen
    }


    public void decNumOfZombies()
    {
        // zombie die, dec num of zombies
        totalNumOfZombies -= 1;
    }
   

    private void createZombie(GameObject zombie_ref)
    {
        // create zombie by the ref
        float distance = Random.Range(40f, 55f);
        GameObject enemy = Instantiate(zombie_ref, RandomPointOnCircleEdge(distance), Quaternion.identity);
        enemy.transform.LookAt(Vector3.zero);
        enemy.GetComponent<Enemy>().incSpeedByLevel(level);
    }

    private Vector3 RandomPointOnCircleEdge(float radius)
    {
        // returns poins on circle (as vector3) by radius
        var vector2 = Random.insideUnitCircle.normalized * radius;
        return new Vector3(vector2.x, 0, vector2.y);
    }


    public bool isGameActive()
    {
        return gameIsActive;
    }

}
