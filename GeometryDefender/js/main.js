"use strict";

const app = new PIXI.Application({
    //play area is 600x600 shop area is 300x600
    width: 900,
    height: 600
});
document.body.appendChild(app.view);

// constants
const sceneWidth = app.view.width;
const sceneHeight = app.view.height;	



app.loader.
    add([
       
        "images/triangle.png",
        "images/square.png",
        "images/circle.png",
        "images/enemy.png",
        "images/bossEnemy.png"
        
    ]);
app.loader.onProgress.add(e => { console.log(`progress=${e.progress}`) });
app.loader.onComplete.add(setup);
app.loader.load();

//script scope objects for buying defenses
let triangleButton;
let squareButton;
let circleButton;

let cursorTriangle;
let cursorSquare;
let cursorCircle;

let triangleDescriptionBox;
let squareDescriptionBox;
let circleDescriptionBox;

//game variables
let stage = app.stage;
let startScene;
let gameScene;
let gameOverScene;
let instructionsScreen;

let lifeLabel;
let moneyLabel;
let scoreLabel;


let shootSound;
let damageSound;

let enemyDeathSound;

let spawnTimer = 2;
let waveTime = 2;
let bossChance = 0.1;
let grid;
let enemyList = [];
let defenseList = [];
let defenseProjectiles = [];
let money = 0;
let life = 0;
let enemiesDefeated = 0;
let paused = false;
let buying = false;
let newDefense;
let bulletList = [];
let type;
let map;
let gameStarted = false;

function setup(){
    //creates visible start menu screen
	startScene = new PIXI.Container();
    stage.addChild(startScene);

	// creates invisible game screen
    gameScene = new PIXI.Container();
    gameScene.visible = false;
    stage.addChild(gameScene);

	//creates invisible game over scene
	gameOverScene = new PIXI.Container();
    gameOverScene.visible = false;
    stage.addChild(gameOverScene);

    //creates invisible instructions screen
    instructionsScreen = new PIXI.Container();
    instructionsScreen.visible = false;
    stage.addChild(instructionsScreen);

    //adds all labels, buttons, and images to containers
    createLabelsAndButtons();

    
    shootSound = new Howl({
        src: ['sounds/shoot.mp3']
        
    });

    damageSound = new Howl({
        src: ['sounds/takeDamage.mp3']
    });

    enemyDeathSound = new Howl({
        src: ['sounds/enemyDie.mp3']
    });

    //adjusts volumes to more listenable levels
    shootSound.volume(0.01);
    //enemyDeathSound.volume(0.1);
    damageSound.volume(0.05);
    
}

