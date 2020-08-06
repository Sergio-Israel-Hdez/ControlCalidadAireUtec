using ControlCalidadAireUtec.Models;
using ControlCalidadAireUtec.Models.BD;
using PagedList;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControlCalidadAireUtec.Hubs;

namespace ControlCalidadAireUtec.Controllers
{
    public class MasterController : Controller
    {
        private MasterModel masModel = new MasterModel();
        private UsuarioModel usModel = new UsuarioModel();
        private const int pageSize = 6;

        // GET: Master
        public ActionResult Index()
        {
            if (Session["idusuario"] != null)
            {
                if (Convert.ToInt32(Session["rolusuario"]) != 2)
                {
                    Session["idusuario"] = null;
                    Session["rolusuario"] = null;
                    Session["estadousuario"] = null;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var usuariodetalle = usModel.DetalleUsuario(Convert.ToInt32(Session["idusuario"]));
                    var cantEstacionesActivas = masModel.CantidadEstaciones(1);
                    ViewBag.cantEstacionesActivas = cantEstacionesActivas;
                    var cantEstacionesInactivas = masModel.CantidadEstaciones(0);
                    ViewBag.cantEstacionesInactivas = cantEstacionesInactivas;
                    return View(usuariodetalle);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult AdministrarUsuarios(int? paginaUsuarios, int? estado,string nombreusuario)
        {
            if (Session["idusuario"] != null)
            {
                if (Convert.ToInt32(Session["rolusuario"]) != 2)
                {
                    Session["idusuario"] = null;
                    Session["rolusuario"] = null;
                    Session["estadousuario"] = null;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var list = new SelectList(new[] { new { ID = "1", Name = "Activo" }, new { ID = "0", Name = "Inactivo" } }, "ID", "Name", 1);
                    ViewData["list"] = list;
                    int pageNumberUsuarios = (paginaUsuarios ?? 1);
                    int estadof = (estado ?? 1);
                    var result = masModel.ListaUsuarios(estadof);
                    if (!String.IsNullOrEmpty(nombreusuario))
                    {
                        result = result.Where(s => s.Nombre.Contains(nombreusuario));
                    }
                    ViewBag.ListaUsuarios = result.ToPagedList(pageNumberUsuarios, pageSize);

                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult AdministrarEstaciones(int? paginaEstaciones, int? estado,string nombreestacion)
        {
            if (Session["idusuario"] != null)
            {
                if (Convert.ToInt32(Session["rolusuario"]) != 2)
                {
                    Session["idusuario"] = null;
                    Session["rolusuario"] = null;
                    Session["estadousuario"] = null;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var list = new SelectList(new[] { new { ID = "1", Name = "Activo" }, new { ID = "0", Name = "Inactivo" } }, "ID", "Name", 1);
                    ViewData["list"] = list;
                    int pageNumberEstaciones = (paginaEstaciones ?? 1);
                    int estadoF = (estado ?? 1);
                    var result = masModel.listaEstaciones(estadoF);
                    if (!String.IsNullOrEmpty(nombreestacion))
                    {
                        result = result.Where(s => s.Censor.Contains(nombreestacion));
                    }
                    ViewBag.ListaEstaciones = result.ToList().ToPagedList(pageNumberEstaciones, pageSize); ;
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult DetalleGoogleSheetMaster(int? idsheet)
        {
            if (Session["idusuario"] != null)
            {
                if (Convert.ToInt32(Session["rolusuario"]) != 2)
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
                        var result = masModel.DetalleGoogleSheet(Convert.ToInt32(idsheet));
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
        public ActionResult ModificarGoogleSheetMaster(int? idsheet)
        {
            if (Session["idusuario"] != null)
            {
                if (Convert.ToInt32(Session["rolusuario"]) != 2)
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
                        var result = masModel.DetalleGoogleSheet(Convert.ToInt32(idsheet));
                        return View(result);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Master");
                    }
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModificarGoogleSheetMaster(Sheets sheets)
        {
            if (ModelState.IsValid)
            {
                var result = masModel.ModificarGoogleSheet(sheets);
                EstacionesHub.BroadcastData();
                if (result != 0)
                {
                    TempData["msg"] = "1";
                }
                else
                {
                    TempData["msg"] = "2";
                }
                return result != 0 ? RedirectToAction("AdministrarEstaciones") : (ActionResult)View(sheets);
            }
            else
            {
                return View(sheets);
            }
        }

        [HttpGet]
        public ActionResult EliminarGoogleSheetMaster(int? idsheet)
        {
            if (Session["idusuario"] != null)
            {
                if (Convert.ToInt32(Session["rolusuario"]) != 2)
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
                        var result = masModel.EliminarGooglesheet(Convert.ToInt32(idsheet));
                        EstacionesHub.BroadcastData();
                        if (result != 0)
                        {
                            TempData["msg"] = "4";
                        }
                        else
                        {
                            TempData["msg"] = "2";
                        }
                        return result != 0 ? RedirectToAction("AdministrarEstaciones") : RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Master");
                    }
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult EliminarUsuarioMaster(int? idusuario)
        {
            if (Session["idusuario"] != null)
            {
                if (Convert.ToInt32(Session["rolusuario"]) != 2)
                {
                    Session["idusuario"] = null;
                    Session["rolusuario"] = null;
                    Session["estadousuario"] = null;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (idusuario != null)
                    {
                        var result = masModel.EliminarUsuario(Convert.ToInt32(idusuario));
                        if (result != 0)
                        {
                            TempData["msg"] = "4";
                        }
                        else
                        {
                            TempData["msg"] = "2";
                        }
                        return result != 0 ? RedirectToAction("AdministrarUsuarios") : RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Master");
                    }
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult DetalleUsuarioMaster(int? idusuario)
        {
            if (Session["idusuario"] != null)
            {
                if (Convert.ToInt32(Session["rolusuario"]) != 2)
                {
                    Session["idusuario"] = null;
                    Session["rolusuario"] = null;
                    Session["estadousuario"] = null;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (idusuario != null)
                    {
                        var result = masModel.DetalleUsuario(Convert.ToInt32(idusuario));
                        return View(result);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Master");
                    }
                }
            }
            else
            {
                return RedirectToAction("Index", "Master");
            }
        }

        [HttpGet]
        public ActionResult ModificarUsuarioMaster(int? idusuario)
        {
            if (Session["idusuario"] != null)
            {
                if (Convert.ToInt32(Session["rolusuario"]) != 2)
                {
                    Session["idusuario"] = null;
                    Session["rolusuario"] = null;
                    Session["estadousuario"] = null;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (idusuario != null)
                    {
                        var list = new SelectList(new[] { new { ID = "1", Name = "Activo" }, new { ID = "0", Name = "Inactivo" } }, "ID", "Name", 1);
                        ViewData["list"] = list;
                        var result = masModel.DetalleUsuario(Convert.ToInt32(idusuario));
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
        public ActionResult ModificarUsuarioMaster(UsuarioEntity usuario)
        {
            if (ModelState.IsValid)
            {
                var result = masModel.ModificarUsuario(usuario);
                if (result != 0)
                {
                    //TempData["msg"] = "<script>alert('Usuario modificado satifactoriamente');</script>";
                    TempData["msg"] = "1";
                }
                else
                {
                    //TempData["msg"] = "<script>alert('Ocurrio un error');</script>";
                    TempData["msg"] = "2";
                }
                return result != 0 ? RedirectToAction("AdministrarUsuarios") : (ActionResult)View(usuario);
            }
            return View(usuario);
        }

        public ActionResult AgregarAdministrador()
        {
            if (Session["idusuario"] != null)
            {
                if (Convert.ToInt32(Session["rolusuario"]) != 2)
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
                return RedirectToAction("Index", "Master");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarAdministrador(UsuarioEntity usuario)
        {
            if (ModelState.IsValid)
            {
                var resultValidarEmail = masModel.ValidarCorreo(usuario.Email);
                if (!resultValidarEmail)
                {
                    var result = masModel.AgregarUsuario(usuario);
                    if (result != 0)
                    {
                        TempData["msg"] = "3";
                    }
                    else
                    {
                        TempData["msg"] = "2";
                    }
                    return result != 0 ? RedirectToAction("AdministrarUsuarios") : (ActionResult)View(usuario);
                }
                else
                {
                    ViewBag.error = "Correo electronico ya existe";
                }
            }
            return View(usuario);
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