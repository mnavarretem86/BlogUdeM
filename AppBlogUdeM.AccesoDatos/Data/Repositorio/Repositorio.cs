using AppBlogUdeM.AccesoDatos.Data.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppBlogUdeM.AccesoDatos.Data.Repositorio
{
    internal class Repositorio<T> : IRepositorio<T> where T : class
    {

        // Campo protegido que almacena una referencia al contexto de la base de datos (DbContext).
        // El contexto es responsable de la conexión con la base de datos y la gestión de las entidades.
        // Al ser 'protected', permite que las clases derivadas del repositorio accedan al contexto.
        protected readonly DbContext Context;

        // Campo interno que representa un conjunto de entidades de tipo T dentro del contexto.
        // DbSet<T> es utilizado por Entity Framework para realizar operaciones CRUD en la base de datos.
        // Al ser 'internal', está disponible solo dentro del ensamblado actual.
        internal DbSet<T> dbSet;

        // Constructor del repositorio que recibe un contexto de base de datos como parámetro.
        // El objetivo es inicializar los campos 'Context' y 'dbSet' para trabajar con la base de datos.
        // El contexto es asignado al campo 'Context', y 'dbSet' se inicializa obteniendo el conjunto
        // de entidades del tipo T utilizando el método 'Set<T>' del contexto.
        public Repositorio(DbContext context)
        {
            // Se asigna el contexto proporcionado al campo protegido 'Context'.
            Context = context;

            // Se inicializa 'dbSet' llamando a 'Set<T>()', que devuelve un DbSet<T> que se puede usar para
            // realizar operaciones sobre la entidad T dentro de la base de datos.
            this.dbSet = context.Set<T>();
        }





        public void Add(T entity)
        {
           dbSet.Add(entity);
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }


        // Método que obtiene una colección de entidades de tipo T con soporte para filtrado, ordenación y propiedades incluidas.
        
        // Parámetros:
        // - filter: Expresión lambda opcional para filtrar las entidades. Por ejemplo, 'x => x.Nombre == "Juan"'.
        // - orderby: Función opcional que define cómo se ordenan los resultados. Por ejemplo, 'query => query.OrderBy(x => x.Apellido)'.
        // - includeProperties: Cadena opcional con los nombres de las propiedades relacionadas a cargar (separadas por comas), similar a 'Include' en Entity Framework.
        // Devuelve una lista de entidades filtradas, ordenadas, e incluyendo propiedades si se especifica.

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null,
                                     Func<IQueryable<T>, IOrderedQueryable<T>>? orderby = null,
                                     string? includeProperties = null)
        {
            // Se inicia la consulta sobre el conjunto de entidades 'dbSet' correspondiente al tipo T.
            IQueryable<T> query = dbSet;

            // Si se proporciona un filtro, se aplica el método 'Where' para restringir los resultados de la consulta.
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Si se especifican propiedades para incluir, se recorren y se añaden a la consulta usando el método 'Include'.
            // El método 'Split' divide la cadena de propiedades por comas, y 'RemoveEmptyEntries' asegura que no haya elementos vacíos.
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty); // Incluye la propiedad relacionada en la consulta.
                }
            }

            // Si se proporciona una función de ordenación, se aplica a la consulta antes de devolver los resultados.
            if (orderby != null)
            {
                return orderby(query).ToList(); // Ordena la consulta y devuelve los resultados como una lista.
            }

            // Si no se especifica ordenación, simplemente se ejecuta la consulta y se devuelve como lista.
            return query.ToList();
        }




        // Método para obtener la primera entidad que cumple con un criterio, o el valor por defecto si no se encuentra ninguna.
        // Parámetros:
        // - filter: Expresión lambda opcional que actúa como filtro para buscar la entidad (por ejemplo, 'x => x.Id == 1').
        // - includeProperties: Cadena opcional que especifica las propiedades relacionadas que deben cargarse junto con la entidad (separadas por comas).
        // Devuelve la primera entidad que coincide con el filtro especificado o 'null' si no se encuentra ninguna entidad.

        public T GetFirstOrDefault(Expression<Func<T, bool>>? filter = null,
                                   string? includeProperties = null)
        {
            // Iniciar la consulta sobre el DbSet de entidades del tipo T.
            IQueryable<T> query = dbSet;

            // Si se proporciona un filtro, se aplica para restringir la consulta.
            if (filter != null)
            {
                query = query.Where(filter); // Filtra los resultados basados en la expresión lambda proporcionada.
            }

            // Si se especifican propiedades para incluir, se recorren y añaden a la consulta.
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty); // Incluye las propiedades relacionadas en la consulta.
                }
            }

            // Retorna la primera entidad que cumple con los criterios aplicados o 'null' si no se encuentra ninguna.
            return query.FirstOrDefault();
        }



        // Método para eliminar una entidad a partir de su ID.
        // Parámetros:
        // - id: El identificador único de la entidad que se desea eliminar.
        public void Remove(int id)
        {
            // Busca la entidad en el DbSet por su ID.
            T entityToRemove = dbSet.Find(id);

            // Si se encuentra la entidad, puedes eliminarla aquí (aunque falta la lógica para eliminarla).
            // El código para eliminar la entidad debería ser: dbSet.Remove(entityToRemove); 
            // Pero en este fragmento no se ha implementado esa lógica aún.
        }


        // Método para eliminar una entidad específica.
        // Parámetros:
        // - entity: La entidad del tipo T que se desea eliminar.
        public void Remove(T entity)
        {
            // Elimina la entidad directamente del DbSet.
            dbSet.Remove(entity);
        }



    }
}
