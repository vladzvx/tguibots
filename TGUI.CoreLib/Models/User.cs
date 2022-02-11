﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGUI.CoreLib.Enums;

namespace TGUI.CoreLib.Models
{
    public class User
    {
        [BsonId]
        public long Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public UserStatus Status { get; set; } 
    }
}
