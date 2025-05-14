using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ECommerceASP.Models
{
    public class Category
    {
        public int Id { get; set; }

        [DisplayName("Nom de catégorie")]  
        [Required(ErrorMessage = "Le nom de la catégorie est requis.")]  
        [StringLength(20, ErrorMessage = "Le nom de la catégorie ne peut pas dépasser 20 caractères.")]
        public string name { get; set; }
    }
}
