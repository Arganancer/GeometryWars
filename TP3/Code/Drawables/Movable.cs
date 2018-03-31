using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;

namespace TP3
{
    /// <summary>
    /// Movable is any object in the game that can move after it has been initialized.
    /// </summary>
    public class Movable : Drawable
    {
        public override float Angle
        {
            get
            {
                return base.Angle;
            }
            set
            {
                base.Angle = value;
                Direction = new Vector2f((float)Math.Cos(Math.PI * Angle / 180.0f), (float)Math.Sin(Math.PI * Angle / 180.0f));
            }
        }

        public float Size { get { return Math.Max(BoundingBox.Height, BoundingBox.Width); } }
        public Vector2f Direction { get; set; }
        public virtual bool IsAlive { get; set; }
        public float Speed { get; set; }
        public CharacterType Type { get; protected set; }

        /// <summary>
        /// Movable Constructor
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="nbVertices"></param>
        /// <param name="color"></param>
        /// <param name="speed"></param>
        public Movable(float posX, float posY, uint nbVertices, float speed, int colorInt)
          : base(posX, posY, nbVertices)
        {
            RandomColor(colorInt, 1);
            this.colorInt = colorInt;
            Angle = 0;
            IsAlive = true;
            Speed = speed;
        }

        /// <summary>
        /// Rotate
        /// </summary>
        /// <param name="angleInDegrees"></param>
        protected virtual void Rotate(float angleInDegrees)
        {
            Angle += angleInDegrees;
        }

        /// <summary>
        /// Advance
        /// </summary>
        /// <param name="nbPixels"></param>
        protected virtual void Advance(float nbPixels)
        {
            Position = Position + Direction * nbPixels;
        }

        /// <summary>
        /// TO BE COMPLETED
        /// </summary>
        /// <param name="deltaT"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        public virtual bool Update(Single deltaT, Game game)
        {
            Advance(Speed);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Bounce()
        {
            if (Position.X < 4)
            {
                Position = new Vector2f(4, Position.Y);
                Angle = -Angle + 180 + rnd.Next((int)Speed, (int)Speed);
            }
            if (Position.X > Game.GAME_WIDTH - 4)
            {
                Position = new Vector2f(Game.GAME_WIDTH - 4, Position.Y);
                Angle = -Angle - 180 + rnd.Next((int)Speed, (int)Speed);
            }
            if (Position.Y < 4)
            {
                Position = new Vector2f(Position.X, 4);
                Angle = -Angle + rnd.Next((int)Speed, (int)Speed);
            }
            if (Position.Y > Game.GAME_HEIGHT - 4)
            {
                Position = new Vector2f(Position.X, Game.GAME_HEIGHT - 4);
                Angle = -Angle + rnd.Next((int)Speed, (int)Speed);
            }
        }
    }
}
