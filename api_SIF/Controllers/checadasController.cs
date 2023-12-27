﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_SIF.Models.EmpleadosN;
using api_SIF.dbContexts;
using Microsoft.AspNetCore.Authorization;
using System.Net.NetworkInformation;

namespace api_SIF.Controllers
{
    [Route("RH/[controller]")]
    [ApiController]
    public class checadasController : ControllerBase
    {
        private readonly RHDBContext _context;

        public checadasController(RHDBContext context)
        {
            _context = context;
        }
        // GET: api/checadas
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<requestChecadas>>> Getchecadas()
        //{
        //    var checadoresLista = from x in _context.checadas
        //                          select new requestChecadas()
        //                          {
        //                             id_checador = x.id_checador,
        //                             nomina= x.nomina,
        //                             fecha= x.fecha,
        //                             fechaHora= x.fechaHora,
        //                             hora= x.hora,  
        //                             id_checada = x.id_checada    ,
        //                             id_empleado = x.id_empleado,
        //                          };
        //    return await checadoresLista.ToListAsync();
        //}

        // GET: api/checadas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<requestChecadas>> Getchecada(int id)
        {
            var checada = await _context.checadas.FindAsync(id);
            if (checada == null)
            {
                return NotFound();
            }
            var reqChecada =  new requestChecadas()
            {
                id_checador = checada.id_checador,
                nomina = checada.nomina,
                fecha = checada.fecha,
                fechaHora = checada.fechaHora,
                hora = checada.hora,
                id_checada = checada.id_checada,
                id_empleado = checada.id_empleado,
            };
            return reqChecada;
        }

        [HttpGet("getEmpleadoChecadas/{from1}/{to}/{sucursal}/{area}/{no_empleado}/{id_turno_request}")]
        public async Task<ActionResult<IEnumerable<requestEmpleadoChecadas>>> GetEmpleadoChecadasint (DateOnly from1, DateOnly to, int sucursal, string area, string no_empleado, string id_turno_request)
        {
            List<DateOnly> fechas = new List<DateOnly>();
            DateOnly date1 = from1;
            while (date1>=from1 && date1<=to)
            {
                fechas.Add(date1);
                date1= date1.AddDays(1);

            }
            List<requestCheck> check = (from c in _context.checadas
                                         where c.fecha >=from1 && c.fecha<=to
                                         select new requestCheck
                                         {
                                             id_checada = c.id_checada,
                                             id_checador = c.id_checador,
                                             fecha = c.fecha,
                                             hora = c.hora.ToString("HH:mm:ss"),
                                             horaM = "",
                                             nomina = (int)c.nomina,
                                             id_empleado = c.id_empleado

                                         }).ToList();
            check = check.OrderBy(x => x.hora).ToList();
            List<requestChecadaCheck> checadas1 = new List<requestChecadaCheck>();
            var turnos = (from p in _context.turnos
                          select new turno
                          {
                              id_turno = p.id_turno,
                              descanso = p.descanso,
                              entrada = p.entrada,
                              entradaF = p.entradaF,
                              turno1 = p.turno1,
                              salida = p.salida,
                              salidaF = p.salida
                          }
                         ).ToList();
                         ;
            var empleados = (from p in _context.empleados                              
                             where p.status==1 && p.id_sucursal==sucursal
                             select new requestEmpleadoChecadas
                             {
                                 id_turno = p.id_turno,
                                 id_empleado = p.id_empleado,
                                 nombre = p.apellidoPaterno + " " + p.apellidoMaterno + " " + p.nombre,
                                 noEmpleado = p.no_empleado,
                                 sucursal = p.id_sucursal,
                                 turno = p.id_turno.ToString(),
                                 area = p.id_area.ToString(),
                                 confianza = (int)p.confianza,
                             }).ToList();

            if (int.TryParse(no_empleado, out int result1))
            {
                if (result1 > 0)
                {
                    empleados = empleados.Where(x => x.noEmpleado == result1).ToList();
                }
            }
            if (int.TryParse(area, out int result2))
            {
                if (result2 > 0)
                {

                    empleados = empleados.Where(x => x.id_area == result2).ToList();

                }
            }
            if (int.TryParse(id_turno_request, out int result3))
            {
                if (result3 > 0)
                {
                    empleados = empleados.Where(x => x.id_turno == result3).ToList();
                }
            }
            foreach (var empleado in empleados)
            {
                foreach (var turno in turnos)
                {
                    if (turno.id_turno == Convert.ToInt32(empleado.turno))
                    {
                        empleado.turno = turno.turno1;
                        break;
                    }
                }
                var checadasEmpleados = new List<requestChecadaCheck>();
                foreach (var fecha in fechas)
                {
                    var checadaEmpleado = new requestChecadaCheck();
                    checadaEmpleado.fecha = fecha;
                    checadaEmpleado.extra = 0;
                    checadaEmpleado.ausentismo = "";                    
                    var checksEmpleado = new List<requestCheck>();
                    foreach (var chec in check) {
                        if (chec.fecha==fecha) {
                            if (empleado.id_empleado == chec.id_empleado)
                            {
                                requestCheck ch = new requestCheck();
                                ch.id_checada = chec.id_checada;
                                ch.id_checador = chec.id_checador;
                                ch.fecha = chec.fecha;
                                ch.hora = chec.hora;
                                ch.horaM = chec.horaM;
                                ch.nomina = chec.nomina;
                                ch.id_empleado = chec.id_empleado;
                                checksEmpleado.Add(ch);
                            }
                        }
                     }
                    checadaEmpleado.check = checksEmpleado;
                    checadasEmpleados.Add(checadaEmpleado);                    
                }                
                empleado.checadas = checadasEmpleados;
            }            
            return empleados;
        }
        // PUT: api/checadas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putchecada(int id, checada checada)
        {
            var entity = _context.checadas.FirstOrDefault(item => item.id_checada == id);

            // Validate entity is not null
            if (entity != null)
            {
                entity.fecha = checada.fecha;                
                entity.hora = checada.hora;
                entity.nomina = checada.nomina;
                _context.SaveChanges();
            }
            return Ok();
        }

        // POST: api/checadas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[Authorize]
        [HttpPost]
        public async Task<ActionResult<checada>> Postchecada(checada checada)
        {
            _context.checadas.Add(checada);
            await _context.SaveChangesAsync();

            return Ok(new { id = checada.id_checada });
        }

        // DELETE: api/checadas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletechecada(int id)
        {
            var checada = await _context.checadas.FindAsync(id);
            if (checada == null)
            {
                return NotFound();
            }

            _context.checadas.Remove(checada);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool checadaExists(int id)
        {
            return _context.checadas.Any(e => e.id_checada == id);
        }
    }
}
