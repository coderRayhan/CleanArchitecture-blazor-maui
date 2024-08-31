using Web.Models.NavigationMenu;

namespace Web.Services.Navigation;

public interface IMenuService
{
    IEnumerable<MenuSectionModel> Features { get; }
}
