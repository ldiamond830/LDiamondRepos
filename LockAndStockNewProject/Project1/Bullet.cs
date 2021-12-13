using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LockAndStock
{
    class Bullet
    {
        private bool isActive = true;
        private Vector2 direction;
        private Texture2D texture;
        private Rectangle position;


        public Rectangle Position
        {
            get { return position; }
        }
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public Bullet(Vector2 direction, Texture2D texture, Rectangle position)
        {
            this.direction = direction;
            this.texture = texture;
            this.position = position;
        }

        public void Update()
        {
            position.X += (int)(15 * direction.X);
            position.Y += (int)(15 * direction.Y);
        }
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, Color.White);
        }
    }
}
