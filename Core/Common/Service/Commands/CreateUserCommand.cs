﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.Commands
{
    public class CreateUserCommand
    {
        public string Username { get; set; }

        public string UnencryptedPassword { get; set; }
    }
}
