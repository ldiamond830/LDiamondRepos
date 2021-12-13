//uses distance formula to get squared distance between 2 points
function GetDistanceSquared(position1, position2){
    
    
    return Math.pow((position2.x - position1.x), 2) + Math.pow((position2.y -position1.y), 2);

}

function GetBoxWithCursor(grid){
    //gets the mouse's positon
    let mousePosition = app.renderer.plugins.interaction.mouse.global;

    for(let i = 0; i < 12; i++){
        for(let j = 0; j < 12; j++){
            
            //loops through each grid box to see if the mouse is within it's bounds
            if(mousePosition.x > grid[i][j].x && mousePosition.x < grid[i][j].x + grid[i][j].width && 
                mousePosition.y > grid[i][j].y && mousePosition.y < grid[i][j].y + grid[i][j].height){

                
                    //returns the box that contains the mouse
                return grid[i][j];
            }
        }
        
    }
    //catch for if mouse is outside grid area
    return null;
}

//finds the maginitude of the plugged in vector
function GetVectorMagnitude(vector){

    return Math.sqrt(vector.x * vector.x + vector.y * vector.y);

}

//normalizes the vector to have a magnitude of 1
function NormalizeVector(vector){
    let magnitude = GetVectorMagnitude(vector);
    return {x:vector.x/magnitude, y:vector.y/magnitude}
}

function CircleColision(object1, object2){
    
    let radiusSqaured = (object1.radius + object2.radius) * (object1.radius + object2.radius);

    //if the squared sum of each object's radius is less than the distance between the two objects
    if(radiusSqaured > GetDistanceSquared(object1.position, object2.position)){
        //returns that the objects are colliding
        return true;
    }
    else{
        return false;
    }

}