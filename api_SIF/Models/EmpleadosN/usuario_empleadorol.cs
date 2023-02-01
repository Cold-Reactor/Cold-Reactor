﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_SIF.Models.EmpleadosN
{
    [Table("usuario_empleadorol")]
    [Index("id_sucursales", Name = "fk_usuario_empleadoRol_sucursales1_idx")]
    [Index("id_crud", Name = "fk_usuario_has_empleadoRol_crud1_idx")]
    [Index("id_rol", Name = "fk_usuario_has_empleadoRol_empleadoRol1_idx")]
    [Index("id_usuario", Name = "fk_usuario_has_empleadoRol_usuario1_idx")]
    public partial class usuario_empleadorol
    {
        [Key]
        [Column(TypeName = "int(11)")]
        public int id_usuario { get; set; }
        [Column(TypeName = "int(11)")]
        public int id_rol { get; set; }
        [Column(TypeName = "int(11)")]
        public int? id_subM { get; set; }
        [Column(TypeName = "int(11)")]
        public int? id_crud { get; set; }
        [Column(TypeName = "int(11)")]
        public int? id_sucursales { get; set; }
        [Column(TypeName = "int(1)")]
        public int? master { get; set; }

        [ForeignKey("id_crud")]
        [InverseProperty("usuario_empleadorols")]
        public virtual crud id_crudNavigation { get; set; }
        [ForeignKey("id_rol")]
        [InverseProperty("usuario_empleadorols")]
        public virtual usuariorol id_rolNavigation { get; set; }
        [ForeignKey("id_sucursales")]
        [InverseProperty("usuario_empleadorols")]
        public virtual sucursale id_sucursalesNavigation { get; set; }
        [ForeignKey("id_usuario")]
        [InverseProperty("usuario_empleadorol")]
        public virtual usuario id_usuarioNavigation { get; set; }
    }
}
