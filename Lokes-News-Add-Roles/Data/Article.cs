using System;
using System.Collections.Generic;

namespace Lokes_News_Add_Roles.Data;

public partial class Article
{
    public int Id { get; set; }

    public DateTime DateStamp { get; set; }

    public string Slug { get; set; } = null!;

    public string Headline { get; set; } = null!;

    public string ContentSummary { get; set; } = null!;

    public string Content { get; set; } = null!;

    public int Views { get; set; }

    public int Likes { get; set; }

    public string ImageUrl { get; set; } = null!;

    public bool IsArchived { get; set; }

    public bool IsDraft { get; set; }

    public string Author { get; set; } = null!;

    public bool NeedsSubscription { get; set; }

    public bool EditorsChoice { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
