using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppBlogUdeM.AccesoDatos.Data.Repositorio.IRepositorio
{


    //REPOSITORIO GENERICO PARA CUALQUIER TIPO DE MODELO
    public interface IRepositorio<T> where T : class
    {
        // Método para obtener una entidad por su id.
        // Recibe un parámetro 'id' de tipo entero que representa el identificador único de la entidad
        // y devuelve una entidad del tipo genérico T.
        T Get(int id);

        // Método para obtener una lista de entidades filtradas y ordenadas.
        // Parámetros:
        // - 'filter': Una expresión lambda opcional que sirve como filtro de búsqueda para filtrar los datos.
        // - 'orderby': Función opcional para ordenar las entidades obtenidas, puede ser null si no se requiere ordenación.
        // - 'includeProperties': Una cadena opcional que especifica las propiedades relacionadas que deben ser cargadas
        //   (similar a la función 'Include' en Entity Framework), útil cuando hay relaciones entre tablas (como 'Lazy Loading').
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderby = null,
            string? includeProperties = null
        );

        // Método para obtener la primera entidad que cumple con un filtro determinado.
        // Parámetros:
        // - 'filter': Una expresión lambda opcional para especificar los criterios de búsqueda (similar al 'where' de SQL).
        // - 'includeProperties': Una cadena opcional que especifica las propiedades relacionadas que deben ser cargadas.
        // Devuelve una entidad de tipo T o null si no se encuentra ninguna entidad que cumpla con los criterios.
        T GetFirstOrDefault(
             Expression<Func<T, bool>>? filter = null,
             string? includeProperties = null
        );

        // Método para agregar una nueva entidad al repositorio.
        // Recibe una entidad de tipo T como parámetro y la agrega al contexto de datos (por ejemplo, en Entity Framework, al DbSet).
        // No devuelve ningún valor.
        void Add(T entity);

        // Método para eliminar una entidad del repositorio usando su id.
        // Recibe un parámetro 'id' de tipo entero que representa el identificador de la entidad a eliminar.
        // No devuelve ningún valor.
        void Remove(int id);

        // Método para eliminar una entidad del repositorio directamente a partir de la entidad.
        // Recibe una entidad de tipo T como parámetro y la elimina del contexto de datos.
        // No devuelve ningún valor.
        void Remove(T entity);
    }
}
