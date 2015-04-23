using PokerTimer.BusinessObjects;
using PokerTimer.BusinessObjects.DataClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PokerTimer.Win.Dialogs
{
    public partial class RegisterUserDlg : Window
    {
        public RegisterUserDlg()
        {
            InitializeComponent();
            DataContext = this;

            BindData();

            txtSearch.Focus();
        }

        private void BindData()
        {
            lbxPlayerList.ItemsSource = App.ParentWindow.PlayerList;
            lbxPlayerList.DisplayMemberPath = "FullDislpayName";
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                lbxFoundUsers.SelectedIndex = 0;
                lbxFoundUsers.Focus();
                ListBoxItem item = lbxFoundUsers.ItemContainerGenerator.ContainerFromIndex(0) as ListBoxItem;
                
                if(item != null)
                    item.Focus();

            }
            else
            {
                var textBox = sender as TextBox;

                if (textBox.Text != string.Empty)
                {
                    var userList = new List<User>();
                    var searchText = textBox.Text.RemoveDiacritics();
                    var worker = new BackgroundWorker();
                    worker.DoWork += delegate
                    {
                        userList = BusinessObjects.Data.UserData.GetListBySearchString(eConnectionString.Online, searchText);
                    };

                    worker.RunWorkerCompleted += delegate
                    {
                        var filteredUserList = userList.Where(u => !App.ParentWindow.PlayerList.Select(p => p.UserId).Contains(u.UserId)).ToList();
                        lbxFoundUsers.ItemsSource = filteredUserList;
                        lbxFoundUsers.DisplayMemberPath = "FullDislpayName";

                        busyIndicator.IsBusy = false;
                    };

                    busyIndicator.IsBusy = true;
                    worker.RunWorkerAsync();
                }

                else
                {
                    lbxFoundUsers.ItemsSource = null;
                }
            }
        }

        private void btnAddPlayer_Click(object sender, RoutedEventArgs e)
        {
            var user = lbxFoundUsers.SelectedItem as User;

            if (user.IsNotNull())
                AddPlayer(user);

            App.ParentWindow.Refresh();
        }

        private void AddPlayer(User user)
        {
            var tr = new TournamentResult();
            tr.User = user;
            tr.UserId = user.UserId;
            tr.TournamentResultId = Guid.NewGuid();
            tr.Rank = App.ParentWindow.PlayerList.Count + 1;

            var dlg = new Dialogs.BuyInDlg();
            dlg.TournamentResult = tr;
            dlg.ShowDialog();

            if (dlg.DialogResult.True())
            {
                lbxFoundUsers.ItemsSource = null;
                txtSearch.Text = string.Empty;
                txtSearch.Focus();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate(txtRegFirstName))
                return;

            if (!Validate(txtRegLastName))
                return;

            if (!Validate(txtRegNickName))
                return;

            User newUser = new User();
            newUser.Email = txtRegEmial.Text;
            newUser.FirstName = txtRegFirstName.Text;
            newUser.LastName = txtRegLastName.Text;
            newUser.NickName = txtRegNickName.Text;
            newUser.PhoneNumber = txtRegPhoneNumber.Text;

            newUser = BusinessObjects.Data.UserData.Create(eConnectionString.Local, newUser);
            InsertUserAsync(newUser);

            //Mailer.Mailer.SendEmail(new EmailData() { Message = newUser.NickName, Subject = "new Player" });

            AddPlayer(newUser);

            txtRegEmial.Text = string.Empty;
            txtRegFirstName.Text = string.Empty;
            txtRegLastName.Text = string.Empty;
            txtRegNickName.Text = string.Empty;
            txtRegPhoneNumber.Text = string.Empty;

            txtSearch.Focus();
        }

        private bool Validate(TextBox elm)
        {
            if (elm.Text.IsNullOrEmpty())
            {
                elm.Background = Brushes.Red;
                return false;
            }
            else
            {
                elm.Background = Brushes.White;
                return true;
            }
        }

        private void btnClose_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lbxFoundUsers_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && lbxFoundUsers.SelectedItem.IsNotNull())
            {
                var user = lbxFoundUsers.SelectedItem as User;

                if (user.IsNotNull())
                    AddPlayer(user);
            }
        }

        private void InsertUserAsync(User user)
        {
            BackgroundWorker bwk = new BackgroundWorker();
            bwk.DoWork += delegate { BusinessObjects.Data.UserData.Insert(eConnectionString.Online, user); };
            bwk.RunWorkerAsync();
        }
    }
}
