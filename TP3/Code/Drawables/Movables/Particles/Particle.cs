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
    /// Particles can either be fragments or smoke.
    /// </summary>
    public class Particle : Movable
    {
        protected CharacterType Type;
        protected float maxSpeed;
        public bool IsDeadly { get; protected set; }
        protected int maxSize;
        protected int size;
        protected int rectangleDivider = 1;
        protected float percentageModifier = 1.0f;
        protected float percentageDuration = 1.0f;

        /// <summary>
        /// Time management variables
        /// </summary>
        protected int totalTime = 0;
        protected int duration = 0;
        protected int endSequence = 0;
        protected float endSequenceStart;

        /// <summary>
        /// 
        /// </summary>
        protected void setParticlePoints()
        {
            this[0] = new Vector2f(Math.Min(-size, -1), Math.Min(-size/ rectangleDivider, -1));
            this[1] = new Vector2f(Math.Min(-size, -1), Math.Max(size / rectangleDivider, 1));
            this[2] = new Vector2f(Math.Max(size, 1), Math.Max(size/ rectangleDivider, 1));
            this[3] = new Vector2f(Math.Max(size, 1), Math.Min(-size/ rectangleDivider, -1));
        }

        /// <summary>
        /// Class Constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="nbVertices"></param>
        /// <param name="color"></param>
        /// <param name="speed"></param>
        public Particle(CharacterType type, Single posX, Single posY, UInt32 nbVertices, Single speed, bool isDeadly, 
            float duration, int size, float endSequenceStart, int colorInt, int rectangleDivider)
            : base(posX, posY, nbVertices, speed, colorInt)
        {
            Type = type;
            maxSpeed = speed;
            Speed = maxSpeed;
            IsDeadly = isDeadly;
            this.duration = rnd.Next((int)(duration * 0.5f), (int)(duration * 1.2f));
            maxSize = size;
            this.endSequenceStart = endSequenceStart;
            this.rectangleDivider = rectangleDivider;
            setParticlePoints();
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="deltaT"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        public override bool Update(float deltaT, Game game)
        {
            percentageDuration = (float)totalTime / duration;
            if (percentageDuration < 1f)
            {
                base.Update(deltaT, game);
                Bounce();
                if (percentageDuration > endSequenceStart)
                {
                    if (rectangleDivider == 1)
                    {
                        percentageModifier = 1f - percentageDuration;
                        size = (int)(maxSize * percentageDuration);
                    }
                    else
                    {
                        percentageModifier = 1f - (float)(endSequence / (duration * (1f - endSequenceStart)));
                        size = (int)(maxSize * percentageModifier);
                    }
                    Speed = (maxSpeed * percentageModifier);
                    endSequence++;
                }
                setParticlePoints();
                RandomColor(colorInt, percentageModifier);
                if (!IsAlive)
                {
                    return false;
                }
                totalTime++;
                return true;
            }
            return false;
        }
    }
}
