namespace Labb5
{
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
