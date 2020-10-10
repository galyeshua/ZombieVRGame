using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject topInfoPanel;

    [SerializeField] private Text m_lifeText;
    [SerializeField] private Text m_ammoText;
    [SerializeField] private Text m_lifeAddText;
    [SerializeField] private Text levelText;
    [SerializeField] private Text levelTextSmall;
    [SerializeField] private Text GameOverMessageText;

    [SerializeField] AudioSource m_click_on_button;

    // for FPS
    private int frameCounter = 0;
    private float timeSum = 0;
    [SerializeField] private Text m_fpsText;

    void Start()
    {
        m_fpsText.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        showFPS();
    }

    private void showFPS()
    {
        timeSum += Time.deltaTime;
        frameCounter++;
        if (frameCounter > 7)
        {
            m_fpsText.text = "FPS: " + (int)(1 / (timeSum / frameCounter));
            frameCounter = 0;
            timeSum = 0;
        }
    }

    public void updateLifeText(float newLifeValue)
    {
        if (newLifeValue >= 0)
            m_lifeText.text = "" + (int)newLifeValue;
        else
            m_lifeText.text = "" + 0;
    }

    public void updateAmmoText(float newValue)
    {
        m_ammoText.text = "" + (int)newValue;
    }

    public void handle_button(string button_name) //check button (Preparing for more buttons for the future)
    {
        m_click_on_button.Play();
        Time.timeScale = 1;

        switch (button_name)
        {
            case "Exit":
                SceneManager.LoadScene("MainMenu");
                break;

            case "GameOver_Back":
                SceneManager.LoadScene("MainMenu");
                break;

            case "GameOver_Again":
                SceneManager.LoadScene("Game");
                break;
        }
    }

    public void showGameOverPanel(float level)
    {
        float last_best_score = PlayerPrefs.GetFloat("score");

        Time.timeScale = 0;

        if (last_best_score > level)
        {
            // score is lower than the best
            GameOverMessageText.text = "Your score: " + level + "\n";
            GameOverMessageText.text += "Best: " + last_best_score;
        }
        else
        {
            // new record
            PlayerPrefs.SetFloat("score", level);
            GameOverMessageText.text = "WOW! You broke the record! \n";
            GameOverMessageText.text += "Your score: " + level;
        }

        gameOverPanel.SetActive(true);
        topInfoPanel.SetActive(false);
    }

    public void updateLevelText(float level)
    {
        levelText.text = "Level " + level;
        levelTextSmall.text = levelText.text;
    }

    public void switchLevelTexts(bool newLevel)
    {
        if(newLevel)
        {
            // level up, display big text
            levelText.gameObject.SetActive(true);
            levelTextSmall.gameObject.SetActive(false);
        } else
        {
            // in game, display small text
            levelText.gameObject.SetActive(false);
            levelTextSmall.gameObject.SetActive(true);
        }
    }

    public IEnumerator addBonusLife(float currentLifePoints, float addValue)
    {
        if (currentLifePoints <= 95)
        {
            m_lifeAddText.gameObject.SetActive(true);
            currentLifePoints += 5;
            updateLifeText(currentLifePoints + 5);
            yield return new WaitForSeconds(2.5f);
        }
        m_lifeAddText.gameObject.SetActive(false);
    }
}
