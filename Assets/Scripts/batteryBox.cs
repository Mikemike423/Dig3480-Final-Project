using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batteryBox : MonoBehaviour
{
    private RubyController rubyController;
    // Start is called before the first frame update
    void Start()
    {
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "froghouse")
        {
            rubyController.setGameState(3);
            Destroy(this.gameObject);
        }
    }

}
