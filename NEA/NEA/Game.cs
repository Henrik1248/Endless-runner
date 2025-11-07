using NEA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEA
{
    public class Game
    {
        public const int STARTING_DIFFICULTY = 10;
        public const int MAXIMUM_DIFFICULTY = 80; // Must be less than 100
        public Form1 CurrentForm { get; set; }
        public Player CurrentPlayer { get; private set; }
        public Chaser CurrentChaser { get; private set; }
        public Map CurrentMap { get; private set; }
        public int Difficulty { get; set; } // Difficulty as a %
        public Game(Player player, Chaser chaser, Map map, Form1 form)
        {
            CurrentForm = form;
            CurrentPlayer = player;
            CurrentChaser = chaser;
            CurrentMap = map;
            Difficulty = STARTING_DIFFICULTY;
        }
        public void CheckForEvent(MapSequence sequence, int column, int row) 
        {
            PreSetItems items = new PreSetItems();
            int itemID = sequence.Map[column, row].ID;
            bool mapChangeRequired = false;
            switch (sequence.Map[column, row].ItemName)
            {
                case Items.Coin:
                    CurrentPlayer.AddCoin(CurrentForm);
                    mapChangeRequired = true;
                    break;
                case Items.DoubleCoins:
                    CurrentPlayer.PowerUpsPickedUp++;
                    CurrentPlayer.AddPowerUp(CurrentForm, items.doubleCoins, itemID);
                    mapChangeRequired = true;
                    break;
                case Items.DoubleScore:
                    CurrentPlayer.PowerUpsPickedUp++;
                    CurrentPlayer.AddPowerUp(CurrentForm, items.doubleScore, itemID);
                    mapChangeRequired = true;
                    break;
                case Items.Invincibility:
                    CurrentPlayer.PowerUpsPickedUp++;
                    CurrentPlayer.AddPowerUp(CurrentForm, items.invincibility, itemID);
                    mapChangeRequired = true;
                    break;
            }
            if (mapChangeRequired)
            {
                DeleteItem(column, row, sequence);
            }
            // check if there has been an event in the game, carries out the event and then removes the item from the map and ajdusts the players stats appropriately
        }
        public void CheckForEvent(MapSequence sequence, List<int> columns, int row)
        {
            foreach (int column in columns)
            {
                CheckForEvent(sequence, column, row);
            }
        }
        public void CheckForEvent(MapSequence sequence, int column, List<int> rows)
        {
            foreach (int row in rows)
            {
                CheckForEvent(sequence, column, row);
            }
        }
        public void DeleteItem(int column, int row, MapSequence sequence)
        {
            PreSetItems items = new PreSetItems();
            int id;

            if (sequence.Map.Length / 3 == 40)           // 40 = 2 * sizeOfMapSequence         sizeOfMapSequence = 20
            {
                if (row >= 20)
                {
                    row -= 20;
                    id = CurrentMap.MapSequences[CurrentMap.MapSequences.Count - 1].Map[column, row].ID;
                    CurrentMap.MapSequences[CurrentMap.MapSequences.Count - 1].Map[column, row] = items.emptySquare;
                }
                else
                {
                    id = CurrentMap.MapSequences[CurrentMap.MapSequences.Count - 2].Map[column, row].ID;
                    CurrentMap.MapSequences[CurrentMap.MapSequences.Count - 2].Map[column, row] = items.emptySquare;
                }
            }
            else
            {
                id = sequence.Map[column, row].ID;
                sequence.Map[column, row] = items.emptySquare;
            } // uses the column, row and map sequence to find the ID of the item that needs deleting and changes the item in the stored map to be an empty square
            CurrentMap.DeleteItemFromMap(id);
            // deletes the item from the displayed map, replacing it with an empty square
        }
        public int GetPlayerDistanceFromChaser() 
        {
            return (int) Math.Floor(Math.Sqrt(Math.Pow(CurrentPlayer.XPosition - CurrentChaser.XPosition,2) + Math.Pow(CurrentPlayer.YPosition - CurrentChaser.YPosition,2)));
        }
        public MapSequence GetMapAheadOfChaser()
        {
            MapSequence lastMap = CurrentMap.MapSequences.Last();

            int rowsAheadOfChaser = (CurrentChaser.YPosition - lastMap.YPosition) / 72;     // 72 = length of square
            int chaserColumn = (CurrentChaser.XPosition - 850) / 72;           // 850 = mapXPos           Since the chaser has finished its move, it is within only one column.

            Item[,] itemsInSequence = new Item[3, rowsAheadOfChaser + 1];

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y <= rowsAheadOfChaser; y++)
                {
                    itemsInSequence[x, rowsAheadOfChaser - y] = CurrentMap.MapSequences[CurrentMap.MapSequences.Count - (1 + (y / 20))].Map[x, 19 - (y % 20)];      // 19 = sizeOfMapSequence - 1       sizeOfMapSequence = 20
                } 
            }
            // loops over all the items ahead of the chaser adding them to an 2D item array whilst preserving the structure of the map
            return new MapSequence(itemsInSequence, chaserColumn, lastMap.EndSquare, lastMap.YPosition);
        }
        public void UpdateDifficulty()
        {
            if (Difficulty < MAXIMUM_DIFFICULTY)
            {
                Difficulty = Difficulty + 100/(Difficulty + 21) + 1;
            }
        }
        public bool GameOver()
        {

            if (CurrentChaser.YPosition - CurrentPlayer.YPosition <= CurrentPlayer.Size)
            {
                if (CurrentChaser.XPosition >= CurrentPlayer.XPosition - CurrentChaser.Size && CurrentChaser.XPosition <= CurrentPlayer.XPosition + CurrentPlayer.Size)        // The use of both CurrentChaser.Size and CurrentPlayer.Size is because the XPosition of the character is measure from the left most point of the character/PictureBox.
                {
                    return true;
                }
            }
            // checks if the chaser is touching the player

            return false;
        }
    }
}


