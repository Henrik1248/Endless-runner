using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA
{
    public class Item
    {
        public string ImageFileLocation { get; protected set; }
        public Items ItemName { get; protected set; }
        public int ID { get; set; }
        public Item(string imgFileLocation, Items name, int id) 
        {
            ImageFileLocation = imgFileLocation;
            ItemName = name;
            ID = id;
        }
    }
    public class Obstacle : Item
    {
        public bool IsCrackedObstacle { get; private set; }
        public int StunnedDuration { get; private set; }

        public Obstacle(string imgFileLocation, Items name, int id, bool cracked) : base(imgFileLocation, name, id)
        {
            IsCrackedObstacle = cracked;
            StunnedDuration = 60;
        }
    }
    public class PowerUp : Item
    {
        public const int START_DURATION = 530;         
        public double Duration { get; set; }               // Duration is in seconds.

        public PowerUp(string imgFileLocation, Items name, int id) : base(imgFileLocation, name, id)
        {
            Duration = START_DURATION;
        }
        public bool IsActive()
        {
            return (Duration > 0);
        }
    }
}
