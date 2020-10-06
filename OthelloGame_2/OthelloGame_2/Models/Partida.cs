//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Partida
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Partida()
        {
            this.Partida1 = new HashSet<Partida>();
            this.Torneo_Partida = new HashSet<Torneo_Partida>();
        }
    
        public int id_partida { get; set; }
        public int movimientos { get; set; }
        public string color { get; set; }
        public int cantidad_fichas { get; set; }
        public string ganador { get; set; }
        public string id_usuario { get; set; }
        public int id_tipo_partida { get; set; }
        public Nullable<int> id_jugador_2 { get; set; }
    
        public virtual Jugador Jugador { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Partida> Partida1 { get; set; }
        public virtual Partida Partida2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Torneo_Partida> Torneo_Partida { get; set; }
        public virtual Tipo_Partida Tipo_Partida { get; set; }
    }
}
