using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using TP3.Code.Character;

namespace TP3
{
    /// <summary>
    /// Type2 enemies are blue stars that bounce around the game until they collide with something that causes them to die.
    /// </summary>
    public class Type2 : Enemy
    {
        private static Music spawnMusic;
        private float speed;
        private int enemyMaxSize = 30;
        private int enemyCurrentSize = 0;
        private static Random rnd = new Random();
        private int currentDrawAngle;

        /// <summary>
        /// Enemy Type1 Constructor
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="nbVertices"></param>
        /// <param name="color"></param>
        /// <param name="speed"></param>
        public Type2(Single posX, Single posY, UInt32 nbVertices, Single speed, int colorInt)
            : base(posX, posY, nbVertices, speed, colorInt)
        {
            spawnsEnemiesWhenDead = true;
            Angle = rnd.Next(-180, 181);
            currentDrawAngle = (int)Angle;
            this.speed = speed;
            setEnemyPoints();
        }

        /// <summary>
        /// 
        /// </summary>
        private void setEnemyPoints()
        {
            this[0] = new Vector2f(0, -enemyCurrentSize/2);
            this[1] = new Vector2f(enemyCurrentSize/6, -enemyCurrentSize/6);
            this[2] = new Vector2f(enemyCurrentSize/2, 0);
            this[3] = new Vector2f(enemyCurrentSize / 6, enemyCurrentSize / 6);
            this[4] = new Vector2f(0, enemyCurrentSize/2);
            this[5] = new Vector2f(-enemyCurrentSize / 6, enemyCurrentSize / 6);
            this[6] = new Vector2f(-enemyCurrentSize / 2, 0);
            this[7] = new Vector2f(-enemyCurrentSize / 6, -enemyCurrentSize / 6);
        }

        /// <summary>
        /// TO BE COMPLETED
        /// </summary>
        /// <param name="deltaT"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        public override bool Update(Single deltaT, Game game)
        {
            if (IsSpawning)
            {
                if (timeSinceSpawnStart < spawningDelay)
                {
                    timeSinceSpawnStart++;
                    float spawningPercentage = (float)timeSinceSpawnStart / spawningDelay;
                    int size = (int)(enemyMaxSize * spawningPercentage);
                    RandomColor(colorInt, spawningPercentage);
                    enemyCurrentSize = size;
                    setEnemyPoints();
                    Speed = speed * spawningPercentage / 2;
                }
                else
                {
                    IsSpawning = false;
                    RandomColor(colorInt, 1);
                    enemyCurrentSize = enemyMaxSize;
                    setEnemyPoints();
                    Speed = speed;
                }
            }
            Bounce();
            Advance(Speed);
            if (Game.Stopwatch.ElapsedMilliseconds > smokeParticleLastCreated.TotalMilliseconds + 20)
            {
                smokeParticleLastCreated = Game.Stopwatch.Elapsed;
                SmokeParticle smokeParticle = new SmokeParticle(Type, Position.X, Position.Y, 4, 1, false, 30, enemyMaxSize / 5, 0.2f, colorInt, 1);
                smokeParticle.Angle = rnd.Next(-180, 181);
                game.smokeParticles.Add(smokeParticle);
            }
            return base.Update(deltaT, game);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="window"></param>
        public override void Draw(RenderWindow window)
        {
            
            float tempAngle = Angle;
            if (Game.GameIsRunning)
            {
                currentDrawAngle += 7;
                Angle = currentDrawAngle;
            }
            base.Draw(window);
            Angle = tempAngle;
        }
    }
}
