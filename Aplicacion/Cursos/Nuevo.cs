using System.Net.Cache;
using CursosOnline.Dominio;
using CursosOnline.Persistencia;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CursosOnline.Aplicacion.Cursos
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {

            //[Required(ErrorMessage="Por favor ingrese el titulo")]
            public Guid? CursoId { get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public List<Guid> ListaInstructor { get; set; }
            public decimal Precio { get; set; }
            public decimal Promocion { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Titulo).NotEmpty().WithMessage("No se ingreso un Titulo para el Curso");
                RuleFor(x => x.Descripcion).NotEmpty().WithMessage("No se ingreso una Descripcion para el Curso");
                RuleFor(x => x.FechaPublicacion).NotEmpty().WithMessage("No se ingreso una Fecha de publicacion para el Curso");
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {

                Guid _cursoID = Guid.NewGuid();
                if(request.CursoId != null){
                    _cursoID = request.CursoId ?? Guid.NewGuid();
                }
                var curso = new Curso
                {
                    CursoId = _cursoID,
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaPublicacion = request.FechaPublicacion,
                    FechaCreacion = DateTime.UtcNow
                };
                _context.Curso.Add(curso);

                if (request.ListaInstructor!=null)
                {
                    foreach (var id in request.ListaInstructor)
                    {
                        var cursoInstructor = new CursoInstructor 
                        { 
                            CursoId = _cursoID,
                            InstructorId = id
                        };
                        _context.CursoInstructor.Add(cursoInstructor);
                    }
                }

                var precioEntidad = new Precio
                {
                    CursoId = _cursoID,
                    PrecioActual = request.Precio,
                    Promocion = request.Promocion,
                    PrecioId = Guid.NewGuid()
                };
                _context.Precio.Add(precioEntidad);

                var valor = await _context.SaveChangesAsync();
                if (valor > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo insertar el nuevo Curso");
            }
        }
    }
}
