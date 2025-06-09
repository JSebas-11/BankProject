using DB_BankProject.ModelsNative;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB_BankProject.ModelsDB;

public partial class UserAccount : IPerson {
    public int UserId { get; set; }

    public string Number { get; set; } = null!;

    public string HashPassword { get; set; } = null!;

    public string OwnerName { get; set; } = null!;

    public decimal Funds { get; set; }
    public decimal InvestedMoney { get; set; }

    public DateTime? RegisteredAt { get; set; }

    public virtual ICollection<Cdt> Cdts { get; set; } = new List<Cdt>();

    public virtual ICollection<Tran> TranReceivers { get; set; } = new List<Tran>();

    public virtual ICollection<Tran> TranSenders { get; set; } = new List<Tran>();

    public virtual ICollection<TransHistory> TransHistories { get; set; } = new List<TransHistory>();
    [NotMapped]
    public PersonType PersonType { get; set; }
    public UserAccount() { }
    public UserAccount(string owner, string number, string password){
        this.OwnerName = owner.Trim().ToUpper();
        this.Number = number;
        this.HashPassword = BCrypt.Net.BCrypt.HashPassword(password.Trim());
        this.Funds = 0;
        this.InvestedMoney = 0;
        this.RegisteredAt = DateTime.Now;
        this.PersonType = PersonType.User;
    }
    public bool Enough(decimal amount){ return this.Funds >= amount; }
    public void WithDraw(decimal amount){ this.Funds -= amount; }
    public void Deposit(decimal amount){ this.Funds += amount; }
    public void Transfer(decimal amount, UserAccount target){
        WithDraw(amount);
        target.Deposit(amount);
    }
    public void Invest(decimal amount){ 
        this.Funds -= amount; 
        this.InvestedMoney += amount; 
    }
}
