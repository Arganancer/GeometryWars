using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using TP3.Code;

namespace TP3
{
    /// <summary>
    /// Stars are background objects that move according to the Hero's movement.
    /// </summary>
    public class Star : Movable
    {
        private int width;
        private int height;
        private int outOfRangeDistance = 200;

        private int colorType;

        private static Random rnd = new Random();

        private float starFactor;

        /// <summary>
        /// Constructeur de la classe Star.
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="speed"></param>
        public Star(float posX, float posY, uint nbVertices, float speed, int colorInt) 
            : base(posX, posY, nbVertices, speed, colorInt)
        {
            starFactor = rnd.Next(1, 3);
            if (starFactor == 2 && rnd.Next(0, 30) == 1)
            {
                starFactor++;
            }
            width = (int)starFactor;
            height = (int)starFactor;
            Speed = starFactor * (rnd.Next(80, 120)/100f);
            if (rnd.Next(0, 10) == 1)
            {
                Speed += +rnd.Next(1, 2);
            }
            this[0] = new Vector2f(0, 0);
            this[1] = new Vector2f(0, width);
            this[2] = new Vector2f(width, height);
            this[3] = new Vector2f(width, 0);
            Position = new Vector2f(rnd.Next(-outOfRangeDistance, Game.GAME_WIDTH + outOfRangeDistance), rnd.Next(-outOfRangeDistance, Game.GAME_HEIGHT + outOfRangeDistance));
        }

        /// <summary>
        /// 
        /// </summary>
        public void Respawn()
        {
            if (Position.X < 0 - outOfRangeDistance)
            {
                Position = new Vector2f(Game.GAME_WIDTH + outOfRangeDistance, Position.Y);
            }
            else if (Position.X > Game.GAME_WIDTH + outOfRangeDistance)
            {
                Position = new Vector2f(0 - outOfRangeDistance, Position.Y);
            }
            else if (Position.Y < 0 - outOfRangeDistance)
            {
                Position = new Vector2f(Position.X, Game.GAME_HEIGHT + outOfRangeDistance);
            }
            else if (Position.Y > Game.GAME_HEIGHT + outOfRangeDistance)
            {
                Position = new Vector2f(Position.X, 0 - outOfRangeDistance);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaT"></param>
        /// <param name="direction"></param>
        public override bool Update(Single deltaT, Game game)
        {
            float percentageModifier = starFactor / 3f;
            RandomColor(6, percentageModifier);
            Angle = game.hero.Angle - 180;
            float speed = game.hero.Speed;
            if (game.hero.isMoving)
            {
                Advance(Speed * speed / 5);
            }
            Respawn();
            return true;
        }
    }
}
