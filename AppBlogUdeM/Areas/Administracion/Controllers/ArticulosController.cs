using AppBlogUdeM.AccesoDatos.Data.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Mvc;

namespace AppBlogUdeM.Areas.Administracion.Controllers
{


    [Area("Administracion")]
    public class ArticulosController : Controller
    {
        //Se llama al contenedor de trabajo cono inyector de dependencias
        private readonly IContenedorTrabajo _contendorTrabajo;

        //constructor

        public ArticulosController(IContenedorTrabajo contenedorTrabajo)
        {

            _contendorTrabajo = contenedorTrabajo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }




        #region Llamadas a Datatable


        [HttpGet]

        public IActionResult Getall()
        {

            return Json(new { data = _contendorTrabajo.Articulo.GetAll(includeProperties: "Categoria") });
        }


        #endregion


    }
}
