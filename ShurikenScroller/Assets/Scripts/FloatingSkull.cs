using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingSkull : Monster
{
    private float speed = 0.01f;
    private bool goingUp = true;
    private bool goingDown = false;
    private Vector3 direction;
    private Vector3 velocity;
    private Vector3 position;
    private float increment = 0;
    // Start is called before the first frame update
    void Start()
    {
        //sets fields
        agroRange = 1000;
        isAlive = true;
        damage = 2;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        position = transform.position;
    }


    public override void UpdateHolder()
   {
        Update();

        if (isAgro)
        {
            direction = player.Position - position;
            
            if (goingUp)
            {
                direction.y += increment;
                increment += 0.1f;

                if(increment > 5)
                {
                    goingUp = false;
                    goingDown = true;
                }
            }
            //goingDown should only be true when goingUp is false
            else
            {
                
                    direction.y += increment;
                    increment -= 0.1f;

                    if (increment < -5)
                    {
                        goingUp = true;
                        goingDown = false;
                    }
               
            }
            
            direction.Normalize();
            velocity = direction * speed;
            position += velocity;
            gameObject.transform.position = position;
        }
   }
}
