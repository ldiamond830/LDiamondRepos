using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypesAndVariablesLucasDiaomd
{
    class Program
    { 
       
        
       
        static void Main(string[] args)
        {
            //declaring variables
            String charaName = "The Legend El Scorcho";

            const int startPoints = 50;
            int strength = startPoints / 5; //20% = 1/5 so dividing the total numb

            int dexerity = strength / 2; 

            int intelligence = 7;

            int health = (dexerity + intelligence) - 2;

            int charisma = startPoints - strength - dexerity - intelligence - health;

            //writing variable values to the console
            Console.WriteLine("Name: " + charaName);

            Console.WriteLine("Strength: " + strength);

            Console.WriteLine("Dexterity: " + dexerity);

            Console.WriteLine("Intelligence: " + intelligence);

            Console.WriteLine("health: " + health);

            Console.WriteLine("Charisma: " + charisma);

            Console.WriteLine("Total: " + startPoints);
                
        }
    }
}
