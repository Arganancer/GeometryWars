using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace TP3.Code
{
    // Used this site as reference for parts of my code in this class: 
    // http://pushbuttonreceivecode.com/blog/working-with-joysticks-in-sfml

    /// <summary>
    /// Controller class. Singleton that is used for all Joystick inputs.
    /// </summary>
    public class Controller
    {
        private static Controller controllerInstance = null;
        private static Joystick.Identification id;
        public static uint joystickPos = 0;
        
        /// <summary>
        /// Private constructor.
        /// </summary>
        private Controller()
        {
            ControllerUpdate();
            for (uint i = 0; i < 7; i++)
            {
                if (Joystick.IsConnected(i))
                {
                    id = Joystick.GetIdentification(i);
                    joystickPos = i;
                    break;
                }
            }
            if (Joystick.IsConnected(joystickPos))
            {
                Console.WriteLine(Joystick.GetButtonCount(joystickPos));
                Console.WriteLine(Joystick.HasAxis(joystickPos, Joystick.Axis.X));
                Console.WriteLine(Joystick.HasAxis(joystickPos, Joystick.Axis.Y));
                Console.WriteLine(Joystick.HasAxis(joystickPos, Joystick.Axis.U));
                Console.WriteLine(Joystick.HasAxis(joystickPos, Joystick.Axis.R));
            }
        }

        /// <summary>
        /// Method that checks to see whether or not the specified controller is plugged in.
        /// </summary>
        /// <returns> Returns true if the controller is plugged in. False if not. </returns>
        public bool ControllerIsPluggedIn()
        {
            if (!Joystick.IsConnected(joystickPos))
            {
                controllerInstance = new Controller();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Method that returns the instance of the controller class.
        /// </summary>
        public static Controller ControllerInstance
        {
            get
            {
                if (controllerInstance == null)
                {
                    controllerInstance = new Controller();
                    return controllerInstance;
                }
                else
                {
                    return controllerInstance;
                }
            }
        }

        /// <summary>
        /// Checks whether or not the specified button is pressed.
        /// </summary>
        /// <param name="i">
        /// Xbox 360 Controller Button Values:
        /// A = 0,
        /// B = 1,
        /// X = 2,
        /// Y = 3,
        /// Left Bumper = 4,
        /// Right Bumper = 5,
        /// Select = 6,
        /// Start = 7,
        /// Left Stick Pressed = 8,
        /// Right Stick Pressed = 9.
        /// </param>
        /// <returns> True if the indicated button is pressed, else false. </returns>
        public bool ControllerIsButtonPressed(uint i)
        {
            if (Joystick.IsButtonPressed(joystickPos, i))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Updates the joystick class. Joysticks do not work if this is not done regularly.
        /// </summary>
        public void ControllerUpdate()
        {
            Joystick.Update();
        }

        #region Left Joystick
        /// <summary>
        /// Checks the Position of the left joystick.
        /// </summary>
        /// <returns> returns the Position of the left joystick. </returns>
        public Vector2f LeftJoystickPos()
        {
            float x = Joystick.GetAxisPosition(joystickPos, Joystick.Axis.X);
            float y = Joystick.GetAxisPosition(joystickPos, Joystick.Axis.Y);
            return new Vector2f(x, y);
        }

        /// <summary>
        /// Checks the speed of inclination of the left joystick.
        /// </summary>
        /// <returns> Returns the speed of inclination of the left joystick (value from 0 to 100) </returns>
        public float LeftJoystickSpeed()
        {
            return (float)Math.Sqrt(Math.Pow(LeftJoystickPos().X, 2) + Math.Pow(LeftJoystickPos().Y, 2));
        }

        /// <summary>
        /// Checks the angle of the left joystick.
        /// </summary>
        /// <returns> returns the angle of the left joystick. </returns>
        public float LeftJoystickAngle()
        {
            return (float) (Math.Atan2(LeftJoystickPos().Y, LeftJoystickPos().X) * 180 / Math.PI);
        }
        #endregion

        #region Right Joystick
        /// <summary>
        /// Checks the Position of the right joystick.
        /// </summary>
        /// <returns> returns the Position of the right joystick. </returns>
        public Vector2f RightJoystickPos()
        {
            float x = Joystick.GetAxisPosition(joystickPos, Joystick.Axis.U);
            float y = Joystick.GetAxisPosition(joystickPos, Joystick.Axis.R);
            return new Vector2f(x, y);
        }

        /// <summary>
        /// Checks the speed of inclination of the right joystick.
        /// </summary>
        /// <returns> Returns the speed of inclination of the right joystick (value from 0 to 100) </returns>
        public float RightJoystickSpeed()
        {
            return (float)Math.Sqrt(Math.Pow(RightJoystickPos().X, 2) + Math.Pow(RightJoystickPos().Y, 2));
        }

        /// <summary>
        /// Checks the angle of the right joystick.
        /// </summary>
        /// <returns> returns the angle of the right joystick. </returns>
        public float RightJoystickAngle()
        {
            return (float)(Math.Atan2(RightJoystickPos().Y, RightJoystickPos().X) * 180 / Math.PI);
        }
        #endregion

        public bool MouseButtonIsPressed(Mouse.Button button)
        {
            if (Mouse.IsButtonPressed(button))
            {
                return true;
            }
            return false;
        }

        public bool KeyboardIsKeyPressed(Keyboard.Key key)
        {
            if (Keyboard.IsKeyPressed(key))
            {
                return true;
            }
            return false;
        }

        public float GetAngleBetweenCursorAndHero(RenderWindow window, Hero hero)
        {
            //Console.WriteLine("Hero X: " + hero.Position.X + " / Mouse X: " + Mouse.GetPosition(window).X);
            //Console.WriteLine("Hero Y: " + hero.Position.Y + " / Mouse Y: " + Mouse.GetPosition(window).Y);
            //Console.WriteLine();
            Vector2f pixelMousePos = (Vector2f)Mouse.GetPosition(window);
            Vector2i worldHeroPos = window.MapCoordsToPixel(hero.Position);

            return (float)(Math.Atan2(pixelMousePos.Y - worldHeroPos.Y, pixelMousePos.X - worldHeroPos.X) * 180 / Math.PI);
        }
    }
}
