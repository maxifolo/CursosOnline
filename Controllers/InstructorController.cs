using CursosOnline.Aplicacion.Instructores;
using CursosOnline.Persistencia.DapperConexion.Instructor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CursosOnline.Controllers
{
    public class InstructorController : MiControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<InstructorModel>>> ObtenerInstructores()
        {
            return await Mediator.Send(new Consulta.Lista()).ConfigureAwait(true);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<InstructorModel>> ObtenerPorId(Guid id)
        {
            return await Mediator.Send(new ConsultaId.Ejecuta { Id = id}).ConfigureAwait(true);
        }
        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta parametros)
        {
            return await Mediator.Send(parametros).ConfigureAwait(true);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Editar(Guid id, Editar.Ejecuta parametros)
        {
            parametros.InstructorId = id;
            return await Mediator.Send(parametros).ConfigureAwait(true);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(Guid id)
        {
            return await Mediator.Send(new Eliminar.Ejecuta { Id = id }).ConfigureAwait(true);
        }
    }
}