function createLabelsAndButtons(){

    //text styles for game assets
   let labelStyle = new PIXI.TextStyle({
        fill: 0xFFFFFF,
        fontSize: 40,
        fontFamily: "magdaClean",
        stroke: 0x0000FF,
        strokeThickness: 6
    });

    let descriptionStyle = new PIXI.TextStyle({
        fill: 0xFFFFFF,
        fontSize: 20,
        fontFamily: "magdaClean",
        stroke: 0x0000FF,
        strokeThickness: 6
    })


    //start menu
    //I had a problem I didn't manage to fix where the font I downloaded would only display on the labels on the game screen because of an issue with the content loader
    //I decided to use screen shots of the text as sprites in order to get around this, which is why many pure text elements on my start and game over screen are sprites rather than text objects
    
    let startButton = new PIXI.Sprite.from('images/start.png')
    startButton.x = 380;
    startButton.y = sceneHeight - 100;
    startButton.interactive = true;
    startButton.buttonMode = true;
    startButton.on("pointerup", startGame);
    startButton.on('pointerover', e => e.target.alpha = 0.7);
    startButton.on('pointerout', e => e.currentTarget.alpha = 1.0);
    startScene.addChild(startButton);


    let titleLabel = new PIXI.Sprite.from('images/title.png')
    titleLabel.x = 270;
    titleLabel.y = 100;
    startScene.addChild(titleLabel);

    let instructionsButton = new PIXI.Sprite.from("images/viewInstructions.png");
    instructionsButton.x = 270;
    instructionsButton.y = sceneHeight - 250;
    instructionsButton.interactive = true;
    instructionsButton.buttonMode = true;
    instructionsButton.on("pointerup", ShowInstructions);
    instructionsButton.on('pointerover', e => e.target.alpha = 0.7);
    instructionsButton.on('pointerout', e => e.currentTarget.alpha = 1.0);
    startScene.addChild(instructionsButton);

   
    //instructions screen
    let instructions = new PIXI.Sprite.from('images/InstructionsScreen.png');
    instructions.width = 900;
    instructions.height = 600;
    instructionsScreen.addChild(instructions);

    let backButton = new PIXI.Sprite.from("images/backButtonInstructions.png");
    
    backButton.x = 5
    backButton.y = sceneHeight/2 - backButton.height/2;
    backButton.interactive = true;
    backButton.buttonMode = true;
    backButton.on("pointerup", returnToMenu);
    backButton.on('pointerover', e => e.target.alpha = 0.7);
    backButton.on('pointerout', e => e.currentTarget.alpha = 1.0);
    instructionsScreen.addChild(backButton);

    //game screen
    let storeLabel = new PIXI.Text("STORE");
    storeLabel.style = labelStyle;
    storeLabel.x = 675;
    storeLabel.y = 10;
    gameScene.addChild(storeLabel);


    
    //button that allows the user to purchase defenses
    squareButton = new PIXI.Sprite.from('images/square.png');
    squareButton.x = 725;
    squareButton.y = 100;
    squareButton.interactive = true;
    squareButton.buttonMode = true;
    squareButton.on("pointerup", buySquare);
    squareButton.on('pointerover', toggleSquareDescription)
    squareButton.on('pointerout', toggleSquareDescription);
    gameScene.addChild(squareButton);

    //label showing the cost for the respective defense
    let squareLabel = new PIXI.Text("COST: $10");
    squareLabel.style = labelStyle;
    squareLabel.x = 650;
    squareLabel.y = 150;
    gameScene.addChild(squareLabel);

    triangleButton = new PIXI.Sprite.from('images/triangle.png');
    triangleButton.x = 725;
    triangleButton.y = 300;
    triangleButton.interactive = true;
    triangleButton.buttonMode = true;
    triangleButton.on('pointerup', buyTriangle);
    triangleButton.on('pointerover', toggleTriangleDescription)
    triangleButton.on('pointerout', toggleTriangleDescription);
    gameScene.addChild(triangleButton);

    let triangleLabel = new PIXI.Text("COST: $20");
    triangleLabel.style = labelStyle;
    triangleLabel.x = 650;
    triangleLabel.y = 350;
    gameScene.addChild(triangleLabel);

    circleButton = new PIXI.Sprite.from('images/circle.png');
    circleButton.x = 725;
    circleButton.y = 500;
    circleButton.interactive = true;
    circleButton.buttonMode = true;
    circleButton.on('pointerup', buyCircle);
    circleButton.on('pointerover',toggleCircleDescription);
    circleButton.on('pointerout', toggleCircleDescription);
    gameScene.addChild(circleButton);

    let circleLabel = new PIXI.Text("COST: $25");
    circleLabel.style = labelStyle;
    circleLabel.x = 650;
    circleLabel.y = 550
    gameScene.addChild(circleLabel);

    //background map
    map = new PIXI.Sprite.from('images/map.png');
    
    map.interactive = true;
    //allows the user to position the defenses they buy
    map.on('pointerup', createDefense);
    //moves cursor overlay sprites to the mouse's position
    map.on("pointermove", UpdateCursorOverlay)

    gameScene.addChild(map);

    //creates a 12x12 grid on top of the map
    grid = generateGrid();

    //purely decorative, represents the object the player is trying to defend
    let powerSource = new PIXI.Sprite.from('images/powerSource.png');
    powerSource.anchor.set(0.5)
    powerSource.x = 325;
    powerSource.y = 575;
    gameScene.addChild(powerSource);

    //labels that tell the user their money and health
    moneyLabel = new PIXI.Text();
    moneyLabel.style = labelStyle;
    moneyLabel.x = 5;
    moneyLabel.y = 5;
    gameScene.addChild(moneyLabel);
    //method to increase the money variable and change the label
    ChangeMoneyBy(50);

    lifeLabel = new PIXI.Text();
    lifeLabel.style = labelStyle;
    lifeLabel.x = 5;
    lifeLabel.y = 60;
    gameScene.addChild(lifeLabel);
    //method to increase the life variable and change the label
    ChangeLifeBy(100);

    //gameover screen
    let gameOverLabel = new PIXI.Sprite.from("images/gameOver.png");
    gameOverLabel.x = 325;
    gameOverLabel.y = 50;
    gameOverScene.addChild(gameOverLabel);

    let returnToMenuButton = new PIXI.Sprite.from("images/backButtonGameOver.png");
    returnToMenuButton.x = 270;
    returnToMenuButton.y = sceneHeight - 100;
    returnToMenuButton.interactive = true;
    returnToMenuButton.buttonMode = true;
    returnToMenuButton.on("pointerup", returnToMenu);
    returnToMenuButton.on('pointerover', e => e.target.alpha = 0.7);
    returnToMenuButton.on('pointerout', e => e.currentTarget.alpha = 1.0);
    gameOverScene.addChild(returnToMenuButton);

    scoreLabel = new PIXI.Text("Enemies Destroyed: ");
    scoreLabel.style = labelStyle;
    scoreLabel.x = 240;
    scoreLabel.y = sceneHeight/2 - scoreLabel.height/2;
    gameOverScene.addChild(scoreLabel);


    //these overlays follow the mouse around but invisible until the user clicks on one of the buy buttons which will reveal one of them to show the user where they are positioning the defense
   cursorTriangle = new PIXI.Sprite.from("images/triangle.png");
   cursorTriangle.anchor.set(0.5);
   cursorTriangle.visible = false;
   gameScene.addChild(cursorTriangle)

   cursorSquare = new PIXI.Sprite.from("images/square.png");
   cursorSquare.anchor.set(0.5);
   cursorSquare.visible = false;
   gameScene.addChild(cursorSquare);

   cursorCircle = new PIXI.Sprite.from("images/circle.png");
   cursorCircle.anchor.set(0.5);
   cursorCircle.visible = false;
   gameScene.addChild(cursorCircle)

   //description boxes with text for each defense
   triangleDescriptionBox = new PIXI.Sprite.from('images/descriptionBackground.png');
   triangleDescriptionBox.x = 610;
   triangleDescriptionBox.y = 100;
   triangleDescriptionBox.visible = false;
   gameScene.addChild(triangleDescriptionBox);

   let triDescriptionText = new PIXI.Text("TRIANGLE \nFires piercing shots \nDamage = 10\nFire Rate = 120 per minute");
   triDescriptionText.style = descriptionStyle;
   triangleDescriptionBox.addChild(triDescriptionText);

   squareDescriptionBox = new PIXI.Sprite.from('images/descriptionBackground.png');
   squareDescriptionBox.x = 610;
   squareDescriptionBox.y = 250;
   squareDescriptionBox.visible = false;
   gameScene.addChild(squareDescriptionBox);

   let squareDescriptionText = new PIXI.Text("SQUARE\nFires homing shots \nDamage = 20\nFire Rate = 60 per minute")
   squareDescriptionText.style = descriptionStyle;
   squareDescriptionBox.addChild(squareDescriptionText);
  
   circleDescriptionBox = new PIXI.Sprite.from('images/descriptionBackground.png');
   circleDescriptionBox.x = 610;
   circleDescriptionBox.y = 275;
   circleDescriptionBox.visible = false;
   gameScene.addChild(circleDescriptionBox);

   let circleText = new PIXI.Text("CIRCLE\nFires a 3 round burst \nDamge = 50 \nFire Rate = 30 per minute");
   circleText.style = descriptionStyle;
   circleDescriptionBox.addChild(circleText);

}

