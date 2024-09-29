using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBlogUdeM.Modelos
{
    public class Articulo
    {

        [Key]
        public int Id { get; set; }

        [Required (ErrorMessage ="Nombre Obligatorio")]
        [Display (Name = "Nombre del Articulo")]
        [StringLength(25, ErrorMessage = "El nombre del Articulo no puede exceder los 25 caracteres.")]
        public String Nombre{ get; set; }

        [Required(ErrorMessage = "La descripción Obligatorio")]
        public String Descripcion { get; set; }

        [Display(Name = "Fecha de creacion")]
        public DateTime FechaCreacion { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Imagen")]

        public string UrlImagen { get; set; }


        [Required(ErrorMessage = "La categoria es  Obligatorio")]
        public int IdCategoria { get; set; }

        [ForeignKey ("IdCategoria")]
        public Categoria Categoria { get; set; }


    }
}
