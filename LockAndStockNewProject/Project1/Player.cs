
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;



namespace LockAndStock
{
    class Player
    {
        private int health;
        private int speed;
        private Texture2D texture;
        private Texture2D projectileTexture;
        private int score;
        private List<Bullet> bulletList = new List<Bullet>();
        private Vector2 shotDirection;
        private Rectangle position;
        bool hasShot = false;
        private bool isInvincible;
        private SoundEffect shotSound;
        private Color color = Color.White;



        public Rectangle Position
        {
            get { return position; }
        }

        public bool IsInvincible
        {
            get { return isInvincible; }
            set { isInvincible = value; }
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }
        public List<Bullet> BulletList
        {
            get { return bulletList; }
        }
        public int Score
        {
            get { return score; }
            set { score = value; }
        }
        public bool HasShot
        {
            get { return hasShot; }
            set { hasShot = value; }
        }

        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public Player(int health, int speed, Texture2D texture, Texture2D projectileTexture, Rectangle position, SoundEffect shotSound)
        {
            this.health = health;
            this.speed = speed;
            this.texture = texture;
            this.projectileTexture = projectileTexture;
            this.position = position;
            this.shotSound = shotSound;
        }

        public void Update(double gametime, MouseState mouse, KeyboardState kbState)
        {
            shotDirection = new Vector2((mouse.X - position.X), (mouse.Y - position.Y));
            shotDirection.Normalize();

            if (isInvincible == true)
            {
                color = Color.Purple;
            }
            else
            {
                color = Color.White;
            }

            if (gametime >= 0.5 && mouse.LeftButton == ButtonState.Pressed)
            {

                bulletList.Add(new Bullet(shotDirection, projectileTexture, new Rectangle(position.X, position.Y, 50, 50)));
                shotSound.Play(0.1f, 0, 0);

                hasShot = true;
                Recoil(shotDirection);
            }

            if (kbState.IsKeyDown(Keys.W))
            {
                position.Y -= speed;

            }
            if (kbState.IsKeyDown(Keys.S))
            {
                position.Y += speed;

            }
            if (kbState.IsKeyDown(Keys.A))
            {
                position.X -= speed;

            }
            if (kbState.IsKeyDown(Keys.D))
            {
                position.X += speed;

            }
        }

        public void Reset()
        {
            score = 0;
            health = 5;
            position.X = 1920 / 2;
            position.Y = 1080 / 2;
            bulletList.Clear();
            isInvincible = false;
        }

        private void Recoil(Vector2 Direction)
        {
            position.X -= (int)(50 * Direction.X);
            position.Y -= (int)(50 * Direction.Y);
        }
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, color);
        }
    }
}
