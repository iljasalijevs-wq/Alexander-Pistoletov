
using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    public string Role { get; set; } = "User";

    public List<BorrowRecord>? BorrowRecords { get; set; }
}
