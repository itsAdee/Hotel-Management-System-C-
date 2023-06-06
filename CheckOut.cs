using System;
using System.IO;
using CsvHelper;
using System.Globalization;
using System.Text.RegularExpressions;
using MembershipNamespace;
using CSVmanager;
using RoomNamespace;
using StayNamespace;

namespace CheckOut
{
    public class CheckOutClass
    {
        static void Main(string[] args)
        {
            CheckOut();

        }
        // Check out function is used to check out a guest from the hotel
        static void CheckOut()
        {
            //read guests.csv and display all guests
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
            //if valid, read stays.csv and display all stays for the selected guest
            if (option > 0 && option <= i)
            {
                //read stays.csv and display all stays for the selected guest
                GuestsFile seledctedguest = guest[option - 1];
                Console.WriteLine("Guest selected: {0} , Passport Number : {1}", seledctedguest.Name, seledctedguest.PassportNumber);
                List<StaysFile> stays = ReadStays(seledctedguest.Name, seledctedguest.PassportNumber);
                int j = 1;
                //display all stays for the selected guest
                foreach (StaysFile stay in stays)
                {
                    Console.WriteLine("{0} , Room Number: {1}, Checkin Date: {2}, Checkout Date: {3}", j, stay.RoomNumber, stay.CheckinDate, stay.CheckoutDate);
                    j++;
                }
                if (stays.Count == 0)
                {
                    Console.WriteLine("No stays found");
                    return;
                }
                // ask user to select a stay
                //take user option
                Console.WriteLine("Enter the room number you want to check out: ");
                int roomnumber = Convert.ToInt32(Console.ReadLine());
                //check if option is valid
                if (roomnumber > 0 && roomnumber <= j)
                { //if valid, read rooms.csv and display all rooms for the selected stay
                    StaysFile selectedstay = stays[roomnumber - 1];
                    RoomsFile room = ReadRooms(selectedstay);
                    double total = 0;
                    Console.WriteLine("Room selected: {0}, Room Type: {1}, Bed Configuration: {2}, Daily Rate: {3}", room.RoomNumber, room.RoomType, room.BedConfiguration, room.DailyRate);
                    // if room type is standard, create a standard room object and calculate total
                    if (room.RoomType == "Standard")
                    {
                        bool isWifi = selectedstay.Wifi == "TRUE" ? true : false;
                        bool isBreakfast = selectedstay.Breakfast == "TRUE" ? true : false;
                        StandardRoom standardroom = new StandardRoom(room.RoomNumber, room.BedConfiguration, room.DailyRate, true, isWifi, isBreakfast);
                        Stay stay = new Stay(selectedstay.CheckinDate, selectedstay.CheckoutDate);
                        stay.AddRoom(standardroom);
                        total = stay.CalculateTotal();


                    }
                    // if room type is deluxe, create a deluxe room object and calculate total
                    else if (room.RoomType == "Deluxe")
                    {
                        bool isExtraBed = selectedstay.ExtraBed == "TRUE" ? true : false;
                        DeluxeRoom deluxeroom = new DeluxeRoom(room.RoomNumber, room.BedConfiguration, room.DailyRate, true, isExtraBed);
                        Stay stay = new Stay(selectedstay.CheckinDate, selectedstay.CheckoutDate);
                        stay.AddRoom(deluxeroom);
                        total = stay.CalculateTotal();

                    }
                    // display total
                    Console.WriteLine("Total amount to be paid: {0}", total);
                    // read guests.csv and display membership status and points
                    GuestsFile guestfile = ReadGuests(seledctedguest.Name, seledctedguest.PassportNumber);
                    Console.WriteLine("Membership Status: {0}, Membership Points: {1}", guestfile.MembershipStatus, guestfile.MembershipPoints);
                    // if membership points are above 100, ask user if they want to use points
                    if (guestfile.MembershipStatus == "Gold" || guestfile.MembershipStatus == "Silver")
                    {
                        // if yes, ask user to enter the number of points they want to use
                        Console.WriteLine("You are eligible for a discount");
                        Console.WriteLine("Enter the number of points you want to use: ");
                        int points = Convert.ToInt32(Console.ReadLine());
                        // if points are valid, deduct points from total and display new total
                        if (points > 0 && points <= guestfile.MembershipPoints)
                        {
                            // deduct points from total and display new total
                            total = total - points;
                            Console.WriteLine("Total amount to be paid: {0}", total);
                            // deduct points from membership points and display new membership points
                            guestfile.MembershipPoints = guestfile.MembershipPoints - points;
                            Console.WriteLine("Press any key to continue to  give payment");
                            // display remaining membership points
                            String key = Console.ReadLine();
                            Console.WriteLine("Remaining Membership Points: {0}", guestfile.MembershipPoints);
                            // find new membership status and display
                            double new_points = total / 10;
                            guestfile.MembershipPoints = guestfile.MembershipPoints + new_points;
                            Console.WriteLine("New Membership Points: {0}", guestfile.MembershipPoints);
                            // find new membership status and display
                            if (guestfile.MembershipPoints > 200)
                            {

                                guestfile.MembershipStatus = "Gold";

                            }
                            else if (guestfile.MembershipPoints > 100)
                            {
                                guestfile.MembershipStatus = "Silver";
                            }
                            Console.WriteLine("Membership Status: {0}, Membership Points: {1}", guestfile.MembershipStatus, guestfile.MembershipPoints);
                            selectedstay.IsCheckedIn = "FALSE";
                            WriteGuests(guestfile);
                            WriteStays(selectedstay);
                            Console.WriteLine("Order completed successfully");
                        }
                        // if points are invalid, display error message and ask user to enter points again
                        else
                        {
                            Console.WriteLine("You have entered an invalid number of points");
                            Console.WriteLine("Invalid option");
                            CheckOut();
                        }

                    }
                    else
                    {
                        Console.WriteLine("You are not eligible for a discount");
                        Console.WriteLine("Press any key to continue to give payment");
                        String key = Console.ReadLine();
                        double new_points = total / 10;
                        guestfile.MembershipPoints = guestfile.MembershipPoints + new_points;
                        Console.WriteLine("New Membership Points: {0}", guestfile.MembershipPoints);
                        if (guestfile.MembershipPoints > 200)
                        {

                            guestfile.MembershipStatus = "Gold";

                        }
                        else if (guestfile.MembershipPoints > 100)
                        {
                            guestfile.MembershipStatus = "Silver";
                        }
                        Console.WriteLine("Membership Status: {0}, Membership Points: {1}", guestfile.MembershipStatus, guestfile.MembershipPoints);
                        selectedstay.IsCheckedIn = "FALSE";
                        WriteGuests(guestfile);
                        WriteStays(selectedstay);
                        Console.WriteLine("Order completed successfully");
                    }
                }
                // if room number is invalid, display error message and ask user to enter room number again
                else
                {
                    Console.WriteLine("You have entered an invalid room number");
                    Console.WriteLine("Invalid option");
                    CheckOut();
                }


            }
            // if guest name is invalid, display error message and ask user to enter guest name again
            else
            {
                Console.WriteLine("You have entered an invalid guest name");
                Console.WriteLine("Invalid option");
                CheckOut();

            }
        }
        // function to read stays.csv
        // and to return a list of stays for a given guest
        static List<StaysFile> ReadStays(String Name, String PassportNumber)
        {
            //create a list of stays
            System.Collections.Generic.List<StaysFile> all_stays =
                new System.Collections.Generic.List<StaysFile>();
            System.Collections.Generic.List<StaysFile> stays =
                new System.Collections.Generic.List<StaysFile>();
            //open stays.csv and read all records
            //open stays.csv and read all records
            using (var stayreader = new StreamReader("Stays.csv"))
            using (var csvreader = new CsvReader(stayreader, CultureInfo.InvariantCulture))
            {
                var stayslist = csvreader.GetRecords<CSVmanager.StaysFile>().ToList();
                all_stays = stayslist;
            }
            //iterate through the list of stays
            for (int i = 0; i < all_stays.Count; i++)
            {
                //if the stay is for the given guest
                if (all_stays[i].Name == Name && all_stays[i].PassportNumber == PassportNumber)
                {
                    if (all_stays[i].IsCheckedIn == "TRUE")
                    {
                        stays.Add(all_stays[i]);
                    }
                }
            }
            return stays;
        }
        // function to read rooms.csv
        // and to return a room for a given stay
        static RoomsFile ReadRooms(StaysFile stays)
        {
            // create a list of rooms
            System.Collections.Generic.List<RoomsFile> all_rooms =
                new System.Collections.Generic.List<RoomsFile>();
            //open rooms.csv and read all records
            using (var roomreader = new StreamReader("Rooms.csv"))
            using (var csvreader = new CsvReader(roomreader, CultureInfo.InvariantCulture))
            {
                var roomslist = csvreader.GetRecords<CSVmanager.RoomsFile>().ToList();
                all_rooms = roomslist;
            }
            List<RoomsFile> rooms = new List<RoomsFile>();
            //iterate through the list of rooms
            for (int i = 0; i < all_rooms.Count; i++)
            {
                //if the room number matches the room number of the stay
                if (all_rooms[i].RoomNumber == stays.RoomNumber)
                {
                    return all_rooms[i];
                }
            }
            return null;


        }

