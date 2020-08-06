using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ControlCalidadAireUtec.Models.BD
{
    [MetadataType(typeof(ValidationSheets))]
    public partial class ValidationSheets
    {
        public int id { get; set; }
        [Required]
        [Display(AutoGenerateField = false)]
        public string nombre { get; set; }
        [Required]
        public string latitud { get; set; }
        [Required]
        public string longitud { get; set; }
        [Required]
        [Display(Name = "llave", AutoGenerateField = false)]
        public string s_key { get; set; }
        public Nullable<int> idUser { get; set; }
        public Nullable<int> Estado { get; set; }

        public virtual Usuarios Usuarios { get; set; }
    }
}