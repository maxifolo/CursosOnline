using AutoMapper;
using CursosOnline.Dominio;
using CursosOnline.Persistencia;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CursosOnline.Aplicacion.Cursos
{
    public class ConsultaId
    {
        public class CursoUnico : IRequest<CursoDto>
        {
            public Guid Id { get; set; }
        }
        public class Manejador : IRequestHandler<CursoUnico, CursoDto>
        {
            private readonly CursosOnlineContext context;
            private readonly IMapper mapper;
            public Manejador(CursosOnlineContext _context, IMapper _mapper)
            {
                this.context = _context;
                this.mapper = _mapper;
            }
            public async Task<CursoDto> Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                var curso = await context.Curso
                    .Include(x => x.ComentarioLista)
                    .Include(x => x.PrecioPromocion)
                    .Include(x => x.InstructoresLink)
                    .ThenInclude(y => y.Instructor)
                    .FirstOrDefaultAsync(z => z.CursoId == request.Id);
                if (curso == null)
                {
                    throw new Exception("El curso solicitado no existe");
                    //throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No se encontro el Curso" });
                }
                var cursoDto = mapper.Map<Curso, CursoDto>(curso);
                return cursoDto;
            }
        }
    }
}
