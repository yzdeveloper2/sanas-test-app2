using Prism.Regions;
using PrismApp.Core.Mvvm;

namespace PrismApp.Modules.Users.ViewModels;

public class UsersListViewModel : RegionViewModelBase
{
    public UsersListViewModel(IRegionManager regionManager)
        : base(regionManager)
    {
    }

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
    }
}