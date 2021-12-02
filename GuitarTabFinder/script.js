//scope level variables
let tabCheck;
let chordsCheck;
let resultCount;
let lastKeyword = "lgd1649-keyword";
let lastArtist = "1gd1649-artist";
let outputString = '';
let bothCheck

window.onload = function(){


     //if there is a keyword in local storage autofills the search boxes
    document.querySelector("#keyword").value = localStorage.getItem(lastKeyword);
    document.querySelector("#artist").value = localStorage.getItem(lastArtist);

    //defines funcion to call when the search button is clicked
    document.querySelector("#search").onclick = search;

    //defines functionality to change values of the check boxes for instument search
    tabCheck = document.querySelector("#tabs");
    chordsCheck = document.querySelector("#chords");
    bothCheck = document.querySelector("#both");

    //sets the both button to be active by default
    bothCheck.checked = true;
    tabCheck.value = "false";
    chordsCheck.value = "false";
    bothCheck.value = "true";

    resultCount = document.querySelector("#limit");
     
    //functions to control the changing of each radio button
     tabCheck.onclick = function(){
         //sets the clicked button's value to true and the others' to false
        tabCheck.value = "true";
        chordsCheck.value = "false";
        bothCheck.value = "false";
     }

     chordsCheck.onclick = function(){
        tabCheck.value = "false";
        chordsCheck.value = "true";
        bothCheck.value = "false";
    }
    bothCheck.onclick = function(){
        tabCheck.value = "false";
        chordsCheck.value = "false";
        bothCheck.value = "true";
    }

    //currently not working
   
}

function search(){
    
   

    //resets output to avoid new result being added on after previous results
    outputString = ''

    //checks what search terms the user has input
    //error catch for empty search terms
    if(document.querySelector("#keyword").value == '' && document.querySelector("#artist").value == ''){

        
        document.querySelector("#output").innerHTML = "error: no search terms";
    }
    //keyword search only
    else if(document.querySelector("#keyword").value != '' && document.querySelector("#artist").value == ''){
        //saves the contents of the keyword search box to local storage
        localStorage.setItem(lastKeyword, document.querySelector("#keyword").value);
        
        keywordSearch(document.querySelector("#keyword").value);
    }
    //artist search only
    else if(document.querySelector("#keyword").value == '' && document.querySelector("#artist").value != ''){
        //saves the contents of the artist search box to local storage
        localStorage.setItem(lastArtist, document.querySelector("#artist").value);
        artistSearch(document.querySelector("#artist").value)
    }
    //keyword and artist search
    else{
        //saves the contents of both search boxes to local storage
        localStorage.setItem(lastKeyword, document.querySelector("#keyword").value);
        localStorage.setItem(lastArtist, document.querySelector("#artist").value);

        //uses the artist followed by the keyword as one long keyword
        keywordSearch(document.querySelector("#artist").value + document.querySelector("#keyword").value)
    }
}

function keywordSearch(a){
    //base url used for all searchses
    let url = "https://www.songsterr.com/a/ra/songs.JSON?pattern="

    //add the keyword to the url
    url +=  a
    
    GetData(url)

}

function artistSearch(a){
    //base url used for all searchses
    let url = "https://www.songsterr.com/a/ra/songs/byartists.JSON?artists="

    //add the artist to the url
    //adds quotes and then URI encodes the search terms as otherwise seaching an artist with a space in their name would result in an error
    a = '"' + a + '"'

    url += encodeURIComponent(a);
    

    
    GetData(url);
}

function GetData(url){
    let xhr = new XMLHttpRequest;
    //loads and formats the results
    xhr.onload = dataLoaded;

    //prints out an error message if the api request fails
    xhr.onerror = dataError;
    
    //gets the data
    xhr.open("GET", url);
    xhr.send();
}

function dataError(){
    document.querySelector("#output").innerHTML = "error: data not found";
}

