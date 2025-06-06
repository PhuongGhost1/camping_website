﻿using ProductService.API.Application.Shared.Type;

namespace ProductService.API.Domain;

public partial class Categories : BaseEntity
{
    public string? Name { get; set; }

    public string? Description { get; set; }
}
