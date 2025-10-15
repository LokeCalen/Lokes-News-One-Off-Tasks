using System;
using System.Collections.Generic;

namespace Lokes_News_Add_Roles.Data;

public partial class Subscription
{
    public int Id { get; set; }

    public int SubscriptionTypeId { get; set; }

    public decimal Price { get; set; }

    public DateTime Created { get; set; }

    public DateTime Expires { get; set; }

    public string UserId { get; set; } = null!;

    public bool PaymentComplete { get; set; }

    public virtual SubscriptionType SubscriptionType { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
