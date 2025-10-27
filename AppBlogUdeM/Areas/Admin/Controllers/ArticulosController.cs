using System.Drawing;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using BlogCore.Models;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrador,Moderador")]
    [Area("Admin")]
    public class ArticulosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager; // Inyección de UserManager

        public ArticulosController(IContenedorTrabajo contenedorTrabajo,
            IWebHostEnvironment hostingEnvironment,
            UserManager<ApplicationUser> userManager) // Añadido como parámetro
        {
            _contenedorTrabajo = contenedorTrabajo;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager; // Asignación
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ArticuloVM artiVM = new ArticuloVM()
            {
                Articulo = new BlogCore.Models.Articulo(),
                ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()
            };

            return View(artiVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArticuloVM artiVM)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;

                if (artiVM.Articulo.Id == 0 && archivos.Count() > 0)
                {
                    // Validar las extensiones de imagen
                    var extension = Path.GetExtension(archivos[0].FileName).ToLower();
                    if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                    {
                        ModelState.AddModelError("Imagen", "Solo se permiten imágenes JPG, JPEG y PNG");
                        artiVM.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();
                        return View(artiVM);
                    }

                    // Nuevo artículo
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");

                    // Redimensionar la imagen
                    using (var memoryStream = new MemoryStream())
                    {
                        archivos[0].CopyTo(memoryStream);
                        using (var imagenOriginal = System.Drawing.Image.FromStream(memoryStream))
                        {
                            // Definir tamaño de la imagen redimensionada
                            int ancho = 1024;
                            int alto = 512;

                            using (var imagenRedimensionada = new Bitmap(imagenOriginal, new Size(ancho, alto)))
                            {
                                string rutaCompleta = Path.Combine(subidas, nombreArchivo + extension);
                                imagenRedimensionada.Save(rutaCompleta); // Guardar la imagen redimensionada

                                artiVM.Articulo.UrlImagen = @"\imagenes\articulos\" + nombreArchivo + extension;
                            }
                        }
                    }

                    // Asigna la fecha de creación y el creador
                    artiVM.Articulo.FechaCreacion = DateTime.Now.ToString();
                    var currentUser = await _userManager.GetUserAsync(User); // Obtener el usuario actual
                    artiVM.Articulo.Creador = currentUser?.Nombre; // Asignar el nombre de usuario

                    _contenedorTrabajo.Articulo.Add(artiVM.Articulo);
                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Imagen", "Debes seleccionar una imagen");
                }
            }

            artiVM.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();
            return View(artiVM);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ArticuloVM artiVM = new ArticuloVM()
            {
                Articulo = new BlogCore.Models.Articulo(),
                ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()
            };

            if (id != null)
            {
                artiVM.Articulo = _contenedorTrabajo.Articulo.Get(id.GetValueOrDefault());
            }

            return View(artiVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ArticuloVM artiVM)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;

                var articuloDesdeBd = _contenedorTrabajo.Articulo.Get(artiVM.Articulo.Id);

                if (archivos.Count() > 0)
                {
                    // Nuevo imagen para el artículo
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");
                    var extension = Path.GetExtension(archivos[0].FileName);

                    // Eliminar la imagen antigua
                    var rutaImagen = Path.Combine(rutaPrincipal, articuloDesdeBd.UrlImagen.TrimStart('\\'));

                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }

                    // Redimensionar la nueva imagen
                    using (var memoryStream = new MemoryStream())
                    {
                        archivos[0].CopyTo(memoryStream);
                        using (var imagenOriginal = System.Drawing.Image.FromStream(memoryStream))
                        {
                            // Definir el tamaño de la imagen redimensionada (ejemplo: 1024x512)
                            int ancho = 1024;
                            int alto = 512;

                            using (var imagenRedimensionada = new Bitmap(imagenOriginal, new Size(ancho, alto)))
                            {
                                // Guardar la imagen redimensionada
                                var rutaCompleta = Path.Combine(subidas, nombreArchivo + extension);
                                imagenRedimensionada.Save(rutaCompleta);

                                artiVM.Articulo.UrlImagen = @"\imagenes\articulos\" + nombreArchivo + extension;
                            }
                        }
                    }
                }
                else
                {
                    // Mantener la imagen existente
                    artiVM.Articulo.UrlImagen = articuloDesdeBd.UrlImagen;
                }

                // Actualizar el creador y la fecha de edición
                artiVM.Articulo.FechaCreacion = DateTime.Now.ToString();
                var currentUser = await _userManager.GetUserAsync(User); // Obtener el usuario actual
                artiVM.Articulo.Creador = currentUser?.Nombre; // Asignar el nombre de usuario

                _contenedorTrabajo.Articulo.Update(artiVM.Articulo);
                _contenedorTrabajo.Save();

                return RedirectToAction(nameof(Index));
            }

            artiVM.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();
            return View(artiVM);
        }





        #region Llamadas a la API
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.Articulo.GetAll(includeProperties: "Categoria") });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var articuloDesdeBd = _contenedorTrabajo.Articulo.Get(id);
            string rutaDirectorioPrincipal = _hostingEnvironment.WebRootPath;
            var rutaImagen = Path.Combine(rutaDirectorioPrincipal, articuloDesdeBd.UrlImagen.TrimStart('\\'));
            if (System.IO.File.Exists(rutaImagen))
            {
                System.IO.File.Delete(rutaImagen);
            }

            if (articuloDesdeBd == null)
            {
                return Json(new { success = false, message = "Error borrando artículo" });
            }

            _contenedorTrabajo.Articulo.Remove(articuloDesdeBd);
            _contenedorTrabajo.Save();
            return Json(new { success = true, message = "Artículo Borrado Correctamente" });
        }
        #endregion
    }
}
