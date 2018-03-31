using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;


namespace TP3
{
    /// <summary>
    /// Classe abstraite désignant n'importe quel objet qui pourrait être dessiné à l'écran.
    /// </summary>
    public abstract class Drawable
    {
        protected static Random rnd = new Random();
        protected ConvexShape shape = null;
        public Vector2f Position { get; set; }
        protected int colorInt;

        public Color Color
        {
            get { return shape.FillColor; }
            set { shape.FillColor = value; } 
        }

        public virtual float Angle
        {
            get { return shape.Rotation; }
            set { shape.Rotation = value; }
        }

        public Vector2f this[uint index]
        {
            get { return shape.GetPoint(index); }
            set { shape.SetPoint(index, value); }
        }

        /// <summary>
        /// Drawable Constructor
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="nbVertices"></param>
        /// <param name="color"></param>
        protected Drawable(float posX, float posY, uint nbVertices)
        {
            Position = new Vector2f(posX, posY);
            shape = new ConvexShape(nbVertices);
            Angle = 0;
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="window"></param>
        public virtual void Draw(RenderWindow window)
        {
            shape.Position = Position;
            window.Draw(shape);
        }

        /// <summary>
        /// Retourne la boîte englobante associée à la forme.  Utilisée pour les collisions.
        /// </summary>
        public FloatRect BoundingBox
        {
            get { return shape.GetGlobalBounds(); }
        }

        /// <summary>
        /// Vérifie si deux éléments affichés s'entrecoupent
        /// </summary>
        /// <param name="m">Le second élément avec lequel vérifier la collision</param>
        /// <returns>true s'il y a collision, false sinon.</returns>
        public bool Intersects(Drawable m)
        {
            FloatRect r = m.BoundingBox;
            r.Left = m.Position.X;
            r.Top = m.Position.Y;
            return BoundingBox.Intersects(r);
        }

        /// <summary>
        /// Vérifie si l'objet contient le point où se trouve l'élément reçu en paramètre
        /// </summary>
        /// <param name="m">L'élément dont il faut vérifier la position</param>
        /// <returns>true si la boîte englobante de l'objet courant contient le point (la position) où se
        /// trouve l'objet reçu en paramètre</returns>
        public bool Contains(Drawable m)
        {
            return BoundingBox.Contains(m.Position.X, m.Position.Y);
        }

        /// <summary>
        /// 0 = blue (Star enemy)
        /// 1 = yellow (Square enemy)
        /// 2 = Orange (Triangle enemy)
        /// 3 = purple (no current use)
        /// 4 = green (Hero)
        /// 5 = grey (Player smoke)
        /// 6 = Blue (Star color)
        /// 7 = White (Wall color)
        /// </summary>
        protected void RandomColor(int i, float percentageModifier)
        {
            // Blue
            if (i == 0)
            {
                Color = new Color((byte)rnd.Next(23, 30), (byte)rnd.Next(166, 175), (byte)rnd.Next(222, 230), (byte)(rnd.Next(222, 254) * percentageModifier));
            }
            // Yellow
            else if (i == 1)
            {
                Color = new Color((byte)rnd.Next(222, 254), (byte)rnd.Next(222, 254), 0, (byte)(rnd.Next(222, 254) * percentageModifier));
            }
            // Orange
            else if (i == 2)
            {
                Color = new Color((byte)rnd.Next(222, 254), (byte)rnd.Next(120, 155), 0, (byte)(rnd.Next(222, 254) * percentageModifier));
            }
            // Purple
            else if (i == 3)
            {
                Color = new Color((byte)rnd.Next(188, 192), 0, (byte)rnd.Next(185, 189), (byte)(rnd.Next(222, 254) * percentageModifier));
            }
            // Green
            else if (i == 4)
            {
                Color = new Color((byte)rnd.Next(70, 74), (byte)rnd.Next(200, 210), (byte)rnd.Next(20, 28), (byte)(rnd.Next(222, 254) * percentageModifier));
            }
            // Grey (smoke)
            else if (i == 5)
            {
                byte rgbGrey = (byte) rnd.Next((int) (100 * percentageModifier), (int) (254 * percentageModifier));
                Color = new Color(rgbGrey, rgbGrey, rgbGrey, (byte) (254 * percentageModifier));
            }
            // Blue (star)
            else if (i == 6)
            {
                byte rgbRG = (byte)rnd.Next(150, 220);
                Color = new Color(rgbRG, rgbRG, (byte)rnd.Next(240, 254), (byte)(rnd.Next(45, 84) + (percentageModifier * 160)));
            }
            // Wall color (transparent white)
            else if (i == 7)
            {
                Color = new Color(128, 128, 128, 80);
            }
        }
    }
}
