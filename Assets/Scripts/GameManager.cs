using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject nextLevelUI;
    public GameObject gameLoseUI;
    public bool seedCollected;
    void Start()
    {
       // nextLevelUI.SetActive(false);
        //gameLoseUI.SetActive(false);    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }

    public void ChooseLevel(int i)
    {
        SceneManager.LoadScene(i+1);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void FinishLine()
    {
        if(seedCollected)
        {
            nextLevelUI.SetActive(true);
        }
        else
        {
            Debug.Log("collect the seed");
        }
        
    }

    public void GameLose()
    {
        gameLoseUI.SetActive(true);
    }


}
