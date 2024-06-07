using IlanSistemi.Models.Entitites;

namespace IlanSistemi.Models
{
    public class GetCategoriesModel
    {
        public Category Category { get; set; }
        public List<Advertisement> Advertisements { get; set; }
    }
}
