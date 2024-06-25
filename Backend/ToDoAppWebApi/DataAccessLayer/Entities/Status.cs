using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class Status
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? IsDeleted { get; set; }

    public virtual ICollection<UserItem> UserItems { get; set; } = new List<UserItem>();
}
