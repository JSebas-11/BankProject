using DB_BankProject.ModelsDB;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;
using System.Transactions;
using System.Windows.Forms;

namespace DB_BankProject.ModelsNative;
public class DataService : IDisposable {
    private readonly BankProjectContext _context;
    //Contructor y destructor
    public DataService(BankProjectContext context){ _context = context; }
    public void Dispose(){ _context.Dispose(); }
    //Obtener lista con todos los usuarios y administradores
    private List<IPerson> GetPeople(){
        List<IPerson> people = new List<IPerson>() { };
        var users = _context.UserAccounts.ToList();
        var admins = _context.AdminAccounts.ToList();

        //Recorremos cada lista de user y admin para agregarles su tipo, ya que prop no esta mappeada
        foreach (UserAccount item in users) { item.PersonType = PersonType.User; }
        foreach (AdminAccount item in admins) { item.PersonType = PersonType.Admin; }

        people.AddRange(users);
        people.AddRange(admins);
        return people;
    }
    public IPerson? GetPerson(string number, string pass){
        List<IPerson> people = GetPeople();
        //Comprobamos entre Administradores y Usuarios si coinciden los datos
        var person = people.FirstOrDefault(p => p.Number == number);
        if (person != null && BCrypt.Net.BCrypt.Verify(pass, person.HashPassword)){
            return person;
        }
        return null;
    }
    public List<PropertyInfo> GetProperties(Type objType){
        return [.. objType.GetProperties()];
    }
    public int? GetUserID(string number){
        UserAccount? user = GetUser(number);
        return user == null ? null : user.UserId;
    }
    public UserAccount? GetUser(string number){
        return _context.UserAccounts.FirstOrDefault(p => p.Number == number);
    }
    public AdminAccount? GetAdmin(string number){
        return _context.AdminAccounts.FirstOrDefault(p => p.Number == number);
    }
    public IQueryable<UserAccount> GetUsers(){ return _context.UserAccounts; }
    public IQueryable<Tran> GetTrans(){ return _context.Trans; }
    public IQueryable<Cdt> GetInvests(){ return _context.Cdts; }
    public int GetTotalUser(){ return _context.UserAccounts.Count(); }
    public decimal GetMoney(){ return _context.UserAccounts.Sum(u => u.Funds); }
    public decimal GetTotalTrans(){ return _context.Trans.Count(); }
    public decimal GetTotalInvest(){
        //El 4 es el numero de las transacciones que representan las inversiones
        return _context.Trans.Where(t => t.TransType == 4).Sum(t => t.Amount);
    }
    public bool ExistsPerson(string number, string pass){
        IPerson? person = GetPerson(number, pass);
        if (person == null){ return false; }
        return true;
    }
    public bool ExistsPerson(string number){
        return GetPeople().Exists(p => p.Number == number);
    }
    public void ApplyChanges(){ _context.SaveChanges(); }
    public void AddAdmin(AdminAccount toAdd){
        _context.AdminAccounts.Add(toAdd);
        _context.SaveChanges(); 
    }
    public bool AddAccount(UserAccount toAdd){
        using var trans = _context.Database.BeginTransaction();
        try {
            //Consulta sql de inserccion ya que tabla usuarios dispara triggers
            _context.Database.ExecuteSqlRaw("INSERT INTO UserAccount " +
                "(number, hashPassword, ownerName, funds, investedMoney, registeredAt) " +
                "VALUES (@p0, @p1, @p2, @p3, @p4, @p5)",
            toAdd.Number, toAdd.HashPassword, toAdd.OwnerName, toAdd.Funds, toAdd.InvestedMoney, toAdd.RegisteredAt);

            trans.Commit();
            return true;
        }   //Rollback automatico con using
        catch (Exception ex) {
            Console.WriteLine($"Error adding account: {ex.Message}");
            return false; 
        }
    }
    public bool DeleteAccount(string number){
        using var trans = _context.Database.BeginTransaction();
        try {
            //Consulta sql de borrado ya que tabla usuarios dispara triggers
            _context.Database.ExecuteSqlInterpolated($"DELETE UserAccount WHERE number = {number}");
            trans.Commit();
            return true;
        }   //Rollback automatico con using
        catch (Exception ex) {
            Console.WriteLine($"Error deleting account: {ex.Message}");
            return false;
        }
    }
    public DataTable? GetUserTrans(string number, int? limit = null){
        string top;
        if (limit == null){ top = ""; }
        else { top = $"TOP {limit}"; }

        return ExecuteQuery($"SELECT {top} ttype.typeDesc, u.ownerName AS sender, r.ownerName AS receiver, t.amount, b.branchLocation, t.dateTrans" +
                            " FROM Trans t " +
                            " INNER JOIN UserAccount u ON u.userId = t.senderId " +
                            " INNER JOIN UserAccount r ON r.userId = t.receiverId " +
                            " INNER JOIN TransType ttype ON ttype.transTypeId = t.transType " +
                            " INNER JOIN Branches b ON b.branchId = t.branchId " +
                            $" WHERE u.number = {number} OR r.number = {number}" +
                            $" ORDER BY t.dateTrans DESC;");
    }
    public DataTable? GetUserInvest(string number, int? limit = null){
        string top;
        if (limit == null) { top = ""; }
        else { top = $"TOP {limit}"; }

        return ExecuteQuery($"SELECT {top} u.ownerName, i.typeDesc AS stat, c.durationMonths, c.interestRate, c.amount, c.profit, c.startDate, c.endDate" +
                            " FROM CDT c " +
                            " INNER JOIN UserAccount u ON u.userId = c.userId " +
                            " INNER JOIN InvStatus i ON i.invId = c.cdtStatus " +
                            $" WHERE u.number = {number}" +
                            $" ORDER BY c.startDate DESC;");
    }
    public DataTable? GetUserTrans(string name, bool b){
        return ExecuteQuery($"SELECT ttype.typeDesc, u.ownerName AS sender, r.ownerName AS receiver, t.amount, b.branchLocation, t.dateTrans" +
                            " FROM Trans t " +
                            " INNER JOIN UserAccount u ON u.userId = t.senderId " +
                            " INNER JOIN UserAccount r ON r.userId = t.receiverId " +
                            " INNER JOIN TransType ttype ON ttype.transTypeId = t.transType " +
                            " INNER JOIN Branches b ON b.branchId = t.branchId " +
                            $" WHERE u.ownerName = '{name.ToUpper()}' OR r.ownerName = '{name.ToUpper()}'" +
                            $" ORDER BY t.dateTrans DESC;");
    }
    public DataTable? GetUserInvest(string name, bool b){
        return ExecuteQuery($"SELECT u.ownerName, i.typeDesc AS stat, c.durationMonths, c.interestRate, c.amount, c.profit, c.startDate, c.endDate" +
                            " FROM CDT c " +
                            " INNER JOIN UserAccount u ON u.userId = c.userId " +
                            " INNER JOIN InvStatus i ON i.invId = c.cdtStatus " +
                            $" WHERE u.ownerName = '{name.ToUpper()}'" +
                            $" ORDER BY c.startDate DESC;");
    }
    public DataTable? ExecuteQuery(string query){
        var table = new DataTable();
        using (var conn = new SqlConnection(BankProjectContext.StrConn)){
            try {
                conn.Open();
                using (var command = new SqlCommand(query, conn)){
                    using (var adpter = new SqlDataAdapter(command)){
                        adpter.Fill(table);
                    }
                }
            }
            catch (Exception ex){
                Console.WriteLine($"Error in query: {ex.Message}");
                return null;
            }
        }
        return table;
    }
    public bool AddTrans(Tran toAdd){
        using var trans = _context.Database.BeginTransaction();
        try {
            //Consulta sql de inserccion ya que tabla trans dispara triggers
            _context.Database.ExecuteSqlRaw("INSERT INTO Trans " +
                "(transType, branchId, senderId, receiverId, amount, dateTrans) " +
                "VALUES (@p0, @p1, @p2, @p3, @p4, @p5)",
            toAdd.TransType, toAdd.BranchId, toAdd.SenderId, toAdd.ReceiverId, toAdd.Amount, toAdd.DateTrans);

            trans.Commit();
            return true;
        }
        catch (Exception ex){
            Console.WriteLine($"Error with transacction: {ex.Message}");
            return false;
        }
    }
    public bool AddCdt(Cdt toAdd){
        try {
            _context.Cdts.Add(toAdd);
            _context.SaveChanges();
            return true;
        }
        catch (Exception ex){
            Console.WriteLine($"Error with CDT: {ex.Message}");
            return false;
        }
    }
}