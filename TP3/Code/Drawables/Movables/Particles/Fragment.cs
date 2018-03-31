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
    /// A fragment is a particle that is called whenever anything explodes.
    /// </summary>
    public class Fragment : Particle
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
        public Fragment(CharacterType type, Single posX, Single posY, UInt32 nbVertices, Single speed, bool isDeadly, 
            float duration, int size, float endSequenceStart, int colorInt, int rectangleDivider)
            : base(type, posX, posY, nbVertices, speed, isDeadly, duration, size, endSequenceStart, colorInt, rectangleDivider)
        {
            this.size = maxSize;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaT"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        public override bool Update(float deltaT, Game game)
        {
            IsAlive = base.Update(deltaT, game);
            return IsAlive;
        }
    }
}
