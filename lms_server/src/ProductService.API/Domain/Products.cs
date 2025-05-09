using ProductService.API.Application.Shared.Type;

namespace ProductService.API.Domain;

public partial class Products : BaseEntity
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public int? Stock { get; set; }

    public string? ImageUrl { get; set; }

    public Guid? CategoryId { get; set; }

    public virtual Categories? Category { get; set; }

    public virtual ICollection<Reviews> Reviews { get; set; } = new List<Reviews>();
}
