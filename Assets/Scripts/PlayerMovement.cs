using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
    public PlayerController controller;
    public Animator anim;
    public GameManager gameManager;

    public float runSpeed = 40f;
    public bool canMove = true;
    bool jump = false;
    float horizontalMove = 0;
    public GameObject gameWinUI;
    [SerializeField] float reloadDelay = 1f;

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }

    }

    private void FixedUpdate()
    {
        anim.SetFloat("speed", Mathf.Abs(horizontalMove));
        if (canMove)
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        }
        else
        {
            controller.Move(0, false, false);
        }
        jump = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "FinishLine")
        {
            canMove = false;
            gameManager.FinishLine();
        }
        if (collision.gameObject.tag == "Death")
        {
            gameManager.GameLose();
        }
        
        if (collision.gameObject.tag == "L1")
        {
            gameManager.ChooseLevel(1);
        }
        if (collision.gameObject.tag == "L2")
        {
            gameManager.ChooseLevel(2);
        }
        if (collision.gameObject.tag == "L3")
        {
            gameManager.ChooseLevel(3);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Death")
        {
            gameManager.GameLose();
        }
        if (collision.gameObject.tag == "Seed")
        {
            //gameManager.seedCollected = true;
            gameManager.CollectSeed();
            GameObject.Destroy(collision.gameObject);
            Debug.Log("seed collected");
        }
        if (collision.gameObject.tag == "FinishLine")
        {
            //canMove = false;
            gameManager.FinishLine();
        }
        if (collision.gameObject.tag == "L1")
        {
            gameManager.ChooseLevel(1);
        }
        if (collision.gameObject.tag == "L2")
        {
            gameManager.ChooseLevel(2);
        }
        if (collision.gameObject.tag == "L3")
        {
            gameManager.ChooseLevel(3);
        }
    }
    void ReloadScene()
    {
        SceneManager.LoadScene(1);
    }
}

