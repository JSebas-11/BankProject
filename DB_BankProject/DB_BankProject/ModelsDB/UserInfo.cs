namespace DB_BankProject.ModelsDB;

public partial class UserInfo {
    public int Id { get; set; }

    public string Number { get; set; } = null!;

    public string OwnerName { get; set; } = null!;

    public DateTime? RegisteredAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
