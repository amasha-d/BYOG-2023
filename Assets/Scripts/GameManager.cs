using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject nextLevelUI;
    public GameObject gameLoseUI;
    public GameObject Player;

    [Header("finish line")]
    public GameObject entryGate;
    public GameObject treeLight;
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
            //GameObject.Destroy(Player);
            nextLevelUI.SetActive(true);
        }
        else
        {
            Debug.Log("collect the seed");
        }
        
    }

    public void GameLose()
    {
        GameObject.Destroy(Player);
        gameLoseUI.SetActive(true);
    }

    public void CollectSeed()
    {
        seedCollected = true;
        entryGate.SetActive(false);
        treeLight.SetActive(true);
    }


}
