using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFML.Audio;
using TP3.Code;
using TP3.Code.Particles;

namespace TP3
{
    /// <summary>
    /// 
    /// La classe « Hero » représente le personnage contrôlable par le joueur.
    /// Cette classe est un Singleton.
    /// </summary>
    public class Hero : Character
    {
        // La seule instance possible de la classe « Hero ».
        private static Hero hero = null;
        // The number of bombs the player starts with.
        public const int NB_BOMBS_AT_BEGINNING = 50;
        // The number of lives the player starts with.
        public const int LIFE_AT_BEGINNING = 5;
        // The player's current amount of lives.
        public int life = 0;
        // The player's current amount of bombs.
        public int nbBombs = 0;
        // The sound made when a bomb is used.
        private Music soundBomb;
        // Whether or not the ship is moving. Is used by stars.
        public bool isMoving = false;
        // True if the right bumper is still held, false otherwise.
        private bool bombButtonStillHeld = false;

        /// <summary>
        /// Constructeur de la classe « Hero ». Il est privé, car « Hero » est un Singleton.
        /// </summary>
        /// <param name="posX"> Position en X de la grille de jeu où le « Hero » sera placé lors de son initialisation. </param>
        /// <param name="posY"> Position en Y de la grille de jeu où le « Hero » sera placé lors de son initialisation. </param>
        /// <param name="nbVertices"> Le nombre de points pour créer la forme de l’« Hero » (3). </param>
        /// <param name="speed"> La vitesse de l’« Hero » lors de son initialisation (n'est jamais utilisé). </param>
        /// <param name="colorInt"> Couleur de l’« Hero » lors de son initialisation. Les choix de couleur son de 0 à 7. 
        /// La couleur de l’« Hero » a la valeur 4. </param>
        private Hero(float posX, float posY, uint nbVertices, float speed, int colorInt)
            : base(posX, posY, nbVertices, speed, colorInt)
        {
            life = LIFE_AT_BEGINNING;
            nbBombs = NB_BOMBS_AT_BEGINNING;
            Type = CharacterType.Hero;

            // placing the points of the Hero ship's shape (Ship starts oriented to the right (>).
            this[0] = new Vector2f(-10, 10);
            this[1] = new Vector2f(25, 0);
            this[2] = new Vector2f(-10, -10);
        }

        /// <summary>
        /// Accesseur pour la seule instance de la classe « Hero ».
        /// </summary>
        public static Hero GetInstance
        {
            get
            {
                if (hero == null)
                {
                    // Initialisation of the Hero ship. It starts in the middle of the screen.
                    hero = new Hero(Game.GAME_WIDTH / 2, Game.GAME_HEIGHT / 2, 3, 0, 4);
                    return hero;
                }
                return hero;
            }
        }