function startGame(){
    //hides the start menu and game over screen and shows the gameplay scene
    startScene.visible = false;
    gameOverScene.visible = false;
    gameScene.visible = true;
    paused = false;
    //sets stats on each start game so that they're always the same if the user restarts from the game over screen
    waveTime = 2;
    life = 100;
    money = 50;
    ChangeLifeBy(0);
    ChangeMoneyBy(0);
    enemiesDefeated = 0;
    //prevents gameLoop from being added to app.ticker multiple times if the player restarts from the game over screen
    if(!gameStarted){
        
        //starts the game loop
        app.ticker.add(gameLoop);
        gameStarted = true
   
    }
    
}

function returnToMenu(){
    //hides all scenes other than the start menu
    startScene.visible = true;
    //hides all so that the same method can be called to return to menu regaurdless of the scene the player is currently in
    instructionsScreen.visible = false;
    gameOverScene.visible = false;
    gameScene.visible = false;
}

function generateGrid(){
    let innerArray = [];
    let outerArray = [];
    let x = 0;
    let y = 0;
    
    //creates 12 arrays each with 12 gridboxes 
    for(let i = 0; i < 12; i++){

        for(let j = 0; j < 12; j++){
            //each grid box is 50x50
            let gridBox = new GridBox(0,0,50,50);
            //positions the box
            gridBox.x = x;
            gridBox.y = y;
            //adds the box to the array representing this row of the array
            innerArray.push(gridBox);
            //increments the position of for the next box
            x+=50;
            //adds the box to the scene
            gameScene.addChild(gridBox);
        }
        //adds the complete row to the overall array
        outerArray.push(innerArray);
        //clears the array for the next loop
        innerArray = []
        //resets the position to start the next row
        x = 0;
        y += 50;
    }

    //makes it so the user cannot place defense on the grid boxes on the enemies' path
    outerArray[0][6].availible = false;
    outerArray[1][6].availible = false;
    outerArray[2][6].availible = false;
    outerArray[2][7].availible = false;
    outerArray[2][8].availible = false;
    outerArray[2][9].availible = false;
    outerArray[3][9].availible = false;
    outerArray[4][9].availible = false;
    outerArray[5][9].availible = false;
    outerArray[5][8].availible = false;
    outerArray[5][7].availible = false;
    outerArray[5][6].availible = false;
    outerArray[5][5].availible = false;
    outerArray[5][4].availible = false;
    outerArray[5][3].availible = false;
    outerArray[5][2].availible = false;
    outerArray[6][2].availible = false;
    outerArray[7][2].availible = false;
    outerArray[8][2].availible = false;
    outerArray[9][2].availible = false;
    outerArray[9][3].availible = false;
    outerArray[9][4].availible = false;
    outerArray[9][5].availible = false;
    outerArray[9][6].availible = false;
    outerArray[10][6].availible = false;
    outerArray[11][6].availible = false;

    return outerArray;

}

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

