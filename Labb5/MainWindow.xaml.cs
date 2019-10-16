using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Labb5WilhelmBerggren
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private delegate void SetButtonContent();
        private event SetButtonContent OnSetButtonContent;
        private ObservableCollection<User> UserCollection { get; set; }
        private ObservableCollection<User> AdminCollection { get; set; }
        private User SelectedUser { get; set; }
        private ListBox SelectedListBox { get; set; }

        private bool updateMode;
        public MainWindow()
        {
            InitializeComponent();
            updateMode = false;

            UserCollection = new ObservableCollection<User>() { new User("Wil", "WilMail"), new User("Si", "SiMail") };
            AdminCollection = new ObservableCollection<User>() { new User("Admwil", "AdmWilMail"), new User("Admsi", "AdmSiMail") };
            SelectedUser = default;

            userSubmitButton.Click += OnUserSubmitButtonClicked;
            userUpdateButton.Click += OnUserUpdateButtonClicked;
            removeUserButton.Click += OnRemoveUserButtonClicked;
            changeRank.Click += OnChangeRankButtonClick;
            OnSetButtonContent += ChangeButtonRankContentName;

            userListBox.ItemsSource = UserCollection;
            userListBox.GotFocus += OnListGotFocus;
            userListBox.SelectionChanged += OnListSelectionChanged;
            userListBox.DisplayMemberPath = "Name";

            adminListBox.ItemsSource = AdminCollection;
            adminListBox.GotFocus += OnListGotFocus;
            adminListBox.SelectionChanged += OnListSelectionChanged;
            adminListBox.DisplayMemberPath = "Name";

            DefaultButtonState();
        }

        private void ChangeButtonRankContentName()
        {
            changeRank.Content = SelectedListBox == adminListBox ? "Make User" : "Make Admin";
        }

        private void OnChangeRankButtonClick(object sender, RoutedEventArgs e)
        {
            if (changeRank.Content.ToString() == "Make User")
            {
                UserCollection.Add(SelectedUser);
                AdminCollection.Remove(SelectedUser);

                DefaultButtonState();
            }

            else if (changeRank.Content.ToString() == "Make Admin")
            {
                AdminCollection.Add(SelectedUser);
                UserCollection.Remove(SelectedUser);

                DefaultButtonState();
            }
        }

        private void OnRemoveUserButtonClicked(object sender, RoutedEventArgs e)
        {
            DefaultButtonState();
            var selectedList = userListBox.SelectedItem == null ? adminListBox : userListBox;

            if (selectedList == adminListBox)
                AdminCollection.Remove(SelectedUser);
            else
                UserCollection.Remove(SelectedUser);
        }

        private void OnListGotFocus(object listbox, RoutedEventArgs e)
        {
            var unselectedList = (ListBox)listbox == userListBox ? adminListBox : userListBox;
            unselectedList.SelectedItem = null;
        }

        private void OnUserSubmitButtonClicked(object sender, RoutedEventArgs e)
        {
            UserCollection.Add(new User(usernameTextBox.Text, emailTextBox.Text));

            usernameTextBox.Text = "";
            emailTextBox.Text = "";
        }

        private void OnUserUpdateButtonClicked(object sender, RoutedEventArgs e)
        {
            updateMode = false;
            SelectedListBox.Items.Refresh();

            SelectedUser.Name = usernameTextBox?.Text;
            SelectedUser.Email = emailTextBox?.Text;

            usernameTextBox.Text = "";
            emailTextBox.Text = "";

            SelectedListBox.SelectedItem = null;
            DefaultButtonState();
        }

        private void OnEditUserButtonClicked(object sender, RoutedEventArgs e)
        {
            updateMode = true;
            usernameTextBox.Text = SelectedUser?.Name;
            emailTextBox.Text = SelectedUser?.Email;

            DefaultButtonState();
            userUpdateButton.IsEnabled = true;
        }

        private void OnListSelectionChanged(object listbox, SelectionChangedEventArgs e)
        {
            SelectedListBox = (ListBox)listbox;
            SelectedUser = (User)SelectedListBox.SelectedItem;

            OnSetButtonContent();
            changeRank.IsEnabled = true;
            editUserButton.IsEnabled = true;
            removeUserButton.IsEnabled = true;

            userNameLabel.Content = SelectedUser?.Name;
            userEmailLabel.Content = SelectedUser?.Email;
        }

        private void HandleTextBoxChange(object sender, TextChangedEventArgs e)
        {
            userSubmitButton.Content = "Submit";
            if (userUpdateButton.IsEnabled)
                userSubmitButton.IsEnabled = false;

            if (string.IsNullOrEmpty(usernameTextBox?.Text) ||
               string.IsNullOrEmpty(emailTextBox?.Text) ||
               string.IsNullOrWhiteSpace(usernameTextBox?.Text) ||
               string.IsNullOrWhiteSpace(emailTextBox?.Text))
            {
                if (updateMode)
                {
                    if (userUpdateButton != null)
                    {
                        userUpdateButton.IsEnabled = false;
                    }
                }
                else
                {
                    if (userSubmitButton != null)
                    {
                        userSubmitButton.IsEnabled = false;
                    }
                }
            }
            else
            {
                if (updateMode)
                {
                    if (userUpdateButton != null)
                        userUpdateButton.IsEnabled = true;
                }
                else
                {
                    if (userSubmitButton != null)
                        userSubmitButton.IsEnabled = true;
                }
            }

            foreach (User user in UserCollection)
            {
                if (usernameTextBox.Text == user.Name)
                {
                    userSubmitButton.IsEnabled = false;
                    if (userUpdateButton.IsEnabled == false)
                    {
                        userSubmitButton.Content = "Invalid Name";
                    }
                }
            }

            foreach (User user in AdminCollection)
            {
                if (usernameTextBox.Text == user.Name)
                {
                    userSubmitButton.IsEnabled = false;
                    if (userUpdateButton.IsEnabled == false)
                    {
                        userSubmitButton.Content = "Invalid Name";
                    }
                }
            }
        }

        public void DefaultButtonState()
        {
            changeRank.Content = "Change Rank";
            userSubmitButton.Content = "Submit";
            changeRank.IsEnabled = false;
            userSubmitButton.IsEnabled = false;
            userUpdateButton.IsEnabled = false;
            editUserButton.IsEnabled = false;
            removeUserButton.IsEnabled = false;
        }
    }

    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public User(string Name, string Email)
        {
            this.Name = Name;
            this.Email = Email;
        }
    }
}
