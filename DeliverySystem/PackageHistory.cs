//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DeliverySystem
{
    using System;
    using System.Collections.Generic;
    
    public partial class PackageHistory
    {
        public int Id { get; set; }
        public int PackageId { get; set; }
        public short ProcCenterId { get; set; }
        public System.DateTime ProcDate { get; set; }
    
        public virtual Package Package { get; set; }
        public virtual ProcCenter ProcCenter { get; set; }
    }
}
