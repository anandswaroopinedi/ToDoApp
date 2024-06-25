using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class User
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? IsDeleted { get; set; }

    public virtual ICollection<UserItem> UserItems { get; set; } = new List<UserItem>();
}
