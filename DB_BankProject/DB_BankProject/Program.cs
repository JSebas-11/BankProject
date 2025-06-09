using DB_BankProject;
using DB_BankProject.ModelsDB;
using DB_BankProject.ModulesFrm;
using DB_BankProject.ModelsNative;

internal static class Program {
    [STAThread]
    static void Main(){
        var context = new DataService(new BankProjectContext());

        if (!context.ExistsPerson("1111111111")){
            //Creacion usuario administrador por defecto si no existe
            context.AddAdmin(new AdminAccount("delgado", "1111111111", "sudo"));
        }

        ApplicationConfiguration.Initialize();
        Application.Run(new FrmLogin(context));
    }
}
