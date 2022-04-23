using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Npc : MonoBehaviour
{
    public float displayTime = 4.0f;
    public GameObject dialogBox;
    float timerDisplay;
    private RubyController rubyController;
    public int level;

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
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false);
            }
        }
    }

    public void DisplayDialog()
    {
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
    }

    public void loadLevel(){
        if (level == 2)
        {
            SceneManager.LoadScene("Level2");
        }
    }
}
