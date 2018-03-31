using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using TP3.Code.Particles;

namespace TP3
{
    /// <summary>
    /// Projectiles are "bullets" that can be shot by either enemies, or by the Hero.
    /// </summary>
    public class Projectile : Movable
    {
        private int ProjectileSpeed;

        /// <summary>
        /// Class Constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="nbVertices"></param>
        /// <param name="color"></param>
        /// <param name="speed"></param>
        public Projectile(CharacterType type, Single posX, Single posY, UInt32 nbVertices, Single speed, int size, int colorInt)
            : base(posX, posY, nbVertices, speed, colorInt)
        {
            this[0] = new Vector2f(size*1.5f, -size/2);
            this[1] = new Vector2f(size*1.5f, size/2);
            this[2] = new Vector2f(size, size);
            this[3] = new Vector2f(size/2, size);
            this[4] = new Vector2f(-size*2, size/4);
            this[5] = new Vector2f(-size*2, -size/4);
            this[6] = new Vector2f(size/2, -size);
            this[7] = new Vector2f(size, -size);

            Type = type;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="deltaT"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        public override bool Update(float deltaT, Game game)
        {
            base.Update(deltaT, game);
            foreach (var wall in game.walls)
            {
                if (wall.Intersects(this))
                {
                    FragmentParticleSplatter.CreateSplatterReflection((int)Position.X, (int)Position.Y, Type, false, Speed, Angle, game, colorInt);
                    return false;
                }
            }
            if (!IsAlive)
            {
                FragmentParticleSplatter.CreateSplatterFollowThrough((int)Position.X, (int)Position.Y, Type, false, Speed, Angle, game, colorInt);
                return false;
            }
            return true;
        }
    }
}
