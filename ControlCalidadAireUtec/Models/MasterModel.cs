using ControlCalidadAireUtec.Models.BD;
using System.Collections.Generic;
using System.Linq;
using ControlCalidadAireUtec.Hubs;

namespace ControlCalidadAireUtec.Models
{
    public class MasterModel
    {
        private ControlAireContext db = new ControlAireContext();

        //administracion de estaciones

        public IEnumerable<SP_ListaGoogleSheetsMaster_Result> listaEstaciones(int estado)
        {
            var result = db.SP_ListaGoogleSheetsMaster(estado);
            return result;
        }

        public int EliminarGooglesheet(int idsheet)
        {
            var result = db.SP_EliminarGoogleSheets(idsheet);
            EstacionesHub.BroadcastData();
            return result != null ? 1 : 0;
        }

        public Sheets DetalleGoogleSheet(int idsheet)
        {
            var result = db.Sheets.Where(x => x.id == idsheet).FirstOrDefault();
            return result;
        }

        public int ModificarGoogleSheet(Sheets she)
        {
            var result = db.SP_ModificarGoogleSheets(she.id, she.nombre, she.s_key, she.latitud, she.longitud, she.Estado);
            EstacionesHub.BroadcastData();
            return result != null ? 1 : 0;
        }

        //administracion de usuarios
        public IEnumerable<UsuarioEntity> ListaUsuarios(int estado)
        {
            var result = db.Usuarios.Select(x => new UsuarioEntity { id = x.id, Nombre = x.Nombre, Apellidos = x.Apellidos, Email = x.Email, Telefono = x.Telefono, Estado = x.Estado, Rol = x.Rol }).Where(x => x.Rol == 1 && x.Estado == estado).OrderBy(x => x.id);
            return result;
        }

        public bool ValidarCorreo(string Email)
        {
            var result = db.Usuarios.Select(x => x.Email == Email).FirstOrDefault();
            return result;
        }

        public int AgregarUsuario(UsuarioEntity usuario)
        {
            var result = db.SP_AgregarUsuario(usuario.Nombre, usuario.Apellidos, usuario.Email, usuario.Password, usuario.Telefono, 1);
            return result != null ? 1 : 0;
        }

        public SP_DetalleUsuario_Result DetalleUsuario(int idusuario)
        {
            var result = db.SP_DetalleUsuario(idusuario).First();
            return result;
        }

        public int ModificarUsuario(UsuarioEntity u)
        {
            var result = db.SP_ModificarUsuario(u.id, u.Nombre, u.Apellidos, u.Email, u.Password, u.Telefono, u.Estado);
            return result != 0 ? 1 : 0;
        }

        public int CantidadEstaciones(int estado)
        {
            var result = db.Sheets.Where(x => x.Estado == estado);
            int cantidad = result.Count();
            return cantidad;
        }

        public int EliminarUsuario(int idusuario)
        {
            Usuarios user = db.Usuarios.Find(idusuario);
            if (user != null)
            {
                user.Estado = 0;
                var result = db.SaveChanges();
                return result != 0 ? 1 : 0;
            }
            else
            {
                return 0;
            }
        }
    }
}