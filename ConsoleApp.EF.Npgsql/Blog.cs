﻿using System;

namespace ConsoleApp.EF.Npgsql
{
    public class Blog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedTimestamp { get; set; }
    }
}