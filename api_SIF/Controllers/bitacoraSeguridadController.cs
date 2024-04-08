﻿using api_SIF.Clases;
using api_SIF.dbContexts;
using api_SIF.Models;
using api_SIF.Models.Empleados;
using api_SIF.Models.EmpleadosN;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_SIF.Controllers
{
    [Route("RH/[controller]")]
    [ApiController]
    public class bitacoraSeguridadController : ControllerBase
    {
        private readonly RHDBContext _context;

        public bitacoraSeguridadController(RHDBContext context)
        {
            _context = context;
        }

        [HttpGet("{sucursal}/{from1}/{to}")]
        public async Task<ActionResult<bitacoraSeguridad>> GetBitacora(int sucursal, DateTime from1, DateTime to)
        {

            List<bitacoraSeguridad> check = (from c in _context.bitacoras
                                             join a in _context.empleados on c.registro equals a.id_empleado
                                             where c.fecha >= from1 && c.fecha <= to && a.id_sucursal == sucursal
                                             select new bitacoraSeguridad
                                             {
                                                 id_bitacoraS = c.id_bitacoraS,
                                                 fecha = c.fecha,
                                                 descripcion = c.descripcion,
                                                 imagen = c.imagen,
                                                 relevante = c.relevante,
                                                 registro = c.registro

                                             }).ToList();
            return Ok(check);
        }
        [HttpPost]
        public async Task<ActionResult<bitacoraSeguridad[]>> PostBitacora(bitacoraSeguridad bitacora)
        {
            _context.bitacoras.Add(bitacora);
            await _context.SaveChangesAsync();
            bitacora.id_bitacoraS = Funciones.ObtenerUltimoId<bitacoraSeguridad>(_context); ;
            return Ok(new { id = bitacora.id_bitacoraS });
        }
    }
}