        /// <summary>
        /// Mise à jour de la classe « Hero ». Fais la validation et le traitement d'entrée de donnée de la manette.
        /// </summary>
        /// <param name="deltaT"> Le temps en seconde écoulé depuis la dernière mise à jour. </param>
        /// <param name="game"> L'instance de la classe du Jeu. Utilisé pour la mise à jour de certaines variables de la classe de Jeu. </param>
        /// <returns> Retourne vrai si l’« Hero » est encore en vie. Retourne faux si l’« Hero » n'a plus de vie. </returns>
        public override bool Update(float deltaT, Game game)
        {
            float speed = 0;
            // Retrieves the Singleton instance of the controller.
            Controller controller = Controller.ControllerInstance;
            if (controller.ControllerIsPluggedIn())
            {
                speed = controller.LeftJoystickSpeed();
            }
            else
            {
                bool W = controller.KeyboardIsKeyPressed(Keyboard.Key.W);
                bool A = controller.KeyboardIsKeyPressed(Keyboard.Key.A);
                bool S = controller.KeyboardIsKeyPressed(Keyboard.Key.S);
                bool D = controller.KeyboardIsKeyPressed(Keyboard.Key.D);
                if (W || A || S || D)
                {
                    speed = 100;
                    if (S && A) { Angle = 135; }
                    else if (S && D) { Angle = 45; }
                    else if (W && A) { Angle = 225; }
                    else if (W && D) { Angle = 315; }
                    else if (W) { Angle = 270; }
                    else if (A) { Angle = 180; }
                    else if (S) { Angle = 90; }
                    else if (D) { Angle = 0; }
                }
                else
                {
                    speed = 0;
                }
            }

            // Checks to see if the left joystick is tilted at least 30% from it's neutral position. 
            // This prevents involuntary movement in case the joystick is slightly slack.
            if (speed > 30)
            {
                isMoving = true;
                // The joystick's speed is about 100 when fully tilted in a direction. 
                // Here we divide that amount by 18 so the hero's speed is never more than 5.55f
                Speed = speed / 18;
                if (controller.ControllerIsPluggedIn())
                {
                    Angle = controller.LeftJoystickAngle();
                }
                // Temporary variable to validate whether or not the ship is outside the bounds of the game.
                Vector2f temp = Position + Direction * Speed;

                // Keeps the Hero's ship inside the game area.
                if (temp.X <= 20) { if (Angle > 0) { Angle = 90; } else { Angle = -90; } }
                else if (temp.X >= Game.GAME_WIDTH - 20) { if (Angle > 0) { Angle = 90; } else { Angle = -90; } }
                else if (temp.Y <= 20) { if (Angle < -90) { Angle = 180; } else { Angle = 0; } }
                else if (temp.Y >= Game.GAME_HEIGHT - 20) { if (Angle < 90) { Angle = 0; } else { Angle = -180; } }

                // After potential angle adjustments above, the temporary var is updated to re-validate the new path.
                temp = Position + Direction * Speed;
                if (!(temp.X <= 20) && !(temp.X >= Game.GAME_WIDTH - 20) && !(temp.Y <= 20) && !(temp.Y >= Game.GAME_HEIGHT - 20))
                {
                    SmokeParticle smokeParticle = new SmokeParticle(Type, Position.X, Position.Y, 4, Speed/2, false, 100, 15, 0.0f, 5, 1) {Angle = Angle - 180};
                    game.smokeParticles.Add(smokeParticle);
                    Advance(Speed);
                }
            }
            else
            {
                isMoving = false;
            }

            // Checks to see if the right joystick is tilted at least 30% from it's neutral position. 
            // This prevents involuntary shooting in case the joystick is slightly slack.
            if (controller.RightJoystickSpeed() > 30 || controller.MouseButtonIsPressed(Mouse.Button.Left) && !controller.ControllerIsPluggedIn())
            {
                Fire(game);
            }

            // Checks to see if the right bumper on the controller has been pressed.
            // This if/else statement ensures that the player must release the right bumper and press it again to shoot a second bomb.
            if (controller.ControllerIsButtonPressed(5) || controller.KeyboardIsKeyPressed(Keyboard.Key.Space) && !controller.ControllerIsPluggedIn())
            {
                if (!bombButtonStillHeld)
                {
                    bombButtonStillHeld = true;
                    FireBomb(game);
                }
            }
            else
            {
                bombButtonStillHeld = false;
            }

            // If the hero's life falls to 0 or less, the game is lost.
            if (life <= 0)
            {
                IsAlive = false;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Fonction pour utiliser une bombe. Cette fonction est appelée quand le joueur pèse sur le « Right Bumper » de la manette.
        /// </summary>
        /// <param name="game"> L'instance de la classe du Jeu. Utilisé pour placer les particules de la bombe dans leur liste. </param>
        private void FireBomb(Game game)
        {
            // This if statement manages whether or not the player can currently use a bomb.
            if (nbBombs > 0 && Game.TimeSinceLastBomb + Game.TimeIntervalBetweenBombUses < DateTime.Now)
            {
                Game.TimeSinceLastBomb = DateTime.Now;
                nbBombs--;
                FragmentParticleExplosion.CreateExplosion((int) Position.X, (int) Position.Y, Type, true, 17, game, 105, 0.5f, 500, 6);
            }
        }

        /// <summary>
        /// Cette méthode est appelée quand le joueur souhaite tirer des projectiles.
        /// </summary>
        /// <param name="game"> L'instance de la classe du Jeu. Utilisé pour placer les projectiles dans leur liste. </param>
        public void Fire(Game game)
        {
            // This if statement manages whether or not the player can currently fire a projectile.
            if (lastfire + TimeSpan.FromMilliseconds(110) < DateTime.Now)
            {
                lastfire = DateTime.Now;
                Controller controller = Controller.ControllerInstance;
                float projectileAngle = 0;
                if (controller.ControllerIsPluggedIn())
                {
                    projectileAngle = controller.RightJoystickAngle() + (rnd.Next(-150, 151) / 100f);
                }
                else
                {
                    projectileAngle = controller.GetAngleBetweenCursorAndHero(game.window, Hero.GetInstance) + (rnd.Next(-150, 151) / 100f);
                }
                Projectile projectile = new Projectile(Type, Position.X, Position.Y, 8, 12, 5, colorInt)
                {
                    // The rnd variable is used to add a slight "spray" effect to the user's projectile.
                    // The angle is altered by a value between -1.5f and 1.5f.
                    Angle = projectileAngle
                };
                game.projectiles.Add(projectile);
            }
        }
    }
}
