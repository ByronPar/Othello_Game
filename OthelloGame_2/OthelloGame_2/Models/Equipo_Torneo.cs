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
    
    public partial class Equipo_Torneo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Equipo_Torneo()
        {
            this.Equipo_T_P = new HashSet<Equipo_T_P>();
        }
    
        public long id_E_T { get; set; }
        public long id_equipo { get; set; }
        public long id_torneo { get; set; }
    
        public virtual Equipo Equipo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Equipo_T_P> Equipo_T_P { get; set; }
        public virtual Torneo Torneo { get; set; }
    }
}
