using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float thrust;
    public float knockTime;
    public float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("breakable") && this.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<pot>().Smash();
        }

        if (collision.gameObject.CompareTag("enemy") || collision.gameObject.CompareTag("Player"))
        { 
            Rigidbody2D hit = collision.GetComponent<Rigidbody2D>();
            if (hit != null)
            {
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * thrust;
                hit.AddForce(difference, ForceMode2D.Impulse);

                if (collision.gameObject.CompareTag("enemy") && collision.isTrigger && !this.gameObject.CompareTag("enemy"))
                {
                    hit.GetComponent<Enemy>().currentState = EnemyState.stagger;
                    collision.GetComponent<Enemy>().Knock(hit, knockTime, damage);
                }

                if (collision.gameObject.CompareTag("Player"))
                {
                    if (collision.GetComponent<PlayerMovement>().currentState != Playerstate.stagger)
                    {
                        hit.GetComponent<PlayerMovement>().currentState = Playerstate.stagger;
                        collision.GetComponent<PlayerMovement>().Knock(knockTime, damage);
                    }
                }
            }
        }
    }
}
