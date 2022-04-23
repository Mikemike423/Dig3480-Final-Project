using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        rigidbody2d.AddForce(transform.right * 150);
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    void Update()
    {
        if (transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "RubyController")
        {
            RubyController player = other.gameObject.GetComponent<RubyController>();

            if (player != null)
            {
                player.ChangeHealth(-1);
            }
            Destroy(this.gameObject);
        }
        if (other.gameObject.tag != "shooter")
        {
            Destroy(this.gameObject);
        }
    }
}
