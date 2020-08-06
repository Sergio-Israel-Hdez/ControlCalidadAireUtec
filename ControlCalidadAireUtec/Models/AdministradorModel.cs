using ControlCalidadAireUtec.Models.BD;
using System.Collections.Generic;
using System.Linq;
using ControlCalidadAireUtec.Hubs;

namespace ControlCalidadAireUtec.Models
{
    public class AdministradorModel
    {
        private ControlAireContext db = new ControlAireContext();

        public IEnumerable<SP_ListaGoogleSheetsAdmin_Result> ListaGoogleSheets(int idusuario)
        {
            var result = db.SP_ListaGoogleSheetsAdmin(idusuario, 1);
            return result;
        }

        public int AgregarGoogleSheet(Sheets sh, int idusuario)
        {
            var result = db.SP_AgregarGoogleSheets(sh.nombre, sh.latitud, sh.longitud, sh.s_key, idusuario);
            EstacionesHub.BroadcastData();
            return result != null ? 1 : 0;
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
    }
}