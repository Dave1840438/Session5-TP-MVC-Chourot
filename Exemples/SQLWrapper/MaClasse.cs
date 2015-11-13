using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectProp
{
    public enum EtatCivil { célibataire, marié, séparé, divorcé, veuf}
    class Users : SqlExpressUtilities.SqlExpressWrapper
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public String Password { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateTime Created { get; set; }
        public String Email { get; set; }
        public EtatCivil CivilState { get; set; }
        public bool Blocked { get; set; }

        public Users(String connectionString)
            : base(connectionString)
        {
        } 
    }
}
