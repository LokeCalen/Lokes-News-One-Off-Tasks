using System;
using System.Collections.Generic;

namespace Lokes_News_Add_Roles.Data;

public partial class SubscriptionType
{
    public int Id { get; set; }

    public string TypeName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
