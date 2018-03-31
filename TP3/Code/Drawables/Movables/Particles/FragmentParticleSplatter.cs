using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace TP3.Code.Particles
{
    /// <summary>
    /// A fragment particle splatter is called either when a projectile hits a wall (reflection) or when it hits another moving object (follow through).
    /// </summary>
    static class FragmentParticleSplatter
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
        /// <param name="angleOfImpactingObject"></param>
        /// <param name="game"></param>
        /// <param name="colorInt"></param>
        public static void CreateSplatterReflection(int posX, int posY, CharacterType characterType, bool isDeadly, 
            float speedOfImpactingObject, float angleOfImpactingObject, Game game, int colorInt)
        {
            int nbOfParticles = rnd.Next(5, 14);
            for (int i = 0; i < nbOfParticles; i++)
            {
                float speed = speedOfImpactingObject * (rnd.Next(60, 140) / 100f);
                float time = 30 * (rnd.Next(70, 130) / 100f);
                if (posX < 5)
                {
                    posX = 5;
                    angleOfImpactingObject = -angleOfImpactingObject + 180;
                }
                if (posX > Game.GAME_WIDTH - 5)
                {
                    posX = Game.GAME_WIDTH - 5;
                    angleOfImpactingObject = -angleOfImpactingObject + 180;
                }
                if (posY < 5)
                {
                    posY = 5;
                    angleOfImpactingObject = -angleOfImpactingObject;
                }
                if (posY > Game.GAME_HEIGHT - 5)
                {
                    posY = Game.GAME_HEIGHT - 5;
                    angleOfImpactingObject = -angleOfImpactingObject;
                }
                float angleModifier = rnd.Next(-4, 5);
                int size = rnd.Next(2, 10);
                if (angleModifier > -1 && angleModifier < 1)
                {
                    speed += rnd.Next(2, 5);
                    size += rnd.Next(3, 7);
                }
                Fragment fragment = new Fragment(characterType, posX, posY, 4, speed, isDeadly, time, size, 0.2f, colorInt, 5);
                fragment.Angle = angleOfImpactingObject + angleModifier;
                game.fragmentParticles.Add(fragment);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="characterType"></param>
        /// <param name="isDeadly"></param>
        /// <param name="speedOfImpactingObject"></param>
        /// <param name="angleOfImpactingObject"></param>
        /// <param name="game"></param>
        /// <param name="colorInt"></param>
        public static void CreateSplatterFollowThrough(int posX, int posY, CharacterType characterType, bool isDeadly, float speedOfImpactingObject, float angleOfImpactingObject, Game game, int colorInt)
        {
            int nbOfParticles = rnd.Next(15, 25);
            for (int i = 0; i < nbOfParticles; i++)
            {
                float speed = speedOfImpactingObject * (rnd.Next(60, 140) / 100f);
                float time = 40 * (rnd.Next(70, 130) / 100f);
                float angleModifier = rnd.Next(-10, 11);
                int size = rnd.Next(2, 10);
                if (angleModifier > -2 && angleModifier < 2)
                {
                    speed += rnd.Next(2, 5);
                    size += rnd.Next(3, 7);
                }
                Fragment fragment = new Fragment(characterType, posX, posY, 4, speed, isDeadly, time, size, 0.2f, colorInt, 5)
                {
                    Angle = angleOfImpactingObject + angleModifier
                };
                game.fragmentParticles.Add(fragment);
            }
        }
    }
}
