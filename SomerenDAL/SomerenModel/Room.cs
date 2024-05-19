using System;

namespace SomerenModel
{
    public class Room
    {     
        public string RoomCode { get; set; }
        public string Building { get; set; } 
        public int Floor { get; set; }   
        public bool IsSingleRoom { get; set; }
        public string Type
        {
            get
            {
                if (IsSingleRoom)
                { return "Teacher"; }
                else
                {
                    return "Student";
                }
            }
        }
        public int SleepingPlaces
        {
            get
            {
                if (IsSingleRoom)
                {
                    return 1;
                } else
                {
                    return 8;
                }
            }
        }

        public string RoomCodeChar
        {
            get {
                string roomCodes = "";
                char[] roomCode = RoomCode.ToCharArray();
                for (int i = 1; i < roomCode.Length; i++)
                {
                    roomCodes += roomCode[i];
                }
                return roomCodes;
            }
        }
            
              
        }
    
}