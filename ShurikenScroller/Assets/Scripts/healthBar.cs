using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class healthBar : MonoBehaviour
{
    public Image HealthBar;
    public float currentHealth;
    public float maxHealth;
    public PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        //player will alawys have their max health at the start of the game
        maxHealth = player.hp;
    }

    // Update is called once per frame
    void Update()
    {

        currentHealth = player.hp;
        //updates the horizontal fill of the image based on how much health the player has remaining
        HealthBar.fillAmount = currentHealth / maxHealth;
    }
}
