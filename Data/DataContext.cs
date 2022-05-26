﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersLibrary.Services;

namespace Data
{
    public class DataContext
    {
        internal List<Service> Services { get; private set; }

        public DataContext() => Services = new List<Service>(); 
        public DataContext(List<Service> services) => Services = services; 
    }
}