﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Authentication
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

    }
}
