using FirebaseAdmin.Auth;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.ViewModels
{
    public class UserCreateViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "userCreate";
        public IScreen HostScreen { get; }

        private string _username;
        private string _email;
        private string _phone;
        private string _password;

        public string Username { get => _username; set => _username = this.RaiseAndSetIfChanged(ref _username, value); }
        public string Email { get => _email; set => _email = this.RaiseAndSetIfChanged(ref _email, value); }
        public string Phone { get => _phone; set => _phone = this.RaiseAndSetIfChanged(ref _phone, value); }
        public string Password { get => _password; set => _password = this.RaiseAndSetIfChanged(ref _password, value); }

        private string _errorMessage;
        public string ErrorMessage { get => _errorMessage; set => _errorMessage = this.RaiseAndSetIfChanged(ref _errorMessage, value); }

        public ReactiveCommand<Unit, Unit> CreateUserCommand { get; }

        public UserCreateViewModel(IScreen hostScreen)
        {
            HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();

            var canCreate = this.WhenAnyValue(x => x.Username, x => x.Email, x => x.Phone, x => x.Password, (name, mail, ph, pass) =>
            {
                return !string.IsNullOrWhiteSpace(name) &&
                       !string.IsNullOrWhiteSpace(mail) &&
                       !string.IsNullOrWhiteSpace(ph) &&
                       !string.IsNullOrWhiteSpace(pass);
            }).DistinctUntilChanged();

            CreateUserCommand = ReactiveCommand.CreateFromTask(CreateUserAsync, canCreate);
        }

        private async Task CreateUserAsync()
        {
            UserRecordArgs args = new UserRecordArgs()
            {
                Email = Email,
                EmailVerified = true,
                PhoneNumber = Phone,
                Password = Password,
                DisplayName = Username,
                Disabled = false
            };

            try
            {
                UserRecord userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(args);
                await HostScreen.Router.NavigateBack.Execute();
            }
            catch (ArgumentException ex)
            {
                ErrorMessage = ex.Message;
            }
            catch (FirebaseAuthException ex)
            {
                GetErrorMessage(ex.Message);
            }
        }

        private void GetErrorMessage(string message)
        {
            message = message.Replace("Unexpected HTTP response with status: 400 (BadRequest)", "");

            string parsedMessage = string.Empty;

            try
            {
                var anonTemplate = new { error = new { message = string.Empty } };
                parsedMessage = JsonConvert.DeserializeAnonymousType(message, anonTemplate).error.message.Replace('_', ' ').ToLower();
            }
            catch (Exception)
            {
                parsedMessage = message;
            }

            ErrorMessage = parsedMessage;
        }

    }
}
