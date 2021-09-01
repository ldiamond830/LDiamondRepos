using System;

//Name: G-Force
//Date: 3/16/21
//Professor Mesh
//Purpose: Make our game run.

namespace Palingenesis
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }
}
