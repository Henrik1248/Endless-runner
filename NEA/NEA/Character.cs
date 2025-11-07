using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;

namespace NEA
{
    public class Character
    {
        public const int PLAYER_BASE_SPEED_VALUE = 2;
        public const int PLAYER_FAST_SPEED_VALUE = 4;
        public const int CHASER_SPEED_VALUE = 3;             //These values will be used to calculate the players/chasers speed at any point in time.
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public int BaseSpeed { get; protected set; }
        public string ImgFileLoc { get; protected set; }
        public int Size { get; protected set; }

        public Character(int xPos, int yPos, string fileLoc, int size) 
        {
            XPosition = xPos;
            YPosition = yPos;
            ImgFileLoc = fileLoc;
            Size = size;
        }
    }
    public class Player : Character
    {
        public int FastSpeed { get; private set; }      //The base speed of both character and the fast speed of the player all increase over time, they are directly proportional to the difficulty of the game at that point in time.
        public bool FastSpeedActive { get; set; }    
        public int Score { get; set; }
        public int Coins { get; set; }
        public int ObstaclesPassed { get; set; }
        public List<int> AlreadyPassedObstacles { get; set; }
        public int PowerUpsPickedUp { get; set; }
        public List<PowerUp> ActivePowerUps { get; set; }
        public int StartXPosition { get; set; } 
        public List<PictureBox> PowerUpImages { get; set; }
        public int StunnedDuration { get; set; }
        public int HighScore { get; set; }
        public Player(int xPos, int yPos, string fileLoc, int size) : base(xPos, yPos, fileLoc, size)
        {
            BaseSpeed = PLAYER_BASE_SPEED_VALUE;
            FastSpeed = PLAYER_FAST_SPEED_VALUE;
            Score = 0;
            Coins = 0;
            ObstaclesPassed = 0;
            AlreadyPassedObstacles = new List<int>();
            PowerUpsPickedUp = 0;
            ActivePowerUps = new List<PowerUp>();
            FastSpeedActive = false;
            StartXPosition = xPos;
            PowerUpImages = new List<PictureBox>() { };
            StunnedDuration = 0;
        }
        public void MoveLeft(int lengthOfSquare, PictureBox player, Map map, Game game) 
        {
            int leftXLimit = 850 + (lengthOfSquare - Size) - ((lengthOfSquare - Size) / 2);            // 850 = mapXPos
            if ((XPosition > leftXLimit) && CanMoveLeft(game))
            {
                XPosition -= FastSpeed;            
                player.Location = new Point(XPosition, YPosition);
            }
            //checks if the player can move left if it can then it moves the player picture box left and adjusts the players XPosition accordingly
        }
        public void MoveRight(int lengthOfSquare, PictureBox player, Map map, Game game) 
        {
            int rightXLimit = 850 + (2 * lengthOfSquare) + ((lengthOfSquare - Size) / 2);           // 850 = mapXPos
            if (XPosition < rightXLimit && CanMoveRight(game))
            {
                XPosition += FastSpeed;           
                player.Location = new Point(XPosition, YPosition);
            }
            //checks if the player can move right if it can then it moves the player picture box right and adjusts the players XPosition accordingly
        }
        public bool CanMoveLeft(Game game)
        {
            if (IsStunned())
            {
                return false;
            }

            Map map = game.CurrentMap;

            if (!(XPosition >= 922 && XPosition < 926) && !(XPosition >= 994 && XPosition < 998))   // 850 = mapXPos   72 = lengthOfSquare     4 = FastSpeed     // 922 = 850 + 72        926 = 922 + 4       //994 = 850 + (2 * 72)       998 = 994 + 4
            {
                return true;                            // The player is not entering another column
            }

            if (map.MapSequences.Count == 1)
            {
                return true;
            }                                      // The player will always be able to move in the start sequence since it only contains empty squares. 

            int row = (YPosition - map.MapSequences.Last().YPosition) / 72;      // 72 = lengthOfSquare       50 = size of player
            bool crossesTwoRows = !(((YPosition - map.MapSequences.Last().YPosition) % 72) <= 24);    // 24 = (72) - (50) + 2        + 2 to consider the fact it may be entering a new row since it is also moving forward when moving left
            bool twoSequences = !(map.MapSequences[map.MapSequences.Count - 2].YPosition > YPosition + 50);  // checks if the player is not in the last map sequence
            List<int[]> rows = new List<int[]>() { };
            int calculatedRow = 20 - (row + 1); // where 20 is the size of a map sequence , calculated row is the row the top of the player is in

            int[,] newMap = new int[3, 20];
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    int value;
                    switch (map.MapSequences[map.MapSequences.Count - 1].Map[x, y].ItemName)
                    {
                        case (Items.Obstacle):
                            value = 1;
                            break;
                        case (Items.CrackedObstacle): 
                            value = 2;
                            break;
                        default:
                            value = 0;
                            break;
                    }
                    newMap[x, y] = value;
                }
            } // simplifies the map sequence the player is in into a 2D integer array where only obstacles are stored as unique integers the rest of the items are all 0s

