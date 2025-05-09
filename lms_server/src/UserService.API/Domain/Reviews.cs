using UserService.API.Application.Shared.Type;

namespace UserService.API.Domain;

public partial class Reviews : BaseEntity
{
    public Guid? UserId { get; set; }

    public Guid? ProductId { get; set; }

    /// <summary>
    /// 1 to 5 stars
    /// </summary>
    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public virtual Users? User { get; set; }
}
