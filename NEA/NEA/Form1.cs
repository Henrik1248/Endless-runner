using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEA
{
    public enum Moves
    {
        Left,
        Right, 
        Forward
    }

    public enum PowerUps
    {
        Invincibility,
        DoubleCoins,
        DoubleScore
    }
    public enum Items
    {
        EmptySquare,
        Obstacle,
        CrackedObstacle,
        Coin,
        DoubleCoins,
        DoubleScore,
        Invincibility
    }
    public enum Menus
    {
        Main,
        EndGame
    }

    public partial class Form1 : Form
    {
        static Menus previousMenu = Menus.Main;  // To determine which menu the game should go back to if the back button is clicked
        static bool savingMap;       // True if a map is being saved and false if a map is being loaded. This is so the game knows whether to load or save a map when a slot button is pressed

        static int mapXPos = 850;
        static int sizeOfMapSequence = 20;          // Must be at least 8 as that is the most amount of coins that can be in a map sequence.
        static int relativeLengthOfSquare = 24;
        static int lengthOfSquare = (relativeLengthOfSquare) * 3;    // The value in the brackets can be changed, the length of square must be a multiple of 3 (chaser speed value) so that the chasers movement is consistent.
        static int lengthOfPlayer = 50;
        static int lengthOfChaser = 50;
        static int startPlayerXPos = mapXPos + lengthOfSquare + ((lengthOfSquare - lengthOfPlayer) / 2);
        static int startPlayerYPos = 450;
        static int startChaserXPos = mapXPos + lengthOfSquare + ((lengthOfSquare - lengthOfChaser) / 2);
        static int startChaserYPos = startPlayerYPos + 200;
        static int squaresAheadOfChaser = 19;
        static int startMapYPos = startChaserYPos - (lengthOfSquare * squaresAheadOfChaser);


        static Player player = new Player(startPlayerXPos, startPlayerYPos, Path.Combine(Application.StartupPath, "Images", "Player Character.png"), lengthOfPlayer);
        PictureBox playerImg;

        static Chaser chaser = new Chaser(startChaserXPos, startChaserYPos, Path.Combine(Application.StartupPath, "Images", "Alien.png"), lengthOfChaser, squaresAheadOfChaser, relativeLengthOfSquare);
        PictureBox chaserImg;

        string backgroundImgFileLocation = Path.Combine(Application.StartupPath, "Images", "Square.png");   // (just a larger version of the empty square image)    // To remove the white spaces between the squares when the map is moving.
        PictureBox backgroundImg = new PictureBox();

        static Map currentMap = new Map();
        static Game game;

        public Form1()
        {
            InitializeComponent();

            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            gameTimer.Stop();

            player.HighScore = GetHighScore();

            backgroundImg = new PictureBox()
            {
                Size = new Size(216, 1500),
                BackgroundImageLayout = ImageLayout.Stretch,
                BackgroundImage = Image.FromFile(backgroundImgFileLocation),
                Location = new Point(850, 0)
            };

            Controls.Add(backgroundImg);
            backgroundImg.SendToBack();

            game = new Game(player, chaser, currentMap, this);

            playerImg = new PictureBox
            {
                Size = new Size(lengthOfPlayer, lengthOfPlayer),
                BackgroundImageLayout = ImageLayout.Zoom,
                BackgroundImage = Image.FromFile(player.ImgFileLoc),
                Location = new Point(player.XPosition, player.YPosition)
            };

            chaserImg = new PictureBox
            {
                Size = new Size(lengthOfChaser, lengthOfChaser),
                BackgroundImageLayout = ImageLayout.Zoom,
                BackgroundImage = Image.FromFile(chaser.ImgFileLoc),
                Location = new Point(chaser.XPosition, chaser.YPosition)
            };

            Controls.Add(playerImg);
            playerImg.BringToFront();
            Controls.Add(chaserImg);
            chaserImg.BringToFront();       // Setting up the player and chaser pictureBoxes

            ShowMainMenu();
        }
        public void CreateStartMap()                          
        {
            int startSequenceSize = sizeOfMapSequence;
            PreSetItems items = new PreSetItems();
            Item[,] startMap = new Item[3, startSequenceSize];
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < startSequenceSize; y++)
                {
                    startMap[x, y] = new Item(items.emptySquare.ImageFileLocation, items.emptySquare.ItemName, items.emptySquare.ID);
                    startMap[x, y].ID = currentMap.IDCount;
                    currentMap.IDCount++;
                }
            }      // creating a map sequence of empty squares to give the player some time to prepare for the game

            MapSequence startSequence = new MapSequence(startMap, 1, 1, startMapYPos);
            currentMap.MapSequences.Add(startSequence);

            ShowMap(startSequence);
        }
        public void CreateMap()
        {
            int startSquare = currentMap.MapSequences.Last().EndSquare;     // This is the end square of the last sequence. so that the sequences connect properly
            int yPos = currentMap.MapSequences.Last().YPosition - (sizeOfMapSequence * lengthOfSquare);
            MapSequence map = currentMap.CreateMapSequence(sizeOfMapSequence, startSquare, game.Difficulty, yPos);
            ShowMap(map);
            chaser.PathFinding(map);
            game.UpdateDifficulty();

            //creating and showing the map and and applying the chasers pathfinding to the new map so it knows where to go
        }
        public void ShowMap(MapSequence mapSequence)
        {
            Item[,] map = mapSequence.Map;
            int startY = mapSequence.YPosition;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < sizeOfMapSequence; y++)
                {
                    PictureBox square = new PictureBox();
                    square.Size = new Size(lengthOfSquare, lengthOfSquare);
                    square.BackgroundImageLayout = ImageLayout.Zoom;
                    square.BackgroundImage = Image.FromFile(map[x, y].ImageFileLocation);

                    int xCor = (lengthOfSquare * x) + mapXPos;
                    int yCor = startY + ((sizeOfMapSequence - 1 - y) * lengthOfSquare);
                    square.Location = new Point(xCor, yCor);

                    Controls.Add(square);
                    square.BringToFront();

                    currentMap.DisplayedMap.Add(map[x, y].ID, square);  //Adds it to a dictionary with a unique ID so it can be identified and altered lately if necessary.

                    square.Show();
                }
            }
            // loops over each item in the map sequence, creates a picture box for each one and then calculates its position on the screen then adds it to the controls
            playerImg.BringToFront();
            chaserImg.BringToFront();
        }
        public void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    player.FastSpeedActive = !player.FastSpeedActive;
                    break;
                case Keys.A:
                    player.MoveLeft(lengthOfSquare, playerImg, currentMap, game);
                    break;
                case Keys.D:
                    player.MoveRight(lengthOfSquare, playerImg, currentMap, game);
                    break;
            }
            //checks if the user had pressed/held down any movement keys and then makes the relevant movements
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            player.MoveForward(game, chaserImg);

            if (currentMap.MapSequences.Last().YPosition > 0)
            {
                if (currentMap.MapSequencesToAdd.Any())
                {
                    currentMap.MapSequences.Add(currentMap.MapSequencesToAdd[0]);
                    currentMap.MapSequencesToAdd.RemoveAt(0);
                    currentMap.MapSequences.Last().YPosition = (currentMap.MapSequences[currentMap.MapSequences.Count - 2].YPosition - (sizeOfMapSequence * lengthOfSquare));
                    ShowMap(currentMap.MapSequences.Last());
                    chaser.PathFinding(currentMap.MapSequences.Last());
                }
                else
                {
                    CreateMap();
                }
            }

            // if the top of the last map sequence is at the top of the screen (it is about to run out),
            // check if there are any mapSequences to add from a loaded map, if so add the next map sequence from there
            // otherwise create a new map sequence

            chaser.Move(game, chaserImg);

            Random random = new Random();
            int randNum = random.Next(1,121);
            if (randNum < game.Difficulty)
            {
                chaser.Move(game, chaserImg);
            }       // This means that the chaser moves more as the difficulty increases.

            player.CheckPowerUps(game.CurrentForm);

            if (game.GameOver())
            {
                EndGame();
            }

            playerDistanceFromChaserLabel.Text = "Distance From Chaser: " + game.GetPlayerDistanceFromChaser().ToString();
        }
        static void SaveMap(int slot)
        {
            int[,] mapToSave = game.CurrentMap.GetMapToSave();  //changes the map to be in the form of an integer array (where each integer corresponds to an item) so it is easier to store, the start map sequence is not included in this as it is always the same so doesn't need to be saved
            string fileLoc = Path.Combine(Application.StartupPath, "Data", "Map" + slot.ToString() + ".txt");
            File.WriteAllText(fileLoc, "");  // clears the text file ready for the current map to be saved
            FileStream fileStream = new FileStream(fileLoc, FileMode.Open, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            using (streamWriter)
            {
                for (int y = 0; y < mapToSave.Length / 3; y++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        string toWrite = (y == mapToSave.Length - 1) ? (mapToSave[x, y].ToString()) : (mapToSave[x, y].ToString() + ' ');
                        streamWriter.Write(toWrite);
                    }
                    streamWriter.Write('\n');
                }
            }
            // writes the integer array to a text file with the x value being the column, the y value being the row and a space between each column
            // the map is stored upside-down so that the y value of the integer array int[x,y] corresponds to the row of the text file
            // this means the first sequence is at the top of the text file and the last sequence at the bottom both upside-down 

        }
        static List<MapSequence> LoadMap(int slot)
        {
            string fileName = Path.Combine(Application.StartupPath, "Data", "Map" + slot.ToString() + ".txt");
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader streamReader = new StreamReader(fileStream);

            int size = File.ReadAllLines(fileName).Length;

            if (size <= 1)
            {
                return new List<MapSequence>() { };
            } // checks if there is actually a map in the text file, any text file storing a map will have at least 20 rows in it 

            int[,] map = new int[3, size];

            using (streamReader)
            {
                string line;
                int y = 0;
                while ((line = streamReader.ReadLine()) != null)
                {
                    List<string> values = line.Split(' ').ToList();
                    values.RemoveAt(values.Count - 1);
                    for (int x = 0; x < 3; x++)
                    {
                        map[x, y] = Convert.ToInt32(values[x]);
                    }
                    y++;
                }
            }

            // reads the text file line by line, then splits each line into a list of integers then adds each integer to the relevant location in the array,
            // this is represented by the x value/the column and the y value/the row

            // Key:
            // 0 = empty square
            // 1 = coin
            // 2 = regular obstacle
            // 3 = cracked obstacle
            // 4 = double coins
            // 5 = double score
            // 6 = invincibility

            int ySize = map.Length / 3;

            List<MapSequence> returnSequences = new List<MapSequence>();

            PreSetItems items = new PreSetItems();

            int i = 0;

            for (int seq = 0; seq < size / sizeOfMapSequence; seq++)
            {
                Item[,] itemSequence = new Item[3, sizeOfMapSequence];
                for (int y = 0; y < sizeOfMapSequence; y++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        switch (map[x, y + (sizeOfMapSequence * seq)])
                        {
                            case 0:
                                itemSequence[x, y] = new Item(items.emptySquare.ImageFileLocation, items.emptySquare.ItemName, items.emptySquare.ID);
                                break;
                            case 1:
                                itemSequence[x, y] = new Item(items.coin.ImageFileLocation, items.coin.ItemName, items.coin.ID); 
                                break;
                            case 2:
                                itemSequence[x, y] = new Obstacle(items.obstacle.ImageFileLocation, items.obstacle.ItemName, items.obstacle.ID, items.obstacle.IsCrackedObstacle); 
                                break;
                            case 3:
                                itemSequence[x, y] = new Obstacle(items.crackedObstacle.ImageFileLocation, items.crackedObstacle.ItemName, items.crackedObstacle.ID, items.crackedObstacle.IsCrackedObstacle); 
                                break;
                            case 4:
                                itemSequence[x, y] = new PowerUp(items.doubleCoins.ImageFileLocation, items.doubleCoins.ItemName, items.doubleCoins.ID);
                                break;
                            case 5:
                                itemSequence[x, y] = new PowerUp(items.doubleScore.ImageFileLocation, items.doubleScore.ItemName, items.doubleScore.ID);
                                break;
                            case 6:
                                itemSequence[x, y] = new PowerUp(items.invincibility.ImageFileLocation, items.invincibility.ItemName, items.invincibility.ID);
                                break;

                        }
                        itemSequence[x, y].ID = i;
                        i++;
                    }
                }
                // converts the integer array into an item array giving each item a unique ID so it can be identified in the DisplayedMap dictionary and altered if necessary
                int startSquare = returnSequences.Any() ? returnSequences.Last().EndSquare : 1;    // the start square would be 1 if there are no other sequences because the sequence before it would then be the start sequence which is not saved (because it is always the same).
                int endSquare = -1;
                for (int column = 0; column < 3; column++)
                {
                    if (map[column, (sizeOfMapSequence - 1) + (seq * sizeOfMapSequence)] != 2)
                    {
                        endSquare = column;
                        break;
                    }
                }
                // calculating relevant values for the map sequence
                int yPos = 0; // This will be defined later before the map is shown

                returnSequences.Add(new MapSequence(itemSequence, startSquare, endSquare, yPos));
            }

            return returnSequences;
        }
        public int GetHighScore()
        {
            string fileName = Path.Combine(Application.StartupPath, "Data", "High Score.txt");
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader streamReader = new StreamReader(fileStream);

            using (streamReader)
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    return Convert.ToInt32(line); 
                }
            }    // gets the number (high score) stored in the text file and returns it
            return 0; // if the text file is empty the high score will be set to 0
        }
        public void SetHighScore()
        {
            string fileLoc = Path.Combine(Application.StartupPath, "Data", "High Score.txt");
            File.WriteAllText(fileLoc, "");
            FileStream fileStream = new FileStream(fileLoc, FileMode.Open, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter(fileStream);

            using (streamWriter)
            {
                streamWriter.WriteLine(player.HighScore.ToString());
            }
            // clears the text file then updates it to score the current high score of the player when they quit the game
        }

        public void EndGame()
        {
            player.UpdateHighScore(); 
            gameTimer.Stop();
            previousMenu = Menus.EndGame;
            ShowEndGameMenu();
        }
        public void HideAllControls()
        {
            foreach (Control control in Controls)
            {
                control.Hide();
            }
        }
        public void ShowMainMenu()
        {
            HideAllControls();
            endlessRunnerLabel.Show();

            highScoreLabel.Text = "High Score: " + player.HighScore.ToString();
            highScoreLabel.Show();
            playButton.Show();
            loadMapButton.Show();
            quitButton.Show();
        }
        public void ShowEndGameMenu()
        {
            HideAllControls();
            gameOverLabel.Show();
            mainMenuButton.Show();
            playAgainButton.Show();
            viewStatisticsButton.Show();
            saveMapButton.Show();
        }
        public void StartGame()
        {
            player.ResetPlayer(); // resets the stats of the player
            chaser = new Chaser(startChaserXPos, startChaserYPos, Path.Combine(Application.StartupPath, "Images", "Alien.png"), lengthOfChaser, squaresAheadOfChaser, relativeLengthOfSquare);
            currentMap = new Map();
            game = new Game(player, chaser, currentMap, this);
            playerImg.Location = new Point(player.XPosition, player.YPosition);
            chaserImg.Location = new Point(chaser.XPosition, chaser.YPosition);
            KeyPreview = true;
            HideAllControls();
            playerImg.Show();
            playerImg.BringToFront();
            chaserImg.Show();
            chaserImg.BringToFront();
            backgroundImg.Show();
            scoreLabel.Location = new Point(333, 57);
            scoreLabel.Show();
            coinCounterLabel.Location = new Point(333, 101);
            coinCounterLabel.Text = "Coins: " + game.CurrentPlayer.Coins.ToString();
            coinCounterLabel.Show();
            playerDistanceFromChaserLabel.Show();
            ActivePowerUpsLabel.Show();
            CreateStartMap();
            gameTimer.Start();
            // resets the game to its initial position of the game starting
        }
        public void StartGame(List<MapSequence> map)
        {
            player.ResetPlayer();
            chaser = new Chaser(startChaserXPos, startChaserYPos, Path.Combine(Application.StartupPath, "Images", "Alien.png"), lengthOfChaser, squaresAheadOfChaser, relativeLengthOfSquare);
            chaser.MoveSequenceNeedsUpdate = true;
            currentMap = new Map(map);
            game = new Game(player, chaser, currentMap, this);
            playerImg.Location = new Point(player.XPosition, player.YPosition);
            chaserImg.Location = new Point(chaser.XPosition, chaser.YPosition);
            KeyPreview = true;
            HideAllControls();
            CreateStartMap();
            playerImg.Show();
            playerImg.BringToFront();
            chaserImg.Show();
            chaserImg.BringToFront();
            backgroundImg.Show();
            scoreLabel.Location = new Point(333, 57);
            scoreLabel.Show();
            coinCounterLabel.Location = new Point(333, 101);
            coinCounterLabel.Text = "Coins: " + game.CurrentPlayer.Coins.ToString();
            coinCounterLabel.Show();
            playerDistanceFromChaserLabel.Show();
            ActivePowerUpsLabel.Show();
            gameTimer.Start();
            // resets the game to its initial position of the game starting and sets up the currentMap so that it will contain the loaded map
        }
        public void ShowMapSlots()
        {
            HideAllControls();
            backButton.Show();
            selectSlotLabel.Show();
            slot1Button.Show();
            slot2Button.Show();
            slot3Button.Show();
            mapInfoLabel.Text = "Please select a slot.";
            mapInfoLabel.Show();
        }
        private void scoreLabel_Click(object sender, EventArgs e)
        {

        }
        private void coinCounterLabel_Click(object sender, EventArgs e)
        {

        }

        private void ActivePowerUpsLabel_Click(object sender, EventArgs e)
        {

        }

        private void powerUpsPickedUpLabel_Click(object sender, EventArgs e)
        {

        }

        private void obstaclesPassedLabel_Click(object sender, EventArgs e)
        {

        }

        private void viewStatisticsButton_Click(object sender, EventArgs e)
        {
            HideAllControls();
            scoreLabel.Location = new Point(880, 358);
            coinCounterLabel.Location = new Point(880, 425);
            powerUpsPickedUpLabel.Location = new Point(880, 492);
            obstaclesPassedLabel.Location = new Point(880, 559);
            powerUpsPickedUpLabel.Text = "Power Ups Picked Up: " + player.PowerUpsPickedUp.ToString();
            obstaclesPassedLabel.Text = "Obstacles Passed: " + player.ObstaclesPassed.ToString();
            scoreLabel.Show();
            coinCounterLabel.Show();
            powerUpsPickedUpLabel.Show();
            obstaclesPassedLabel.Show();
            backButton.Show();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            switch (previousMenu)
            {
                case Menus.Main:
                    ShowMainMenu();
                    break;
                case Menus.EndGame:
                    ShowEndGameMenu();
                    break;
            }
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            SetHighScore(); // stores the current high score of the player
            Close();
        }

        private void playAgainButton_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void endlessRunnerLabel_Click(object sender, EventArgs e)
        {

        }

        private void mainMenuButton_Click(object sender, EventArgs e)
        {
            previousMenu = Menus.Main;
            ShowMainMenu();
        }

        private void saveMapButton_Click(object sender, EventArgs e)
        {
            savingMap = true;
            ShowMapSlots();
        }

        private void loadMapButton_Click(object sender, EventArgs e)
        {
            savingMap = false;
            ShowMapSlots();
        }

        private void slot1Button_Click(object sender, EventArgs e)
        {
            if (savingMap)
            {
                if (currentMap.GetMapToSave().Length != 0) // check if there is a map to save (this does not include the start map sequence since it is always the same)
                {
                    SaveMap(1);
                    mapInfoLabel.Text = "Map saved";
                }
                else
                {
                    mapInfoLabel.Text = "There is no map to save";
                }
            }
            else // if a map is not being saved, it must be being loaded instead
            {
                if (LoadMap(1).Any())     // check if there is actualy a map stored in the text file
                {
                    StartGame(LoadMap(1));
                }
                else
                {
                    mapInfoLabel.Text = "This map slot is empty";
                }
            }
        }

        private void slot2Button_Click(object sender, EventArgs e)
        {
            if (savingMap)
            {
                if (currentMap.GetMapToSave().Length != 0)
                {
                    SaveMap(2);
                    mapInfoLabel.Text = "Map saved";
                }
                else
                {
                    mapInfoLabel.Text = "There is no map to save";
                }
            }
            else
            {
                if (LoadMap(2).Any())
                {
                    StartGame(LoadMap(2));
                }
                else
                {
                    mapInfoLabel.Text = "This map slot is empty";
                }
            }
        }

        private void slot3Button_Click(object sender, EventArgs e)
        {
            if (savingMap)
            {
                if (currentMap.GetMapToSave().Length != 0)
                {
                    SaveMap(3);
                    mapInfoLabel.Text = "Map saved";
                }
                else
                {
                    mapInfoLabel.Text = "There is no map to save";
                }
            }
            else
            {
                if (LoadMap(3).Any())
                {
                    StartGame(LoadMap(3));
                }
                else
                {
                    mapInfoLabel.Text = "This map slot is empty";
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}