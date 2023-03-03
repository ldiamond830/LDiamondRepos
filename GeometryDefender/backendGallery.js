//one of the tower classes I created
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

    fireBullet(dt = 1/12, target = null){
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
        //accessor for the current positon
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
    
    move(dt=1/60){
        //piercing shots travel on the same path all the way through
        if(this.type != "piercing"){
            //alters the path of the bullet to compensate for the movement of the enemy
            this.getPath();  
        }


        this.x += this.fwd.x * this.speed * dt;
        this.y += this.fwd.y * this.speed * dt;
    }

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

//excerpt of the full game loop
function gameLoop(){
   
    if(!paused){
    //calculates delta time
    let dt = 1/app.ticker.FPS;
    if(dt > 1/12) dt =1/12;

//updates each tower the player has purchased
    for(let d of defenseList){

        let target;
        //if this defense has no target
        if(d.target == null){
            target = d.findTarget(enemyList);
        }
       
        //catch for if there are no enemies in range
        if(target != null){
            
            //starts the defense's firing method
            let newBullet = d.fireBullet(dt, target)
            
            //if the defense's fire rate is still on cool down the method will return null
            if(newBullet != null){
                //plays a shooting sound effect and adds the new bullet to screen and list
                shootSound.play();
                bulletList.push(newBullet);
                gameScene.addChild(newBullet);
            } 
        }
    }
      
 function GetBoxWithCursor(grid){
    //gets the mouse's positon
    let mousePosition = app.renderer.plugins.interaction.mouse.global;

   //bounding box test for each grid box
    for(let i = 0; i < 12; i++){
        for(let j = 0; j < 12; j++){
            if(mousePosition.x > grid[i][j].x && mousePosition.x < grid[i][j].x + grid[i][j].width && 
                mousePosition.y > grid[i][j].y && mousePosition.y < grid[i][j].y + grid[i][j].height){
                return grid[i][j];
            }
        }
    }
    //catch for if mouse is outside grid area
    return null;
}


function buySquare(){
    if(money >= 10 && !buying){
      //shows a square which follows the mouse cursor
      cursorSquare.visible = true;
      
      ChangeMoneyBy(-10);
       
      //updates type variable to be used in the CreateDefense function
      type = "square";
      buying = true;
    }
}
