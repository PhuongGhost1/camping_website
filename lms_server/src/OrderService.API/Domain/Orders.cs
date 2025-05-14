using OrderService.API.Application.Shared.Type;

namespace OrderService.API.Domain;

public partial class Orders : BaseEntity
{
    public Guid? UserId { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Orderitems> Orderitems { get; set; } = new List<Orderitems>();
}
