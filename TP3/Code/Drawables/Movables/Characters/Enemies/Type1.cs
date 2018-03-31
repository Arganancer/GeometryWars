using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using TP3.Code.Character;

namespace TP3
{
    /// <summary>
    /// Type1 enemies are yellow squares that chase and shoot the Hero.
    /// </summary>
    class Type1 : Enemy
    {
        private static Music spawnMusic;
        private float speed;
        private int enemyMaxSize = 20;
        private int enemyCurrentSize = 0;

        /// <summary>
        /// Enemy spawning music.
        /// </summary>
        static Type1()
        {
            //spawnMusic = new Music("Enemy_Spawn_Green.wav");
        }

        /// <summary>
        /// 
        /// </summary>
        private void setEnemyPoints()
        {
            this[0] = new Vector2f(-enemyCurrentSize / 2, -enemyCurrentSize / 2);
            this[1] = new Vector2f(-enemyCurrentSize / 2, enemyCurrentSize / 2);
            this[2] = new Vector2f(enemyCurrentSize / 2, enemyCurrentSize / 2);
            this[3] = new Vector2f(enemyCurrentSize / 2, -enemyCurrentSize / 2);
        }

        /// <summary>
        /// Enemy Type1 Constructor
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="nbVertices"></param>
        /// <param name="color"></param>
        /// <param name="speed"></param>
        public Type1(Single posX, Single posY, UInt32 nbVertices, Single speed, int colorInt)
            : base(posX, posY, nbVertices, speed, colorInt)
        {
            spawnsEnemiesWhenDead = false;
            this.speed = speed;
            setEnemyPoints();
        }

        private bool RightAngle = false;
        private float AngleModifier = 0;

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
                    float spawningPercentage = (float) timeSinceSpawnStart / spawningDelay;
                    int size = (int) (enemyMaxSize * spawningPercentage);
                    RandomColor(colorInt, spawningPercentage);
                    enemyCurrentSize = size;
                    setEnemyPoints();
                    Speed = speed * spawningPercentage / 2;
                }
                else
                {
                    RandomColor(colorInt, 1);
                    IsSpawning = false;
                    enemyCurrentSize = enemyMaxSize;
                    setEnemyPoints();
                    Speed = speed;
                }
            }
            float AngleToHero =
                (float) (Math.Atan2(game.hero.Position.Y - Position.Y, game.hero.Position.X - Position.X) * 180 / Math.PI);
            if (RightAngle)
            {
                if (AngleModifier < 60)
                {
                    AngleModifier += 3;
                }
                else
                {
                    RightAngle = false;
                }
            }
            else
            {
                if (AngleModifier > -60)
                {
                    AngleModifier -= 3;
                }
                else
                {
                    RightAngle = true;
                }
            }
            Angle = AngleToHero + AngleModifier;
            if (Game.Stopwatch.ElapsedMilliseconds > smokeParticleLastCreated.TotalMilliseconds + 15)
            {
                smokeParticleLastCreated = Game.Stopwatch.Elapsed;
                SmokeParticle smokeParticle = new SmokeParticle(Type, Position.X, Position.Y, 4, Speed / 2, false, 25,
                    enemyMaxSize / 2, 0.2f, colorInt, 1);
                smokeParticle.Angle = Angle - 180;
                game.smokeParticles.Add(smokeParticle);
            }
            Advance(Speed);
            Fire(deltaT, game);
            return base.Update(deltaT, game);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaT"></param>
        /// <param name="game"></param>
        public void Fire(float deltaT, Game game)
        {
            if (Math.Sqrt(Math.Pow(game.hero.Position.X - Position.X, 2) + Math.Pow(game.hero.Position.Y - Position.Y, 2)) < 200)
            {
                if (lastfire + TimeSpan.FromMilliseconds(1000) < DateTime.Now)
                {
                    lastfire = DateTime.Now;
                    Projectile projectile = new Projectile(Type, Position.X, Position.Y, 8, 7, 5, colorInt)
                    {
                        Angle = (float)((Math.Atan2(game.hero.Position.Y - Position.Y, game.hero.Position.X - Position.X) * 180 / Math.PI) + Angle)/2
                    };
                    game.projectiles.Add(projectile);
                }
            }
        }
    }
}
