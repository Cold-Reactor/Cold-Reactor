﻿using api_SIF.dbContexts;
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
    public class sucursalesController : ControllerBase
    {
        private readonly RHDBContext _context;
        public sucursalesController(RHDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<requestSucursales>>> GetSucursales()
        {
            var sucursalesLista = from x in  _context.sucursales
                                  select new requestSucursales()
                                  {
                                      id_sucursal = x.id_sucursal,
                                      sucursal = x.sucursal
                                  }
                                 
                            ;

            return await sucursalesLista.ToListAsync();
            //return await _context.empleados.ToListAsync();
        }
    }
}
