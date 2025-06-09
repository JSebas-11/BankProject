using DB_BankProject.ModelsDB;
using DB_BankProject.ModelsNative;
using FontAwesome.Sharp;
using Guna.UI2.WinForms;

namespace DB_BankProject.ModulesFrm {
    internal class FrmAdminMain : Form {
        //----------------------------Atributos----------------------------
        private readonly FrmLogin logRef;
        private readonly IPerson current;
        private readonly DataService data;
        private AdminAccount? admin;
        private Guna2Button btnHome;
        private Guna2Button btnUsers;
        private Guna2Button btnHistory;
        private Guna2Button btnConfig;
        private Guna2Button btnBack;
        //Panel en el que se mostrara el contenido del menu en cuestion
        private PnlDoubleBuff pnlContent;
        private Form currentFrm;

        public FrmAdminMain(FrmLogin logReference, IPerson current, DataService data){
            this.logRef = logReference;
            this.current = current;
            this.data = data;
            this.admin = this.data.GetAdmin(current.Number);
            this.InitAttrs();
            this.InitCompts();
        }
        //----------------------------BOTONES----------------------------
        private void btnHome_Click(object sender, EventArgs e){
            OpenSection(new FrmAdminHome(this.admin, this.data, this));
        }
        private void btnUsers_Click(object sender, EventArgs e){
            OpenSection(new FrmAdminUsers(this.data));
        }
        private void btnConfig_Click(object sender, EventArgs e){
            OpenSection(new FrmConfig(this.current, this.data));
        }
        private void btnHistory_Click(object sender, EventArgs e){
            OpenSection(new FrmAdminHistory(this.data));
        }
        private void btnBack_Click(object sender, EventArgs e){
            this.Close();
            this.logRef.Show();
        }
        //------------------------------FUNCIONES-------------------------------
        private bool IsCurrent(Form frm){
            //Verificar si el form esta abierto, para evitar cargarlo de nuevo
            if (currentFrm != null && currentFrm.GetType() == frm.GetType()){
                return true;
            }
            return false;
        }
        private void OpenSection(Form frm){
            if (IsCurrent(frm)) { return; }
            pnlContent.Controls.Clear();
            //Desactivar topLevel de los Formularios y le quitar borde con los botones
            frm.TopLevel = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            pnlContent.Controls.Add(frm);
            pnlContent.Tag = frm;
            currentFrm = frm;
            frm.Show();
        }
        //----------------------------INICIALIZACIONES----------------------------
        private void InitAttrs(){
            this.Text = "Admin Menu";
            this.ClientSize = AppProperties.szScreen;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.ControlBox = false;

        }
        private void InitCompts(){
            //----------------------------CONTENEDOR PRINCIPAL----------------------------
            TableLayoutPanel tlpMain = Default.GenTabLayPnl(1, 2, AppProperties.clrTrans, true, false);
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25.0f));
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65.0f));

            //----------------------------ASIDE----------------------------
            TableLayoutPanel tlpAside = Default.GenTabLayPnl(5, 1, AppProperties.clrBgBlue, true);

            btnHome = Default.GenButton("Menu Principal");
            btnHome.Click += btnHome_Click;
            TableLayoutPanel tlpBtnHome = Default2.GenButtonCard("Informacion general y ejecuta tus propias SQL queries", 
                btnHome, IconChar.Home);

            btnUsers = Default.GenButton("Usuarios");
            btnUsers.Click += btnUsers_Click;
            TableLayoutPanel tlpBtnUsers = Default2.GenButtonCard("Consulta y administra informacion de usuarios registrados",
                btnUsers, IconChar.User);

            btnHistory = Default.GenButton("Trans/Inversiones");
            btnHistory.Click += btnHistory_Click;
            TableLayoutPanel tlpBtnHistory = Default2.GenButtonCard("Visualiza todas las transacciones e inversiones realizadas por usuarios",
                btnHistory, IconChar.History);

            btnConfig = Default.GenButton("Configuracion");
            btnConfig.Click += btnConfig_Click;
            TableLayoutPanel tlpBtnConfig = Default2.GenButtonCard("Accede a tus datos personales y modifica la informacion relevante",
                btnConfig, IconChar.Gear);

            btnBack = Default.GenButton("Volver");
            btnBack.Click += btnBack_Click;
            TableLayoutPanel tlpBtnBack = Default2.GenButtonCard("Cierra sesion y regresa a la pantalla de inicio de sesion",
                btnBack, IconChar.Backward);

            tlpAside.Controls.Add(tlpBtnHome, 0, 0);
            tlpAside.Controls.Add(tlpBtnUsers, 0, 1);
            tlpAside.Controls.Add(tlpBtnHistory, 0, 2);
            tlpAside.Controls.Add(tlpBtnConfig, 0, 3);
            tlpAside.Controls.Add(tlpBtnBack, 0, 4);
            
            //----------------------------Main----------------------------
            pnlContent = new PnlDoubleBuff() { Dock = DockStyle.Fill };
            OpenSection(new FrmAdminHome(this.admin, this.data, this));

            //----------------------------FIN CONTENEDOR PRINCIPAL----------------------------
            tlpMain.Controls.Add(tlpAside, 0, 0);
            tlpMain.Controls.Add(pnlContent, 1, 0);

            this.Controls.Add(tlpMain);

        }
    }
}
