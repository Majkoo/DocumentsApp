namespace DocumentsApp.Shared.Exceptions;

public class BadRequestException : Exception
{
    public string Title { get; set; }

    public BadRequestException(string title, string message) : base(message)
    {
        Title = title;
    }
}