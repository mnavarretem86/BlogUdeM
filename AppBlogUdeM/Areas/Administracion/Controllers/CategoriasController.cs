using AppBlogUdeM.AccesoDatos.Data.Repositorio;
using AppBlogUdeM.AccesoDatos.Data.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using AppBlogUdeM.Modelos;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace AppBlogUdeM.Areas.Administracion.Controllers
{

    [Area("Administracion")]
    public class CategoriasController : Controller
    {

        //Se llama al contenedor de trabajo cono inyector de dependencias
        private readonly IContenedorTrabajo _contendorTrabajo;

        //constructor


        public CategoriasController(IContenedorTrabajo contenedorTrabajo)
        {

            _contendorTrabajo = contenedorTrabajo;
        }




        //Método Get
        [HttpGet]


        public IActionResult Index()
        {
            return View();
        }


        #region Formulario Creacion de Categoria

        [HttpGet]
        public IActionResult Create()
        {


			return View();
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categoria categoria)
        {
            // Verifica que el modelo sea válido
            if (!ModelState.IsValid)
            {
                return View(categoria);
            }

            // Verifica si ya existe una categoría con el mismo nombre
            var categoriaConMismoNombre = _contendorTrabajo.Categoria.GetFirstOrDefault(c => c.Nombre.ToLower() == categoria.Nombre.ToLower());
            if (categoriaConMismoNombre != null)
            {
                ModelState.AddModelError("Nombre", "El nombre de la categoría ingresado ya existe. Por favor, ingrese un nombre diferente.");
            }

            // Verifica si ya existe una categoría con el mismo 'Orden'
            var categoriaExistente = _contendorTrabajo.Categoria.GetFirstOrDefault(c => c.Orden == categoria.Orden);
            if (categoriaExistente != null)
            {
                ModelState.AddModelError("Orden", "El orden ingresado ya existe. Por favor, ingrese un orden diferente.");
            }

            // Verifica la validez del modelo después de las validaciones personalizadas
            if (ModelState.IsValid)
            {
                _contendorTrabajo.Categoria.Add(categoria);
                _contendorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }

            // Si hay errores, retorna el modelo a la vista
            return View(categoria);
        }



        //metodo editar
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Categoria categoria = new Categoria();
            categoria = _contendorTrabajo.Categoria.Get(id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                //Logica para actualizar en BD
                _contendorTrabajo.Categoria.Update(categoria);
                _contendorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }









        #endregion






        #region Metodo para borrar categoria



        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var FromDb = _contendorTrabajo.Categoria.Get(id);
            if (FromDb == null)
            {
                return Json(new { success = false, message = "Error borrando categoría" });
            }

            _contendorTrabajo.Categoria.Remove(FromDb);
            _contendorTrabajo.Save();
            return Json(new { success = true, message = "Categoría Borrada Correctamente" });
        }



        #endregion








        #region Llamadas a Datatable


        [HttpGet]

        public IActionResult Getall()
        {

            return Json( new {data = _contendorTrabajo.Categoria.GetAll()} );
        }


        #endregion


    }
}
