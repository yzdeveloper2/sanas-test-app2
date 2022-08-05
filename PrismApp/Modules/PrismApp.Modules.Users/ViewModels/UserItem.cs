using System.Collections.ObjectModel;

namespace PrismApp.Modules.Users.ViewModels
{
    public class UserItem
    {
        public UserItem(int id, string firstName, string lastName, string title, int? supervisorId)
        {
            Name = GetName(firstName, lastName);
            Title = title;
            Id = id;
            Reports = new ObservableCollection<UserItem>();
            SupervisorId = supervisorId;    
        }

        public string Name { get; }

        public string Title { get; }

        public int Id { get; }

        public int? SupervisorId { get; }

        public ObservableCollection<UserItem> Reports { get; set; }

        public void AddReport(UserItem item)
        {
            Reports.Add(item);
        }

        private static string GetName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                return string.Empty;
            }

            return $"{firstName} {lastName}";
        }
    }
}
