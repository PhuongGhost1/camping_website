using System;
using System.Collections.Generic;

namespace PaymentService.API.Domain;

public partial class Payments
{
    public Guid Id { get; set; }

    public Guid? OrderId { get; set; }

    /// <summary>
    /// e.g., credit_card, paypal, cod
    /// </summary>
    public string? PaymentMethod { get; set; }

    /// <summary>
    /// e.g., pending, completed, failed
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// from payment gateway
    /// </summary>
    public string? TransactionId { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? PaidAt { get; set; }
}
