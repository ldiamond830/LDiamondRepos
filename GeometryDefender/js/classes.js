//I had intended to make this the parent class of square circle and triangle but that caused an issue where the sprite would render properly so I just copy and pasted the all code into each, leaving this here for now in case I can make it work at a later date\
/*
class Defense extends PIXI.Sprite {
    constructor(x=0, y =0){
        super();
        this.x = x;
        this.y = y;
        this.position = {x:this.x, y:this.y};
        this.damage;
        
        this.fireRate;
        this.fireTimer;
        this.range = 15;
        this.target;
        
    }

    fireBullet( dt = 1/12){
        if(target != null){
            let enemyPosition = {x:target.x, y:target.y};
            
            fireTimer -= dt;
    
            
    
            if(fireTimer <= 0){
                fireTimer = fireRate;
                let bulletPath = {x:target.x - this.x, y:target.y - this.y};
                let newBullet = new Bullet(0x00000, this.x, this.y, bulletPath);
                return newBullet
            }
        }
        
    }

    findTarget(enemyList){

        for(let i = 0; i < enemyList.Count; i++){
            if(GetDistanceSquared(this.position, enemyList[i].position) < range * range){
                return enemyList[i];
            }
        }

        return null;
    }
}
*/
class Square extends PIXI.Sprite{
    constructor(x = 0, y =0){
        
        super(app.loader.resources["images/square.png"].texture);
        this.x = x;
        this. y = y;
        //sets the anchor to the center of the image
        this.anchor.set(0.5);
        this.damage = 25;
        this.fireRate = 1;
        this.fireTimer = this.fireRate;
        this.range = 200;
        this.target;

    }

    fireBullet( dt = 1/12, target = null){
        if(target != null){

            
            //reduces the time till the bullet is fired at each call
            this.fireTimer -= dt;
    
            
            //when the timer is at 0
            if(this.fireTimer <= 0){
                //resets the timer
                this.fireTimer = this.fireRate;
                //creates a new bullet
                let newBullet = new Bullet(0x00ace6, this.x + 20, this.y + 20, target, this.damage);
               
                return newBullet;
            }
            //return if timer > 0
            else{
                return null;
            }
        }
        //return if there's no target in range
        else{
            return null;
        }
       
    }

    findTarget(enemyList){

        for(let e of enemyList){
            //finds an enemy who's distance away is less than range
            if(GetDistanceSquared(this.position, e.position) < this.range * this.range){
                return e;
            }
        }

        //if there are no enemies in range returns null
        return null;
    }
}

class Triangle extends PIXI.Sprite{
    constructor(x = 0, y =0){
        
        super(app.loader.resources["images/triangle.png"].texture);
        this.x = x;
        this. y = y;
        this.anchor.set(0.5);
        //fires fast but does less damage
        this.damage = 10;
        this.fireRate = 0.5;
        this.fireTimer = this.fireRate;
        this.range = 200;
        this.target = null;

    }

    fireBullet( dt = 1/12, target = null){
        if(target != null){
            
            //reduces the time till the bullet is fired at each call
            this.fireTimer -= dt;
    
            
    
            if(this.fireTimer <= 0){
                this.fireTimer = this.fireRate;
                //shots fired by tirangles have a piercing effect meaning they can hit multiple enemies
                let newBullet = new Bullet(0x00ace6, this.x + 20, this.y + 20, target, this.damage, "piercing");
               
                return newBullet;
            }
            //if there is still more time before the next shot
            else{
                return null;
            }
        }
        //no targets in range
        else{
            return null;
        }
       
    }

    findTarget(enemyList){

        for(let e of enemyList){
            //finds an enemy who's distance away is less than range
            if(GetDistanceSquared(this.position, e.position) < this.range * this.range){
                return e;
            }
        }

        //if there are no enemies in range returns null
        return null;
    }
}

class Circle extends PIXI.Sprite{
    constructor(x = 0, y =0){
        
        super(app.loader.resources["images/circle.png"].texture);
        this.x = x;
        this. y = y;
        this.anchor.set(0.5);
        //does more damage but fires slower
        this.damage = 15;
        this.fireRate = 2;
        this.fireTimer = this.fireRate;
        this.range = 200;
        this.target;
        this.burstCounter = 2;

    }

