using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureChest : Interactable
{
    public Item content;
    public Inventory playerInventory;
    public bool isOpen;
    public Signal raiseItem;
    public GameObject dialogBox;
    public Text dialogText;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerInRange)
        {
            if (!isOpen)
            {
                //chest is open
                OpenChest();
            }
            else
            {
                //chest is already open
                ChestAlreadyOpen();
            }
        }
    }
    public void OpenChest()
    {
        //dialog window on
        dialogBox.SetActive(true);
        //dialog text = contents text
        dialogText.text = content.itemDescription;
        //add content to invetory
        playerInventory.AddItem(content);
        playerInventory.currentItem = content;
        //raise the signal to player to animate
        raiseItem.Raise();
        //raise the context clue off
        context.Raise();
        //set the chest to open
        isOpen = true;
        anim.SetBool("opened", true);
    }

    public void ChestAlreadyOpen()
    {
        //turn the dialog off
        dialogBox.SetActive(false);
        //raise the signal to the player to stop anim
        raiseItem.Raise();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger && !isOpen)
        {
            context.Raise();
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger && !isOpen)
        {
            context.Raise();
            playerInRange = false;
        }
    }
}
