namespace DB_BankProject.ModelsDB;
public partial class TransHistory {
    public int HistId { get; set; }

    public int? UserId { get; set; }

    public int? TransId { get; set; }

    public virtual Tran? Trans { get; set; }

    public virtual UserAccount? User { get; set; }
}
