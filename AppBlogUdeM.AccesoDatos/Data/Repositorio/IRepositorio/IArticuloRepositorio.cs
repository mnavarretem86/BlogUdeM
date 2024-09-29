using AppBlogUdeM.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBlogUdeM.AccesoDatos.Data.Repositorio.IRepositorio
{

    public interface IArticuloRepositorio : IRepositorio<Articulo>
    {

        void Update(Articulo articulo);

    }

}