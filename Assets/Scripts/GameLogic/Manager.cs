using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject topInfoPanel;
    [SerializeField] Text levelText;
    private bool keepPlay = true;
    private float level = 1f;

    [SerializeField] GameObject m_zombieEasyRef;
    [SerializeField] GameObject m_zombieHardRef;
    [SerializeField] GameObject[] m_currentEnemies;

    private float totalNumOfZombies;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("createEnemies");
        //startNewLevel();

        //for (int i = 0; i < 5; i++)
        //{
        //    createZombie(Random.Range(40f, 50f), m_enemyRef[Random.Range(0, m_enemyRef.Length)]);
        //}
    }


    IEnumerator createEnemies()
    {
        levelText.gameObject.SetActive(true);
        levelText.text = "Level " + level;
        yield return new WaitForSeconds(2.5f);
        levelText.gameObject.SetActive(false);

        float numOfZombiesEasy = (int)(2 * level);
        float numOfZombiesHard = (int)(((numOfZombiesEasy + 1) / 3) + (level / 2));

        totalNumOfZombies = numOfZombiesEasy + numOfZombiesHard;

        Debug.Log("Easy: " + numOfZombiesEasy);
        Debug.Log("Hard: " + numOfZombiesHard);

        keepPlay = true;

        for (int i = 0; i < numOfZombiesEasy; i++)
        {
            createZombie(Random.Range(40f, 55f), m_zombieEasyRef, level);
            yield return new WaitForSeconds(Random.Range(0.3f, 1f));
        }

        for (int i = 0; i < numOfZombiesHard; i++)
        {
            createZombie(Random.Range(40f, 60f), m_zombieHardRef, level);
            yield return new WaitForSeconds(Random.Range(0.6f, 1.5f));
        }

        while(keepPlay)
        {
            if (totalNumOfZombies > 0)
                yield return new WaitForSeconds(1f);
            else
                keepPlay = false;
        }

        level += 1;
        StartCoroutine("createEnemies");
    }

    public void decNumOfZombies()
    {
        totalNumOfZombies -= 1;
    }

    //private void startNewLevel()
    //{
    //    //while(keepPlay)
    //    //{
    //        keepPlay = false;
    //        StartCoroutine("showLevelText");
    //        keepPlay = true;
    //        StartCoroutine("createEnemies");
    //        level += 1;
    //    //}
    //}


    //IEnumerator createEnemies()
    //{
    //    float numOfZombiesEasy = (int)((2 * level) + 1);
    //    float numOfZombiesHard = (int)((numOfZombiesEasy / 3) + (level / 2));

    //    Debug.Log("Easy: " + numOfZombiesEasy);
    //    Debug.Log("Hard: " + numOfZombiesHard);

    //    yield return new WaitForSeconds(0.5f);

    //    for (int i=0; i< numOfZombiesEasy; i++)
    //    {
    //        createZombie(Random.Range(40f, 50f), m_zombieEasyRef);
    //        yield return new WaitForSeconds(Random.Range(0.1f, 0.7f));
    //    }

    //    for (int i = 0; i < numOfZombiesHard; i++)
    //    {
    //        createZombie(Random.Range(40f, 50f), m_zombieHardRef);
    //        yield return new WaitForSeconds(Random.Range(0.1f, 0.7f));
    //    }

    //}


    public void gameOver()
    {
        //keepPlay = false;
        //GameObject.Find("Player").SetActive(false);
        //GameObject.Find("aim").SetActive(true);
        //gameOverPanel.SetActive(true);
        //topInfoPanel.SetActive(false);
    }


    private void createZombie(float distance, GameObject zombie_ref, float level)
    {
        GameObject enemy = Instantiate(zombie_ref, RandomPointOnCircleEdge(distance), Quaternion.identity);
        enemy.transform.LookAt(Vector3.zero);
        enemy.GetComponent<Enemy>().add_level_speed(level);
    }

    private Vector3 RandomPointOnCircleEdge(float radius)
    {
        var vector2 = Random.insideUnitCircle.normalized * radius;
        return new Vector3(vector2.x, 0, vector2.y);
    }

    //IEnumerator showLevelText()
    //{
    //    levelText.gameObject.SetActive(true);
    //    levelText.text = "Level " + level;
    //    yield return new WaitForSeconds(2.5f);
    //    levelText.gameObject.SetActive(false);
    //}
}
