using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Audio;
using SFML.System;
namespace TP3
{
    /// <summary>
    /// A character is an abstract class. A character must be either a Hero, or an enemy.
    /// </summary>
    public abstract class Character : Movable
    {
        private static Music firesound;
        protected DateTime lastfire;
        protected double fireDelay;
        protected TimeSpan smokeParticleLastCreated = TimeSpan.Zero;

        /// <summary>
        /// Static constructor so the fire sound is only loaded once (at the beginning of the program).
        /// </summary>
        static Character()
        {
            //firesound = new Music("Fire_normal.wav");
        }

        /// <summary>
        /// Character Constructor
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="nbVertices"></param>
        /// <param name="color"></param>
        /// <param name="speed"></param>
        protected Character(float posX, float posY, uint nbVertices, float speed, int colorInt)
            : base(posX, posY, nbVertices, speed, colorInt)
        {
        }
    }
}
