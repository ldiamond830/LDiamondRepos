using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//Name: G-Force
//Date: 3/16/21
//Updated: 3/28/21
//Professor Mesh
//Purpose: To provide an amazing visual novel experience.

namespace Palingenesis
{
    class Dialogue
    {
        private SpriteFont font;
        private bool isPlayer;
        private string convo;
        private string playerName = "Theophania";
        private string bossName;
        private int dialogueNum;

        private Texture2D player;
        private Texture2D boss;
        private Texture2D background;
        private Texture2D textBoxColor;
        private Texture2D textBoxName;

        //rectangles for text boxes
        private Rectangle backgroundBox;
        private Rectangle textBox;
        private Rectangle playerBox;
        private Rectangle bossBox;
        private Rectangle PlayerName;
        private Rectangle BossName;
        private Color BossColor;
       


        /// <summary>
        /// constructor that gets all of the texture info and actual dialogue
        /// </summary>
        /// <param name="player"></param>
        /// <param name="boss"></param>
        /// <param name="background"></param>
        /// <param name="textBoxColor"></param>
        /// <param name="textBoxName"></param>
        /// <param name="font"></param>
        /// <param name="convo"></param>
        /// <param name="isPlayer"></param>
        public Dialogue(Texture2D player, Texture2D boss, Texture2D background, Texture2D textBoxColor, Texture2D textBoxName, SpriteFont font, Color BossColor, string bossName, string convo, bool isPlayer)
        { 
            this.player = player;
            this.boss = boss;
            this.background = background;
            this.font = font;
            this.isPlayer = isPlayer;
            this.bossName = bossName;
            this.convo = convo;
            this.BossColor = BossColor;
            this.textBoxColor = textBoxColor;
            this.textBoxName = textBoxName;

            backgroundBox = new Rectangle(0, 0, 1920, 1080);
            textBox = new Rectangle(0, 800, 1920, 280);
            playerBox = new Rectangle(0, 100, 600, 1080);
            bossBox = new Rectangle(1400, 100, 550, 1080);
            PlayerName = new Rectangle(50, 700, 300, 150);
           
            if (bossName == "Rice Goddess")
            {
                BossName = new Rectangle(1490, 700, 350, 150);
            }
            else if(bossName == "Naga")
            {
                BossName = new Rectangle(1450, 700, 275, 150);
            }
            else
            {
                BossName = new Rectangle(1500, 700, 300, 150);
            }
        }
      

        /// <summary>
        /// Specific draw method that draws all of the components for each frame
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            //draws boss blacked out and displays player name box
            if (isPlayer == true)
            {
                sb.Draw(background, backgroundBox, Color.White);
                sb.Draw(boss, bossBox, Color.Black);
                sb.Draw(player, playerBox, Color.White);
                sb.Draw(textBoxColor, textBox, Color.White);
                sb.Draw(textBoxName, PlayerName, Color.Red);
                sb.DrawString(font, playerName, new Vector2(65, 750), Color.White);
                sb.DrawString(font, convo, new Vector2(60, 900), Color.White);
            }
            
            //draws player blacked out and displays boss name box
            else
            {
                sb.Draw(background, backgroundBox, Color.White);
                sb.Draw(player, playerBox, Color.Black);
                sb.Draw(boss, bossBox, Color.White);
                sb.Draw(textBoxColor, textBox, Color.White);
                sb.Draw(textBoxName, BossName, BossColor);
                sb.DrawString(font, bossName, new Vector2(1515, 750), Color.White);
                sb.DrawString(font, convo, new Vector2(60, 900), Color.White);
            }
        }
    }
}
