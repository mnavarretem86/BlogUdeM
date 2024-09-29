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
    internal class Articulopositorio : Repositorio<Articulo>, IArticuloRepositorio
    {

        private readonly ApplicationDbContext _db;


        public Articulopositorio(ApplicationDbContext db) : base(db)
        {
            _db = db; 
        }


        public void Update(Articulo articulo)
        {

            var objetoDb = _db.Articulo.FirstOrDefault(s => s.Id == articulo.Id);


            objetoDb.Nombre = articulo.Nombre; 
            objetoDb.Descripcion = articulo.Descripcion;
            objetoDb.UrlImagen = articulo.UrlImagen;
            objetoDb.IdCategoria = articulo.IdCategoria;

            //_db.SaveChanges();
        }
    }
}