//updates projectiles fired by towers
    for(let b of bulletList){
        //updates the position of bullet
        b.move();
        //for piercing shots that have already hit a target
        if(b.hasHit == true){
           //runs the timer for how long until the bullet can do damage again
            b.HitCoolDown(dt);
        }
        for(let e of enemyList){
            //checks if the bullet is colliding with any enemy
            if(CircleColision(b,e) && !b.hasHit){
                //piercing shots continue moving after hitting
                if(b.type != "piercing")
                {
                    
                    //deactivates the bullet
                    b.isAlive = false;
                    //removes the bullet from the screen
                    gameScene.removeChild(b);
                }
                else{
                    //if the bullet is piercing it will keep moving but won't be able to do damage until hasHit is reset by the HitCoolDown function
                    b.hasHit = true;
                }
               // updates the enemy's health
                e.health -= b.damage;
            }

            //piercing shots are removed when the are outside the bounds of the scene rather than when they hit
            if(b.type == "piercing"){
                if(b.x <= 0 - b.width || b.x >= sceneWidth + b.width 
                    || b.y <= 0 - b.height || b.y >= sceneHeight + b.height){
                    b.isAlive = false;
                
                    gameScene.removeChild(b);
                }
            }
        }

        //removes bullets that were fired at targets that already died
        if(b.target.isAlive == false){
            b.isAlive = false;
            gameScene.removeChild(b);
        }
    }

    //creates a new enemy after a variable amount of time
    SpawnEnemy(dt);

