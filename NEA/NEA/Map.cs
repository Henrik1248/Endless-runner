using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEA
{
    public class Map
    {
        public List<MapSequence> MapSequences { get; set; }
        public List<MapSequence> MapSequencesToAdd { get; set; }  // These are the sequences that are loaded an need to be added to MapSequences when necessary
        public Dictionary<int, PictureBox> DisplayedMap { get; set; }
        public int IDCount { get; set; }  // to ensure unique IDs for each item
        public int SequencesLeftToShow { get; set; }  // from the loaded map sequences
        public List<List<int>> MapToSave{ get; set; }       // this will have three list of integers each representing a column of the map which will be converted to a 2D integer array and stored in a text file when necessary

        // Key:
        // 0 = empty square
        // 1 = regular obstacle
        // 2 = cracked obstacle
        // 3 = double coins
        // 4 = double score
        // 5 = invincibility
        // 6 = coin

        public Map()
        {
            MapSequences = new List<MapSequence>();
            MapSequencesToAdd = new List<MapSequence>();
            DisplayedMap = new Dictionary<int, PictureBox> { };
            IDCount = 1;
            SequencesLeftToShow = 0;
            MapToSave = new List<List<int>>();       
            for (int x = 0; x < 3; x++)
            {
                MapToSave.Add(new List<int>());
            }
        }
        public Map(List<MapSequence> mapSequences)
        {
            MapSequences = new List<MapSequence>();
            MapSequencesToAdd = mapSequences;
            DisplayedMap = new Dictionary<int, PictureBox> { };
            IDCount = MapSequencesToAdd.Last().Map[2,19].ID + 1;          // 19 = sizeOfMapSequence - 1
            SequencesLeftToShow = mapSequences.Count - 1;
            MapToSave = new List<List<int>>();      
            for (int x = 0; x < 3; x++)
            {
                MapToSave.Add(new List<int>());
            }
            AdjustMapToSave(mapSequences);       // adds the loaded map sequences to the MapToSave
        }
        public MapSequence CreateMapSequence(int size, int startSquare, int difficulty, int yPos)      //Should return a MapSequence object not a 2D item array.
        {
            //make adjustments to this to make it consider difficulty as well as 
            //not allowing cracked obstacles to spawn directly behind regular obstacles
            //not creating routes where the player can get trapped //no dead ends (player can get out through a cracked obstacle)
            //no impossible to get power-ups

            int[,] tempMap = new int[3, size];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    tempMap[i, j] = 0;
                }
            } //creates a 2D integer of all 0s (setting up a blank map sequence)

            tempMap[startSquare, 0] = 1;

            int currentX = startSquare;
            int currentY = 0;             // representing the x and y positions of the map that the algorithm is using to create a path through the map

            string currentMove = "";
            string previousMove = "";

            List<int> xCordsInPath = new List<int>();
            List<int> yCordsInPath = new List<int>();

            bool validMove = false;

            Random randomNum = new Random();

            while (currentY != size - 1)     // until it is at the last row of the map sequence 
            {
                validMove = false;
                while (!validMove)
                {
                    int move = randomNum.Next(1, 4);
                    switch (move)
                    {
                        case 1:
                            currentMove = "LEFT";     //could replace the move strings with the values from the moves enum you created
                            break;
                        case 2:
                            currentMove = "FORWARD";
                            break;
                        case 3:
                            currentMove = "RIGHT";
                            break;
                    } // randomly adds a direction
                    if ((currentMove == "LEFT" && previousMove != "RIGHT") && currentX != 0)
                    {
                        validMove = true;
                        previousMove = currentMove;
                    }
                    else if ((currentMove == "RIGHT" && previousMove != "LEFT") && currentX != 2)
                    {
                        validMove = true;
                        previousMove = currentMove;
                    }
                    else if (currentMove == "FORWARD")
                    {
                        validMove = true;
                        previousMove = currentMove;
                    }
                    else
                    {
                        validMove = false;
                    } // checks if the move is valid (it is not going to the same place it was already at)
                }
                switch (currentMove)
                {
                    case "LEFT":
                        currentX -= 1;
                        break;
                    case "RIGHT":
                        currentX += 1;
                        break;
                    case "FORWARD":
                        currentY += 1;
                        break;
                } // updating the position in the map
                xCordsInPath.Add(currentX);
                yCordsInPath.Add(currentY); // creating a path through the map so there is always a route for the player to take since the path will never contain obstacles of any kind
                tempMap[currentX, currentY] = 1; // 1 = empty (a path for the player)
            }

            int numberOfSquaresNotInPath = 0; // This number will represent how many squares are available for an obstacle to be put in them.

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (tempMap[x, y] == 0)
                    {
                        numberOfSquaresNotInPath++;
                    }
                }
            }

            int numberOfObstacles = (difficulty * numberOfSquaresNotInPath) / 100;
            // Calculating the number of squares in the map sequence that should be obstacles

            int xVal = 0;
            int yVal = 0;

            int counterX = 0;
            int counterY = 0;

            while (numberOfObstacles > 0)
            {
                xVal = counterX % 3;
                yVal = counterY % size;

                if (tempMap[xVal, yVal] == 0 )  // meaning that square is not part of the path
                {
                    int isEmpty = randomNum.Next(1,3); // 1 - 2    1 = no obstacle/empty square, 2 = obstacle
                    int crackedObstacle = randomNum.Next(1, 4); // 1 - 3    1 = cracked obstacle, (2, 3) = obstacle
                    tempMap[xVal, yVal] = isEmpty == 1 ? 0 : (crackedObstacle == 1 ? 3 : 2);   // setting the square
                    if (!(isEmpty == 1))
                    {
                        counterX++;
                        counterY++;
                        numberOfObstacles--;
                    }
                }
                
                counterX++; 
                counterY++;
            }  // while there are still obstacles to be added, it goes through each square in the map repeatedly, checks if it is not part of the map, if it isn't then it will randomly be set to either an empty square (0), an obstacle (2) or a cracked obstacle (3) until all obstacles have been added

            bool hasPowerUp = false;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    if (tempMap[x, y] == 0)
                    {
                        int item = randomNum.Next(1, 101);     
                        if (item < 10 && !hasPowerUp)
                        {
                            tempMap[x, y] = 4;   // 4 = power-up
                            hasPowerUp = true;     // ensures there is only one power-up per map sequence so there aren't too many
                        }
                        else
                        {
                            tempMap[x, y] = 1;   // 1 = empty square
                        }
                    }
                }
            }
            
            //randomly sets each empty square to either a power-up (unlikey) or an empty square

            for (int y = 1; y < (size - 1); y++)
            {
                if (tempMap[0, y] == 2 && tempMap[1, y - 1] == 2 && tempMap[0, y - 1] != 2)
                {
                    tempMap[1, y - 1] = randomNum.Next(1, 3) == 1 ? 1 : 3;         // 1 - 2     1 = empty square, 2 = cracked obstacle
                }
                if (tempMap[2, y] == 2 && tempMap[1, y - 1] == 2 && tempMap[2, y - 1] != 2)
                {
                    tempMap[1, y - 1] = randomNum.Next(1, 3) == 1 ? 1 : 3;         // 1 - 2     1 = empty square, 2 = cracked obstacle
                }
            }       // this ensures there are no places in the map where the player can get stuck




            // 1 = empty square
            // 2 = regular obstacle
            // 3 = cracked obstacle
            // 4 = power-up

            for (int i = 0; i < 3; i++)
            {
                if (!(i == currentX))
                {
                    tempMap[i, size - 1] = 2;
                }
            }          // This makes sure that the two squares in the last row that are not the end square are always regular obstacles, this helps with the chasers pathfinding algorithm to ensure it doesn't take an inefficient route.

            PreSetItems items = new PreSetItems();

            Item[,] map = new Item[3, size];

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    switch (tempMap[x, y])
                    {
                        case 1:
                            map[x, y] = new Item(items.emptySquare.ImageFileLocation, items.emptySquare.ItemName, items.emptySquare.ID);
                            break;
                        case 2:
                            map[x, y] = new Obstacle(items.obstacle.ImageFileLocation, items.obstacle.ItemName, items.obstacle.ID, items.obstacle.IsCrackedObstacle);
                            break;
                        case 3:
                            map[x, y] = new Obstacle(items.crackedObstacle.ImageFileLocation, items.crackedObstacle.ItemName, items.crackedObstacle.ID, items.crackedObstacle.IsCrackedObstacle);
                            break;
                        case 4:
                            int num = randomNum.Next(0, 3);
                            PowerUp[] powerUps = new PowerUp[3] { items.doubleCoins, items.doubleScore, items.invincibility };
                            map[x, y] = powerUps[num];   // randomly selecting a power-up
                            break;
                    }
                }
            } // converting the 2D integer array into a 2D item array by changing each integer into its corresponding item

            

            int startCoin = randomNum.Next(0, xCordsInPath.Count - 8);
            int amountOfCoins = randomNum.Next(4, 9);
                
            for (int i = startCoin; i <= startCoin + amountOfCoins; i++)
            {
                map[xCordsInPath[i], yCordsInPath[i]] = new Item(items.coin.ImageFileLocation, items.coin.ItemName, items.coin.ID);
            }  // makes a random number (betweeen 4 and 8) of adjacent squares in the path become coins

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    map[x, y].ID = IDCount;
                    IDCount++;
                }
            } // gives each item a unique ID so that it can be identfied and altered later if required

            MapSequence returnMap = new MapSequence(map, startSquare, currentX, yPos);

            MapSequences.Add(returnMap);

            AdjustMapToSave(new List<MapSequence>() { returnMap }); // adds the newly created map to the MapToSave (as soon as the map sequence is created so that its original state with all its coins and power-ups not collected is captured and stored)

            return returnMap;
        }
        public void DeleteItemFromMap(int id)
        {
            DisplayedMap[id].BackgroundImage = Image.FromFile(@"Images\Square.png");
            // uses the unique ID of the item to change the DisplayedMaps picture box of that item to show the picture of an empty square
        }
        public void AdjustMapToSave(List<MapSequence> map)
        {
            foreach (MapSequence sequence in map)
            {
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < sequence.Map.Length / 3; y++)
                    {
                        switch (sequence.Map[x, y].ItemName) 
                        {
                            case Items.EmptySquare:
                                MapToSave[x].Add(0);
                                break;
                            case Items.Coin:
                                MapToSave[x].Add(1);
                                break;
                            case Items.Obstacle:
                                MapToSave[x].Add(2);
                                break;
                            case Items.CrackedObstacle:
                                MapToSave[x].Add(3);
                                break;
                            case Items.DoubleCoins:
                                MapToSave[x].Add(4);
                                break;
                            case Items.DoubleScore:
                                MapToSave[x].Add(5);
                                break;
                            case Items.Invincibility:
                                MapToSave[x].Add(6);
                                break;
                        }
                    }
                }
            }  

            // loops over each map sequence and loops over each item in the item array of the current sequence 
            // it then adds that items corresponding integer to the MapToSave in the relevant location

            // Key:
            // 0 = empty square
            // 1 = coin
            // 2 = regular obstacle
            // 3 = cracked obstacle
            // 4 = double coins
            // 5 = double score
            // 6 = invincibility
        }
        public int[,] GetMapToSave()
        {
            int ySize = MapToSave[0].Count;
            int[,] returnMap = new int[3, ySize];

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    returnMap[x, y] = MapToSave[x][y];
                }
            }       // Converts MapToSave into a 2D integer array to be stored

            return returnMap;
        }
    }
}
