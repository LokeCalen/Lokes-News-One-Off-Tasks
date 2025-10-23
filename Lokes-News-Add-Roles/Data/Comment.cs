using System;
using System.Collections.Generic;

namespace Lokes_News_Add_Roles.Data;

public partial class Comment
{
    public int Id { get; set; }

    public int ArticleId { get; set; }

    public string CommentContent { get; set; } = null!;

    public string Author { get; set; } = null!;

    public virtual Article Article { get; set; } = null!;
}
