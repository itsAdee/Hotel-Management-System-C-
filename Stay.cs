namespace StayNamespace
{
    public class Stay
    {
        DateOnly checkinDate;
        public DateOnly CheckinDate
        {
            get { return checkinDate; }
            set
            {
                checkinDate = value;
            }
        }
        DateOnly checkoutDate;
        public DateOnly CheckoutDate
        {
            get { return checkoutDate; }
            set
            {
                checkoutDate = value;
            }
        }

        public List<RoomNamespace.Room> roomList;

        public Stay()
        {
            roomList = new List<RoomNamespace.Room>();
        }

        public Stay(DateOnly checkinDate, DateOnly checkoutDate)
        {
            this.checkinDate = checkinDate;
            this.checkoutDate = checkoutDate;
            roomList = new List<RoomNamespace.Room>();
        }

        public void AddRoom(RoomNamespace.Room Room)
        {
            this.roomList.Add(Room);
        }

        public double CalculateTotal()
        {
            double totalCost = 0;
            foreach (RoomNamespace.Room room in roomList)
            {
                totalCost += room.CalculateCharges();
            }
            //calculate difference of dates
            int totaldays = (new DateTime(checkoutDate.Year, checkoutDate.Month, checkoutDate.Day) - new DateTime(checkinDate.Year, checkinDate.Month, checkinDate.Day)).Days;
            totalCost *= totaldays;
            return totalCost;
        }
        public override string ToString()

        {
            String roomslist = "";
            foreach (RoomNamespace.Room room in roomList)
            {
                roomslist = roomslist + room.RoomNumber + ",";
            }
            return "CheckInDate: " + checkinDate + "CheckOutDate: " + checkoutDate + "Rooms: " + roomslist;
        }
    }

}