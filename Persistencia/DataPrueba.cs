using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CursosOnline.Dominio;
using Microsoft.AspNetCore.Identity;

namespace CursosOnline.Persistencia
{
    public class DataPrueba
    {
        public static async Task InsertarData(CursosOnlineContext context, UserManager<Usuario> userManager)
        {
            if (!userManager.Users.Any())
            {
                var usuario = new Usuario { NombreCompleto = "Maxi Follonier", UserName = "MaxiFollo", Email = "maxifollonier@gmail.com" };
                await userManager.CreateAsync(usuario, "Password123$");
            }
        }
    }
}
