using AppBlogUdeM.AccesoDatos.Data.Repositorio.IRepositorio;
using AppBlogUdeM.Data;
using AppBlogUdeM.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBlogUdeM.AccesoDatos.Data.Repositorio
{
    // Clase ContenedorTrabajo que implementa la interfaz IContenedorTrabajo.
    // Se encarga de gestionar el contexto de la base de datos y los repositorios asociados.
    public class ContenedorTrabajo : IContenedorTrabajo
    {
        // Contexto de la base de datos para realizar operaciones de acceso a datos.
        private readonly ApplicationDbContext _db;

        // Constructor que recibe el contexto de la base de datos.
        // Inicializa el repositorio de categorías al crear una instancia de ContenedorTrabajo.
        // Parámetros:
        // - db: Instancia de ApplicationDbContext que se utilizará para realizar operaciones de acceso a datos.
        public ContenedorTrabajo(ApplicationDbContext db)
        {
            _db = db; // Asigna el contexto de la base de datos a la variable de instancia.
            Categoria = new CategoriaRepositorio(_db); // Inicializa el repositorio de categorías.
            Articulo = new Articulopositorio(_db); // Inicializa el repositorio de Articulos.


        }

        // Propiedad para acceder al repositorio de categorías.
        // Se utiliza para realizar operaciones sobre categorías desde otras clases.
        public ICategoriaRepositorio Categoria { get; private set; }
        public IArticuloRepositorio Articulo { get; private set; }

        // Método para liberar los recursos utilizados por el contexto de la base de datos.
        // Implementa IDisposable para garantizar la correcta liberación de recursos.
        public void Dispose()
        {
            _db.Dispose(); // Libera el contexto de la base de datos.
        }

        // Método para guardar los cambios realizados en el contexto de la unidad de trabajo.
        // Persiste todas las modificaciones en la base de datos.
        public void Save()
        {
            _db.SaveChanges(); // Guarda los cambios realizados en el contexto de la base de datos.
        }
    }

}
