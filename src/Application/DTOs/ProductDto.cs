using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public record ProductDto(
    int Id, string Name, string Description, decimal Price,
    string PictureUrl, string Brand, string Category, int StockQuantity);

public record CreateProductDto(
    [Required, StringLength(200)] string Name,
    [Required, StringLength(2000)] string Description,
    [Range(0.01, 1_000_000)] decimal Price,
    [Required] string PictureUrl,
    [Required, StringLength(100)] string Brand,
    [Required, StringLength(100)] string Category,
    [Range(0, int.MaxValue)] int StockQuantity);
