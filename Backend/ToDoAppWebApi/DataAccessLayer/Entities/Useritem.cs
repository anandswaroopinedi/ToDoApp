using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class UserItem
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ItemId { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? CompletedOn { get; set; }

    public DateTime? NotifyOn { get; set; }

    public int StatusId { get; set; }

    public int? IsDeleted { get; set; }

    public int? IsNotifyCancelled { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
