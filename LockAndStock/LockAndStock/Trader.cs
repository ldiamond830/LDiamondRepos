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
    class Trader : enemy
    {
        public Trader(Texture2D texture, SoundEffect voiceLine, Rectangle position) : base(true, 3, texture, voiceLine, false, position)
        {

        }
    }
}
