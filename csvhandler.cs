namespace CSVmanager
{
    public class GuestsFile
    {
        public string Name { get; set; }
        public string PassportNumber { get; set; }
        public string MembershipStatus { get; set; }
        public double MembershipPoints { get; set; }
    }

    public class RoomsFile
    {
        public string RoomType { get; set; }
        public int RoomNumber { get; set; }
        public string BedConfiguration { get; set; }
        public int DailyRate { get; set; }
    }
    public class StaysFile
    {
        public string Name { get; set; }
        public string PassportNumber { get; set; }
        public string IsCheckedIn { get; set; }
        public DateOnly CheckinDate { get; set; }
        public DateOnly CheckoutDate { get; set; }
        public int RoomNumber { get; set; }

        public string Wifi { get; set; }

        public string Breakfast { get; set; }

        public string ExtraBed { get; set; }


    }

}