﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CursosOnline.Dominio
{
    public class Usuario:IdentityUser
    {
        public string NombreCompleto { get; set; }
    }
}
