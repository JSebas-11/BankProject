using DB_BankProject.ModelsNative;
using BCrypt.Net;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB_BankProject.ModelsDB;

public partial class AdminAccount : IPerson {
    public int AdminId { get; set; }

    public string Number { get; set; } = null!;

    public string HashPassword { get; set; } = null!;

    public string OwnerName { get; set; } = null!;
    [NotMapped]
    public PersonType PersonType { get; set; }
    public AdminAccount() { }
    public AdminAccount(string owner, string number, string password){
        this.OwnerName = owner.Trim().ToUpper();
        this.Number = number;
        this.HashPassword = BCrypt.Net.BCrypt.HashPassword(password.Trim());
        this.PersonType = PersonType.Admin;
    }
}
