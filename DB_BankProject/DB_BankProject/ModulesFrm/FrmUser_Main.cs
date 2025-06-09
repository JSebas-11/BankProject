using DB_BankProject.ModelsDB;
using DB_BankProject.ModelsNative;
using DB_BankProject.Properties;
using FontAwesome.Sharp;
using Guna.UI2.WinForms;
using System.Data;
using System.Drawing.Drawing2D;

namespace DB_BankProject.ModulesFrm {
    internal class FrmUserMain : Form {
        //----------------------------Atributos----------------------------
        private readonly FrmLogin logRef;
        private readonly IPerson current;
        private readonly DataService data;
        private UserAccount? user;
        private AdminAccount admin;
        private Guna2Button btnHome;
        private Guna2Button btnTrans;
        private Guna2Button btnInvest;
        private Guna2Button btnConfig;
        private Guna2Button btnBack;
        //Panel en el que se mostrara el contenido del menu en cuestion
        private PnlDoubleBuff pnlContent;
        private Form currentFrm;

        public FrmUserMain(FrmLogin logReference, IPerson current, DataService data){
            this.logRef = logReference;
            this.current = current;
            this.data = data;
            this.user = data.GetUser(current.Number);
            this.InitAttrs();
            this.InitCompts();
        }
        //----------------------------BOTONES----------------------------
        private void btnHome_Click(object sender, EventArgs e){
            OpenSection(new FrmUserHome(this.user, this.data, this));
        }
        private void btnTrans_Click(object sender, EventArgs e){
            OpenSection(new FrmUserReg(this.user, this.data));
        }
        private void btnInvest_Click(object sender, EventArgs e){
            OpenSection(new FrmUserInvest(this.user, this.data));
        }
        private void btnConfig_Click(object sender, EventArgs e){
            OpenSection(new FrmConfig(this.current, this.data));
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
            this.Text = "User Menu";
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
            TableLayoutPanel tlpBtnHome = Default2.GenButtonCard("Informacion general y ultimos movimientos", 
                btnHome, IconChar.Home);

            btnTrans = Default.GenButton("Movimientos");
            btnTrans.Click += btnTrans_Click;
            TableLayoutPanel tlpBtnTrans = Default2.GenButtonCard("Consulta el registro de todos tus movimientos",
                btnTrans, IconChar.MoneyCheckDollar);

            btnInvest = Default.GenButton("Inversiones");
            btnInvest.Click += btnInvest_Click;
            TableLayoutPanel tlpBtnInvest = Default2.GenButtonCard("Consulta tus inversiones o crea nuevas",
                btnInvest, IconChar.Seedling);

            btnConfig = Default.GenButton("Configuracion");
            btnConfig.Click += btnConfig_Click;
            TableLayoutPanel tlpBtnConfig = Default2.GenButtonCard("Accede a tus datos personales y modifica la informacion relevante",
                btnConfig, IconChar.Gear);

            btnBack = Default.GenButton("Volver");
            btnBack.Click += btnBack_Click;
            TableLayoutPanel tlpBtnBack = Default2.GenButtonCard("Cierra sesion y regresa a la pantalla de inicio de sesion",
                btnBack, IconChar.Backward);

            tlpAside.Controls.Add(tlpBtnHome, 0, 0);
            tlpAside.Controls.Add(tlpBtnTrans, 0, 1);
            tlpAside.Controls.Add(tlpBtnInvest, 0, 2);
            tlpAside.Controls.Add(tlpBtnConfig, 0, 3);
            tlpAside.Controls.Add(tlpBtnBack, 0, 4);
            
            //----------------------------Main----------------------------
            pnlContent = new PnlDoubleBuff() { Dock = DockStyle.Fill };
            OpenSection(new FrmUserHome(this.user, this.data, this));

            //----------------------------FIN CONTENEDOR PRINCIPAL----------------------------
            tlpMain.Controls.Add(tlpAside, 0, 0);
            tlpMain.Controls.Add(pnlContent, 1, 0);

            this.Controls.Add(tlpMain);

        }
    }
}
