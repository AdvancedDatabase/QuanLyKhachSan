//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IvivuCustomer.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class KhachSan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KhachSan()
        {
            this.LoaiPhongs = new HashSet<LoaiPhong>();
            this.NhanViens = new HashSet<NhanVien>();
        }
    
        public byte maKS { get; set; }
        public string tenKS { get; set; }
        public byte soSao { get; set; }
        public string soNha { get; set; }
        public string duong { get; set; }
        public string quan { get; set; }
        public string thanhPho { get; set; }
        public Nullable<int> giaTB { get; set; }
        public string moTa { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LoaiPhong> LoaiPhongs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NhanVien> NhanViens { get; set; }
    }
}
