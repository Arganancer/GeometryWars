using System;
using System.IO;
using SFML;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFML.Audio;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TP3.Code;
using TP3.Code.Character;

namespace TP3
{
    /// <summary>
    /// Classe principale de la partie qui fait la gestion de toutes les autres classes.
    /// </summary>
    public class Game
    {

        #region Game Variables
        // Constantes et propriétés statiques
        public const int GAME_WIDTH = 1620;
        public const int GAME_HEIGHT = 880;
        public const int VIEW_WIDTH = 1920;
        public const int VIEW_HEIGHT = 1080;
        public const uint FRAME_LIMIT = 60;
        const float DELTA_T = 1.0f / (float)FRAME_LIMIT;
        private static Random rnd = new Random();
        public bool HeroIsMoving = false;
        public static Language CurrentLanguage = Language.English;
        public static bool GameIsRunning;
        public static TimeSpan TimeIntervalBetweenBombUses = new TimeSpan(0, 0, 2);
        public static DateTime TimeSinceLastBomb;

        // SFML
        public RenderWindow window = null;
        Font font = new Font("Data/emulogic.ttf");
        Text text = null;

        // Propriétés pour la partie
        public static Stopwatch Stopwatch = new Stopwatch();
        public List<Projectile> projectiles = new List<Projectile>();
        public List<Projectile> projectilesToRemove = new List<Projectile>();
        public List<Enemy> enemies = new List<Enemy>();
        public List<Enemy> enemiesToAdd = new List<Enemy>();
        public List<Enemy> enemyToRemove = new List<Enemy>();
        public List<Fragment> fragmentParticles = new List<Fragment>();
        public List<Fragment> particlesToRemove = new List<Fragment>();
        public List<SmokeParticle> smokeParticles = new List<SmokeParticle>();
        public List<SmokeParticle> smokeParticlesToRemove = new List<SmokeParticle>();
        public List<Star> stars = new List<Star>();
        public Hero hero;
        public Wall[] walls = new Wall[4];
        private View view;
        private Controller controller = Controller.ControllerInstance;
        public Score score;

        // FPS properties
        private float framesPerSecond = 0.0f;
        private float fpsTemp = 0.0f;
        Stopwatch fps = new Stopwatch();

        // Joystick event properties
        private bool leftStickStillPressed = false;
        private bool startButtonStillHeld = false;
        private bool selectButtonStillHeld = false;
        private bool debugEnemiesSpawn = false;

        #endregion

        #region SFML window events
        /// <summary>
        /// Gère la fermeture de l'écran.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClose(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }

