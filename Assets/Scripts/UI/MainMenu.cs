using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    RaycastHit m_hit;
    Ray m_ray;

    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject HowToPlayPanel;
    [SerializeField] GameObject AreYouSurePanel;
    [SerializeField] AudioSource m_click_on_button;

    // Update is called once per frame
    void Update()
    {
        checkInput();
    }

    private void checkInput()
    {
        // check if player click on button
        if (Input.anyKeyDown)
        {
            m_ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            if (Physics.Raycast(m_ray, out m_hit) && m_hit.rigidbody != null)
            {
                if (m_hit.transform.tag == "UIButton")
                    m_click_on_button.Play();

                // Identify the button
                switch (m_hit.transform.name)
                {
                    case "Play":
                        SceneManager.LoadScene("Game");
                        break;


                    case "HowToPlay":
                        menuPanel.SetActive(false);
                        HowToPlayPanel.SetActive(true);
                        break;


                    case "HowToPlay_Back":
                        menuPanel.SetActive(true);
                        HowToPlayPanel.SetActive(false);
                        break;


                    case "Exit":
                        menuPanel.SetActive(false);
                        AreYouSurePanel.SetActive(true);
                        break;


                    case "AreYouSure_Back":
                        menuPanel.SetActive(true);
                        AreYouSurePanel.SetActive(false);
                        break;


                    case "AreYouSure_Exit":
                        Application.Quit();
                        break;

                }

            }
        }
    }
}
