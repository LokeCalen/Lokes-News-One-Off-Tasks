using System;
using System.Collections.Generic;

namespace Lokes_News_Add_Roles.Data;

public partial class Like
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public int ArticleId { get; set; }

    public virtual Article Article { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
