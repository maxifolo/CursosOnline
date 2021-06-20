using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CursosOnline.Dominio
{
    public class CursoInstructor
    {
        public Guid CursoId { get; set; }
        public Guid InstructorId { get; set; }
        public Curso Curso { get; set; }
        public Instructor Instructor { get; set; }
    }
}
