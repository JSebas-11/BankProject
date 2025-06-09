using DB_BankProject.ModelsDB;
using DB_BankProject.ModelsNative;
using DB_BankProject.Properties;
using FontAwesome.Sharp;
using Guna.UI2.WinForms;
using System.Data;

namespace DB_BankProject.ModulesFrm {
    internal class FrmAdminHome : Form {
        //----------------------------Atributos----------------------------
        private readonly FrmAdminMain menu;
        private readonly AdminAccount admin;
        private readonly DataService data;
        private Guna2TextBox txbQuery;
        private Guna2DataGridView dataQuery;
        private Guna2Button btnQuery;
        private Guna2MessageDialog msg;

        public FrmAdminHome(AdminAccount admin, DataService data, FrmAdminMain menu){
            this.admin = admin;
            this.data = data;
            this.menu = menu;
            this.InitAttrs();
            this.InitCompts();
        }
        //----------------------------BOTONES----------------------------
        private void btnQuery_Click(object sender, EventArgs e){
            if (!CorrectQuery(txbQuery.Text.ToUpper())){
                msg.Show("Error en la query:\n-Solo se permiten SELECT queries\n-Muy corta");
                return;
            }
            LoadData();
        }
        //------------------------------FUNCIONES-------------------------------
        private void LoadData(){
            DataTable? queryRes = data.ExecuteQuery(txbQuery.Text.ToUpper());
            if (queryRes == null){
                msg.Show("No se pudo ejecutar la query correctamente");
                return;
            }
            dataQuery.DataSource = null;
            dataQuery.DataSource = queryRes;
            dataQuery.Refresh();
        }
        private bool CorrectQuery(string query){
            if (query.Trim().Length < 10 || !query.Trim().StartsWith("SELECT")) {
                return false;
            };
            return true;
        }
        //----------------------------INICIALIZACIONES----------------------------
        private void InitAttrs(){
            this.Text = "Admin Menu Home";
            this.Dock = DockStyle.Fill;
            this.ControlBox = false;
            this.Padding = new Padding(0);
            this.Margin = new Padding(0);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.msg = new Guna2MessageDialog(){
                Style = MessageDialogStyle.Dark,
                Caption = "Informacion",
                Buttons = MessageDialogButtons.OK,
                Icon = MessageDialogIcon.Information,
                Parent = this.menu
            };
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.UpdateStyles();
        }
        private void InitCompts(){
            //----------------------------CONTENEDOR PRINCIPAL----------------------------
            TableLayoutPanel tlpMain = Default.GenTabLayPnl(12, 10, AppProperties.clrTrans, false);
            tlpMain.BackgroundImage = Resources.bgMenu;
            tlpMain.BackgroundImageLayout = ImageLayout.Stretch;

            //----------------------------Header----------------------------
            Label lblAdmin = Default.GenLabel($"Hola admin, {this.admin.OwnerName}", ContentAlignment.MiddleLeft,
                new Font(AppProperties.fntTitle.Name, AppProperties.fntTitle.Size, FontStyle.Underline));
            IconPictureBox icnHead = Default.GenIconBox(IconChar.UserTie, DockStyle.Fill, clr: AppProperties.clrTxt);

            tlpMain.Controls.Add(lblAdmin, 0, 0);
            tlpMain.SetColumnSpan(lblAdmin, 4);
            tlpMain.Controls.Add(icnHead, 9, 0);
            tlpMain.SetRowSpan(icnHead, 2);

            //----------------------------Main (info)----------------------------
            Label lblData = Default.GenLabel("Informacion general", ContentAlignment.MiddleCenter, AppProperties.fntTitle);
            Label lblUser = Default.GenLabel($"Usuarios actuales: {data.GetTotalUser()}", ContentAlignment.MiddleLeft, AppProperties.fntSubTitle);
            Label lblTrans = Default.GenLabel($"Total transacciones: {data.GetTotalTrans()}", ContentAlignment.MiddleLeft, AppProperties.fntSubTitle);
            Label lblMoney = Default.GenLabel($"Almacenado: ${data.GetMoney()}", ContentAlignment.MiddleLeft, AppProperties.fntSubTitle);
            Label lblInvest = Default.GenLabel($"Invertido: ${data.GetTotalInvest()}", ContentAlignment.MiddleLeft, AppProperties.fntSubTitle);

            IconPictureBox icnUser = Default.GenIconBox(IconChar.Users, DockStyle.Fill, clr: AppProperties.clrTxt);
            IconPictureBox icnTrans = Default.GenIconBox(IconChar.MoneyCheck, DockStyle.Fill, clr: AppProperties.clrTxt);
            IconPictureBox icnMoney = Default.GenIconBox(IconChar.Coins, DockStyle.Fill, clr: AppProperties.clrTxt);
            IconPictureBox icnInvest = Default.GenIconBox(IconChar.PiggyBank, DockStyle.Fill, clr: AppProperties.clrTxt);

            tlpMain.Controls.Add(lblData, 3, 1);
            tlpMain.SetColumnSpan(lblData, 4);
            tlpMain.Controls.Add(lblUser, 2, 2);
            tlpMain.SetColumnSpan(lblUser, 3);
            tlpMain.Controls.Add(icnUser, 1, 2);
            tlpMain.Controls.Add(lblTrans, 6, 2);
            tlpMain.SetColumnSpan(lblTrans, 3);
            tlpMain.Controls.Add(icnTrans, 5, 2);
            tlpMain.Controls.Add(lblMoney, 2, 3);
            tlpMain.SetColumnSpan(lblMoney, 3);
            tlpMain.Controls.Add(icnMoney, 1, 3);
            tlpMain.Controls.Add(lblInvest, 6, 3);
            tlpMain.SetColumnSpan(lblInvest, 3);
            tlpMain.Controls.Add(icnInvest, 5, 3);

            //----------------------------Main (query)----------------------------
            txbQuery = Default.GenTxtBox("Ingresa tu query personalizada", 100);
            TableLayoutPanel tlpQuery = Default2.GenInputCard(IconChar.Database, txbQuery, false);

            btnQuery = Default.GenButton("▶ Ejecutar");
            btnQuery.Click += btnQuery_Click;
            dataQuery = Default2.GenDataGrid(true);

            tlpMain.Controls.Add(tlpQuery, 1, 5);
            tlpMain.SetColumnSpan(tlpQuery, 8);
            tlpMain.Controls.Add(btnQuery, 4, 6);
            tlpMain.SetColumnSpan(btnQuery, 2);
            tlpMain.Controls.Add(dataQuery, 2, 7);
            tlpMain.SetColumnSpan(dataQuery, 6);
            tlpMain.SetRowSpan(dataQuery, 4);

            //----------------------------FIN CONTENEDOR PRINCIPAL----------------------------
            this.Controls.Add(tlpMain);
        }
    }
}
