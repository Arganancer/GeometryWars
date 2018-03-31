using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace TP3.Code
{
    /// <summary>
    /// Walls are visible bounds around the exterior of the playable area.
    /// </summary>
    public class Wall : Drawable
    {
        private int wallWidth = 5;
        public Wall(float posX, float posY, uint nbVertices, int colorInt) 
            : base(posX, posY, nbVertices)
        {
            RandomColor(colorInt, 1f);
            // Top wall
            if (posX == 0 && posY == 0)
            {
                this[0] = new Vector2f(0, 0);
                this[1] = new Vector2f(0, wallWidth);
                this[3] = new Vector2f(Game.GAME_WIDTH, 0);
                this[2] = new Vector2f(Game.GAME_WIDTH, wallWidth);
            }
            // Right wall
            else if (posX == Game.GAME_WIDTH && posY == 0)
            {
                this[0] = new Vector2f(0, 0);
                this[1] = new Vector2f(- wallWidth, 0);
                this[3] = new Vector2f(0, Game.GAME_HEIGHT);
                this[2] = new Vector2f(- wallWidth, Game.GAME_HEIGHT);
            }
            // Bottom wall
            else if (posX == Game.GAME_WIDTH && posY == Game.GAME_HEIGHT)
            {
                this[0] = new Vector2f(0, 0);
                this[1] = new Vector2f(0, 0 - wallWidth);
                this[3] = new Vector2f(- Game.GAME_WIDTH, 0);
                this[2] = new Vector2f(- Game.GAME_WIDTH, 0 - wallWidth);
            }
            // Left wall
            else if (posX == 0 && posY == Game.GAME_HEIGHT)
            {
                this[0] = new Vector2f(0, 0);
                this[1] = new Vector2f(0 + wallWidth, 0);
                this[3] = new Vector2f(0, - Game.GAME_HEIGHT);
                this[2] = new Vector2f(0 + wallWidth, - Game.GAME_HEIGHT);
            }
        }
    }
}
