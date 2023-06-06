using System;
using System.IO;
using CsvHelper;
using System.Globalization;
using System.Text.RegularExpressions;
using MembershipNamespace;
using CSVmanager;

// #pragma warning disable CS0168, CS0219, CS8618, CS8604


namespace HotelManagementSystem
{

    class Program
    {


        //display all guests
        public static void DisplayGuests()
        {
            using (var guestreader = new StreamReader("Guests.csv"))
            using (var csvreader = new CsvReader(guestreader, CultureInfo.InvariantCulture))
            {
                var guests = csvreader.GetRecords<CSVmanager.GuestsFile>().ToList();
                int i = 1;
                foreach (var rec in guests)
                {

                    Console.WriteLine(" {0} , Name: {1}, PassportNumber: {2}, MembershipStatus: {3}, MembershipPoints: {4}", i, rec.Name, rec.PassportNumber, rec.MembershipStatus, rec.MembershipPoints);
                    i++;

                }
            }
        }

        //display all rooms
        public static void DisplayRooms()
        {
            using (var roomreader = new StreamReader("Rooms.csv"))
            using (var csvreader = new CsvReader(roomreader, CultureInfo.InvariantCulture))
            {
                var rooms = csvreader.GetRecords<CSVmanager.RoomsFile>().ToList();
                int i = 1;
                foreach (var rec in rooms)
                {

                    Console.WriteLine(" {0} , RoomType: {1}, RoomNumber: {2}, BedConfiguration: {3}, DailyRate: {4}", i, rec.RoomType, rec.RoomNumber, rec.BedConfiguration, rec.DailyRate);
                    i++;


                }
            }

        }

        //register a guest
        public static void RegisterGuest()
        {
            try
            {
                //take input foe name and passprt number

                Console.WriteLine("Enter Name: ");
                string name = Console.ReadLine();
                //check if number contains only alphabets
                if (Regex.IsMatch(name, @"^[a-zA-Z]+$"))
                {
                    Console.WriteLine("Name is valid");
                }
                else
                {
                    Console.WriteLine("Name is not valid");
                    RegisterGuest();
                }
                Console.WriteLine("Enter Passport Number: ");
                string passport = Console.ReadLine();

                //create a membership object
                Membership membership = new Membership();

                //append data into guests.csv usinf file.writeline

                using (var guestswriter = new StreamWriter("Guests.csv", true))
                using (var csvwriter = new CsvWriter(guestswriter, CultureInfo.InvariantCulture))
                {
                    csvwriter.WriteRecord(new CSVmanager.GuestsFile
                    {
                        Name = name,
                        PassportNumber = passport,
                        MembershipStatus = membership.Status,
                        MembershipPoints = membership.Points
                    });
                }





            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                RegisterGuest();
            }
        }

