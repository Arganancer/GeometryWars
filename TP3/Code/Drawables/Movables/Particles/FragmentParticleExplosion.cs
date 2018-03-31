using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace TP3.Code.Particles
{
    /// <summary>
    /// A fragment particle explosion is called when a character dies, and when the Hero uses a bomb.
    /// </summary>
    static class FragmentParticleExplosion
    {
        private static Random rnd = new Random();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="characterType"></param>
        /// <param name="isDeadly"></param>
        /// <param name="speedOfImpactingObject"></param>
        /// <param name="game"></param>
        /// <param name="duration"></param>
        /// <param name="endSequenceStart"></param>
        /// <param name="quantity"></param>
        /// <param name="colorInt"></param>
        public static void CreateExplosion(int posX, int posY, CharacterType characterType, bool isDeadly, float speedOfImpactingObject, Game game, float duration, float endSequenceStart, int quantity, int colorInt)
        {
            int nbOfParticles = rnd.Next(quantity + 100, quantity + 200);
            for (int i = 0; i < nbOfParticles; i++)
            {
                float speed = speedOfImpactingObject * (rnd.Next(85, 115) / 100f);
                float time = duration * (rnd.Next(85, 115) / 100f);
                float angleModifier = rnd.Next(-4, 5);
                int size = rnd.Next(2, 16);
                if (angleModifier > -1 && angleModifier < 1)
                {
                    speed += rnd.Next(1, 3);
                    size += rnd.Next(3, 4);
                }
                Fragment fragment = new Fragment(characterType, posX, posY, 4, speed, isDeadly, time, size, endSequenceStart, colorInt, 5)
                {
                    Angle = +rnd.Next(-180, 180) + angleModifier
                };
                game.fragmentParticles.Add(fragment);

            }
        }
    }
}
