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
    public int state = 0;

    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;
        GameObject rubyControllerObject = GameObject.FindWithTag("RubyController");

        {

            rubyController = rubyControllerObject.GetComponent<RubyController>();

            print("Npc can see player script");

        }
        if (rubyController == null)
        {

            print("Cant find player script");

        }
    }

    void Update()
    {
        level = rubyController.getLevel();
        //state = 
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
        if (state == 0)
        {
            timerDisplay = displayTime;
            dialogBox.SetActive(true);
        }
        if (state == 1)
        {
            timerDisplay = displayTime;
            dialogBox2.SetActive(true);
            item.SetActive(true);
        }
        if (state == 2)
        {
            timerDisplay = displayTime;
            dialogBox2.SetActive(true);
            rubyController.setGameState();
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
