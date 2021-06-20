using CursosOnline.Persistencia.DapperConexion.Instructor;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CursosOnline.Aplicacion.Instructores
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Grado { get; set; }
        }
        public class EjecutaValida : AbstractValidator<Ejecuta>
        {
            public EjecutaValida()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellido).NotEmpty();
                RuleFor(x => x.Grado).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly IInstructor _instructorRepository;
            public Manejador(IInstructor instructor)
            {
                _instructorRepository = instructor;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var instructorNuevo = new InstructorModel
                {
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    Grado = request.Grado
                };
                var resultado = await _instructorRepository.Nuevo(instructorNuevo);
                if (resultado > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo ingresar el instructor");
            }
        }
    }
}
