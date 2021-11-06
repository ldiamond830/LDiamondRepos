﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace EndGame
{
    //parent class for all the different bosses
    abstract class Boss
    {
        //fields
        protected int health;
        protected int damage;
        protected int moveSpeed;
        protected int projectileSpeed;
        protected Rectangle position;
        protected const int windowWidth = 1920;
        protected const int windowHeight = 1080;
        protected Texture2D projectileTexture;
        protected Texture2D texture;
        protected Player player;
        protected List<BossBullet> bulletList;
        protected bool attackSwitch = false;
        
        //properties

        //remember to check if I ever actually used this
        public bool AttackSwitch
        {
            get { return attackSwitch; }
            set { attackSwitch = value; }
        }


        public List<BossBullet> BulletList
        {
            get { return bulletList; }
        }
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public Rectangle Position
        {
            get { return position; }
        }

        //constructor
        public Boss (int health, int damage, int moveSpeed, int projectileSpeed, Rectangle position, Texture2D projectileTexture, Texture2D texture, Player player)
        {
            this.health = health;
            this.damage = damage;
            this.moveSpeed = moveSpeed;
            this.projectileSpeed = projectileSpeed;
            this.position = position;
            this.projectileTexture = projectileTexture;
            this.texture = texture;
            this.player = player;
            bulletList = new List<BossBullet>();
        }

        //update method to be overwritten
        public virtual void Update()
        {

        }
       
       
        //stock draw method
        public virtual void Draw(SpriteBatch sb, Color color)
        {
            sb.Draw(texture, position, color);
        }
    }
}
