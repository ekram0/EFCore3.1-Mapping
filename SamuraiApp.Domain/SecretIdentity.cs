using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamuraiApp.Domain
{
    public class SecretIdentity
    {
        public int Id { get; set; }
        public string RealName { get; set; }

        //// no need to use fluent api as we use FK.
        public int SamuraiId { get; set; }

        ////in case of using no conventian declaritve name, 
        ///you have to mention this in OnModelCreating
        ////public int secretID { get; set; }

        ////incase of declaring Samurai as below insrtead of the above, 
        ////we need to use fluent api in OnNodelCreating method.
        ////based on declaration below , the samurai will be child not the principle.
        //public Samurai Samurai { get; set; }
    }
}
