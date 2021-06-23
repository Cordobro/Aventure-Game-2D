using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Playerstate
{
    idle,
    walk,
    attack,
    interact,
    stagger
}


public class PlayerMovement : MonoBehaviour {

    public Playerstate currentState;
    public float speed;
    private Rigidbody2D myRigidBody;
    private Vector3 change;
    private Animator animator;
    private bool timeToUpdateAnimationAndMove;
    public FloatValue currentHealth;
    public Signal playerHealthSignal;
    public VectorValue startingPosition;
    public Inventory playerInventory;
    public SpriteRenderer reveivedItemSprite;

    // Start is called before the first frame update
    void Start()
    {
        currentState = Playerstate.walk;
        animator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        transform.position = startingPosition.initialValue;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState == Playerstate.interact)
        {
            return;
        }

        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("attack") &&
            currentState != Playerstate.attack &&
            currentState != Playerstate.stagger)
        {
            StartCoroutine(AttackCo());
        }
        else if (currentState == Playerstate.walk || currentState == Playerstate.idle)
        {
            timeToUpdateAnimationAndMove = true;
        }
    }

    void FixedUpdate()
    {
        if (timeToUpdateAnimationAndMove)
        {
            UpdateAnimationAndMoving();
            timeToUpdateAnimationAndMove = false;
        }
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = Playerstate.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(0.3f);
        if(currentState != Playerstate.interact) {
            currentState = Playerstate.walk;
        }
        
    }

    public void RaiseItem()
    {
        if(playerInventory.currentItem != null)
        {
            if (currentState != Playerstate.interact)
            {
                animator.SetBool("receive item", true);
                currentState = Playerstate.interact;
                reveivedItemSprite.sprite = playerInventory.currentItem.itemSprite;
            }
            else
            {
                animator.SetBool("receive item", false);
                currentState = Playerstate.idle;
                reveivedItemSprite.sprite = null;
                playerInventory.currentItem = null;
            }
        }
    }

    void UpdateAnimationAndMoving()
    { 
        if (change != Vector3.zero)
        {
            Movecharacter();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    void Movecharacter()
    {
        change.Normalize();
        myRigidBody.MovePosition(
            transform.position + change * speed * Time.deltaTime);
    }

    public void Knock(float knockTime, float damage)
    {
        currentHealth.runTimeValue -= damage;
        playerHealthSignal.Raise();
        if (currentHealth.runTimeValue > 0){
            StartCoroutine(KnockCo(knockTime));
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator KnockCo(float knockTime)
    {
        if (myRigidBody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidBody.velocity = Vector2.zero;
            currentState = Playerstate.idle;
        }

    }

}
