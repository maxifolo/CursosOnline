using CursosOnline.Aplicacion.ManejadorError;
using CursosOnline.Persistencia.DapperConexion.Instructor;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CursosOnline.Aplicacion.Instructores
{
    public class ConsultaId
    {
        public class Ejecuta : IRequest<InstructorModel>
        {
            public Guid Id { get; set; }
        }
        public class Manejador : IRequestHandler<Ejecuta, InstructorModel>
        {
            private readonly IInstructor _instructorRepository;
            public Manejador(IInstructor instructor)
            {
                _instructorRepository = instructor;
            }

            public async Task<InstructorModel> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                InstructorModel instructor = await _instructorRepository.ObtenerPorId(request.Id);
                if (instructor != null)
                {
                    return instructor;
                }
                throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No se pudo encontrar el instructor" });
            }
        }
    }
}
