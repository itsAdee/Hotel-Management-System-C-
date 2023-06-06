namespace GuestNamespace
{

    public class Guest
    {
        String Name;
        public string name
        {
            get { return Name; }
            set
            {
                Name = value;
            }
        }
        String passportNum;

        public string PassportNum
        {
            get { return passportNum; }
            set
            {
                passportNum = value;
            }
        }
        StayNamespace.Stay hotelStay;
        public StayNamespace.Stay HotelStay
        {
            get { return hotelStay; }
            set
            {
                hotelStay = value;
            }
        }

        MembershipNamespace.Membership Member;

        public MembershipNamespace.Membership  member { 
            get { return Member; }
            set
            {
                Member = value;
            }
         }

         bool isCheckedIn;
        public bool IsCheckedIn
        {
            get { return isCheckedIn; }
            set
            {
                isCheckedIn = value;
            }
        }

        public Guest()
        {
            Name = "";
            passportNum = "";
            hotelStay = new StayNamespace.Stay();
            Member = new MembershipNamespace.Membership();
            isCheckedIn = false;
        }

        public Guest(string name, string passportNum, StayNamespace.Stay hotelStay, MembershipNamespace.Membership member, bool isCheckedIn)
        {
            Name = name;
            this.passportNum = passportNum;
            this.hotelStay = hotelStay;
            this.Member = member;
            this.isCheckedIn = isCheckedIn;
        }



        public override string ToString()
        {
            return "Guest Name: " + Name + " Passport Number: " + passportNum + " Hotel Stay: " + hotelStay + " Membership: " + Member + " Is Checked In: " + isCheckedIn;
        }

    }

}

