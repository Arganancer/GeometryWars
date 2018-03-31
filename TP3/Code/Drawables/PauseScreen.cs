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
    /// Pause screen is a transparent overlay over the rest of the game objects when the game is paused.
    /// </summary>
    public class PauseScreen : Drawable
    {
        private int wallWidth = 5;
        public PauseScreen(float posX, float posY, uint nbVertices) 
            : base(posX, posY, nbVertices)
        {
            if (posX == 0 && posY == 0)
            {
                Color = new Color(100, 100, 100, 60);
                this[0] = new Vector2f(-300, -300);
                this[1] = new Vector2f(Game.GAME_WIDTH + 300, -300);
                this[2] = new Vector2f(Game.GAME_WIDTH + 300, Game.GAME_HEIGHT + 300);
                this[3] = new Vector2f(-300, Game.GAME_HEIGHT + 300);
            }
        }
    }
}
