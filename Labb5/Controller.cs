using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Labb5
{
    internal class Controller
    {
        internal delegate void SetButtonContent();
        internal event SetButtonContent OnSetButtonContent;

        internal MainWindow view { get; set; }

        internal ObservableCollection<UserModel> UserCollection { get; set; }
        internal ObservableCollection<UserModel> AdminCollection { get; set; }
        internal UserModel SelectedUser { get; set; }
        internal ListBox SelectedListBox { get; set; }
        internal bool updateMode;


        public Controller(MainWindow view)
        {
            this.view = view;
        }
        internal void Initialize()
        {
            DefaultButtonState();
            updateMode = false;

            UserCollection = new ObservableCollection<UserModel>() { new UserModel("Wil", "WilMail"), new UserModel("Si", "SiMail") };
            AdminCollection = new ObservableCollection<UserModel>() { new UserModel("Admwil", "AdmWilMail"), new UserModel("Admsi", "AdmSiMail") };

            view.userSubmitButton.Click += OnUserSubmitButtonClicked;
            view.userUpdateButton.Click += OnUserUpdateButtonClicked;
            view.removeUserButton.Click += OnRemoveUserButtonClicked;
            view.changeRank.Click += OnChangeRankButtonClick;
            view.editUserButton.Click += OnEditUserButtonClicked;
            OnSetButtonContent += ChangeButtonRankContentName;
            view.usernameTextBox.TextChanged += HandleTextBoxChange;
            view.emailTextBox.TextChanged += HandleTextBoxChange;
            view.userListBox.ItemsSource = UserCollection;
            view.adminListBox.ItemsSource = AdminCollection;


            view.userListBox.GotFocus += OnListGotFocus;
            view.userListBox.SelectionChanged += OnListSelectionChanged;
            view.userListBox.DisplayMemberPath = "Name";

            view.adminListBox.GotFocus += OnListGotFocus;
            view.adminListBox.SelectionChanged += OnListSelectionChanged;
            view.adminListBox.DisplayMemberPath = "Name";

        }

        internal void Run()
        {
            view.Show();
        }

        public void UpdateView()
        {
        }

        public void OnUserSubmitButtonClicked(object sender, RoutedEventArgs e)
        {
            UserCollection.Add(new UserModel(view.usernameTextBox.Text, view.emailTextBox.Text));

            view.usernameTextBox.Text = "";
            view.emailTextBox.Text = "";
        }

        public void OnUserUpdateButtonClicked(object sender, RoutedEventArgs e)
        {
            updateMode = false;
            SelectedListBox.Items.Refresh();

            SelectedUser.Name = view.usernameTextBox?.Text;
            SelectedUser.Email = view.emailTextBox?.Text;

            view.usernameTextBox.Text = "";
            view.emailTextBox.Text = "";

            SelectedListBox.SelectedItem = null;
            DefaultButtonState();
        }

        public void OnRemoveUserButtonClicked(object sender, RoutedEventArgs e)
        {
            DefaultButtonState();
            var selectedList = view.userListBox.SelectedItem == null ? view.adminListBox : view.userListBox;

            if (selectedList == view.adminListBox)
                AdminCollection.Remove(SelectedUser);
            else
                UserCollection.Remove(SelectedUser);
        }

        public void OnChangeRankButtonClick(object sender, RoutedEventArgs e)
        {
            if (view.changeRank.Content.ToString() == "Make User")
            {
                UserCollection.Add(SelectedUser);
                AdminCollection.Remove(SelectedUser);

                DefaultButtonState();
            }

            else if (view.changeRank.Content.ToString() == "Make Admin")
            {
                AdminCollection.Add(SelectedUser);
                UserCollection.Remove(SelectedUser);

                DefaultButtonState();
            }
        }

        public void ChangeButtonRankContentName()
        {
            view.changeRank.Content = SelectedListBox == view.adminListBox ? "Make User" : "Make Admin";
        }

        public void OnListGotFocus(object listbox, RoutedEventArgs e)
        {
            var unselectedList = (ListBox)listbox == view.userListBox ? view.adminListBox : view.userListBox;
            unselectedList.SelectedItem = null;
        }

        public void OnEditUserButtonClicked(object sender, RoutedEventArgs e)
        {
            updateMode = true;
            view.usernameTextBox.Text = SelectedUser?.Name;
            view.emailTextBox.Text = SelectedUser?.Email;

            DefaultButtonState();
            view.userUpdateButton.IsEnabled = true;
        }

        public void OnListSelectionChanged(object listbox, SelectionChangedEventArgs e)
        {
            SelectedListBox = (ListBox)listbox;
            SelectedUser = (UserModel)SelectedListBox.SelectedItem;

            OnSetButtonContent();
            view.changeRank.IsEnabled = true;
            view.editUserButton.IsEnabled = true;
            view.removeUserButton.IsEnabled = true;

            view.userNameLabel.Content = SelectedUser?.Name;
            view.userEmailLabel.Content = SelectedUser?.Email;
        }

        public void HandleTextBoxChange(object sender, TextChangedEventArgs e)
        {
            view.userSubmitButton.Content = "Submit";
            if (view.userUpdateButton.IsEnabled)
                view.userSubmitButton.IsEnabled = false;

            if (string.IsNullOrEmpty(view.usernameTextBox?.Text) ||
               string.IsNullOrEmpty(view.emailTextBox?.Text) ||
               string.IsNullOrWhiteSpace(view.usernameTextBox?.Text) ||
               string.IsNullOrWhiteSpace(view.emailTextBox?.Text))
            {
                if (updateMode)
                {
                    if (view.userUpdateButton != null)
                    {
                        view.userUpdateButton.IsEnabled = false;
                    }
                }
                else
                {
                    if (view.userSubmitButton != null)
                    {
                        view.userSubmitButton.IsEnabled = false;
                    }
                }
            }
            else
            {
                if (updateMode)
                {
                    if (view.userUpdateButton != null)
                        view.userUpdateButton.IsEnabled = true;
                }
                else
                {
                    if (view.userSubmitButton != null)
                        view.userSubmitButton.IsEnabled = true;
                }
            }

            foreach (UserModel user in UserCollection)
            {
                if (view.usernameTextBox.Text == user.Name)
                {
                    view.userSubmitButton.IsEnabled = false;
                    if (view.userUpdateButton.IsEnabled == false)
                    {
                        view.userSubmitButton.Content = "Invalid Name";
                    }
                }
            }

            foreach (UserModel user in AdminCollection)
            {
                if (view.usernameTextBox.Text == user.Name)
                {
                    view.userSubmitButton.IsEnabled = false;
                    if (view.userUpdateButton.IsEnabled == false)
                    {
                        view.userSubmitButton.Content = "Invalid Name";
                    }
                }
            }
        }

        public void DefaultButtonState()
        {
            view.changeRank.Content = "Change Rank";
            view.userSubmitButton.Content = "Submit";
            view.changeRank.IsEnabled = false;
            view.userSubmitButton.IsEnabled = false;
            view.userUpdateButton.IsEnabled = false;
            view.editUserButton.IsEnabled = false;
            view.removeUserButton.IsEnabled = false;
        }
    }
}
