using FirebaseAdmin.Auth;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.ViewModels
{
    public class UsersListViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "users";

        public IScreen HostScreen { get; }

        public ObservableCollection<ExportedUserRecord> Users { get; private set; }

        public ReactiveCommand<Unit, Unit> GetUsers { get; }
      

        public UsersListViewModel(IScreen hostScreen)
        {
            HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();

            GetUsers = ReactiveCommand.CreateFromTask(RefreshList);
        }

        private async Task RefreshList()
        {
            var users = await LoadUsers();
            Users = new ObservableCollection<ExportedUserRecord>(users);
        }

        private async Task<List<ExportedUserRecord>> LoadUsers()
        {
            return await FirebaseAuth.DefaultInstance.ListUsersAsync(null).ToList();
        }
    }
}