//updates enemies
    for(let e of enemyList){
        
        //removes dead enemies
        if(e.health <= 0){
            //plays an audio cue whenever an enemy dies
            enemyDeathSound.play();
            //removes the enemy from the screen
            gameScene.removeChild(e);

            e.isAlive = false;
            //every enemy killed gives money to buy additional defenses with
            if(e.type == 'standard'){
                ChangeMoneyBy(1);
            }
            else{//boss enemeies give additional money on death
                
                ChangeMoneyBy(5);
            }
            
            //updates the number of enemies killed to be shown at on the game overscreen
            enemiesDefeated++;
        }

        
        //moves the enemy
        e.move(dt);

        //once the enemy reaches the power source
        if(e.y > 550){
            //plays an audio cue to tell the player they're taking damage
            damageSound.play();
            //reduces life
            ChangeLifeBy(e.damage);
            //removes the enemy that reched the end
            e.isAlive = false;
            gameScene.removeChild(e);
            
            
        }
    }
    
    //games over condition
    if(life <= 0){
        //shows the game over screen
        gameScene.visible = false;
        gameOverScene.visible = true;
        life = 100;
        //clears the bullets, defenses, and enemies for the next play through
        bulletList.forEach(b=>gameScene.removeChild(b));
        defenseList.forEach(d=>gameScene.removeChild(d));
        enemyList.forEach(e=>gameScene.removeChild(e));

        bulletList = [];
        defenseList = [];
        enemyList = [];
        //updates the score label
        scoreLabel.text = `Enemies Destroyed: ${enemiesDefeated}`
        //stops the gameloop from running through
        paused = true;
        
    }

    //filters out dead enemies
    enemyList = enemyList.filter(e=>e.isAlive);
    //filters out bullets that have hit
    bulletList = bulletList.filter(b=>b.isAlive);
    }

}

function buySquare(){
    
    //checks if the user has enough money and isn't positioning another defense
    if(money >= 10 && !buying){
        
        //shows a square which follows the mouse cursor
        cursorSquare.visible = true;
       ChangeMoneyBy(-10);
        //updates type variable to be used in the CreateDefense function
        type = "square";
        buying = true;
    }
}


function buyTriangle(){
    //checks if the user has enough money and isn't positioning another defense
    if(money >= 20 && !buying){
        //shows a triangle which follows the mouse cursor
        cursorTriangle.visible = true;
        ChangeMoneyBy(-20);
        
        type = "triangle"
        buying = true;
    }
    

}

function buyCircle(){
    //checks if the user has enough money and isn't positioning another defense
    if(money >= 25 && !buying){
        //shows a circle which follows the mouse cursor
        cursorCircle.visible = true;
        ChangeMoneyBy(-25);
        type = "circle";
        buying = true;
    }
    
}

//updates for life and money labels and underlying variables
function ChangeMoneyBy(value){
    //only uses one function with a plus because for all values where I want to reduce the value I can plug in a negative number, and this makes the code a little drier
    money += value;
    moneyLabel.text = `Money: $${money}`;
    
}

function ChangeLifeBy(value){
    life += value;
    lifeLabel.text = `Life: ${life}`;
}


