using CursosOnline.Aplicacion.Contratos;
using CursosOnline.Aplicacion.ManejadorError;
using CursosOnline.Dominio;
using CursosOnline.Persistencia;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CursosOnline.Aplicacion.Seguridad
{
    public class UsuarioActualizar
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string NombreCompleto { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public ImagenGeneral ImagenPerfil { get; set; }
        }
        public class EjecutaValida : AbstractValidator<Ejecuta>
        {
            public EjecutaValida()
            {
                RuleFor(x => x.NombreCompleto).NotEmpty();
                RuleFor(x => x.Username).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }
        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly CursosOnlineContext _context;
            private readonly UserManager<Usuario> _userManager;
            private readonly IJwtGenerador _jwtGenerador;
            private readonly IPasswordHasher<Usuario> _passwordHasher;
            public Manejador(CursosOnlineContext context, UserManager<Usuario> userManager, IJwtGenerador jwtGenerador, IPasswordHasher<Usuario> passwordHasher)
            {
                _context = context;
                _userManager = userManager;
                _jwtGenerador = jwtGenerador;
                _passwordHasher = passwordHasher;
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuarioIdem = await _userManager.FindByNameAsync(request.Username);
                if (usuarioIdem == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No existe un usuario con este username" });
                }
                var resultado = await _context.Users.Where(x => x.Email == request.Email && x.UserName != request.Username).AnyAsync();
                if (resultado)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.InternalServerError , new { mensaje = "Este Email ya pertenece a otro usuario" });
                }

                if (request.ImagenPerfil != null)
                {
                    var resultadoImagen = await _context.Documento.Where(x => x.ObjetoReferencia == new Guid(usuarioIdem.Id)).FirstOrDefaultAsync();
                    if (resultadoImagen == null)
                    {
                        var imagen = new Documento
                        {
                            Contenido = System.Convert.FromBase64String(request.ImagenPerfil.Data),
                            Nombre = request.ImagenPerfil.Nombre,
                            Extension = request.ImagenPerfil.Extension,
                            ObjetoReferencia = new Guid(usuarioIdem.Id),
                            DocumentoId = Guid.NewGuid(),
                            FechaCreacion = DateTime.UtcNow
                        };
                        _context.Documento.Add(imagen);
                    }
                    else
                    {
                        resultadoImagen.Contenido = System.Convert.FromBase64String(request.ImagenPerfil.Data);
                        resultadoImagen.Nombre = request.ImagenPerfil.Nombre;
                        resultadoImagen.Extension = request.ImagenPerfil.Extension;
                        //await _context.SaveChangesAsync();
                    }
                }

                usuarioIdem.NombreCompleto = request.NombreCompleto;
                usuarioIdem.PasswordHash = _passwordHasher.HashPassword(usuarioIdem, request.Password);
                usuarioIdem.Email = request.Email;

                var resultRoles = await _userManager.GetRolesAsync(usuarioIdem);
                var listaRoles = new List<string>(resultRoles);

                var imagenPerfil = await _context.Documento.Where(x => x.ObjetoReferencia == new Guid(usuarioIdem.Id)).FirstOrDefaultAsync();

                var resultUpdate = await _userManager.UpdateAsync(usuarioIdem);
                if (resultUpdate.Succeeded)
                {
                    if (imagenPerfil != null)
                    {
                        return new UsuarioData
                        {
                            NombreCompleto = usuarioIdem.NombreCompleto,
                            UserName = usuarioIdem.UserName,
                            Email = usuarioIdem.Email,
                            Token = _jwtGenerador.CrearToken(usuarioIdem, listaRoles),
                            ImagenPerfil = new ImagenGeneral
                            {
                                Data = System.Convert.ToBase64String(imagenPerfil.Contenido),
                                Extension = imagenPerfil.Extension,
                                Nombre = imagenPerfil.Nombre
                            }
                        };
                    }
                    else
                    {
                        return new UsuarioData
                        {
                            NombreCompleto = usuarioIdem.NombreCompleto,
                            UserName = usuarioIdem.UserName,
                            Email = usuarioIdem.Email,
                            Token = _jwtGenerador.CrearToken(usuarioIdem, listaRoles)
                        };
                    }
                    
                }

                throw new Exception("No se pudo actualizar el usuario");

            }
        }
    }
}
