namespace Models.InputModels
{
    public class User
    {

        public int? Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int? Isdeleted { get; set; }
    }
}
