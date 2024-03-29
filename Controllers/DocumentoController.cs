﻿using CursosOnline.Aplicacion.Documentos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CursosOnline.Controllers
{
    public class DocumentoController : MiControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<ArchivoGenerico>> ObtenerDocumento(Guid id)
        {
            return await Mediator.Send(new ObtenerArchivo.Ejecuta { Id = id });
        }
        [HttpPost]
        public async Task<ActionResult<Unit>> GuardarArchivo(SubirArchivo.Ejecuta parametros)
        {
            return await Mediator.Send(parametros);
        }
    }
}


