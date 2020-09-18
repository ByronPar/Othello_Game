//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Othello_Game.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Partida
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Partida()
        {
            this.Ficha = new HashSet<Ficha>();
            this.Jugador_Partida = new HashSet<Jugador_Partida>();
        }
    
        public int id_partida { get; set; }
        public string color_siguiente { get; set; }
        public string id_GanadorPartida { get; set; }
        public int id_tipo_partida { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ficha> Ficha { get; set; }
        public virtual Jugador Jugador { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Jugador_Partida> Jugador_Partida { get; set; }
        public virtual Tipo_Partida Tipo_Partida { get; set; }
    }
}