function dataLoaded(e){
    let xhr = e.target;
    
    //parses the API request to an arry
    let obj = JSON.parse(xhr.responseText);
    
    //checks if the API actually returned some results
    if(!obj||obj.length == 0){
        document.querySelector("#output").innerHTML = "<b> no results found </b>";
        //if not returns the method
        return;
    }

    
    //sorts the results based on whether they're tabs/chord charts
    let results = GetTypes(obj);


    
   
    //if the number of desired results is less than the total number of results
    if(resultCount.value < results.length){
        //preforms the loop the desired number of result times
        for(let i = 1; i < resultCount.value; i++){

            let singleResult = results[i];
    
            //creates a url for the current result
            let singleUrl = "http://www.songsterr.com/a/wa/song?id=" + singleResult.id;

            let singleTitle = singleResult.title;
            //debugger;
            let artist = singleResult.artist.name;

            //results have different formating if the user selects both in the radio buttons
            if(bothCheck.value == "true"){

                let TabOrChord = singleResult.TabOrChord;
                //adds to the output string the title and artist of the current result and makes them a clickable link
            outputString += `<div class='singleResult'>${TabOrChord}<a class = "link" href =${singleUrl} target="_blank" rel="noopener noreferrer"> ${singleTitle} by ${artist}</a></div>`
            }
            else{
                outputString += `<div class='singleResult'><a class = "link" href =${singleUrl} target="_blank" rel="noopener noreferrer"> ${singleTitle} by ${artist}</a></div>`
            }
            


            
        }
    }
    else{
        //if there are less results than the max number preforms the same process for every result
        for(let i = 0; i < results.length; i++){
            let singleResult = results[i];
    
            


            let singleUrl = "http://www.songsterr.com/a/wa/song?id=" + singleResult.id;
            let singleTitle = singleResult.title;
            let artist = singleResult.artist.name

            if(bothCheck.value == "true"){
                let TabOrChord = singleResult.TabOrChord;
                //adds to the output string the title and artist of the current result and makes them a clickable link
            outputString += `<div class='singleResult'>${TabOrChord}<a class = "link" href =${singleUrl} target="_blank" rel="noopener noreferrer"> ${singleTitle} by ${artist}</a></div>`
            }
            else{
                outputString += `<div class='singleResult'><a href =${singleUrl} target="_blank" rel="noopener noreferrer"> ${singleTitle} by ${artist}</a></div>`
            }
        }
    }
    
    //sets the out put section equal to output string
    document.querySelector("#output").innerHTML = outputString;

    //moves the footer to below the search output if there are enough results that they would start clipping the text otherwise
    if(resultCount.value == 25 || resultCount.value == 20){
   document.querySelector("footer").style.position = "inherit";

    }
    else{
        document.querySelector("footer").style.position = "fixed";
    }
    
}

function GetTypes(obj){
    //shell object
    let arraryToReturn = [];

    //checks which radio button the user has selected
    if(bothCheck.value == "true")
    {
       
        for(let i = 0; i<obj.length; i++){
            //checks if each object is a standard tab or a chord chart
            if(obj[i].chordsPresent == true){
                obj[i].TabOrChord = `<div class = 'type'>Chords: </div>`;
            }
            else{
                obj[i].TabOrChord = `<div class = 'type'>Tab: </div>`;
            }

            arraryToReturn.push(obj[i]);
        }
    }
    else if (tabCheck.value == "true"){

        
        if(tabCheck.value == "true"){
            //adds a header to the begin of the output
           outputString += `<h3>TABS:</h3>`;
            for(let i = 0; i<obj.length; i++){

                //adds all of the tabs to the returned array
                if(obj[i].tabTypes.includes("PLAYER")){
                    
                    arraryToReturn.push(obj[i]);
                    
                }
            }
        }
    
    
        
        
        
    }
    else
    {
        
        outputString += `<h3>CHORD CHARTS:</h3>`
        for(let i = 0; i < obj.length; i++){
            if(obj[i].tabTypes.includes("CHORDS")){

                //checks if the result is a chord chart
                if(obj[i].chordsPresent == true){

                    
                    
                    arraryToReturn.push(obj[i]);
                }
            }
        }
        
    }

    //returns the sorted result array
    return arraryToReturn;

}

