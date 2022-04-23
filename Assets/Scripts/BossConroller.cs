using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossConroller : MonoBehaviour
{
    public float speed;
    public bool vertical;
    public float changeTime = 4.0f;

    Rigidbody2D rigidbody2D;
    float timer;
    bool shooting = false;
    int bossHealth = 5;
    bool broken = true;
    public GameObject smoke;

    Animator animator;
    private RubyController rubyController;

    public AudioSource audioSource;
    public AudioClip fixedClip;

    public GameObject bullet;
    public Vector3 gunPos;

    // Start is called before the first frame update
    void Start()
    {

        GameObject rubyControllerObject = GameObject.FindWithTag("RubyController"); //this line of code finds the RubyController script by looking for a "RubyController" tag on Ruby
        if (rubyControllerObject != null)

        {

            rubyController = rubyControllerObject.GetComponent<RubyController>(); //and this line of code finds the rubyController and then stores it in a variable

            print("Found the RubyConroller Script!");

        }
        if (rubyController == null)
        {

            print("Cannot find GameController Script!");

        }
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if (!broken)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            if (!shooting)
            {
                shooting = true;
                GameObject projectileObject = Instantiate(bullet, gunPos, Quaternion.identity);

                Projectile projectile = bullet.GetComponent<Projectile>();
            }
            timer = changeTime;
        }
    }

    void FixedUpdate()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if (!broken)
        {
            return;
        }
        Vector2 position = rigidbody2D.position;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    //Public because we want to call it from elsewhere like the projectile script
    public void Fix()
    {
        if (bossHealth > 0)
        {
            bossHealth--;
        }
        else
        {
            smoke.SetActive(false);
            broken = false;
            rigidbody2D.simulated = false;
            rubyController.ChangeScore(1);
        }
    }
}
