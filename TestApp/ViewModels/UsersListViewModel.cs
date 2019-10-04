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

        public ReactiveCommand<Unit, Unit> GetUsersCommand { get; }
        public ReactiveCommand<Unit, Unit> CreateUserCommand { get; }
        public ReactiveCommand<string, Unit> RemoveUserCommand { get; }

        public UsersListViewModel(IScreen hostScreen)
        {
            HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();

            GetUsersCommand = ReactiveCommand.CreateFromTask(RefreshList);
            CreateUserCommand = ReactiveCommand.CreateFromTask(NavigateToUserCreate);
            RemoveUserCommand = ReactiveCommand.CreateFromTask<string>(RemoveUserAsync);
        }

        private async Task NavigateToUserCreate()
        {
            await HostScreen.Router.Navigate.Execute(new UserCreateViewModel(HostScreen));
        }

        private async Task RefreshList()
        {
            var users = await LoadUsers();
            Users = new ObservableCollection<ExportedUserRecord>(users);
            this.RaisePropertyChanged(nameof(Users));
        }

        private async Task RemoveUserAsync(string uid)
        {
            await FirebaseAuth.DefaultInstance.DeleteUserAsync(uid);

            Users.Remove(Users.FirstOrDefault(x => x.Uid == uid));
        }

        private async Task<List<ExportedUserRecord>> LoadUsers()
        {
            return await FirebaseAuth.DefaultInstance.ListUsersAsync(null).ToList();
        }
    }
}