        static DateOnly datecreator(string time)
        {
            try
            {
                Console.WriteLine("Enter the {0} year", time);
                int year = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter the {0} month", time);
                int month = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter the {0} day", time);
                int day = int.Parse(Console.ReadLine());
                DateOnly date = new DateOnly(year, month, day);
                //checking validation
                if (date < new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                {
                    Console.WriteLine("Date cannot be from past");
                    Console.WriteLine("Please try again");
                    return datecreator(time);
                }
                return date;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Please try again");
                return datecreator(time);
            }
        }


        static string optioncreator(string opt)
        {
            Console.WriteLine("do you want {0} answer in Y/N?", opt);
            string option = Console.ReadLine();

            if (option == "Y" || option == "y")
            {
                return "TRUE";
            }
            else if (option == "N" || option == "n")
            {
                return "FALSE";
            }
            else
            {
                Console.WriteLine("Invalid option please try again");
                return optioncreator(opt);
            }

        }

        static void storeCheckin(string Name, string PassportNumber, string IsCheckedIn, DateOnly CheckinDate, DateOnly CheckoutDate, int RoomNumber, string Wifi, string Breakfast, string ExtraBed)
        {
            //write into stay.csv
            using (var staywriter = new StreamWriter("Stays.csv", true))
            using (var csvwriter = new CsvWriter(staywriter, CultureInfo.InvariantCulture))
            {
                csvwriter.NextRecord();
                csvwriter.WriteRecord(new CSVmanager.StaysFile
                {
                    Name = Name,
                    PassportNumber = PassportNumber,
                    IsCheckedIn = IsCheckedIn,
                    CheckinDate = CheckinDate,
                    CheckoutDate = CheckoutDate,
                    RoomNumber = RoomNumber,
                    Wifi = Wifi,
                    Breakfast = Breakfast,
                    ExtraBed = ExtraBed
                });
            }
        }

        public static List<RoomsFile> listAllAvailableRooms(List<RoomsFile> allrooms)

        {
            // copilot list all rooms and then check them using from stay rooms list
            using (var reader = new StreamReader("Stays.csv"))
            using (var csvreader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var stayRecords = csvreader.GetRecords<StaysFile>().ToList();
                List<RoomsFile> availableRooms = new List<RoomsFile>();
                foreach (var room in allrooms)
                {
                    bool isAvailable = true;
                    foreach (var stay in stayRecords)
                    {
                        if (room.RoomNumber == stay.RoomNumber)
                        {
                            Console.WriteLine(room.RoomNumber);
                            Console.WriteLine(stay.IsCheckedIn);
                            if (stay.IsCheckedIn == "TRUE")
                            {
                                isAvailable = false;
                                break;
                            }
                        }
                    }
                    if (isAvailable)
                    {
                        availableRooms.Add(room);
                    }
                }
                return availableRooms;
            }

        }
        //check in a guest  
        public static void CheckInGuest()
        {
            int i = 1;
            System.Collections.Generic.List<GuestsFile> guest =
                new System.Collections.Generic.List<GuestsFile>();
            //open guests.csv and read all records
            using (var guestreader = new StreamReader("Guests.csv"))
            using (var csvreader = new CsvReader(guestreader, CultureInfo.InvariantCulture))
            {
                var guests = csvreader.GetRecords<CSVmanager.GuestsFile>().ToList();
                guest = guests;
                foreach (var rec in guests)
                {

                    Console.WriteLine(" {0} , Name: {1}, PassportNumber: {2}, MembershipStatus: {3}, MembershipPoints: {4}", i, rec.Name, rec.PassportNumber, rec.MembershipStatus, rec.MembershipPoints);
                    i++;

                }
            }
            //take user option
            Console.WriteLine("Enter the number of the guest you want to check in: ");
            int option = Convert.ToInt32(Console.ReadLine());
            //check if option is valid
            if (option > 0 && option <= i)
            {

            }
            else
            {
                Console.WriteLine("Invalid option");
                CheckInGuest();
            }
            GuestsFile seledctedguest = guest[option - 1];

            //take checkindate
            DateOnly checkindate = datecreator("check in");
            //take checkoutdate
            DateOnly checkoutdate = datecreator("check out");
            //check if checkoutdate is greater than checkindate
            if (checkoutdate < checkindate)
            {
                Console.WriteLine("Checkout date cannot be before checkin date");
                Console.WriteLine("Please try again");
                checkoutdate = datecreator("check out");
            }
            System.Collections.Generic.List<CSVmanager.RoomsFile> room =
           new System.Collections.Generic.List<CSVmanager.RoomsFile>();
            //open rooms.csv and display data
            using (var roomreader = new StreamReader("Rooms.csv"))
            using (var csvreader = new CsvReader(roomreader, CultureInfo.InvariantCulture))
            {
                var rooms = csvreader.GetRecords<CSVmanager.RoomsFile>().ToList();
                room = rooms;
            }
            //list all available rooms
            List<RoomsFile> availableRooms = listAllAvailableRooms(room);
            i = 1;
            foreach (var rec in availableRooms)
            {
                Console.WriteLine(" {0} , RoomType: {1}, RoomNumber: {2}, BedConfiguration: {3}, DailyRate: {4}", i, rec.RoomType, rec.RoomNumber, rec.BedConfiguration, rec.DailyRate);
                i++;
            }
            //take user option
            Console.WriteLine("Enter the number of the room you want to check in: ");
            int option2 = Convert.ToInt32(Console.ReadLine());
            //check if option is valid
            if (option2 > 0 && option2 <= i)
            {

            }
            else
            {
                Console.WriteLine("Invalid option");
                CheckInGuest();
            }
            RoomsFile selectedroom = availableRooms[option2 - 1];

            if (selectedroom.RoomType == "Standard")
            {
                // require wifi [Y/N] & require breakfast [Y/N]
                string wifi = optioncreator("wifi");
                string breakfast = optioncreator("breakfast");
                storeCheckin(
                    seledctedguest.Name,
                    seledctedguest.PassportNumber,
                    "TRUE",
                    checkindate,
                    checkoutdate,
                    selectedroom.RoomNumber,
                    wifi,
                    breakfast,
                    "FALSE");
            }
            if (selectedroom.RoomType == "Deluxe")
            {
                string ExtraBed = optioncreator("extra bed");
                storeCheckin(
                    seledctedguest.Name,
                    seledctedguest.PassportNumber,
                    "TRUE",
                    checkindate,
                    checkoutdate,
                    selectedroom.RoomNumber,
                    "TRUE",
                    "TRUE",
                    ExtraBed);
            }
        }
    }

}
