namespace DB_BankProject.ModelsNative;
//Interfaz que nos ayudara para el login y el acceso a x permisos
public interface IPerson {
    public string Number { get; set; }
    public string HashPassword { get; set; }
    public string OwnerName { get; set; }
    public PersonType PersonType { get; set; }
}
