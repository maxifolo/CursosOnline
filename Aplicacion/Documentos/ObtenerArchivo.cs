using CursosOnline.Aplicacion.ManejadorError;
using CursosOnline.Persistencia;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CursosOnline.Aplicacion.Documentos
{
    public class ObtenerArchivo
    {
        public class Ejecuta : IRequest<ArchivoGenerico>
        {
            public Guid Id { get; set; }
        };
        public class Manejador : IRequestHandler<Ejecuta, ArchivoGenerico>
        {
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context)
            {
                _context = context;
            }

            public async Task<ArchivoGenerico> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var archivo = await _context.Documento.Where(x => x.ObjetoReferencia == request.Id).FirstOrDefaultAsync();
                if (archivo != null)
                {
                    return new ArchivoGenerico
                    {
                        Data = System.Convert.ToBase64String(archivo.Contenido),
                        Nombre = archivo.Nombre,
                        Extension = archivo.Extension
                    };
                }

                throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No se encontro la imagen" });
            }
        };
    }
}
