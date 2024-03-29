﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CursosOnline.Dominio
{
    public class Documento
    {
        public Guid DocumentoId { get; set; }
        public Guid ObjetoReferencia { get; set; }
        public string Nombre { get; set; }
        public string Extension { get; set; }
        public byte[] Contenido { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}



