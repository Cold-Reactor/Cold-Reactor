﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_SIF.Models.EmpleadosN;
using api_SIF.dbContexts;

namespace api_SIF.Controllers
{
    [Route("RH/[controller]")]
    [ApiController]
    public class usuariosController : ControllerBase
    {
        private readonly RHDBContext _context;

        public usuariosController(RHDBContext context)
        {
            _context = context;
        }

        // GET: api/usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<usuario>>> Getusuarios()
        {
            return await _context.usuarios.ToListAsync();
        }

        // GET: api/usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<usuario>> Getusuario(int id)
        {
            var usuario = await _context.usuarios.FindAsync(id);

            if (usuario == null)
            {
                usuario = new usuario();
            }

            return usuario;
        }

        // PUT: api/usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putusuario(int id, usuario usuario)
        {
            if (id != usuario.id_usuario)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!usuarioExists(id))
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

        // POST: api/usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<usuario>> Postusuario(usuario usuario)
        {
            _context.usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getusuario", new { id = usuario.id_usuario }, usuario);
        }

        // DELETE: api/usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deleteusuario(int id)
        {
            var usuario = await _context.usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool usuarioExists(int id)
        {
            return _context.usuarios.Any(e => e.id_usuario == id);
        }
    }
}
