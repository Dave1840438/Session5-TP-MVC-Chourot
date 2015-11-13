using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ObjectProp
{
    class Program
    {
        static void Main(string[] args)
        {

            String connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\Nicolas Chourot\Desktop\SQLWrapper\MaBD.mdf';Integrated Security=True";
            Users users = new Users(connectionString);

            //  c R u d - READ
            if (users.SelectAll())
                do
                {
                    Console.WriteLine(users.FirstName + " " + users.LastName + " " + users.Created.ToLongDateString() + " " + users.CivilState.ToString() + " " + users.Blocked.ToString());
                } while (users.Next());

            // c r u D - DELETE
            users.DeleteRecordByID(3);


            // C r u d - CREATE
            /*
            users.Name = "achourot";
            users.Password = "allo";
            users.FirstName = "Alain";
            users.LastName = "Chourot";
            users.Created = DateTime.Now;
            users.Email = "achourot@gmail.com";
            users.Insert();
            */
            
            // c r U d - UPDATE
            users.SelectByFieldName("Name", "achourot");
            users.FirstName = "Fred's";
            users.CivilState = EtatCivil.séparé;
            users.Blocked = false;
            users.Update();
            
        }
    }
}
