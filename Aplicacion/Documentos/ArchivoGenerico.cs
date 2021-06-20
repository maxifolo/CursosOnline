using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CursosOnline.Aplicacion.Documentos
{
    public class ArchivoGenerico
    {
        public Guid? ObjetoReferencia { get; set; }
        public Guid? DocumentoId { get; set; }
        public string Nombre { get; set; }
        public string Extension { get; set; }
        public string Data { get; set; }
    }
}

