﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_SIF.Models.EmpleadosN;
using api_SIF.dbContexts;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace api_SIF.Controllers
{
    [Route("RH/[controller]")]
    [ApiController]
    public class empleadosController : ControllerBase
    {
        private readonly RHDBContext _context;

        public empleadosController(RHDBContext context)
        {
            _context = context;
        }

        // GET: api/empleados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<empleadoRequest>>> Getempleados()
        {
            var empleadosLista = from x in _context.empleados
                        select new empleadoRequest()
                        {

                            id_empleado = x.id_empleado,
                            no_empleado = x.no_empleado,
                            nombre = x.nombre,
                            apellidoPaterno = x.apellidoPaterno,
                            apellidoMaterno = x.apellidoMaterno,
                            estadoCivil = x.estadoCivil,
                            sexo = x.sexo,
                            fechaNacimiento = x.fechaNacimiento,
                            IMSS = x.IMSS,
                            telefono = x.telefono,
                            telefonoEmergencias = x.telefonoEmergencias,
                            email = x.email,
                            CURP = x.CURP,
                            RFC = x.RFC,
                            id_ciudad = x.id_ciudad,
                            id_estado = x.id_estado,
                            direccion = x.direccion,
                            CP = x.CP,
                            gradoEstudios = x.gradoEstudios,
                            carrera = x.carrera,
                            instituto = x.instituto,
                            titulo = x.titulo,
                            id_empleadoTipo = x.id_empleadoTipo,
                            id_puesto = x.id_puesto,
                            jefeInmediato = x.jefeInmediato,
                            id_turno = x.id_turno,
                            salarioDiario = x.salarioDiario,
                            id_nomina = x.id_nomina,
                            fechaIngreso = x.fechaIngreso,
                            id_empresa = x.id_empresa,
                            id_sucursal = x.id_sucursal,
                            presencial = x.presencial,
                            parentesco = x.parentesco,
                            imagen = x.imagen,
                            firma = x.firma,
                            id_rol = x.id_rol,
                            status = x.status,
                            externo = x.externo
                        };

            return await empleadosLista.ToListAsync();
            //return await _context.empleados.ToListAsync();
        }

        // GET: api/empleados/5
        [HttpGet("{id}")]
        public async Task<ActionResult<empleado>> Getempleado(int id)
        {
            var empleado = await _context.empleados.FindAsync(id);

            if (empleado == null)
            {
                return NotFound();
            }

            return empleado;
        }

        // PUT: api/empleados/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putempleado(int id, empleado empleado)
        {
            if (id != empleado.id_empleado)
            {
                return BadRequest();
            }

            _context.Entry(empleado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!empleadoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/empleados
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<empleado>> Postempleado(empleado empleado)
        {
            _context.empleados.Add(empleado);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getempleado", new { id = empleado.id_empleado }, empleado);
        }

        // DELETE: api/empleados/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deleteempleado(int id)
        {
            var empleado = await _context.empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }

            _context.empleados.Remove(empleado);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool empleadoExists(int id)
        {
            return _context.empleados.Any(e => e.id_empleado == id);
        }
    }
}
