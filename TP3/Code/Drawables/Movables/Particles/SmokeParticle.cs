using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
namespace TP3
{
    /// <summary>
    /// Smoke particles are geometric movable objects that drift behind character ships as they move around the screen.
    /// </summary>
    public class SmokeParticle : Particle
    {
        /// <summary>
        /// Class Constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="nbVertices"></param>
        /// <param name="color"></param>
        /// <param name="speed"></param>
        public SmokeParticle(CharacterType type, Single posX, Single posY, UInt32 nbVertices, Single speed, bool isDeadly, 
            int duration, int size, float endSequenceStart, int colorInt, int rectangleDivider)
            : base(type, posX, posY, nbVertices, speed, isDeadly, duration, size, endSequenceStart, colorInt, rectangleDivider)
        {
            maxSize = rnd.Next((int)(size*0.5f), (int)(size*1.5f));
            this.size = 0;
            Speed = (speed * rnd.Next(5, 11) / 10);
            this.duration = (rnd.Next((int)(duration * 0.5f), (int)(duration * 1.2f)));
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="deltaT"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        public override bool Update(float deltaT, Game game)
        {
            Angle += rnd.Next(-5, 6);
            IsAlive = base.Update(deltaT, game);
            return IsAlive;
        }
    }
}

