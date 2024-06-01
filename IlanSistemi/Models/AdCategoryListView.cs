using IlanSistemi.Models.Entitites;

namespace IlanSistemi.Models
{
    public class AdCategoryListView
    {
        public Advertisement Advertisement { get; set; }
        public List<Category>? Categories { get; set; }
    }
}
