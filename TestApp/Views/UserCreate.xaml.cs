﻿using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TestApp.ViewModels;

namespace TestApp.Views
{
    /// <summary>
    /// Interaction logic for UserCreate.xaml
    /// </summary>
    public partial class UserCreate : ReactiveUserControl<UserCreateViewModel>
    {
        public UserCreate()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.BindCommand(ViewModel, x => x.CreateUserCommand, x => x.CreateUserButton).DisposeWith(disposables);
                this.Bind(ViewModel, x => x.Username, x => x.UsernameText.Text).DisposeWith(disposables);
                this.Bind(ViewModel, x => x.Email, x => x.EmailText.Text).DisposeWith(disposables);
                this.Bind(ViewModel, x => x.Phone, x => x.PhoneText.Text).DisposeWith(disposables);
                this.Bind(ViewModel, x => x.Password, x => x.PasswordText.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.ErrorMessage, x => x.ErrorLabel.Text).DisposeWith(disposables);
            });
        }
    }
}
