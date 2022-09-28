namespace DocumentsApp.Data.Dtos.DocumentDtos;

// ja bym zrobil 3x puta zamiast 1x update tak szczerze,
// bo name i description bedzie uzytkownik duzo rzadziej zmienial niz sam content
public class UpdateDocumentDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Content { get; set; }
}