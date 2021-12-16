using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool Paused;
    private GameObject PausePanel;

    public GameObject Player;
    public PlayerController Pc;

    public TextMesh Level1Gems;
    public TextMesh Level2Gems;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        PausePanel = GameObject.Find("PausePanel");
        PausePanel.SetActive(false);
        Pc = Player.GetComponent<PlayerController>();
        int amount1 = PlayerPrefs.GetInt("Level1");
        int amount2 = PlayerPrefs.GetInt("Level2");
        Level1Gems.text = amount1 + "/4";
        Level2Gems.text = amount2 + "/11";


    }

    //Pauses or resumes the game when activated
    public void PauseGame()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            PausePanel.SetActive(true);


        }
        else if (Time.timeScale == 0)
        {
  
            Time.timeScale = 1;
            PausePanel.SetActive(false);

        }
    }

    public void StartGame()
    {
        if(Pc.levelSelected != "")
        {
            SceneManager.LoadScene(Pc.levelSelected);
        }
       
    }

    // Quits the game whenever its activated
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Application Quit");
    }

    //Restarts the current scene/level
    public void Restart()
    {
        Time.timeScale = 1;
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        Paused = false;
    }

    public void Menu()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
        SceneManager.LoadScene("Menu");
    }

    

    // Update is called once per frame
    void Update()
    {

        // Pausing and resuming the game when pressing ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
            Paused = true;
        } 
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartGame();
        }
    }
}
