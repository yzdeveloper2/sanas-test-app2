using PrismApp.Models;
using PrismApp.Modules.Users.ViewModels;
using System.Collections.ObjectModel;

namespace PrismApp.Tests.Users.ViewModels
{
    public class UsersListViewModelTests
    {
        [Fact]
        public void WhenBuildTree_ThenTreeValid()
        {
            // Given
            var testCase = new[] { 
                new User { Id = 5, FirstName = "fn5", LastName = "ln5", Title = "l3", SupervisorId = 3 },
                new User { Id = 4, FirstName = "fn4", LastName = "ln4", Title = "l3", SupervisorId = 3 },
                new User { Id = 2, FirstName = "fn2", LastName = "ln2", Title = "l2", SupervisorId = 1 },
                new User { Id = 3, FirstName = "fn3", LastName = "ln3", Title = "l2", SupervisorId = 1 },
                new User { Id = 1, FirstName = "fn1", LastName = "ln1", Title = "l1", SupervisorId = null },
            };

            // When
            var tree = UsersListViewModel.BuildTree(testCase);

            // Then
            var resultTree = tree.ToList();
            Assert.Single(resultTree);
            AssertUsers(testCase[4], resultTree[0]);
            Assert.Equal(2, resultTree[0].Reports.Count);
            if (resultTree[0].Reports[0].Id == 2)
            {
                AssertLevel1(testCase[2], testCase[3], testCase, resultTree[0].Reports);
            }
            else
            {
                AssertLevel1(testCase[3], testCase[2], testCase, resultTree[0].Reports);
            }
        }

        private static void AssertLevel1(User user1, User user2, User[] testcase, ObservableCollection<UserItem> reports)
        {
            AssertUsers(user1, reports[0]);
            AssertUsers(user2, reports[1]);
            Assert.Empty(reports[0].Reports);
            Assert.Equal(2, reports[1].Reports.Count);
            if (reports[1].Reports[0].Id == 4)
            {
                AssertLevel2(testcase[1], testcase[0], reports[1].Reports);
            }
            else
            {
                AssertLevel2(testcase[0], testcase[1], reports[1].Reports);
            }
        }

        private static void AssertLevel2(User user1, User user2, ObservableCollection<UserItem> reports)
        {
            AssertUsers(user1, reports[0]);
            AssertUsers(user2, reports[1]);
        }

        private static void AssertUsers(User user, UserItem userItem)
        {
            Assert.Equal(user.Title, userItem.Title);
            Assert.Equal($"{user.FirstName} {user.LastName}", userItem.Name);
            Assert.Equal(user.Id, userItem.Id);
            Assert.Equal(user.SupervisorId, userItem.SupervisorId);
        }
    }
}
