using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public Text txtScore;
    // Start is called before the first frame update
    void Start()
    {
        
        int score = PlayerPrefs.GetInt("PlayerScore");
        txtScore.text = "Pisteet:" + score + "/4";
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level Selection");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
