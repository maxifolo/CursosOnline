using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CursosOnline.Persistencia;
using CursosOnline.Dominio;

namespace CursosOnline.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly CursosOnlineContext _context;
        private readonly ILogger<WeatherForecastController> _logger;
        
        public WeatherForecastController(CursosOnlineContext context ,ILogger<WeatherForecastController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Curso> Get()
        {
            return _context.Curso.ToList<Curso>();
        }

    }
}
