using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBlogUdeM.Modelos
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ingrese un nombre de categoría")]
        [Display(Name = "Nombre Categoria")]
        [StringLength(25, ErrorMessage = "El nombre de la categoría no puede exceder los 25 caracteres.")]
        public string Nombre { get; set; }

        [Display(Name = "Orden de Visualización")]
        public int? Orden { get; set; }


    }
}