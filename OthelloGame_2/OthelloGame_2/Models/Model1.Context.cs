﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OthelloGame_2.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DataBase : DbContext
    {
        public DataBase()
            : base("name=DataBase")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Color> Color { get; set; }
        public virtual DbSet<Equipo> Equipo { get; set; }
        public virtual DbSet<Equipo_Jugador> Equipo_Jugador { get; set; }
        public virtual DbSet<Equipo_T_P> Equipo_T_P { get; set; }
        public virtual DbSet<Equipo_Torneo> Equipo_Torneo { get; set; }
        public virtual DbSet<Ficha> Ficha { get; set; }
        public virtual DbSet<Jugador> Jugador { get; set; }
        public virtual DbSet<Jugador_P_C> Jugador_P_C { get; set; }
        public virtual DbSet<Jugador_Partida> Jugador_Partida { get; set; }
        public virtual DbSet<Pais> Pais { get; set; }
        public virtual DbSet<Partida> Partida { get; set; }
        public virtual DbSet<Ronda> Ronda { get; set; }
        public virtual DbSet<Tipo_Partida> Tipo_Partida { get; set; }
        public virtual DbSet<Torneo> Torneo { get; set; }
    }
}
