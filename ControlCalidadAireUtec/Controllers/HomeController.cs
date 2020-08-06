using ControlCalidadAireUtec.Models;
using ControlCalidadAireUtec.Models.BD;
using System;
using System.Web.Mvc;
using System.Linq;
using Newtonsoft.Json;

namespace ControlCalidadAireUtec.Controllers
{
    public class HomeController : Controller
    {
        private UsuarioModel usModel = new UsuarioModel();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetEstacionesData()
        {
            ControlAireContext db = new ControlAireContext();
            var resultubi = usModel.ListaGoogleSheetUbi();

            return PartialView("GetEstacionesData", db.Sheets.Where(x => x.Estado == 1).ToList());
        }

        [HttpGet]
        public ActionResult DetalleSensor(int? idsheet)
        {
            if (idsheet != null)
            {
                var result = usModel.DetalleGoogleSheet(Convert.ToInt32(idsheet));
                return View(result);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult GraficoPM(int? idsheet)
        {
            if (idsheet != null)
            {
                var result = usModel.DetalleGoogleSheet(Convert.ToInt32(idsheet));
                return View(result);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public ActionResult GraficoPM10(int? idsheet)
        {
            if (idsheet != null)
            {
                var result = usModel.DetalleGoogleSheet(Convert.ToInt32(idsheet));
                return View(result);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public ActionResult GraficoTemp(int? idsheet)
        {
            if (idsheet != null)
            {
                var result = usModel.DetalleGoogleSheet(Convert.ToInt32(idsheet));
                return View(result);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
       [HttpGet]
       public ActionResult Acerca()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(SP_Validar_Result usuarios)
        {
            if (ModelState.IsValid)
            {
                if (usuarios.Email == null || usuarios.Password == null)
                {
                    TempData["msgHome"] = "2";
                    return View(usuarios);
                }
                else
                {
                    var result = usModel.Validar(usuarios);
                    if (result != null)
                    {
                        int rol, estado;
                        foreach (var item in result)
                        {
                            Session["idusuario"] = item.id;
                            Session["rolusuario"] = item.Rol;
                            Session["estadousuario"] = item.Estado;
                        }
                        rol = Convert.ToInt32(Session["rolusuario"]);

                        estado = Convert.ToInt32(Session["estadousuario"]);
                        if (estado != 0)
                        {
                            if (rol == 1)
                            {
                                //administrador
                                return RedirectToAction("Index", "Administrador");
                            }
                            if (rol == 2)
                            {
                                //Master
                                return RedirectToAction("Index", "Master");
                            }
                        }
                        else
                        {
                            TempData["msgHome"] = "1";
                            return View(usuarios);
                        }
                    }
                    else
                    {
                        TempData["msgHome"] = "1";
                        return View(usuarios);
                    }
                }
            }

            return View(usuarios);
        }
    }
}