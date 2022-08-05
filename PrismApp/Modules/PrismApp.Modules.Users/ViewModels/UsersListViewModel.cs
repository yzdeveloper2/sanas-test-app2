using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Regions;
using PrismApp.Core.Mvvm;
using PrismApp.Models;
using PrismApp.Services.Interfaces;

namespace PrismApp.Modules.Users.ViewModels;

public class UsersListViewModel : RegionViewModelBase
{
    private readonly IUserService _userService;

    private ObservableCollection<UserItem> _users;

    public UsersListViewModel(IRegionManager regionManager, IUserService userService)
        : base(regionManager)
    {
        _userService = userService;
        _users = new ObservableCollection<UserItem>();
    }

    public ObservableCollection<UserItem> Items
    {
        get => _users;
        set => SetProperty(ref _users, value);
    }

    public DelegateCommand LoadCommand => new(LoadSync);

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
    }

    internal static ObservableCollection<UserItem> BuildTree(IEnumerable<User> users)
    {
        var usersDict = new Dictionary<int, UserItem>();
        var supervisorsDict = new Dictionary<int, List<UserItem>>();
        var result = new ObservableCollection<UserItem>();
        foreach (var u in users)
        {
            if (!u.Id.HasValue || string.IsNullOrWhiteSpace(u.LastName) ||
                string.IsNullOrWhiteSpace(u.FirstName) || string.IsNullOrWhiteSpace(u.Title))
            {
                continue;
            }

            if (usersDict.ContainsKey(u.Id.Value))
            {
                continue; // duplicate id, should not get here
            }

            var ui = new UserItem(u.Id.Value, u.FirstName, u.LastName, u.Title, u.SupervisorId);
            if (ui.SupervisorId.HasValue)
            {
                if (usersDict.TryGetValue(ui.SupervisorId.Value, out var supervisor))
                {
                    // supervisor known 
                    supervisor.AddReport(ui);
                }
                else
                {
                    // supervisor unknown
                    if (supervisorsDict.TryGetValue(ui.SupervisorId.Value, out var list))
                    {
                        list.Add(ui);
                    }
                    else
                    {
                        supervisorsDict.Add(ui.SupervisorId.Value, new List<UserItem> { ui });
                    }
                }
            }
            else
            {
                result.Add(ui); // top level
            }

            usersDict.Add(ui.Id, ui);

            if (supervisorsDict.TryGetValue(ui.Id, out var unsupervised))
            {
                // supervisor found
                foreach (var un in unsupervised)
                {
                    ui.AddReport(un);
                }

                supervisorsDict.Remove(ui.Id);
            }
        }

        return result;
    }

    private void LoadSync()
    {
        Task.Run(async () => await LoadAsync());
    }

    private async Task LoadAsync()
    {
        var users = await _userService.GetUsers();
        Items = BuildTree(users);
    }
}