            if (twoSequences)
            {
                calculatedRow = 40 - (row + 1);   // adjusts the value of calculated row to consider that the player is not in the last map sequence

                newMap = new int[3, 40];         // 40 = 2 * size of map sequence        size of map sequence = 20
                for (int i = 0; i <= 1; i++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        for (int y = 0; y < 20; y++)
                        {
                            int value;
                            switch (map.MapSequences[map.MapSequences.Count - (2 - i)].Map[x, y].ItemName)
                            {
                                case (Items.Obstacle):
                                    value = 1;
                                    break;
                                case (Items.CrackedObstacle):
                                    value = 2;
                                    break;
                                default:
                                    value = 0;
                                    break;
                            }
                            newMap[x, y + (i * 20)] = value;
                        }
                    }
                }
            }// simplifies the 2 map sequences the player is in into a 2D integer array where only obstacles are stored as unique integers the rest of the items are all 0s

            rows.Add(new int[3] { newMap[0, calculatedRow], newMap[1, calculatedRow], newMap[2, calculatedRow] });

            if (crossesTwoRows)
            {
                rows.Add(new int[3] { newMap[0, calculatedRow - 1], newMap[1, calculatedRow - 1], newMap[2, calculatedRow - 1] });
            }
            // adds the relevant rows of the map sequence(s) to a list of integer arrays

            MapSequence mapSequence;

            if (twoSequences)
            {
                Item[,] itemSequence = new Item[3, 40];

                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 40; y++)
                    {
                        itemSequence[x, y] = map.MapSequences[map.MapSequences.Count + (y / 20) - 2].Map[x, y - (20 * (y / 20))];
                    }
                }

                mapSequence = new MapSequence(itemSequence, map.MapSequences[map.MapSequences.Count - rows.Count].StartSquare, map.MapSequences.Last().EndSquare, map.MapSequences.Last().YPosition);
            }
            else
            {
                mapSequence = map.MapSequences.Last();
            } // gets the relevant map sequences and makes that one new map sequence

            // 850 is the left most x position of the map
            // 72 is the length of a square
            // 4 is how much the player moves left and right each time

            if (XPosition >= 922 && XPosition < 926)        // 922 = 850 + 72        926 = 922 + 4
            {
                if (!IsPowerUpActive(Items.Invincibility))
                {
                    if (rows.Count == 2)
                    {
                        if (rows[0][0] == 2 && rows[1][0] == 2)
                        {
                            HitCrackedObstacle(game, mapSequence, 0, calculatedRow);
                            HitCrackedObstacle(game, mapSequence, 0, calculatedRow - 1);
                            return false;
                        }
                        else if (rows[0][0] == 2)
                        {
                            HitCrackedObstacle(game, mapSequence, 0, calculatedRow);
                            return false;
                        }
                        else if (rows[1][0] == 2)
                        {
                            HitCrackedObstacle(game, mapSequence, 0, calculatedRow - 1);
                            return false;
                        }

                        else if (rows[0][0] == 1 || rows[1][0] == 1)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (rows[0][0] == 2)
                        {
                            HitCrackedObstacle(game, mapSequence, 0, calculatedRow);
                            return false;
                        }

                        else if (rows[0][0] == 1)
                        {
                            return false;
                        }
                    }
                }
                List<int> rowsToCheck = crossesTwoRows ? new List<int>() { calculatedRow, calculatedRow - 1 } : new List<int>() { calculatedRow};
                game.CheckForEvent(mapSequence, 0, rowsToCheck);
                // checks if the player hit any obstacles or cracked obstacles and affects the player accordingly 
                // otherwise (if the player doesn't hit any obstacles) the game checks for other events such as getting a power-up or a coin
            }
            else if (XPosition >= 994 && XPosition < 998)    //994 = 850 + (2 * 72)       998 = 994 + 4
            {
                if (!IsPowerUpActive(Items.Invincibility))
                {
                    if (rows.Count == 2)
                    {
                        if (rows[0][1] == 2 && rows[1][1] == 2)
                        {
                            HitCrackedObstacle(game, mapSequence, 1, calculatedRow);
                            HitCrackedObstacle(game, mapSequence, 1, calculatedRow - 1);
                            return false;
                        }
                        else if (rows[0][1] == 2)
                        {
                            HitCrackedObstacle(game, mapSequence, 1, calculatedRow);
                            return false;
                        }
                        else if (rows[1][1] == 2)
                        {
                            HitCrackedObstacle(game, mapSequence, 1, calculatedRow - 1);
                            return false;
                        }

                        else if (rows[0][1] == 1 || rows[1][1] == 1)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (rows[0][1] == 2)
                        {
                            HitCrackedObstacle(game, mapSequence, 1, calculatedRow);
                            return false;
                        }

                        else if (rows[0][1] == 1)
                        {
                            return false;
                        }
                    }
                }
                List<int> rowsToCheck = crossesTwoRows ? new List<int>() { calculatedRow, calculatedRow - 1 } : new List<int>() { calculatedRow };
                game.CheckForEvent(mapSequence, 1, rowsToCheck);
            }
            return true;    // returns true if no obstacles will be hit by the player when they enter the new column
        }
        public bool CanMoveRight(Game game)
        {
            if (IsStunned())
            {
                return false;
            }

            Map map = game.CurrentMap;

            if (!(XPosition <= 872 && XPosition > 868) && !(XPosition <= 944 && XPosition > 940))          // 850 = mapXPos   72 = lengthOfSquare     4 = FastSpeed           // 922 = 850 + 72        872 = 922 - 50    868 = 872 - 4          // 994 = 850 + (2 * 72)        944 = 994 - 50      940 = 944 - 4   
            {
                return true;                            // The player is not entering another column
            }

            if (map.MapSequences.Count == 1)
            {
                return true;
            }                                      // The player will always be able to move in the start sequence since it is only empty squares. 

            int row = (YPosition - map.MapSequences.Last().YPosition) / 72;
            bool crossesTwoRows = !(((YPosition - map.MapSequences.Last().YPosition) % 72) <= 22);    // 22 = (length of square) - (length of player)
            bool twoSequences = !(map.MapSequences[map.MapSequences.Count - 2].YPosition > YPosition + 50);  // oneSequence means there is one map sequence above the bottom of the player
            List<int[]> rows = new List<int[]>() { };
            //if ((map.MapSequences.Last().Map[x, row].ItemName == ItemName.Obstacle) || (map.MapSequences.Last().Map[x, row].ItemName == ItemName.CrackedObstacle))
            int calculatedRow = 20 - (row + 1); // where 20 is the size of a map sequence 

            int[,] newMap = new int[3, 20];
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    int value;
                    switch (map.MapSequences[map.MapSequences.Count - 1].Map[x, y].ItemName)
                    {
                        case (Items.Obstacle):
                            value = 1;
                            break;
                        case (Items.CrackedObstacle):
                            value = 2;
                            break;
                        default:
                            value = 0;
                            break;
                    }
                    newMap[x, y] = value;
                }
            }

            if (twoSequences)
            {
                calculatedRow = 40 - (row + 1);

                newMap = new int[3, 40];         // 40 = 2 * size of map sequence            size of map sequence = 20
                for (int i = 0; i <= 1; i++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        for (int y = 0; y < 20; y++)
                        {
                            int value;
                            switch (map.MapSequences[map.MapSequences.Count - (2 - i)].Map[x, y].ItemName)
                            {
                                case (Items.Obstacle):
                                    value = 1;
                                    break;
                                case (Items.CrackedObstacle):
                                    value = 2;
                                    break;
                                default:
                                    value = 0;
                                    break;
                            }
                            newMap[x, y + (i * 20)] = value;
                        }
                    }
                }
            }

            rows.Add(new int[3] { newMap[0, calculatedRow], newMap[1, calculatedRow], newMap[2, calculatedRow] });

            if (crossesTwoRows)
            {
                rows.Add(new int[3] { newMap[0, calculatedRow - 1], newMap[1, calculatedRow - 1], newMap[2, calculatedRow - 1] });
            }

            MapSequence mapSequence;

            if (twoSequences)
            {
                Item[,] itemSequence = new Item[3, 40];

                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 40; y++)
                    {
                        itemSequence[x, y] = map.MapSequences[map.MapSequences.Count + (y / 20) - 2].Map[x, y - (20 * (y / 20))];
                    }
                }

                mapSequence = new MapSequence(itemSequence, map.MapSequences[map.MapSequences.Count - rows.Count].StartSquare, map.MapSequences.Last().EndSquare, map.MapSequences.Last().YPosition);
            }
            else
            {
                mapSequence = map.MapSequences.Last();
            }

            // 850 is the left most x position of the map
            // 72 is the length of a square
            // 50 is the length of the player
            // 4 is how much the player moves left and right each time

            if (XPosition <= 872 && XPosition > 868)        // 922 = 850 + 72        872 = 922 - 50    868 = 872 - 4
            {
                if (!IsPowerUpActive(Items.Invincibility))
                {
                    if (rows.Count == 2)
                    {
                        if (rows[0][1] == 2 && rows[1][1] == 2)
                        {
                            HitCrackedObstacle(game, mapSequence, 1, calculatedRow);
                            HitCrackedObstacle(game, mapSequence, 1, calculatedRow - 1);
                            return false;
                        }
                        else if (rows[0][1] == 2)
                        {
                            HitCrackedObstacle(game, mapSequence, 1, calculatedRow);
                            return false;
                        }
                        else if (rows[1][1] == 2)
                        {
                            HitCrackedObstacle(game, mapSequence, 1, calculatedRow - 1);
                            return false;
                        }

                        else if (rows[0][1] == 1 || rows[1][1] == 1)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (rows[0][1] == 2)
                        {
                            HitCrackedObstacle(game, mapSequence, 1, calculatedRow);
                            return false;
                        }

                        else if (rows[0][1] == 1)
                        {
                            return false;
                        }
                    }
                }
                List<int> rowsToCheck = crossesTwoRows ? new List<int>() { calculatedRow, calculatedRow - 1 } : new List<int>() { calculatedRow };
                game.CheckForEvent(mapSequence, 1, rowsToCheck);
            }
            else if (XPosition <= 944 && XPosition > 940)       // 994 = 850 + (2 * 72)        944 = 994 - 50      940 = 944 - 4   
            {
                if (!IsPowerUpActive(Items.Invincibility))
                {
                    if (rows.Count == 2)
                    {
                        if (rows[0][2] == 2 && rows[1][2] == 2)
                        {
                            HitCrackedObstacle(game, mapSequence, 2, calculatedRow);
                            HitCrackedObstacle(game, mapSequence, 2, calculatedRow - 1);
                            return false;
                        }
                        else if (rows[0][2] == 2)
                        {
                            HitCrackedObstacle(game, mapSequence, 2, calculatedRow);
                            return false;
                        }
                        else if (rows[1][2] == 2)
                        {
                            HitCrackedObstacle(game, mapSequence, 2, calculatedRow - 1);
                            return false;
                        }

                        else if (rows[0][2] == 1 || rows[1][2] == 1)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (rows[0][2] == 2)
                        {
                            HitCrackedObstacle(game, mapSequence, 2, calculatedRow);
                            return false;
                        }

                        else if (rows[0][2] == 1)
                        {
                            return false;
                        }
                    }
                }
                List<int> rowsToCheck = crossesTwoRows ? new List<int>() { calculatedRow, calculatedRow - 1 } : new List<int>() { calculatedRow };
                game.CheckForEvent(mapSequence, 2, rowsToCheck);
            }
            return true;
            // mostly same as the CanMoveLeft but some differences due to the fact the player is moving right instead of left
            // some variations between the methods may come from the fact that the x coordinates of the player and map are treated as being the left most point of that thing
        }
        public void MoveForward(Game game, PictureBox chaserImg) 
        {
            if (CanMoveForward(game))
            {
                int speed = FastSpeedActive ? FastSpeed : BaseSpeed;     // speedOfMap will be replaced with the fast speed of the Player at that point in time of the game.
                foreach (KeyValuePair<int, PictureBox> pictureID in game.CurrentMap.DisplayedMap)
                {
                    PictureBox picture = pictureID.Value;
                    picture.Location = new Point(picture.Location.X, picture.Location.Y + speed);

                    if (picture.Location.Y > 1200)    
                    {
                        picture.Dispose();
                    }
                } // Moves each picture box in the displayed map down on the screen to give the illusion that the player is moving forwards
                chaserImg.Location = new Point(game.CurrentChaser.XPosition, game.CurrentChaser.YPosition + speed); // the chaser also moves down away from the player as the player is moving forwards
                game.CurrentChaser.YPosition += speed;

                foreach (MapSequence mapSequence in game.CurrentMap.MapSequences)
                {
                    mapSequence.YPosition += speed;
                }  // updates the y position of each map sequence 
                AddScore(game);   // gives the player score since they moved forwards
            }
        }
        public bool CanMoveForward(Game game)
        {
            if (IsStunned())
            {
                StunnedDuration--;
                return false;
            }
            Map currentMap = game.CurrentMap;
            MapSequence playersMapSequence;
            if (currentMap.MapSequences.Count > 1) 
            {
                int mapYPos = currentMap.MapSequences.Last().YPosition;
                if ((YPosition - mapYPos) % 72 < 4 && (YPosition - mapYPos) % 72 > -4) // Check if the player is entering a new row.    // 20 = sizeOfMapSequence, 72 = length of square, 4 = FastSpeed
                { 
                    playersMapSequence =  currentMap.MapSequences[(currentMap.MapSequences.Count - 1) - (((YPosition - 5) - mapYPos) / 1440)];       // 1440 = 72 * 20 = lengthOfSquare * sizeOfMapSequence    - 5 because the player will be just below the row they are about to enter (are not in yet)
                }     // determines the map sequence of the row the player is entering
                else
                {
                    return true;
                }
            }
            else        // if there is only the first map sequence then return true as it is full of empty squares
            {
                return true;
            }

            int numRows = (YPosition - playersMapSequence.YPosition) / 72; // the number of rows between the top of the player and the top of the newest map sequence        // 72 == lengthOfSquare

            int currentRow = 20 - numRows;           // 20 = sizeOfMapSequence

            Item firstSquare = playersMapSequence.Map[0, currentRow];                                 
            Item secondSquare = playersMapSequence.Map[1, currentRow];
            Item thirdSquare = playersMapSequence.Map[2, currentRow];
            Item[] items = new Item[3] {firstSquare, secondSquare, thirdSquare};
            // the items of the row the player is entering

            if (XPosition <= StartXPosition - Size)       // player is in left most column only           
            {
                if (!IsPowerUpActive(Items.Invincibility))
                {
                    if (firstSquare.ItemName == Items.CrackedObstacle)
                    {
                        HitCrackedObstacle(game, playersMapSequence, 0, currentRow);
                        return false;
                    }
                    else if (firstSquare.ItemName == Items.Obstacle)
                    {
                        return false;
                    }
                } // checks if the player will hit an obstacle and carries out the relevant effect
                game.CheckForEvent(playersMapSequence, 0, currentRow);
                // if the player is not going to hit an obstacle, see if it will hit a coin / power-up
            }
            else if (XPosition <= StartXPosition - ((72 - Size)/2))         // player is in left and middle column       
            {
                if (!IsPowerUpActive(Items.Invincibility))
                {
                    if (firstSquare.ItemName == Items.CrackedObstacle && secondSquare.ItemName == Items.CrackedObstacle)
                    {
                        HitCrackedObstacle(game, playersMapSequence, 0, currentRow);
                        HitCrackedObstacle(game, playersMapSequence, 1, currentRow);
                        return false;
                    }
                    else if (firstSquare.ItemName == Items.CrackedObstacle)
                    {
                        HitCrackedObstacle(game, playersMapSequence, 0, currentRow);
                        return false;
                    }
                    else if (secondSquare.ItemName == Items.CrackedObstacle)
                    {
                        HitCrackedObstacle(game, playersMapSequence, 1, currentRow);
                        return false;
                    }
                    else if (firstSquare.ItemName == Items.Obstacle || secondSquare.ItemName == Items.Obstacle)
                    {
                        return false;
                    }
                }
                game.CheckForEvent(playersMapSequence, new List<int>() { 0, 1 }, currentRow);
            }
            else if (XPosition <= StartXPosition + ((72 - Size) / 2))    //player is in middle column
            {
                if (!IsPowerUpActive(Items.Invincibility))
                {
                    if (secondSquare.ItemName == Items.CrackedObstacle)
                    {
                        HitCrackedObstacle(game, playersMapSequence, 1, currentRow);
                        return false;
                    }
                    else if (secondSquare.ItemName == Items.Obstacle)
                    {
                        return false;
                    }
                }
                game.CheckForEvent(playersMapSequence, 1, currentRow);
            }
            else if (XPosition <= StartXPosition + (72 - ((72 - Size) / 2)))     // player is in middle and right column
            {
                if (!IsPowerUpActive(Items.Invincibility))
                {
                    if (secondSquare.ItemName == Items.CrackedObstacle && thirdSquare.ItemName == Items.CrackedObstacle)
                    {
                        HitCrackedObstacle(game, playersMapSequence, 1, currentRow);
                        HitCrackedObstacle(game, playersMapSequence, 2, currentRow);
                        return false;
                    }
                    else if (secondSquare.ItemName == Items.CrackedObstacle)
                    {
                        HitCrackedObstacle(game, playersMapSequence, 1, currentRow);
                        return false;
                    }
                    else if (thirdSquare.ItemName == Items.CrackedObstacle)
                    {
                        HitCrackedObstacle(game, playersMapSequence, 2, currentRow);
                        return false;
                    }
                    else if (secondSquare.ItemName == Items.Obstacle || thirdSquare.ItemName == Items.Obstacle)
                    {
                        return false;
                    }
                }
                game.CheckForEvent(playersMapSequence, new List<int>() { 1, 2 }, currentRow);
            }
            else         // player is in right column
            {
                if (!IsPowerUpActive(Items.Invincibility))
                {
                    if (thirdSquare.ItemName == Items.CrackedObstacle)
                    {
                        HitCrackedObstacle(game, playersMapSequence, 2, currentRow);
                        return false;
                    }
                    else if (thirdSquare.ItemName == Items.Obstacle)
                    {
                        return false;
                    }
                }
                game.CheckForEvent(playersMapSequence, 2, currentRow);
            }

            foreach (Item item in items)
            {
                if ((item.ItemName == Items.Obstacle || item.ItemName == Items.CrackedObstacle) && !AlreadyPassedObstacles.Contains(item.ID))
                {
                    ObstaclesPassed++;            // at this point it is known that the player won't hit any obstacles so this statistic of the player is updated
                    AlreadyPassedObstacles.Add(item.ID);   // to make sure it doesn't count obstacles the player has already passed
                }
            }

            return true;
        }
        public void AddPowerUp(Form1 form, PowerUp powerUp, int id) 
        {
            bool alreadyHasPowerUp = false;
            int index = -1;
            for (int i = 0; i < ActivePowerUps.Count; i++)
            {
                if (powerUp.ItemName == ActivePowerUps[i].ItemName)
                {
                    alreadyHasPowerUp = true; 
                    index = i;
                    break;
                }
            } // checks if the player already has the power-up active
            if (alreadyHasPowerUp)
            {
                ActivePowerUps[index].Duration = powerUp.Duration; //reset the duration of the power-up
            }
            else
            {
                ActivePowerUps.Add(new PowerUp(powerUp.ImageFileLocation, powerUp.ItemName, id)); // give the player the power-up
                PictureBox powerUpImage = new PictureBox
                {
                    Size = new Size(72, 72),        // 72 = lengthOfSquare
                    BackgroundImageLayout = ImageLayout.Zoom,
                    BackgroundImage = Image.FromFile(powerUp.ImageFileLocation),
                    Name = powerUp.ItemName.ToString()
                };
                PowerUpImages.Add(powerUpImage);
                form.Controls.Add(powerUpImage); // make the power-up a picture box and display it to the screen in the area which displays the players active power-ups
            }
        }
        public void CheckPowerUps(Form1 form)
        {
            for (int i = 0; i < ActivePowerUps.Count; i++)
            {
                PowerUp powerUp = ActivePowerUps[i];
                powerUp.Duration--;
                if (!powerUp.IsActive())
                {
                    RemovePowerUp(form, powerUp);
                    i--;
                }
            }         // loop through each power-up the player currently has active, for each one: decrease the power-ups duration and check if the power-up has run out, if it has remove it
            if (ActivePowerUps.Count >= 1)
            {
                FormatActivePowerUps(); // format the display on the screen
            }
        }
        public void RemovePowerUp(Form1 form, PowerUp powerUp)
        {
            ActivePowerUps.Remove(powerUp); // remove it from the players list
            int index = -1;
            for (int i = 0; i < PowerUpImages.Count; i++)
            {
                if (PowerUpImages[i].Name == powerUp.ItemName.ToString())
                {
                    index = i; 
                    break;
                }
            }
            form.Controls.Remove(PowerUpImages[index]);
            PowerUpImages.RemoveAt(index);                 // find the index of the PictureBox in the PowerUpImages list that corresponds to the power-up that needs removing and removes it from the screen and PowerUpImages list
        }
        public void FormatActivePowerUps()
        {
            for (int i = 0; i < PowerUpImages.Count;  i++)
            {
                PowerUpImages[i].Location = new Point(1100, 100 + (73 * i));          // 73 = length of square + 1     length of square = 72         + 1 to give a gap between each power-up image 
            } // display the active power-ups in a column on the screen
        }
        public void AddScore(Game game) 
        {
            Form1 form = game.CurrentForm;
            bool doubleScoreActive = IsPowerUpActive(Items.DoubleScore);
            Score += game.Difficulty * (doubleScoreActive? 2 : 1);
            form.scoreLabel.Text = "Score: " + Score.ToString();
        }
        public void AddCoin(Form1 form) 
        {
            bool doubleCoinsActive = IsPowerUpActive(Items.DoubleCoins);
            Coins = Coins + (doubleCoinsActive? 2 : 1);
            form.coinCounterLabel.Text = "Coins: " + Coins.ToString();
        }
        public bool IsPowerUpActive(Items powerUpName)
        {
            foreach (PowerUp powerUp in ActivePowerUps)
            {
                if (powerUp.ItemName == powerUpName)
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsStunned()
        {
            return (StunnedDuration > 0);
        }
        public void HitCrackedObstacle(Game game, MapSequence sequence, int column, int row)
        {
            PreSetItems items = new PreSetItems();
            StunnedDuration = items.crackedObstacle.StunnedDuration;     
            game.DeleteItem(column, row, sequence);      // deletes the cracked obstacle from the map since it has been hit
            game.CurrentChaser.MoveSequenceNeedsUpdate = true;        // chaser needs a new move sequence since the map has changed so the fastest route may have changed
        }
        public void UpdateHighScore()
        {
            HighScore = Score > HighScore ? Score : HighScore;
        }
        public void ResetPlayer()
        {
            Score = 0;
            Coins = 0;
            ObstaclesPassed = 0;
            AlreadyPassedObstacles.Clear();
            PowerUpsPickedUp = 0;
            ActivePowerUps.Clear();
            FastSpeedActive = false;
            XPosition = StartXPosition;
            PowerUpImages.Clear();
            StunnedDuration = 0;
        }
    }

    public class Chaser : Character
    {
        public List<Moves> MoveSequence { get; set; }
        public int iterationsInMove { get; set; }
        public int iterationsLeftInMove { get; set; }
        public int InMapSequence { get; set; }
        public bool MoveSequenceNeedsUpdate { get; set; }
        public Chaser(int xPos, int yPos, string fileLoc, int size, int squaresAheadOfChaser, int relativeLengthOfSquare) : base (xPos, yPos, fileLoc, size)
        {
            BaseSpeed = CHASER_SPEED_VALUE;
            MoveSequence = new List<Moves>() { };
            for (int i = 0; i <= squaresAheadOfChaser; i++)
            {
                MoveSequence.Add(Moves.Forward);
            }     // so that the chaser can move through the start map sequence of each game
            iterationsInMove = relativeLengthOfSquare;
            iterationsLeftInMove = iterationsInMove;
            InMapSequence = 0;
            MoveSequenceNeedsUpdate = false;
        }

        public void PathFinding(MapSequence mapSequence) // start and end square can be 0, 1 or 2    // Maybe change name of this method to something more appropriate.
        {
            Item[,] itemMap = mapSequence.Map;

            int startSquare = mapSequence.StartSquare;
            int endSquare = mapSequence.EndSquare;

            int size = itemMap.Length / 3;

            List<int> squares = new List<int>();
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    switch (itemMap[x, y].ItemName)
                    {
                        case Items.Obstacle:
                            squares.Add(0);
                            break;
                        case Items.CrackedObstacle:
                            squares.Add(0);
                            break;
                        default:
                            squares.Add(1);
                            break;                    // Obstacles and cracked obstacles are the only things the chaser can't go through.
                    }
                }
            } // creating the list of integers which represents what squares the chaser can and cant go through with a 1 or 0 respectively, defining where the chaser starts and where it needs to go to

            List<int> shortestDistanceToSquare = new List<int>();
            for (int i = 0; i < squares.Count; i++)
            {
                shortestDistanceToSquare.Add(10000);
            }
            shortestDistanceToSquare[startSquare] = 0;

            List<int> previousSquare = new List<int>();
            for (int i = 0; i < squares.Count; i++)
            {
                previousSquare.Add(-1);     // -1 represents no previous square or null
            }

            List<List<int>> adjacencyList = new List<List<int>>();
            for (int i = 0; i < squares.Count; i++)        //creating the adjacency list for all the squares/nodes
            {
                adjacencyList.Add(new List<int>());
                if (squares[i] == 1)
                {
                    switch (i % 3)
                    {
                        case 0:  // left column
                            if (squares[i + 1] == 1)         // Is the square to the right of it not an obstacle
                            {
                                adjacencyList[i].Add(i + 1);
                            }
                            break;
                        case 1:  // middle column
                            if (squares[i - 1] == 1)         // Is the square to the left of it not an obstacle
                            {
                                adjacencyList[i].Add(i - 1);
                            }
                            if (squares[i + 1] == 1)         // Is the square to the right of it not an obstacle
                                {
                                adjacencyList[i].Add(i + 1);
                            }
                            break;
                        case 2:   // right column
                            if (squares[i - 1] == 1)         // Is the square to the left of it not an obstacle
                            {
                                adjacencyList[i].Add(i - 1);
                            }
                            break;
                    }
                    if (i + 3 < squares.Count)
                    {
                        if (squares[i + 3] == 1)          // Is the square in front of it not an obstacle
                        {
                            adjacencyList[i].Add(i + 3);
                        }
                    }
                }
            }    // heuristic = not connecting a square to the square behind it as that will never be the most efficient path forward
            // creating the adjacency list for each node 

            int currentSquare = startSquare;
            List<int> visitedSquares = new List<int>();
            List<int> queue = new List<int>() { currentSquare};
            while (queue.Any())
            {
                if (adjacencyList[currentSquare].Any())    //check if the adjacent squares have all beeen visited (if they have then you dont need to run this block of code)
                {
                    foreach (int square in adjacencyList[currentSquare])
                    {
                        if (!visitedSquares.Contains(square))
                        {
                            queue.Add(square);
                            if (shortestDistanceToSquare[currentSquare] + 1 < shortestDistanceToSquare[square]) // + 1 because the distance between any two adjacent squares is one
                            {
                                shortestDistanceToSquare[square] = shortestDistanceToSquare[currentSquare] + 1;
                                previousSquare[square] = currentSquare;
                            }
                        }
                    }
                }
                visitedSquares.Add(currentSquare);
                while (queue.Any())
                {
                    if (visitedSquares.Contains(queue[0]))
                    {
                        queue.RemoveAt(0);
                    }
                    else
                    {
                        currentSquare = queue[0];    // The currentSquare is the next node in the queue.
                        break;
                    }
                }
            } // preforms a breadth first traversal on the 2D array starting from the start square
            // updating the shortest path and previous square to each square and marking squares as visited when necessary until all squares have been visited

            endSquare = squares.Count - (3 - endSquare);  //edit the value of endSquare to correspond to the correct position in the grid of squares
            List<Moves> moves = new List<Moves>();
            int tempSquare = endSquare;
            int counter = 0;

            while (tempSquare != startSquare)
            {
                counter++;
                switch (tempSquare - previousSquare[tempSquare])
                {
                    case 3:
                        moves.Add(Moves.Forward);
                        break;
                    case 1:
                        moves.Add(Moves.Right);
                        break;
                    case -1:
                        moves.Add(Moves.Left);
                        break;
                }
                tempSquare = previousSquare[tempSquare];
            } // working backwards by looking at the previous square to each square from the end square to the start square and for each one adding the move to a list of moves that represents the move the chaser needs to take to follow the shortest path

            moves.Reverse(); // because it started from the end square originally

            foreach (Moves move in moves)
            {
                MoveSequence.Add(move); // so that the chaser can move between map sequences
            }
            MoveSequence.Add(Moves.Forward);
        }


        public void Move(Game game, PictureBox chaserImg)
        {
            switch (MoveSequence[0])
            {
                case Moves.Forward:
                    if (YPosition - game.CurrentPlayer.YPosition < 4)       // the chaser is basically horizontally alligned with the player
                    {
                        List<MapSequence> sequences = game.CurrentMap.MapSequences;
                        int rowsAheadOfChaser = (YPosition - sequences.Last().YPosition) / 72;// 850 = mapXPos      72 = lengthOfSquare
                        int chaserColumn = (XPosition - 850) / 72;           

                        int criticalDistanceBetweenChaserAndPlayer = 40;      
                        int playerXPos = game.CurrentPlayer.XPosition;
                        if (XPosition - playerXPos < criticalDistanceBetweenChaserAndPlayer + game.CurrentPlayer.Size && XPosition > playerXPos)  // Chaser is to the right of the player.
                        {
                            MoveSequence[0] = Moves.Left;
                            iterationsLeftInMove = iterationsLeftInMove;                           
                        }
                        else if (playerXPos - XPosition < criticalDistanceBetweenChaserAndPlayer + Size && playerXPos > XPosition)  // Chaser is to the left of the player.
                        {
                            MoveSequence[0] = Moves.Right;
                            iterationsLeftInMove = iterationsLeftInMove;
                        }
                        // The use of both the chasers size and players size is because the XPosition of the character is measure from the left most point of the character/PictureBox.

                        return;
                        // checks if the chaser is adjacent to the player and is close enough to move towards the player and catch them before the player moves forward too much
                    }
                    chaserImg.Location = new Point(chaserImg.Location.X, chaserImg.Location.Y - BaseSpeed);
                    YPosition -= BaseSpeed;
                    break;
                case Moves.Left:
                    chaserImg.Location = new Point(chaserImg.Location.X - BaseSpeed, chaserImg.Location.Y);
                    XPosition -= BaseSpeed;
                    break;
                case Moves.Right:
                    chaserImg.Location = new Point(chaserImg.Location.X + BaseSpeed, chaserImg.Location.Y);
                    XPosition += BaseSpeed;
                    break;
            } // if the chaser is not horizontally alligned with the player, then move the chaser as usual updating the chasers image location and chasers x and y positions

            iterationsLeftInMove--;

            if (iterationsLeftInMove == 0)
            {
                MoveSequence.RemoveAt(0);
                iterationsLeftInMove = iterationsInMove;

                if (MoveSequenceNeedsUpdate)
                {
                    MoveSequence.Clear();
                    PathFinding(game.GetMapAheadOfChaser());
                    MoveSequenceNeedsUpdate = false;
                }
            }
            // iterationsInMove is how many little moves the chaser needs to make before it completes a move
            // once iterationsLeftInMove reaches 0, the chaser has finished its move so that move is deleted from its sequence and the iterationsLeftInMove is reset to the start value of iterationsInMove since it is starting another move
            // if the chasers move sequence needs an update, its current move sequence is cleared, the map that the chaser needs moves for is fetched and the move sequence calculated
        }
    }
}
