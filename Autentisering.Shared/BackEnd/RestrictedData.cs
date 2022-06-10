namespace Authorization.Shared.Dto.BackEnd;

public class RestrictedData
{
    public string Name { get; set; }
    public int Value { get; set; }
    public string? Jti { get; set; }
    public DateTime? ValidTo { get; set; }
}
