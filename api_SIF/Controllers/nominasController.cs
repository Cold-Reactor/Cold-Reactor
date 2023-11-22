﻿using api_SIF.Clases;
using api_SIF.dbContexts;
using api_SIF.Models.EmpleadosN;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_SIF.Controllers
{
    [Route("RH/[controller]")]
    [ApiController]
    public class nominasController : ControllerBase
    {

        private readonly RHDBContext _context;
        public nominasController(RHDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Nomina>>> Getnominas()
        {
            var nominasLista = _context.nominas;
            return await nominasLista.ToListAsync();
        }
        [HttpGet("{nombre}")]
        public async Task<ActionResult<Nomina>> GetNomina(string nombre)
        {
            var nominaS = _context.nominas.SingleOrDefault(x => x.nomina == nombre);
            if (nominaS == null)
            {
                return NotFound();
            }

            return nominaS;
        }
        [HttpPost]
        public async Task<ActionResult<Nomina>> PostNomina(Nomina reqNomina)
        {
            var entidadExistente = _context.nominas.SingleOrDefault(e => e.id_nomina == reqNomina.id_nomina);
            if (entidadExistente == null)
            {               
                _context.nominas.Add(reqNomina);
                _context.SaveChanges();
                reqNomina.id_nomina = Funciones.ObtenerUltimoId<Nomina>(_context);
            }
            else
            {
                reqNomina.id_nomina = entidadExistente.id_nomina;
                entidadExistente.nomina = reqNomina.nomina;
                _context.SaveChanges();
            }
            return Ok(new { id = reqNomina.id_nomina });
        }
    }
}
