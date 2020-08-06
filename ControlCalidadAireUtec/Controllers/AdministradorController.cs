using ControlCalidadAireUtec.Models;
using ControlCalidadAireUtec.Models.BD;
using PagedList;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControlCalidadAireUtec.Controllers
{
    public class AdministradorController : Controller
    {
        private AdministradorModel adminModel = new AdministradorModel();
        private const int PageSize = 6;

        // GET: Administrador
        public ActionResult Index(int? paginaEstaciones,string nombreestacion,string orden)
        {
            int pageNumberEstaciones = (paginaEstaciones ?? 1);
            ViewBag.ordennombre = String.IsNullOrEmpty(orden) ? "name_desc" : "";
            ViewBag.ordenid = orden == "id" ? "id_desc" : "id";
            if (Session["idusuario"] != null)
            {
                if (Convert.ToInt32(Session["rolusuario"]) != 1)
                {
                    Session["idusuario"] = null;
                    Session["rolusuario"] = null;
                    Session["estadousuario"] = null;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var result = adminModel.ListaGoogleSheets(Convert.ToInt32(Session["idusuario"]));
                    if (!String.IsNullOrEmpty(nombreestacion))
                    {
                        result = result.Where(s => s.Censor.Contains(nombreestacion));
                    }
                    switch (orden)
                    {   
                        case "name_desc":
                            result = result.OrderByDescending(s => s.Censor);
                            break;
                        case "id":
                            result = result.OrderBy(s => s.id);
                            break;
                        case "id_desc":
                            result = result.OrderByDescending(s => s.id);
                            break;
                        default:
                            result = result.OrderBy(s => s.Censor);
                            break;
                    }
                    ViewBag.Mysheets = result.ToList().ToPagedList(pageNumberEstaciones, PageSize);
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult AgregarGoogleSheet()
        {
            if (Session["idusuario"] != null)
            {
                if (Convert.ToInt32(Session["rolusuario"]) != 1)
                {
                    Session["idusuario"] = null;
                    Session["rolusuario"] = null;
                    Session["estadousuario"] = null;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index", "Administrador");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarGoogleSheet(Sheets sheets)
        {
            if (ModelState.IsValid)
            {
                int id = Convert.ToInt32(Session["idusuario"]);
                var result = adminModel.AgregarGoogleSheet(sheets, id);
                if (result != 0)
                {
                    TempData["msgAdmin"] = "3";
                }
                else
                {
                    TempData["msgAdmin"] = "2";
                }
                return result != 0 ? RedirectToAction("Index") : (ActionResult)View(sheets);
            }
            return View(sheets);
        }

        [HttpGet]
        public ActionResult DetalleGoogleSheet(int? idsheet)
        {
            if (Session["idusuario"] != null)
            {
                if (Convert.ToInt32(Session["rolusuario"]) != 1)
                {
                    Session["idusuario"] = null;
                    Session["rolusuario"] = null;
                    Session["estadousuario"] = null;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (idsheet != null)
                    {
                        var result = adminModel.DetalleGoogleSheet(Convert.ToInt32(idsheet));
                        return View(result);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Administrador");
                    }
                }
            }
            else
            {
                return RedirectToAction("Index", "Administrador");
            }
        }

        [HttpGet]
        public ActionResult ModificarGoogleSheet(int? idsheet)
        {
            if (Session["idusuario"] != null)
            {
                if (Convert.ToInt32(Session["rolusuario"]) != 1)
                {
                    Session["idusuario"] = null;
                    Session["rolusuario"] = null;
                    Session["estadousuario"] = null;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (idsheet != null)
                    {
                        var list = new SelectList(new[] { new { ID = "1", Name = "Activo" }, new { ID = "0", Name = "Inactivo" } }, "ID", "Name", 1);
                        ViewData["list"] = list;
                        var result = adminModel.DetalleGoogleSheet(Convert.ToInt32(idsheet));
                        return View(result);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Administrador");
                    }
                }
            }
            else
            {
                return RedirectToAction("Index", "Administrador");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModificarGoogleSheet(Sheets sheets)
        {
            if (ModelState.IsValid)
            {
                var result = adminModel.ModificarGoogleSheet(sheets);

                if (result != 0)
                {
                    TempData["msgAdmin"] = "1";
                }
                else
                {
                    TempData["msgAdmin"] = "2";
                }
                return result != 0 ? RedirectToAction("Index") : (ActionResult)View(sheets);
            }
            return View(sheets);
        }

        [HttpGet]
        public ActionResult EliminarGoogleSheet(int? idsheet)
        {
            if (Session["idusuario"] != null)
            {
                if (idsheet != null)
                {
                    var result = adminModel.EliminarGooglesheet(Convert.ToInt32(idsheet));

                    if (result != 0)
                    {
                        TempData["msgAdmin"] = "4";
                    }
                    else
                    {
                        TempData["msgAdmin"] = "2";
                    }
                    return result != 0 ? RedirectToAction("Index") : RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index", "Administrador");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public ActionResult GraficoPMAdmin(int? idsheet)
        {
            if (idsheet != null)
            {
                var result = adminModel.DetalleGoogleSheet(Convert.ToInt32(idsheet));
                return View(result);
            }
            else
            {
                return RedirectToAction("Index", "Administrador");
            }
        }
        [HttpGet]
        public ActionResult GraficoPM10Admin(int? idsheet)
        {
            if (idsheet != null)
            {
                var result = adminModel.DetalleGoogleSheet(Convert.ToInt32(idsheet));
                return View(result);
            }
            else
            {
                return RedirectToAction("Index", "Administrador");
            }
        }
        [HttpGet]
        public ActionResult GraficoTempAdmin(int? idsheet)
        {
            if (idsheet != null)
            {
                var result = adminModel.DetalleGoogleSheet(Convert.ToInt32(idsheet));
                return View(result);
            }
            else
            {
                return RedirectToAction("Index", "Administrador");
            }
        }

        public ActionResult Desconectar()
        {
            Session["idusuario"] = null;
            Session["rolusuario"] = null;
            Session["estadousuario"] = null;

            Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
            Response.Cache.SetNoStore();

            return RedirectToAction("Index", "Home");
        }
    }
}