        static GuestsFile ReadGuests(String Name, String PassportNumber)
        {
            //create a list of guests
            System.Collections.Generic.List<GuestsFile> all_guests =
                new System.Collections.Generic.List<GuestsFile>();
            //open guests.csv and read all records
            //open guests.csv and read all records
            using (var guestreader = new StreamReader("Guests.csv"))
            using (var csvreader = new CsvReader(guestreader, CultureInfo.InvariantCulture))
            {
                var guestslist = csvreader.GetRecords<GuestsFile>().ToList();
                all_guests = guestslist;
            }
            //iterate through the list of guests
            List<GuestsFile> guests = new List<GuestsFile>();
            for (int i = 0; i < all_guests.Count; i++)
            {
                //if the guest name and passport number matches
                if (all_guests[i].Name == Name && all_guests[i].PassportNumber == PassportNumber)
                {
                    return all_guests[i];
                }
            }
            return null;
        }
        // function to write Guests.csv
        // and to update the membership points and status
        static void WriteGuests(GuestsFile guest)
        {
            try
            {
                //create a list of guests
                System.Collections.Generic.List<GuestsFile> all_guests =
                    new System.Collections.Generic.List<GuestsFile>();
                //open guests.csv and read all records
                using (var guestreader = new StreamReader("Guests.csv"))
                using (var csvreader = new CsvReader(guestreader, CultureInfo.InvariantCulture))
                {
                    var guestslist = csvreader.GetRecords<GuestsFile>().ToList();
                    all_guests = guestslist;
                }
                //iterate through the list of guests
                for (int i = 0; i < all_guests.Count; i++)
                {
                    //if the guest name and passport number matches
                    if (all_guests[i].Name == guest.Name && all_guests[i].PassportNumber == guest.PassportNumber)
                    {
                        all_guests[i].MembershipPoints = guest.MembershipPoints;
                        all_guests[i].MembershipStatus = guest.MembershipStatus;
                    }
                }
                //write the updated list of guests to guests.csv
                using (var writer = new StreamWriter("Guests.csv"))
                using (var csvwriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csvwriter.WriteRecords(all_guests);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        // function to write stays.csv
        // and to update the stay
        static void WriteStays(StaysFile stay)
        {
            try
            {
                //create a list of stays
                System.Collections.Generic.List<StaysFile> all_stays =
                    new System.Collections.Generic.List<StaysFile>();
                //open stays.csv and read all records
                using (var stayreader = new StreamReader("Stays.csv"))
                using (var csvreader = new CsvReader(stayreader, CultureInfo.InvariantCulture))
                {
                    var stayslist = csvreader.GetRecords<CSVmanager.StaysFile>().ToList();
                    all_stays = stayslist;
                }
                //iterate through the list of stays
                for (int i = 0; i < all_stays.Count; i++)
                {
                    //if the stay is for the given guest
                    if (all_stays[i].Name == stay.Name && all_stays[i].PassportNumber == stay.PassportNumber && all_stays[i].RoomNumber == stay.RoomNumber)
                    {
                        all_stays[i].IsCheckedIn = stay.IsCheckedIn;
                    }
                }
                //write the updated list of stays to stays.csv
                using (var writer = new StreamWriter("Stays.csv"))
                using (var csvwriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csvwriter.WriteRecords(all_stays);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}
