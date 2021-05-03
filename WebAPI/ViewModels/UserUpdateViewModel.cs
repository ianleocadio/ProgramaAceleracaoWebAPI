using WebAPI.ViewModels.Common;

namespace WebAPI.ViewModels
{
    public class UserUpdateViewModel : EntityViewModel
    {
        public string? Name { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
    }
}
