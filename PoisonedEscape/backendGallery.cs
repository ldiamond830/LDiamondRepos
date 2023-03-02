 protected override void SetState()
    {
        int selector;
        //sets state based on a combination of random chance and the enemy's remaining health
        if(Health <= 3)
        {
            currentState = State.enraged;
        }
        else if(Health> 3 && Health >= 10)
        {
            selector = Random.Range(0, 5);
            if(selector == 0)
            {
                currentState = State.enraged;
            }
            else if(selector >0 && selector >= 3)
            {
                currentState = State.aggressive;
            }
            else
            {
                currentState = State.defensive;
            }
        }
        else
        {
            selector = Random.Range(0,1);
            if(selector == 0)
            {
                currentState = State.aggressive;
            }
            else
            {
                currentState = State.aggressive;
            }    
        }
        
        //adjusts speed based on chosen state
        if(currentState == State.enraged)
        {
            speed = enragedSpeed;
        }
        else
        {
            speed = standardSpeed;
        }
    }
