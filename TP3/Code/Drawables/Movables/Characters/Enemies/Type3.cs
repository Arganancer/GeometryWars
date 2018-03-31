using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Audio;
using SFML.System;

namespace TP3.Code.Character
{
    /// <summary>
    /// Type3 enemies are small orange triangles that are very deadly.
    /// </summary>
    class Type3 : Enemy
    {
        private float speed;
        private int enemyMaxSize = 8;
        private int enemyCurrentSize = 0;
        private bool rotatesClockwise = false;
        private float angleModifier = 0.0f;
        private bool isIncreasing = true;

        /// <summary>
        /// Enemy Type1 Constructor
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="nbVertices"></param>
        /// <param name="color"></param>
        /// <param name="speed"></param>
        public Type3(Single posX, Single posY, UInt32 nbVertices, Single speed, int colorInt)
            : base(posX, posY, nbVertices, speed, colorInt)
        {
            Angle = rnd.Next(-180, 181);
            if (rnd.Next(0, 2) == 1)
            {
                rotatesClockwise = true;
            }
            spawningDelay = 40;
            this.speed = speed;
            setEnemyPoints();
        }

        /// <summary>
        /// 
        /// </summary>
        private void setEnemyPoints()
        {
            this[0] = new Vector2f(enemyCurrentSize, 0);
            this[1] = new Vector2f(-enemyCurrentSize, -enemyCurrentSize);
            this[2] = new Vector2f(-enemyCurrentSize/2f, 0);
            this[3] = new Vector2f(-enemyCurrentSize, enemyCurrentSize);
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
            if (Speed < 10 + speed)
            {
                DetermineAngle();
            }
            else
            {
                if (rotatesClockwise)
                {
                    Angle += rnd.Next(28, 35);
                }
                else
                {
                    Angle -= rnd.Next(28, 35);
                }
            }

            Speed += 0.02f;
            Advance(Speed);
            if (Game.Stopwatch.ElapsedMilliseconds > smokeParticleLastCreated.TotalMilliseconds + 50)
            {
                smokeParticleLastCreated = Game.Stopwatch.Elapsed;
                SmokeParticle smokeParticle = new SmokeParticle(Type, Position.X, Position.Y, 4, Speed / 2, false, 35, enemyMaxSize / 2, 0.2f, colorInt, 1);
                smokeParticle.Angle = rnd.Next(-7, 8);
                game.smokeParticles.Add(smokeParticle);
            }
            if (Speed > 11 + speed)
            {
                Projectile projectile = new Projectile(Type, Position.X, Position.Y, 8, 21, 8, colorInt)
                {
                    Angle =(float)(Math.Atan2(game.hero.Position.Y - Position.Y, game.hero.Position.X - Position.X) * 180 / Math.PI)
                };
                game.projectiles.Add(projectile);
                IsAlive = false;
            }
            return base.Update(deltaT, game);
        }

        /// <summary>
        /// 
        /// </summary>
        private void DetermineAngle()
        {
            if (rotatesClockwise)
            {
                if (isIncreasing)
                {
                    angleModifier += 0.02f;
                    if (angleModifier >= 4)
                    {
                        isIncreasing = false;
                    }
                }
                else
                {
                    angleModifier -= 0.02f;
                    if (angleModifier <= 1)
                    {
                        isIncreasing = true;
                    }
                }
                Angle += Math.Max(1, rnd.Next((int)(2 * angleModifier), (int)(4 * angleModifier)));
            }
            else
            {
                if (isIncreasing)
                {
                    angleModifier += 0.02f;
                    if (angleModifier >= -1)
                    {
                        isIncreasing = false;
                    }
                }
                else
                {
                    angleModifier -= 0.02f;
                    if (angleModifier <= -4)
                    {
                        isIncreasing = true;
                    }
                }
                Angle += Math.Min(rnd.Next((int)(4 * angleModifier), (int)(2 * angleModifier)), -1);
            }
        }
    }
}
