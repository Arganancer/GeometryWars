using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using TP3.Code.Character;
using TP3.Code.Particles;

namespace TP3
{
    /// <summary>
    /// An enemy can be one of three types of antagonistic character.
    /// </summary>
    public abstract class Enemy : Character
    {
        protected bool IsSpawning;
        protected int nbUpdates;
        protected Color spawningColor = new Color(255, 255, 0, 0);
        protected int spawningSize = 0;
        public bool spawnsEnemiesWhenDead = false;

        protected int spawningDelay = 70;
        protected int timeSinceSpawnStart = 0;

        /// <summary>
        /// Enemy Constructor
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="nbVertices"></param>
        /// <param name="color"></param>
        /// <param name="speed"></param>
        protected Enemy(Single posX, Single posY, UInt32 nbVertices, Single speed, int colorInt) 
            : base(posX, posY, nbVertices, speed, colorInt)
        {
            Type = CharacterType.Enemy;
            IsSpawning = true;
        }

        /// <summary>
        /// TO BE COMPLETED
        /// </summary>
        /// <param name="deltaT"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        public override bool Update(Single deltaT, Game game)
        {
            if (!IsAlive)
            {
                Explode(game);
                if (spawnsEnemiesWhenDead)
                {
                    game.enemiesToAdd.Add(new Type3(Position.X, Position.Y, 4, rnd.Next(2, 4), 2));
                    //game.enemiesToAdd.Add(new Type3(Position.X, Position.Y, 4, rnd.Next(2, 4), 2));
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public void Explode(Game game)
        {
            FragmentParticleExplosion.CreateExplosion((int)Position.X, (int)Position.Y, Type, false, 6, game, 32, 0.2f, 50, colorInt);
        }
    }
}
