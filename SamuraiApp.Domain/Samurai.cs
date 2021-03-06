﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamuraiApp.Domain
{
    public class Samurai
    {
        public Samurai(string name, string secretName) : this()
        {
            Name = name;
            SecretIdentity = new SecretIdentity { RealName = secretName };
        }
        public Samurai()
        {
            Quotes = new List<Quote>();
            SamuraiBattles = new List<SamuraiBattle>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        //public FullNameSamurai BetterName { get; set; }

        public List<Quote> Quotes { get; set; }
        public List<SamuraiBattle> SamuraiBattles { get; set; }
        public SecretIdentity SecretIdentity { get; set; }

    }
}
