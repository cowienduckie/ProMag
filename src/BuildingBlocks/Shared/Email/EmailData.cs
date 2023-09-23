namespace Shared.Email;

public static class EmailConstants
{
    public const string MultipleEmailAddressSeparator = ";";
}

public enum Priority
{
    High = 1,
    Normal = 2,
    Low = 3
}

public class EmailData
{
    public List<string> ToAddress { get; } = new();
    public List<string> CcAddress { get; } = new();
    public List<string> BccAddress { get; } = new();
    public string FromAddress { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Body { get; set; } = null!;
    public bool IsHtml { get; set; }
    public Priority Priority { get; set; }
    public List<string> Tags { get; set; } = new();
}