using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CursosOnline.Aplicacion.ManejadorError
{
    public class ManejadorExcepcion:Exception
    {
        public HttpStatusCode Codigo { get; }
        public Object Errores { get; }
        public ManejadorExcepcion(HttpStatusCode code, Object errores = null)
        {
            Codigo = code;
            Errores = errores;
        }
    }
}
