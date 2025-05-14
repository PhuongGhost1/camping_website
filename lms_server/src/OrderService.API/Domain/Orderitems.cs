namespace OrderService.API.Domain;

public partial class Orderitems
{
    public Guid Id { get; set; }

    public Guid? OrderId { get; set; }

    public Guid? ProductId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    /// <summary>
    /// pending, paid, refunded, canceled
    /// </summary>
    public string? Status { get; set; }

    public virtual Orders? Order { get; set; }
}
