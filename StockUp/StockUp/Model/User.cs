using System;
namespace StockUp.Model
{
    public class User
    {
        public String Emp_id { get; set; }

        public String First { get; set; }
        public String Last { get; set; }
        public String Email { get; set; }
        public int IsAdmin { get; set; } // tiny int 0 1

        public String Passsword { get; set; }

        public User(String Emp_id,String First, String Last, String Email, int IsAdmin, String Password)
        {
            this.Emp_id = Emp_id;
            this.First = First;
            this.Last = Last;
            this.Email = Email;
            this.IsAdmin = IsAdmin;
            this.Passsword = Password;
        }
    }
}