function SpawnEnemy(dt = 1/12){
    //reduces the spawn timer by the change in time
    spawnTimer -= dt;

    //once the spawnTimer reaches zero
    if(spawnTimer <= 0){
        let newEnemy;
        //if a random float is less than bossChance spawns a boss enemy
        if(Math.random() < bossChance){
            
            newEnemy = new enemy(300,0, "boss", 'images/bossEnemy.png');
        }
        else{
            newEnemy = new enemy(300,0, "standard", 'images/enemy.png');
        }
        //resets the spawn timer
        spawnTimer = waveTime;
        
        //adds the new enemy
        enemyList.push(newEnemy);
        gameScene.addChild(newEnemy);

    }


    //after defeating certain amounts of enemies the time between each spawn will get shorter and the chance of boss enemies spawning increases
    if(enemiesDefeated == 10){
        
        waveTime = 1.75;
        bossChance = 0.15;
    }
    else if(enemiesDefeated == 20){
        waveTime = 1.5;
        bossChance = 0.2;
    }
    else if(enemiesDefeated == 30){
        waveTime = 1.25
        bossChance = 0.25;
    }
    else if (enemiesDefeated == 40){
        waveTime == 1;
        bossChance = 0.3;
    }
    else if(enemiesDefeated == 50){
        waveTime = 0.5;
        bossChance = 0.35;
    }


    
}

//called when the user clicks on the map 
 function createDefense(){
    if(buying == true){
        
        //gets the box that the cursor is in
        let box = GetBoxWithCursor(grid);

        //if the box isn't already filled or on the enemy path
        if(box.availible){
            
            //checks which buy button the user pressed and creates a defense based on the type
            if(type == "triangle"){
                //creates a new defense positioned inside the selected box
                newDefense = new Triangle(box.x + box.width/2, box.y + box.height/2);
                
                //hides the image that was following the cursor
                cursorTriangle.visible = false;
            }
            else if(type == "square"){
               //hides the image that was following the cursor
                cursorSquare.visible = false;
                
                 //creates a new defense positioned inside the selected box
                newDefense = new Square(box.x + box.width/2, box.y + box.height/2);
              
            }
            else{
                //hides the image that was following the cursor
                cursorCircle.visible = false;
                
                //creates a new defense positioned inside the selected box
                newDefense = new Circle(box.x + box.width/2, box.y + box.height/2);
                
            }

            //sets the grid box to unavailible to prevent defenses from being stacked
            box.availible = false;
            //adds the new defense to the scene
            defenseList.push(newDefense)
            gameScene.addChild(newDefense)
            //sets buying to false so the user can't keep clicking to add more defenses without pressing one of the store buttons first
            buying = false;
        }
        
       
    }
}

function UpdateCursorOverlay(e){
    //has each sprite follow the position of the cursor
    let pos = e.data.global;
    cursorTriangle.x = pos.x;
    cursorTriangle.y = pos.y;
    
    cursorCircle.x = pos.x;
    cursorCircle.y = pos.y;

    cursorSquare.x = pos.x;
    cursorSquare.y = pos.y;
}

//called when user presses show instructions in startScene
function ShowInstructions(){
    startScene.visible = false;
    instructionsScreen.visible = true;
}

//each of these three functions is call when the user moves their cursor on/off of each buy button toggles the visibility of the respective description box
//also changes the alpha of the button to add a visual cue to when the button is clickable
function toggleTriangleDescription(){
    if(triangleDescriptionBox.visible == false){
        triangleDescriptionBox.visible = true;
        triangleButton.alpha = 0.7;
    }
    else{
        triangleDescriptionBox.visible = false;
        triangleButton.alpha = 1.0;
    }
    
}

function toggleSquareDescription(){
    if(squareDescriptionBox.visible == false){
        squareDescriptionBox.visible = true;
        squareButton.alpha = 0.7;
    }
    else{
        squareDescriptionBox.visible = false;
        squareButton.alpha = 1.0;
    }
    
}

function toggleCircleDescription(){
    if(circleDescriptionBox.visible == false){
        circleDescriptionBox.visible = true;
        circleButton.alpha = 0.7;
    }
    else{
        circleDescriptionBox.visible = false;
        circleButton.alpha = 1.0;
    } 
}

