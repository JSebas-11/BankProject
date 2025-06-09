
using DB_BankProject.ModelsNative;
using DB_BankProject.Properties;
using FontAwesome.Sharp;
using Guna.UI2.WinForms;
using System.Windows.Interop;


namespace DB_BankProject.ModulesFrm {
    internal class FrmConfig : Form {
        //----------------------------Atributos----------------------------
        private readonly IPerson person;
        private DataService data;
        private Guna2Button btnEdit;
        private Guna2Button btnChangePass;
        private Label lblNum;
        private Label lblName;
        public FrmConfig(IPerson current, DataService data){
            this.person = current;
            this.data = data;
            this.InitAttrs();
            this.InitCompts();
        }
        //----------------------------BOTONES----------------------------
        private void btnEdit_Click(object sender, EventArgs e){
            new FrmEdit(this.person, this.data).ShowDialog();
            LoadLabels();
        }
        private void btnChangePass_Click(object sender, EventArgs e){
            new FrmChangePass(this.person, this.data).ShowDialog();
        }
        //------------------------------FUNCIONES-------------------------------
        public void LoadLabels(){
            lblNum.Text = $"Numero asociado: {person.Number}";
            lblName.Text = $"Titular: {person.OwnerName}";
        }
        //----------------------------INICIALIZACIONES----------------------------
        private void InitAttrs(){
            this.Text = "Configuration";
            this.Dock = DockStyle.Fill;
            this.ControlBox = false;
            this.Padding = new Padding(0);
            this.Margin = new Padding(0);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.UpdateStyles();
        }
        private void InitCompts(){
            //----------------------------CONTENEDOR PRINCIPAL----------------------------
            TableLayoutPanel tlpMain = Default.GenTabLayPnl(10, 10, AppProperties.clrTrans, false);
            tlpMain.BackgroundImage = Resources.gradientBG;
            tlpMain.BackgroundImageLayout = ImageLayout.Stretch;

            //----------------------------Header----------------------------
            IconPictureBox icn = Default.GenIconBox(IconChar.Server, DockStyle.Fill);
            Label lblTit = Default.GenLabel("DATOS PERSONALES", ContentAlignment.MiddleCenter, AppProperties.fntTitle);

            tlpMain.Controls.Add(icn, 9, 0);
            tlpMain.SetRowSpan(icn, 2);
            tlpMain.Controls.Add(lblTit, 3, 1);
            tlpMain.SetColumnSpan(lblTit, 4);
            tlpMain.SetRowSpan(lblTit, 2);
            
            //----------------------------Main----------------------------
            IconPictureBox icnType = Default.GenIconBox(IconChar.AddressCard, DockStyle.Fill, clr: AppProperties.clrTxtGray);
            Label lblType = Default.GenLabel($"Tipo usuario: {person.PersonType}", ContentAlignment.MiddleLeft, AppProperties.fntSubTitle);
            IconPictureBox icnNum = Default.GenIconBox(IconChar.Phone, DockStyle.Fill, clr: AppProperties.clrTxtGray);
            lblNum = Default.GenLabel($"Numero asociado: {person.Number}", ContentAlignment.MiddleLeft, AppProperties.fntSubTitle);
            IconPictureBox icnName = Default.GenIconBox(IconChar.UserTie, DockStyle.Fill, clr: AppProperties.clrTxtGray);
            lblName = Default.GenLabel($"Titular: {person.OwnerName}", ContentAlignment.MiddleLeft, AppProperties.fntSubTitle);

            tlpMain.Controls.Add(icnType, 2, 3);
            tlpMain.Controls.Add(lblType, 3, 3);
            tlpMain.SetColumnSpan(lblType, 5);
            tlpMain.Controls.Add(icnNum, 2, 4);
            tlpMain.Controls.Add(lblNum, 3, 4);
            tlpMain.SetColumnSpan(lblNum, 5);
            tlpMain.Controls.Add(icnName, 2, 5);
            tlpMain.Controls.Add(lblName, 3, 5);
            tlpMain.SetColumnSpan(lblName, 5);

            //----------------------------Main----------------------------
            btnEdit = Default.GenButton("✏️ Editar Datos");
            btnEdit.Click += btnEdit_Click;

            btnChangePass = Default.GenButton("🔐 Cambiar contraseña");
            btnChangePass.Click += btnChangePass_Click;

            tlpMain.Controls.Add(btnEdit, 1, 8);
            tlpMain.SetColumnSpan(btnEdit, 3);
            tlpMain.Controls.Add(btnChangePass, 6, 8);
            tlpMain.SetColumnSpan(btnChangePass, 3);
            
            //----------------------------FIN CONTENEDOR PRINCIPAL----------------------------
            this.Controls.Add(tlpMain);
        }
    }
}