        /// <summary>
        /// Gère les cas ou une touche de clavier est appuyée.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyPressed(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.F4))
            {
                CurrentLanguage = Language.English;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.F5))
            {
                CurrentLanguage = Language.French;
            }
        }

        /// <summary>
        /// Constructeur de la classe Game.
        /// </summary>s
        public Game()
        {
            text = new Text("", font) {CharacterSize = 25};
            window = new RenderWindow(new SFML.Window.VideoMode(VIEW_WIDTH, VIEW_HEIGHT), "Game", Styles.Fullscreen);
            window.Closed += new EventHandler(OnClose);
            window.KeyPressed += new EventHandler<KeyEventArgs>(OnKeyPressed);
            window.SetKeyRepeatEnabled(false);
            window.SetFramerateLimit(FRAME_LIMIT);
        }
        #endregion

        /// <summary>
        /// Méthode d'initialisation qui est appelée quand la classe est créée.
        /// </summary>
        private void InitGame()
        {
            fps.Start();
            GameIsRunning = true;
            score = Score.GetInstanceScore();
            Stopwatch.Start();
            hero = Hero.GetInstance;
            walls[0] = new Wall(0, 0, 4, 7);
            walls[1] = new Wall(0, GAME_HEIGHT, 4, 7);
            walls[2] = new Wall(GAME_WIDTH, 0, 4, 7);
            walls[3] = new Wall(GAME_WIDTH, GAME_HEIGHT, 4, 7);
            for (int i = 0; i < 4000; i++)
            {
                stars.Add(new Star(0, 0, 4, 0, 6));
            }
            view = new View(new Vector2f(GAME_WIDTH / 2f, GAME_WIDTH / 2f), new Vector2f(VIEW_WIDTH, VIEW_HEIGHT));
            window.SetView(view);
        }

        /// <summary>
        /// Méthode principale de la partie.
        /// </summary>
        public void Run()
        {
            if (File.Exists("Data/st.txt"))
            {
                if (ErrorCode.OK == StringTable.GetInstance().Parse(File.ReadAllLines("Data/st.txt")))
                {
                    InitGame();
                    {
                        window.SetActive();

                        while (window.IsOpen)
                        {
                            window.Clear(Color.Black);
                            window.DispatchEvents();
                            if (false == Update())
                            {
                                break;
                            }
                            Draw();
                            window.Display();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Méthode qui est appelée une fois par frame pour dessiner tout les objets à l'écran.
        /// </summary>
        public void Draw()
        {
            // Update Camera position
            view.Center = new Vector2f(((1f / 11f) * hero.Position.X + (10f / 11f) * GAME_WIDTH / 2), ((1f / 11f) * hero.Position.Y + (10f / 11f) * GAME_HEIGHT / 2));
            window.SetView(view);
            // Parcourez les listes appropriées pour faire afficher les éléments demandés.
            foreach (var projectile in projectiles)
            {
                projectile.Draw(window);
            }
            foreach (var star in stars)
            {
                star.Draw(window);
            }
            foreach (var wall in walls)
            {
                wall.Draw(window);
            }
            foreach (var particle in fragmentParticles)
            {
                particle.Draw(window);
            }
            foreach (var particle in smokeParticles)
            {
                particle.Draw(window);
            }
            foreach (var character in enemies)
            {
                character.Draw(window);
            }
            hero.Draw(window);
            #region Draw Text
            if (!GameIsRunning)
            {
                PauseScreen pauseScreen = new PauseScreen(0, 0, 4);
                pauseScreen.Draw(window);
                // Pause text
                text.DisplayedString = $"{StringTable.GetInstance().GetValue(CurrentLanguage, "ID_PAUSE")}";
                text.Position = new Vector2f(Game.GAME_WIDTH / 2 - (text.DisplayedString.Length * text.CharacterSize) / 2, GAME_HEIGHT / 2 - text.CharacterSize);
                window.Draw(text);

                // Start prompt text
                if (controller.ControllerIsPluggedIn())
                {
                    text.DisplayedString = $"{StringTable.GetInstance().GetValue(CurrentLanguage, "ID_START_PROMPT")}";
                }
                else
                {
                    text.DisplayedString = $"{StringTable.GetInstance().GetValue(CurrentLanguage, "ID_START_KEYBOARD_PROMPT")}";
                }
                text.CharacterSize = 18;
                text.Position = new Vector2f(Game.GAME_WIDTH / 2 - (text.DisplayedString.Length * text.CharacterSize) / 2, GAME_HEIGHT / 2 + text.CharacterSize);
                window.Draw(text);

                // Debug enemy spawning text
                text.DisplayedString = $"{StringTable.GetInstance().GetValue(CurrentLanguage, "ID_CONTROL_DEBUG")}";
                text.Position = new Vector2f(Game.GAME_WIDTH / 2 - (text.DisplayedString.Length * text.CharacterSize) / 2, GAME_HEIGHT / 2 + text.CharacterSize * 2 + 5);
                window.Draw(text);

                // How to use bomb text
                text.DisplayedString = $"{StringTable.GetInstance().GetValue(CurrentLanguage, "ID_CONTROL_BOMB")}";
                text.Position = new Vector2f(Game.GAME_WIDTH / 2 - (text.DisplayedString.Length * text.CharacterSize) / 2, GAME_HEIGHT / 2 + text.CharacterSize * 3 + 10);
                window.Draw(text);

                text.CharacterSize = 25;
            }

            // Temps total
            float timeElapsed = Stopwatch.ElapsedMilliseconds;
            timeElapsed /= 1000;
            timeElapsed = (float) Math.Round(timeElapsed, 2);
            text.DisplayedString = string.Format("{1} = {0,-5}", timeElapsed, StringTable.GetInstance().GetValue(CurrentLanguage, "ID_TOTAL_TIME"));
            text.Position = new Vector2f(0, GAME_HEIGHT + 5);
            window.Draw(text);

            // Points de vie
            text.DisplayedString = string.Format("{1} = {0,-4}", hero.life, StringTable.GetInstance().GetValue(CurrentLanguage,"ID_LIFE"));
            text.Position = new Vector2f(GAME_WIDTH - text.DisplayedString.Length * text.CharacterSize, - text.CharacterSize - 5);
            window.Draw(text);

            // Number of bombs
            text.DisplayedString = string.Format("{1} = {0,-4}", hero.nbBombs, StringTable.GetInstance().GetValue(CurrentLanguage, "ID_BOMBS"));
            text.Position = new Vector2f(Game.GAME_WIDTH / 2 - (text.DisplayedString.Length * text.CharacterSize) /2, -text.CharacterSize - 5);
            window.Draw(text);

            // Score
            text.DisplayedString = string.Format("{1} = {0,-4}", score.GetScore(), StringTable.GetInstance().GetValue(CurrentLanguage, "ID_SCORE"));
            text.Position = new Vector2f(0, -text.CharacterSize - 5);
            window.Draw(text);

            // FPS
            framesPerSecond++;
            if (fps.Elapsed > TimeSpan.FromMilliseconds(1000))
            {
                fpsTemp = framesPerSecond;
                fps.Restart();
                framesPerSecond = 0;
            }
            text.Position = new Vector2f(GAME_WIDTH - 250, GAME_HEIGHT + 5);
            text.DisplayedString = string.Format("{1} = {0,-4}", fpsTemp, "FPS");
            window.Draw(text);
            #endregion
        }

        /// <summary>
        /// Méthode qui est appelée une fois par frame pour mettre à jour tout les objets présents dans la partie.
        /// </summary>
        /// <returns> Retourn vrai si le héros est en vie, faux sinon. </returns>
        public bool Update()
        {
            JoystickEvents();
            if (GameIsRunning)
            {
                // Update Score
                score.Update(Stopwatch);

                #region Updates

                // Update Stars and Particles   
                foreach (var star in stars)
                {
                    star.Update(DELTA_T, this);
                }
                foreach (var particle in fragmentParticles)
                {
                    if (!particle.Update(DELTA_T, this))
                    {
                        particlesToRemove.Add(particle);
                    }
                }
                foreach (var particle in smokeParticles)
                {
                    if (!particle.Update(DELTA_T, this))
                    {
                        smokeParticlesToRemove.Add(particle);
                    }
                }
                // Update Personnages et projectiles
                hero.Update(DELTA_T, this);
                foreach (var enemy in enemies)
                {
                    if (!enemy.Update(DELTA_T, this))
                    {
                        enemyToRemove.Add(enemy);
                    }
                }
                foreach (var projectile in projectiles)
                {
                    if (!projectile.Update(DELTA_T, this))
                    {
                        projectilesToRemove.Add(projectile);
                    }
                }

                #endregion

                #region Gestion des collisions

                // Projectile Collisions
                foreach (var projectile in projectiles)
                {
                    if (projectile.IsAlive)
                    {
                        foreach (var projectile2 in projectiles)
                        {
                            if (projectile.Type != projectile2.Type && projectile.Intersects(projectile2))
                            {
                                projectile.IsAlive = false;
                                projectile2.IsAlive = false;
                                break;
                            }
                        }
                        if (projectile.Type == CharacterType.Hero)
                        {
                            foreach (var enemy in enemies)
                            {
                                if (enemy.Intersects(projectile))
                                {
                                    score.EnemyKilled(Stopwatch);
                                    enemy.IsAlive = false;
                                    projectile.IsAlive = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (hero.Intersects(projectile))
                            {
                                hero.life -= 1;
                                projectile.IsAlive = false;
                            }
                        }
                    }
                }

                // Deadly Particle Collisions
                foreach (var particle in fragmentParticles)
                {
                    if (particle.IsDeadly && particle.IsAlive)
                    {
                        foreach (var enemy in enemies)
                        {
                            if (particle.Intersects(enemy))
                            {
                                enemy.IsAlive = false;
                                particle.IsAlive = false;
                                break;
                            }
                        }
                        foreach (var projectile in projectiles)
                        {
                            if (projectile.Type == CharacterType.Enemy && particle.Intersects(projectile))
                            {
                                projectile.IsAlive = false;
                                particle.IsAlive = false;
                                break;
                            }
                        }
                    }
                }

                // Enemy -> Hero Collisions
                foreach (var enemy in enemies)
                {
                    if (enemy.IsAlive && hero.Intersects(enemy))
                    {
                        enemy.spawnsEnemiesWhenDead = false;
                        score.HeroLostLife(Stopwatch);
                        enemy.IsAlive = false;
                        hero.life -=1;
                    }
                }

                #endregion

                #region Retraits

                // Retrait des ennemis détruits et des projectiles inutiles
                foreach (var projectile in projectilesToRemove)
                {
                    projectiles.Remove(projectile);
                }
                projectilesToRemove.Clear();
                foreach (var enemy in enemyToRemove)
                {
                    enemies.Remove(enemy);
                }
                enemyToRemove.Clear();
                foreach (var particle in particlesToRemove)
                {
                    fragmentParticles.Remove(particle);
                }
                particlesToRemove.Clear();
                foreach (var particle in smokeParticlesToRemove)
                {
                    smokeParticles.Remove(particle);
                }
                smokeParticlesToRemove.Clear();

                #endregion

                #region Spawning des nouveaux ennemis

                // On veut avoir au minimum 5 ennemis (n'incluant pas les triangles). Il faut les ajouter ici
                int nbEnemies = 0;
                foreach (var enemy in enemies)
                {
                    if (enemy.spawnsEnemiesWhenDead)
                    {
                        nbEnemies++;
                    }
                }
                if (nbEnemies < 5)
                {
                    nbEnemies = 5 - nbEnemies;
                }
                else
                {
                    nbEnemies = 1;
                }
                if (debugEnemiesSpawn && TimeSinceLastBomb + TimeIntervalBetweenBombUses < DateTime.Now)
                {
                    SpawnEnemies(nbEnemies);
                }
                enemiesToAdd.Clear();

                #endregion

            }
            // Retourn true si le héros est en vie, false sinon.
            if (hero.IsAlive)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Méthode qui rajoute une bombe au joueur. Cette méthode n'a pas été implantée due à un manque de temps.
        /// </summary>
        public void AddBomb()
        {
            hero.nbBombs++;
        }

        /// <summary>
        /// Méthode qui spawn des ennemies.
        /// </summary>
        /// <param name="nbEnemies"></param>
        private void SpawnEnemies(int nbEnemies)
        {
            for (int i = 0; i < nbEnemies; i++)
            {
                int enemy = rnd.Next(0, 2);
                bool addEnemy = false;
                int enemyPosX = rnd.Next(20, GAME_WIDTH - 20);
                int enemyPosY = rnd.Next(20, GAME_HEIGHT - 20);
                while (
                    Math.Sqrt(Math.Pow(hero.Position.X - enemyPosX, 2) + Math.Pow(hero.Position.Y - enemyPosY, 2)) <
                    200)
                {
                    enemyPosX = rnd.Next(20, GAME_WIDTH - 20);
                    enemyPosY = rnd.Next(20, GAME_HEIGHT - 20);
                }
                if (nbEnemies > 1)
                {
                    addEnemy = true;
                }
                else if (enemies.Count < 100)
                {
                    if (rnd.Next(0, 188) == 1)
                    {
                        addEnemy = true;
                    }
                }
                if (addEnemy)
                {
                    if (enemy == 0)
                    {
                        enemies.Add(new Type1(enemyPosX, enemyPosY, 4, 3, 1));
                    }
                    else if (enemy == 1)
                    {
                        enemies.Add(new Type2(enemyPosX, enemyPosY, 8, 4, 0));
                    }
                }
            }
            foreach (var enemyToAdd in enemiesToAdd)
            {
                enemies.Add(enemyToAdd);
            }
        }

        /// <summary>
        /// Méthode qui gère les commandes de manettes non liées au héros.
        /// </summary>
        private void JoystickEvents()
        {
            // Update Joystick <- necessary for joystick functionality.
            controller.ControllerUpdate();
            // Change Language (Left stick click)
            if (controller.ControllerIsButtonPressed(8))
            {
                if (!leftStickStillPressed)
                {
                    leftStickStillPressed = true;
                    if (CurrentLanguage == Language.English)
                    {
                        CurrentLanguage = Language.French;
                    }
                    else if (CurrentLanguage == Language.French)
                    {
                        CurrentLanguage = Language.English;
                    }
                }
            }
            else
            {
                leftStickStillPressed = false;
            }
            // Enable/Disable enemy spawning
            if (controller.ControllerIsButtonPressed(6) || controller.KeyboardIsKeyPressed(Keyboard.Key.P))
            {
                if (!selectButtonStillHeld)
                {
                    selectButtonStillHeld = true;
                    debugEnemiesSpawn = !debugEnemiesSpawn;
                }
            }
            else
            {
                selectButtonStillHeld = false;
            }
            // Pause/Unpause game
            Pause();
        }

        /// <summary>
        /// Méthode qui met la partie en pause.
        /// </summary>
        public void Pause()
        {
            if (controller.ControllerIsButtonPressed(7) || controller.KeyboardIsKeyPressed(Keyboard.Key.Escape))
            {
                if (!startButtonStillHeld)
                {
                    startButtonStillHeld = true;
                    if (Stopwatch.IsRunning)
                    {
                        Stopwatch.Stop();
                        GameIsRunning = false;
                    }
                    else
                    {
                        Stopwatch.Start();
                        GameIsRunning = true;
                    }
                }
            }
            // Code from when the game only supported controller play.
            //else if (!controller.ControllerIsPluggedIn())
            //{
            //    Stopwatch.Stop();
            //    GameIsRunning = false;
            //}
            else
            {
                startButtonStillHeld = false;
            }
        }
    }
}
