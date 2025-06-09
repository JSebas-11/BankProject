using DB_BankProject.ModelsNative;

namespace DB_BankProject.ModelsDB;

public partial class Tran {
    public int TransId { get; set; }

    public int? TransType { get; set; }
    
    public int? BranchId { get; set; }

    public int? SenderId { get; set; }

    public int? ReceiverId { get; set; }

    public decimal Amount { get; set; }

    public DateTime DateTrans { get; set; }

    public virtual UserAccount? Receiver { get; set; }

    public virtual UserAccount? Sender { get; set; }

    public virtual ICollection<TransHistory> TransHistories { get; set; } = new List<TransHistory>();
    public Tran() { }
    public Tran(int type, int branch, int senderId, int? receiverId, decimal amount){
        this.TransType = type;
        this.BranchId = branch;
        this.SenderId = senderId;
        this.ReceiverId = receiverId ?? null;
        this.Amount = amount;
        this.DateTrans = DateTime.Now;
    }
}
