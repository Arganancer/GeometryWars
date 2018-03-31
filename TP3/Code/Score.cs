using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace TP3
{
    /// <summary>
    /// Classe qui gère le score accumulé du joueur pour la partie en cours.
    /// </summary>
    public class Score
    {
        private static Score score = null;
        private static int multiplier;
        private static int scoreTotal;
        private static Time timeSinceLastLostLife;
        private DateTime scoreTimeLastUpdated;

        /// <summary>
        /// Constructeur de la classe Score.
        /// </summary>
        private Score()
        {
            multiplier = 1;
            scoreTotal = 0;
            timeSinceLastLostLife = Time.Zero;
        }

        /// <summary>
        /// Retourne la seule instance de la class Score.
        /// </summary>
        /// <returns></returns>
        public static Score GetInstanceScore()
        {
            if (score == null)
            {
                score = new Score();
                return score;
            }
            return score;
        }

        /// <summary>
        /// Est appelé quand le Hero perd une vie. Ceci réinitialise le multiplicateur de bonus.
        /// </summary>
        /// <param name="stopwatch"> La minuterie principale de la partie. </param>
        public void HeroLostLife(Stopwatch stopwatch)
        {
            timeSinceLastLostLife = Time.FromMilliseconds((int) stopwatch.ElapsedMilliseconds);
        }

        /// <summary>
        /// Retourne le score actuel du joueur pour la partie courante.
        /// </summary>
        /// <returns> Le score actuel du joueur pour la partie courante. </returns>
        public string GetScore()
        {
            return scoreTotal + "(x" + multiplier + ")";
        }

        /// <summary>
        /// Méthode qui calcule la partie du pointage qui est progressivement accumulé grâce au temps écoulé.
        /// </summary>
        /// <param name="stopwatch"> La minuterie principale de la partie. </param>
        public void Update(Stopwatch stopwatch)
        {
            if (stopwatch.IsRunning)
            {
                if (scoreTimeLastUpdated + TimeSpan.FromMilliseconds(150) < DateTime.Now)
                {
                    scoreTimeLastUpdated = DateTime.Now;
                    if (Time.FromMilliseconds((int) stopwatch.ElapsedMilliseconds) - timeSinceLastLostLife >= Time.Zero &&
                        Time.FromMilliseconds((int) stopwatch.ElapsedMilliseconds) - timeSinceLastLostLife <
                        Time.FromSeconds(16))
                    {
                        multiplier = 1;
                    }
                    else if (Time.FromMilliseconds((int) stopwatch.ElapsedMilliseconds) - timeSinceLastLostLife >=
                             Time.FromSeconds(16) &&
                             Time.FromMilliseconds((int) stopwatch.ElapsedMilliseconds) - timeSinceLastLostLife <
                             Time.FromSeconds(32))
                    {
                        multiplier = 2;
                    }
                    else if (Time.FromMilliseconds((int) stopwatch.ElapsedMilliseconds) - timeSinceLastLostLife >=
                             Time.FromSeconds(32) &&
                             Time.FromMilliseconds((int) stopwatch.ElapsedMilliseconds) - timeSinceLastLostLife <
                             Time.FromSeconds(64))
                    {
                        multiplier = 3;
                    }
                    else
                    {
                        multiplier = 4;
                    }
                // Time Score
                scoreTotal += (int) ((stopwatch.ElapsedMilliseconds / 10000) * multiplier);
                }
            }
        }

        /// <summary>
        /// Méthode qui ajoute au pointage total chaque fois que le joueur tue un ennemi avec un projectile.
        /// </summary>
        /// <param name="stopwatch"> La minuterie principale de la partie. </param>
        public void EnemyKilled(Stopwatch stopwatch)
        {
            scoreTotal += (int)(stopwatch.ElapsedMilliseconds / 100) * multiplier;
        }
    }
}
