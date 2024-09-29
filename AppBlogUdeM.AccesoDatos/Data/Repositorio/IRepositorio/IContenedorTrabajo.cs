using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBlogUdeM.AccesoDatos.Data.Repositorio.IRepositorio
{
    // Interfaz IContenedorTrabajo que representa una unidad de trabajo para manejar múltiples repositorios.
    // Esta interfaz implementa IDisposable para liberar recursos de manera controlada.
    public interface IContenedorTrabajo : IDisposable
    {
        // Propiedad para acceder al repositorio de categorías.
        // Devuelve una instancia de ICategoriaRepositorio, permitiendo realizar operaciones sobre categorías.
        ICategoriaRepositorio Categoria { get; }

        // Método para guardar los cambios realizados en el contexto de la unidad de trabajo.
        // Este método persiste las modificaciones de todos los repositorios asociados en una única transacción.
        void Save();
    }

}
