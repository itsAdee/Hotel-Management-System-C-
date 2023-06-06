namespace RoomNamespace
{
    public class Room
    {
        int roomNumber;

        public int RoomNumber
        {
            get { return roomNumber; }
            set
            {
                roomNumber = value;
            }
        }
        string bedConfiguration;

        public string BedConfiguration
        {
            get { return bedConfiguration; }
            set
            {
                bedConfiguration = value;
            }
        }
        double dailyRate;

        public double DailyRate
        {
            get { return dailyRate; }
            set
            {
                dailyRate = value;
            }
        }
        private bool isAvail;
        public bool IsAvail
        {
            get { return isAvail; }
            set
            {
                isAvail = value;
            }
        }

        public Room(){
            roomNumber = 0;
            bedConfiguration = "";
            dailyRate = 0;
            isAvail = false;
        }
        public Room(int roomNumber, string bedConfiguration, double dailyRate, bool isAvail)
        {
            RoomNumber = roomNumber;
            BedConfiguration = bedConfiguration;
            DailyRate = dailyRate;
            IsAvail = isAvail;
        }

        public virtual double CalculateCharges()
        {
            return dailyRate;
        }

        public override string ToString()
        {
            return "Room Number: " + roomNumber + ", Bed Configuration: " + bedConfiguration + ", Daily Rate: " + dailyRate + ", Available: " + isAvail;
        }

        



    }
    public class StandardRoom : Room
    {
        bool requireWifi;
        public bool RequireWifi
        {
            get { return requireWifi; }
            set
            {
                requireWifi = value;
            }
        }
        bool requireBreakfast;

        public bool RequireBreakfast
        {
            get { return requireBreakfast; }
            set
            {
                requireBreakfast = value;
            }
        }

        public StandardRoom() { }

        public StandardRoom(int roomNumber, string bedConfiguration, double dailyRate, bool isAvail, bool requireWifi, bool requireBreakfast) : base(roomNumber, bedConfiguration, dailyRate, isAvail)
        {
            RequireWifi = requireWifi;
            RequireBreakfast = requireBreakfast;
        }
        public override double CalculateCharges()
        {
            int charges = 0;
            if (requireWifi)
            {
                charges += 10;
            }
            if (requireBreakfast)
            {
                charges += 20;
            }
            return base.CalculateCharges() + charges;
        }
    }

    public class DeluxeRoom : Room
    {

        bool additionalBed;
        public bool AdditionalBed
        {
            get { return additionalBed; }
            set
            {
                additionalBed = value;
            }
        }
        public DeluxeRoom(int roomNumber, string bedConfiguration, double dailyRate, bool isAvail, bool additionalBed) : base(roomNumber, bedConfiguration, dailyRate, isAvail)
        {
            AdditionalBed = additionalBed;
        }
        public override double CalculateCharges()
        {
            int cost = 0;
            if (additionalBed)
            {
                cost += 25;
            }
            return base.CalculateCharges() + cost;
        }
    }
}