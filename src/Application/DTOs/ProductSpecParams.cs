namespace Application.DTOs;

/// <summary>
/// DESIGN PATTERN: Data Transfer Object (DTO) / Parameter Object Pattern
/// Encapsule tous les paramètres de filtre, tri et pagination envoyés par le client (HTTP Query String).
/// Principe SOLID: Single Responsibility Principle (SRP).
/// </summary>
public class ProductSpecParams
{
    private const int MaxPageSize = 50;
    public int PageIndex { get; set; } = 1;

    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    public string? Brand { get; set; }
    public string? Category { get; set; }
    public string? Sort { get; set; }

    private string? _search;
    public string? Search
    {
        get => _search;
        set => _search = value?.ToLower();
    }
}