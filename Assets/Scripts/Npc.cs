using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Npc : MonoBehaviour
{
    public float displayTime = 4.0f;
    public GameObject dialogBox;
    public GameObject dialogBox2;
    float timerDisplay;
    private RubyController rubyController;
    public int level;
    public GameObject item;
    public GameObject itemPos;
    public int state = 0;

    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;
        GameObject rubyControllerObject = GameObject.FindWithTag("RubyController");
        if (rubyControllerObject != null)
        {

            rubyController = rubyControllerObject.GetComponent<RubyController>();

            print("Npc can see player script");

        }
        if (rubyControllerObject == null)
        {

            print("Cant find player script");

        }
    }

    void Update()
    {
        level = rubyController.getLevel();
        state = rubyController.getGameState();
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false);
                dialogBox2.SetActive(false);
            }
        }
    }

    public void DisplayDialog()
    {
        //state zero is base state and state one is load level 2
        if (state == 0)
        {
            timerDisplay = displayTime;
            dialogBox.SetActive(true);
        }
        if (state == 1)
        {
            loadLevel();
            rubyController.setGameState(0);
        }
        //state two should create the battery box and state three should end the game
        if (state == 2)
        {
            timerDisplay = displayTime;
            dialogBox2.SetActive(true);
            Instantiate(item, itemPos.transform.position, Quaternion.identity);
            rubyController.setGameState(2);
        }
        //state two should be final state
        if (state == 3)
        {
            rubyController.setGameState(4);
        }
    }

    public void loadLevel()
    {
        if (level == 2)
        {
            SceneManager.LoadScene("Level2");
        }
    }
}
