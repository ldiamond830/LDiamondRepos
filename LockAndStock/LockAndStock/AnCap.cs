using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace LockAndStock
{
    class AnCap : enemy
    {


        public AnCap(Texture2D texture, SoundEffect voiceLine, Rectangle position) : base(true, 5, texture, voiceLine, false, position)
        {

        }
    }
}
