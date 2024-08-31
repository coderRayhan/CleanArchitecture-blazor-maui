using MudBlazor;
using Web.Models.NavigationMenu;

namespace Web.Services.Navigation;

public class MenuService : IMenuService
{
    private readonly List<MenuSectionModel> _features = new()
    {
        new MenuSectionModel
        {
            Title = "Application",
            SectionItems = new List<MenuSectionItemModel>
            {
                new()
                {
                    Title = "Home",
                    Icon = Icons.Material.Filled.Home,
                    Href = "/"
                },
                new()
                {
                    Title = "Common Setup",
                    Icon = Icons.Material.Filled.Settings,
                    PageStatus = PageStatus.Completed,
                    IsParent = true,
                    MenuItems = new List<MenuSectionSubItemModel>
                    {
                        new()
                        {
                            Title = "Lookups",
                            Href = "/pages/Lookups",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Lookup Details",
                            Href = "/pages/LookupDetails",
                            PageStatus = PageStatus.Completed
                        }
                    }
                }
            }
        }
    };
    public IEnumerable<MenuSectionModel> Features => _features;
}
