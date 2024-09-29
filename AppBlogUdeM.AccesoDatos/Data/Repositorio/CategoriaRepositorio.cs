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
    internal class CategoriaRepositorio : Repositorio<Categoria>, ICategoriaRepositorio
    {

        private readonly ApplicationDbContext _db;

        // Constructor que recibe el contexto de la base de datos.
        // Se llama al constructor de la clase base para inicializar el repositorio genérico.
        // Parámetros:
        // - db: Instancia de ApplicationDbContext que se utilizará para realizar operaciones de acceso a datos.
        public CategoriaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db; // Asigna el contexto de la base de datos a la variable de instancia.
        }

        // Método para actualizar una categoría existente en la base de datos.
        // Parámetros:
        // - categoria: La instancia de Categoria que contiene los nuevos valores a actualizar.
        public void Update(Categoria categoria)
        {
            // Busca la categoría existente en la base de datos utilizando su Id.
            var objetoDb = _db.Categorias.FirstOrDefault(s => s.Id == categoria.Id);

            // Si se encuentra la categoría, se actualizan sus propiedades.
            objetoDb.Nombre = categoria.Nombre; // Actualiza el nombre de la categoría.
            objetoDb.Orden = categoria.Orden;   // Actualiza el orden de la categoría.

            // Guarda los cambios realizados en el contexto de la base de datos.
            //_db.SaveChanges();
        }
    }
}
