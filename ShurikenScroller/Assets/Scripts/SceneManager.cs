using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SceneManager : MonoBehaviour
{
    public List<Monster> enemyList = new List<Monster>();
    public List<FireBall> fireBallList = new List<FireBall>();
    public List<Shuriken> shurikenList = new List<Shuriken>();
    public PlayerController player;
    public GameObject crossHair;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Monster enemy in enemyList)
        {
            //calls each enemy's unique update method
            enemy.UpdateHolder();

            foreach(Shuriken shuriken in shurikenList)
            {
                if(CollisionDetector(shuriken.spriteRenderer, enemy.spriteRenderer))
                {
                    //stops updating both the enemy and the projectile if they hit
                    enemy.isAlive = false; //may not be necesarry
                    enemy.spriteRenderer.enabled = false;
                    enemy.enabled = false;
                    enemyList.Remove(enemy);

                    
                    shurikenList.Remove(shuriken);
                    shuriken.spriteRenderer.enabled = false;
                }
            }
        }

        foreach(FireBall fireBall in fireBallList)
        {
            //collision detection for player and enemy projectile
            if (CollisionDetector(player.spriteRenderer, fireBall.spriteRenderer))
            {
                //on hit removes fireball from scene
                fireBallList.Remove(fireBall);
                fireBall.spriteRenderer.enabled = false;
                fireBall.enabled = false;

                //reduces player hp by set amount
                player.hp -= fireBall.damage;

            }
        }
        //moves player cross hair
        updateCrossHair();
    }


    bool CollisionDetector(SpriteRenderer sprite1, SpriteRenderer sprite2)
    {
        //gets the bounds of both sprites being checked for collisions
        Bounds bounds1 = sprite1.bounds;
        Bounds bounds2 = sprite2.bounds;

        //checks for any intersection
        if (bounds1.Intersects(bounds2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void updateCrossHair()
    {
        //gets mouse location
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = Camera.main.transform.position.y - 0.5f;
        Vector3 clickPos = Camera.main.ScreenToWorldPoint(mousePos);
        //sets mouse.z to 0 to avoid shuriken going behind the camera
        clickPos.z = 0;

        crossHair.transform.position = clickPos;
    }
}
