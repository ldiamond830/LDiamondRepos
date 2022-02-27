using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Renamed because calling a script scene manager creates issues with the SceneManagment library
public class SceneController : MonoBehaviour
{
    public List<Monster> enemyList = new List<Monster>();
    public List<FireBall> fireBallList = new List<FireBall>();
    public List<Shuriken> shurikenList = new List<Shuriken>();
    public List<Wave> waveList = new List<Wave>();
    public PlayerController player;
    public GameObject crossHair;
    public Camera cameraObject;
    private float cameraHeight;
    private float cameraWidth;
    private float cameraTop;
    private float cameraBottom;
    private float cameraLeft;
    private float cameraRightEdge;
    public Text scoreText;
    private float endTimer;

    // Start is called before the first frame update
    void Start()
    {
        //sets the camera variables to the size of the main camera
        cameraHeight = cameraObject.orthographicSize * 2f;
        cameraWidth = cameraHeight * cameraObject.aspect;

        foreach(Monster enemy in enemyList)
        {
            //in case I forget to set one in the inspector
            if(enemy.player != player)
            {
                enemy.player = player;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetCameraBounds();

        foreach (Monster enemy in enemyList)
        {
            //calls each enemy's unique update method
            enemy.UpdateHolder();

            //floating skulls deal damage on contact
            if (enemy is FloatingSkull)
            {
                if (CollisionDetector(player.spriteRenderer, enemy.spriteRenderer))
                {
                    player.hp -= enemy.damage;
                    enemy.isAlive = false; //may not be necesarry
                    enemy.spriteRenderer.enabled = false;
                    enemy.enabled = false;
                    enemyList.Remove(enemy);


                }
            }

            foreach (Shuriken shuriken in shurikenList)
            {
                if (CollisionDetector(shuriken.spriteRenderer, enemy.spriteRenderer))
                {
                    //stops updating both the enemy and the projectile if they hit
                    enemy.isAlive = false; //may not be necesarry
                    enemy.spriteRenderer.enabled = false;
                    enemy.enabled = false;
                    enemyList.Remove(enemy);


                    shurikenList.Remove(shuriken);
                    shuriken.enabled = false;
                    shuriken.spriteRenderer.enabled = false;

                    player.score += enemy.PointValue;
                }

                if (!shuriken.isActive)
                {
                    shuriken.enabled = false;
                    shuriken.spriteRenderer.enabled = false;
                    shurikenList.Remove(shuriken);
                }
            }
        }

        foreach (FireBall fireBall in fireBallList)
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
            if (fireBall.hitBack)
            {
                foreach(Monster enemy in enemyList)
                {
                    if(CollisionDetector(fireBall.spriteRenderer, enemy.spriteRenderer))
                    {
                        enemy.spriteRenderer.enabled = false;
                        enemy.enabled = false;
                        enemyList.Remove(enemy);

                        fireBallList.Remove(fireBall);
                        fireBall.spriteRenderer.enabled = false;
                        fireBall.enabled = false;

                        player.score += enemy.PointValue * 2;
                    }
                }
            }

            foreach(Shuriken shuriken in shurikenList)
            {
                if(CollisionDetector(fireBall.spriteRenderer, shuriken.spriteRenderer))
                {
                    //if a shuriken hits a fireball it will bounce off in the opposite direction it was previously moving
                    fireBall.direction = new Vector3(fireBall.direction.x * -1, fireBall.direction.y * -1, 0);

                    shurikenList.Remove(shuriken);
                    shuriken.enabled = false;
                    shuriken.spriteRenderer.enabled = false;
                    fireBall.hitBack = true;
                }


            }

            
           // if (cleaner(fireBall.gameObject))
           // {
           //     fireBallList.Remove(fireBall);
           //     fireBall.spriteRenderer.enabled = false;
           //     fireBall.enabled = false;
           // }
        }

        //wave hit detection
        foreach (Wave wave in waveList)
        {
            if (CollisionDetector(player.spriteRenderer, wave.spriteRenderer))
            {
                wave.spriteRenderer.enabled = false;
                wave.enabled = false;
                waveList.Remove(wave);

                player.hp -= wave.damage;
            }

           // if (cleaner(wave.gameObject))
           // {
           //     waveList.Remove(wave);
           // }
        }
        //moves player cross hair
        updateCrossHair();

        if (player.hp <= 0)
        {
            SceneManager.LoadScene("LossScene");
            
        }



        if(endTimer >= 70)
        {
            scoreText.color = Color.white;
            scoreText.text = "Final Score: " + player.score;
        }
        endTimer += Time.deltaTime;
    }
    //called each frame, changes the camera variable so that they update along with the player's movement
    private void SetCameraBounds()
    {
        
        cameraTop = Mathf.Abs(player.Position.y + cameraHeight);
        cameraBottom = Mathf.Abs(player.Position.y - cameraHeight);
        cameraRightEdge = Mathf.Abs((player.Position.x + cameraWidth))/2;
        cameraLeft = Mathf.Abs((player.Position.x - cameraWidth))/2;


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

    private bool cleaner(GameObject target)
    {
        if(target.transform.position.x < cameraLeft || target.transform.position.x > cameraRightEdge || target.transform.position.y > cameraTop || target.transform.position.y < cameraBottom)
        {
            //removes projectiles that have gone off screne
            target.SetActive(false);

            return true;
        }

        return false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3(player.Position.x, cameraBottom), new Vector3(2,2,2));
        Gizmos.DrawCube(new Vector3(player.Position.x, cameraTop), new Vector3(2, 2, 2));
        Gizmos.DrawCube(new Vector3(cameraRightEdge, player.Position.y), new Vector3(2, 2, 2));
        Gizmos.DrawCube(new Vector3(cameraLeft, player.Position.y), new Vector3(2, 2, 2));
    }
}
