using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEA
{
    internal class PreSetItems  
    {
        public Obstacle obstacle = new Obstacle(Path.Combine(Application.StartupPath, "Images", "Asteroid.png"), Items.Obstacle, 0, false);
        public Obstacle crackedObstacle = new Obstacle(Path.Combine(Application.StartupPath, "Images", "Cracked Asteroid.png"), Items.CrackedObstacle, 0, true);
        public PowerUp doubleScore = new PowerUp(Path.Combine(Application.StartupPath, "Images", "Double Score.png"), Items.DoubleScore, 0);
        public PowerUp doubleCoins = new PowerUp(Path.Combine(Application.StartupPath, "Images", "Double Coins.png"), Items.DoubleCoins, 0);
        public PowerUp invincibility = new PowerUp(Path.Combine(Application.StartupPath, "Images", "Invincibility.png"), Items.Invincibility, 0);
        public Item coin = new Item(Path.Combine(Application.StartupPath, "Images", "Coin.png"), Items.Coin, 0);
        public Item emptySquare = new Item(Path.Combine(Application.StartupPath, "Images", "Square.png"), Items.EmptySquare, 0);
        // these are the templates for each item so that it is simpler to create them elsewhere
        public PreSetItems() 
        {

        }
    }
}
