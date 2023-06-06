namespace MembershipNamespace
{
    public class Membership
    {
        string status;
        public string Status
        {
            get { return status; }
            set
            {
                status = value;
            }
        }

        int points;
        public int Points
        {
            get { return points; }
            set
            {
                points = value;
            }
        }

        public Membership()
        {
            status = "";
            points = 0;
        }

        public Membership(string status, int points)
        {
            Status = status;
            Points = points;
        }



        void EarnPoints(double amount)
        {
            points += (int)amount / 10;
        }

        bool RedeemPoints(int points)
        {
            if (points > 100)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return "Status: " + status + " Points: " + points;
        }
    }
}