    fireBullet( dt = 1/12, target = null){
        if(target != null){
            
            
            this.fireTimer -= dt;
    
            
    
            if(this.fireTimer <= 0){
                //fires a 3 round burst
                if(this.burstCounter == 0){
                    //after the end of the burst there are 2 seconds until the next burst
                    this.fireTimer = this.fireRate;
                    
                    this.burstCounter = 2;
                   
                }
                else{
                    //if there are shots left in the burst the time till the next shot is set to 0.1
                    this.fireTimer = 0.1;
                    this.burstCounter--;
                }
                
                
                let newBullet = new Bullet(0x00ace6, this.x + 20, this.y + 20, target, this.damage);
               
                return newBullet;
            }
            //if there is still more time before the next shot
            else{
                return null;
            }
        }
         //no targets in range
        else{
            return null;
        }
       
    }

    findTarget(enemyList){

        for(let e of enemyList){
            //finds an enemy who's distance away is less than range
            if(GetDistanceSquared(this.position, e.position) < this.range * this.range){
                return e;
            }
        }

         //if there are no enemies in range returns null
        return null;
    }
}

class GridBox extends PIXI.Graphics{
    constructor(x = 0, y = 0, width = 50, height = 50){
        super();
       
        //draws a hollow rectangle
        this.lineStyle(2,0xFFFFFFF,0.1);
        this.drawRect(0,0, width, height)
        this.endFill();
        
        this.x = x;
        this.y = y;

        //easy accessor for the current positon
        this.position = {x:this.x, y:this.y};
        //bool for if a defense can be placed on this box
        this.availible = true;
        
        
        
    }

    

    
}

class enemy extends PIXI.Sprite{
    constructor(x = 300, y = 0, type = 'standard', sprite = "images/enemy.png"){
        
        //uses a different sprite depending on what is entered in the constructor
        super(app.loader.resources[sprite].texture);

        this.type = type;
        if(this.type == 'standard'){
            this.health = 100;
            //uses negative damage number so that it reduces the value when plugged into the ChangeLifeBy function
            this.damage = -10;
        }
        //boss type enemies have more health and do more damage if they make contact with the power source
        else{
            this.health = 300;
            this.damage = -25;
        }
        
        this.x = x;
        this.y = y;
        this.radius = 20;
        
       
        
        this.direction = {x:0,y:1};
        this.speed = 50;
        this.isAlive = true;
        this.turn5Passed = false;
        
        

    }
    //taken from circle blast exercise
    move(dt=1/60){
        this.x += this.direction.x * this.speed * dt;
        this.y += this.direction.y * this.speed * dt;

        this.changeDirection()
    }

    changeDirection(){
        //changes direction at the location of each turn on the map 
        if(this.y > 100 ){
            this.direction = {x:1,y:0}; 
            
        }
        
        if(this.x > 450 ){
            this.direction = {x:0,y:1};
            
        }

        if(this.y > 250 ){
            this.direction = {x:-1,y:0};
            
        }
       

        if(this.x <100 ){
            this.direction = {x:0,y:1};
            
        }
 
        if(this.y >450){
            this.direction = {x:1,y:0};
            this.turn5Passed = true;
        }

        //has the extra boolean to prevent this from being triggered earlier on the path
        if(this.x > 300 && this.turn5Passed == true){
            this.direction = {x:0,y:1}
            
        }

       
        
    }

    
}

class Bullet extends PIXI.Graphics{
    constructor(color = 0x00ace6, x=0, y=0, target = null, damage = 25, type = "standard"){
        super();
        this.type = type;
        this.damage = damage;
        this.radius = 10;
        //drawn as a circle
        this.beginFill(color);
        this.drawCircle(0,0, this.radius);
        this.endFill();

        this.x = x;
        this.y = y;
        //easy accessor for the current positon
        this.position = {x:this.x, y:this.y};
        this.target = target;
        //variables
        this.fwd;
        
        if(target != null){
            this.getPath();
        }
        this.hasHit;
        if(this.type == "piercing"){
            this.hitCoolDown = 0.2;
            this.hasHit = false;
        }

        this.speed = 300;
        this.isAlive = true;
        Object.seal(this);


    }
    //taken from circle blast exercise and modified
    move(dt=1/60){
        //piercing shots travel on the same path all the way through
        if(this.type != "piercing"){
            //alters the path of the bullet to compensate for the movement of the enemy
            this.getPath();
    ``  }


        this.x += this.fwd.x * this.speed * dt;
        this.y += this.fwd.y * this.speed * dt;
    }

    //gets a normalized vector between the bullet and it's target
    getPath(){
        this.fwd = {x:(this.target.x + this.target.width/2) - this.x, y:(this.target.y + this.target.height/2) - this.y};
        this.fwd = NormalizeVector(this.fwd);
    }

    //for piercing shots, called so that they con't deal damage every frame to objects the intersect
    HitCoolDown(dt = 1/12){
        this.hitCoolDown -= dt;
        if(this.hitCoolDown <= 0){
            this.hasHit = false;
            this.hitCoolDown = 0.2;
        }
    }
}