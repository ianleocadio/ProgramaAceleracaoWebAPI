
using System.ComponentModel.DataAnnotations;

namespace WebAPI.ViewModels.Common
{
    public class EntityViewModel
    {
        [Required]
        public long ID { get; set; }
    }
}
