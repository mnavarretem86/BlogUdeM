using AppBlogUdeM.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBlogUdeM.AccesoDatos.Data.Repositorio.IRepositorio
{
    // Interfaz ICategoria que extiende la funcionalidad de IRepositorio para la entidad Categoria.
    // Esta interfaz define operaciones específicas para manejar categorías en el sistema.
    public interface ICategoriaRepositorio : IRepositorio<Categoria>
    {
        // Método para actualizar una categoría existente.
        // Parámetros:
        // - categoria: La instancia de la entidad Categoria que se desea actualizar.
        void Update(Categoria categoria);

    }

}