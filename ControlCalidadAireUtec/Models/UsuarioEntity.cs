namespace ControlCalidadAireUtec.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class UsuarioEntity
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellidos { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Telefono { get; set; }

        public Nullable<int> Estado { get; set; }
        public int Rol { get; set; }
    }
}