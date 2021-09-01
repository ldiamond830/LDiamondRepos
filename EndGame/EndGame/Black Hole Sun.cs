﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace EndGame
{
    class Black_Hole_Sun : Boss
    {

        public Black_Hole_Sun(Texture2D projectileTexture, Texture2D texture, Player player) : base(500, 10, 5, 5, new Rectangle(960, 540, 100, 100), projectileTexture, texture, player)
        {

        }
    }
}
