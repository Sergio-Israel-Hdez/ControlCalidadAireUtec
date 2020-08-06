using ControlCalidadAireUtec.Models.BD;
using System.Collections.Generic;
using System.Linq;

namespace ControlCalidadAireUtec.Models
{
    public class UsuarioModel
    {
        private ControlAireContext db = new ControlAireContext();

        /// <summary>
        ///
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IEnumerable<SP_Validar_Result> Validar(SP_Validar_Result user)
        {
            var result = db.SP_Validar(user.Email, user.Password);
            return result != null ? result : null;
        }

        public Usuarios DetalleUsuario(int id)
        {
            var result = db.Usuarios.Find(id);
            return result;
        }

        public IEnumerable<Sheets> ListaGoogleSheetUbi()
        {
            var result = from list in db.Sheets where list.Estado == 1 select list;
            return result;
        }

        /// <summary>
        /// parametro id es el identificador del registro de google sheet
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sheets DetalleGoogleSheet(int id)
        {
            var result = db.Sheets.Where(x => x.id == id).FirstOrDefault();
            return result;
        }
    }
}