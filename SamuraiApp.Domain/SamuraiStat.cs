﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamuraiApp.Domain
{
    public class SamuraiStat
    {
        public int SamuraiId { get; private set; }
        public int NumberOfBattle { get; private set; }
        public string Name { get; private set; }
        public string EarliestBattle { get; private set; }
    }
}
