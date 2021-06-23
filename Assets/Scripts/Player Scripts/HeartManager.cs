using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{

    public Image[] hearts;
    public Sprite fullheart;
    public Sprite halfFullHeart;
    public Sprite emptyHeart;
    public FloatValue heartContainers;
    public FloatValue playerCurrentHealth;


    // Start is called before the first frame update
    void Start()
    {
        InitHearts();
    }

    public void InitHearts()
    {
        for (int i = 0; i < heartContainers.initialValue; i++)
        {
            hearts[i].gameObject.SetActive(true);
            hearts[i].sprite = fullheart;
        }
    }

    public void UpdateHearts()
    {
        float tempHealth = playerCurrentHealth.runTimeValue / 2;
        for(int i = 0; i < heartContainers.initialValue; i++)
        {
            if(i <= tempHealth-1)                                     //fullHeart
            {
                hearts[i].sprite = fullheart;
            }
            else if(i >= tempHealth)                                 //emptyHeart
            {
                hearts[i].sprite = emptyHeart;
            }
            else                                                    //halfHeart
            {
                hearts[i].sprite = halfFullHeart;            
            }
        }
    }
}
