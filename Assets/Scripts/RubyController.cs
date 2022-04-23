using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public static int level = 1;
    public float speed = 3.0f;

    public int maxHealth = 5;

    public GameObject damageSparks;
    public GameObject projectilePrefab;

    public AudioClip throwSound;
    public AudioClip hitSound;

    public int health { get { return currentHealth; } }
    int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    AudioSource audioSource;

    public TextMeshProUGUI robotsFixed;
    public TextMeshProUGUI stateText;
    public int numFix = 0;

    public bool isDone = false;
    public bool won = false;
    public bool lost = false;

    public GameObject musicPlayer;
    public AudioSource music;
    public AudioClip winMusic;
    public AudioClip loseMusic;
    public bool musicPlayed = false;
    public AudioClip cogPickup;

    public TextMeshProUGUI ammoText;
    public int ammo = 4;

    // Start is called before the first frame update
    void Start()
    {
        music = musicPlayer.GetComponent<AudioSource>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        robotsFixed.text = "Fixed: " + numFix;
        ammoText.text = "Ammo: " + ammo;


        //Makes it so the game is lost the moment zero health is reached
        if (currentHealth <= 0 && !isDone)
        {
            ChangeHealth(0);
        }

        //Handles movement if game is not done
        if (!isDone)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            Vector2 move = new Vector2(horizontal, vertical);

            if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                lookDirection.Set(move.x, move.y);
                lookDirection.Normalize();
            }

            animator.SetFloat("Look X", lookDirection.x);
            animator.SetFloat("Look Y", lookDirection.y);
            animator.SetFloat("Speed", move.magnitude);

            if (isInvincible)
            {
                invincibleTimer -= Time.deltaTime;
                if (invincibleTimer < 0)
                    isInvincible = false;
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                Launch();
            }
        }

        //If win condition is reached
        if (isDone && won && level == 2)
        {
            stateText.gameObject.SetActive(true);
            stateText.text = "You Won! \n Press R to restart";
            if (!musicPlayed)
            {
                music.loop = false;
                music.Stop();
                music.clip = winMusic;
                music.Play();
                musicPlayed = true;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("Main");
                level = 1;
            }
        }

        //If loss condition is reached
        if (isDone && won && level == 1)
        {
            stateText.gameObject.SetActive(true);
            level++;
            isDone = false;
            stateText.text = "You won level 1 \n Talk To Jambi By Pressing X to go to the next level!";
        }

        if (isDone && lost)
        {
            stateText.gameObject.SetActive(true);
            if (!musicPlayed)
            {
                music.loop = false;
                music.Stop();
                music.clip = loseMusic;
                music.Play();
                musicPlayed = true;
            }
            stateText.text = "You Lost! \n Press R to restart";
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (level == 1)
                {
                    SceneManager.LoadScene("Main");
                }
                if (level == 2)
                {
                    SceneManager.LoadScene("level2");
                }
            }
        }

        //quits out of the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        //handles talking to npcs
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                Npc character = hit.collider.GetComponent<Npc>();
                if (character != null && level == 1)
                {
                    character.DisplayDialog();
                }
                else
                {
                    character.loadLevel();
                }
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (currentHealth > 0)
        {
            if (amount < 0)
            {
                if (isInvincible)
                    return;

                isInvincible = true;
                invincibleTimer = timeInvincible;

                Instantiate(damageSparks, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
                PlaySound(hitSound);
            }

            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

            UiHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
        }
        else
        {
            isDone = true;
            lost = true;
            return;
        }
    }

    public void ChangeScore(int amount)
    {
        if (numFix < 4)
        {
            numFix = numFix + amount;
            robotsFixed.text = "Fixed: " + numFix;
        }
        if (numFix >= 4)
        {
            isDone = true;
            won = true;
            return;
        }

    }

    void Launch()
    {
        if (ammo > 0)
        {
            GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(lookDirection, 300);

            animator.SetTrigger("Launch");

            PlaySound(throwSound);
            ammo--;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "ammo")
        {
            ammo = ammo + 3;
            Destroy(other.gameObject);
            PlaySound(cogPickup);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public int getLevel()
    {
        return level;
    }
}