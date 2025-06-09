using DB_BankProject.ModelsNative;

namespace DB_BankProject.ModelsDB;

public partial class Cdt {
    public int CdtId { get; set; }

    public int? CdtStatus { get; set; }

    public int? UserId { get; set; }

    public decimal? Amount { get; set; }
    public decimal? Profit { get; set; }

    public decimal InterestRate { get; set; }

    public int DurationMonths { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
    public virtual UserAccount? User { get; set; }
    public Cdt() { }
    public Cdt(int userId, decimal amount, decimal profit, int months){
        this.CdtStatus = (int)InvestStatus.Active;
        this.UserId = userId;
        this.Amount = amount;
        this.Profit = profit;
        this.InterestRate = AppProperties.interestRate;
        this.DurationMonths = months;
        this.StartDate = DateTime.Now;
        this.EndDate = DateTime.Now.AddDays(DurationMonths*30);
    }
}
