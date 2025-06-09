using DB_BankProject.ModelsDB;
using DB_BankProject.ModelsNative;
using DB_BankProject.Properties;
using FontAwesome.Sharp;
using Guna.UI2.WinForms;

namespace DB_BankProject.ModulesFrm {
    internal class FrmUserHome : Form {
        //----------------------------Atributos----------------------------
        private bool show;
        private readonly FrmUserMain menu;
        private readonly UserAccount user;
        private readonly DataService data;
        private Guna2DataGridView dataTrans;
        private Guna2DataGridView dataInvs;
        private Label lblMoney;
        private Guna2Button btnShowHideMoney;
        private Guna2Button btnWithdraw;
        private Guna2Button btnDeposit;
        private Guna2Button btnTransfer;
        private Guna2Button btnInvest;
        private Guna2Button btnRefresh;
        private Guna2MessageDialog msg;

        public FrmUserHome(UserAccount user, DataService data, FrmUserMain menu){
            this.user = user;
            this.data = data;
            this.menu = menu;
            this.InitAttrs();
            this.InitCompts();
            LoadData();
        }
        //----------------------------BOTONES----------------------------
        private void btnShowHideMoney_Click(object sender, EventArgs e){
            this.show = !this.show;
            lblMoney.Text = show ? $"{this.user.Funds}" : "***";
            btnShowHideMoney.Text = show ? "🔒" : "👁️";
        }
        private void btnWithdraw_Click(object sender, EventArgs e){ 
            new FrmWithDraw(this.user, this.data).ShowDialog();
        }
        private void btnDeposit_Click(object sender, EventArgs e){ 
            new FrmDeposit(this.user, this.data).ShowDialog();
        }
        private void btnTransfer_Click(object sender, EventArgs e){
            new FrmTransfer(this.user, this.data).ShowDialog();
        }
        private void btnInvest_Click(object sender, EventArgs e){
            this.msg.Show("Para invertir dirigete a la seccion de Inversiones del panel izquierdo");
        }
        private void btnRefresh_Click(object sender, EventArgs e){ LoadData(); }
        //------------------------------FUNCIONES-------------------------------
        private void LoadData(){
            dataTrans.DataSource = null;
            dataInvs.DataSource = null;
            dataTrans.DataSource = this.data.GetUserTrans(this.user.Number, 5);
            dataInvs.DataSource = this.data.GetUserInvest(this.user.Number, 5);
            dataTrans.Refresh();
            dataInvs.Refresh();
        }
        //----------------------------INICIALIZACIONES----------------------------
        private void InitAttrs(){
            this.Text = "User Menu Home";
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
            TableLayoutPanel tlpMain = Default.GenTabLayPnl(18, 12, AppProperties.clrTrans, false);
            tlpMain.BackgroundImage = Resources.bgMenu;
            tlpMain.BackgroundImageLayout = ImageLayout.Stretch;

            //----------------------------Header----------------------------
            Label lblTit = Default.GenLabel($"HOLA!, {this.user.OwnerName}", ContentAlignment.MiddleLeft, 
                new Font(AppProperties.fntTitle.FontFamily, AppProperties.fntTitle.Size, FontStyle.Underline));
            IconPictureBox icn = Default.GenIconBox(IconChar.Briefcase, DockStyle.Fill, clr: AppProperties.clrTxt);

            tlpMain.Controls.Add(lblTit, 0, 0);
            tlpMain.SetColumnSpan(lblTit, 7);
            tlpMain.Controls.Add(icn, 11, 0);
            tlpMain.SetRowSpan(icn, 2);

            //----------------------------Main----------------------------
            Label lblDesc = Default.GenLabel("Saldo actual:", ContentAlignment.MiddleCenter, AppProperties.fntSubTitle);
            lblMoney = Default.GenLabel("***", ContentAlignment.TopLeft, AppProperties.fntTitle);
            Label lblNum = Default.GenLabel($"Numero: ******{this.user.Number.Substring(6)}", ContentAlignment.MiddleLeft, AppProperties.fntSubTitle);

            IconPictureBox icnMoney = Default.GenIconBox(IconChar.DollarSign, DockStyle.Fill, clr: AppProperties.clrTxt);
            IconPictureBox icnWithdraw = Default.GenIconBox(IconChar.ArrowRightFromBracket, DockStyle.Fill, clr: AppProperties.clrTxt);
            IconPictureBox icnDeposit = Default.GenIconBox(IconChar.TentArrowsDown, DockStyle.Fill, clr: AppProperties.clrTxt);
            IconPictureBox icnTransfer = Default.GenIconBox(IconChar.MoneyBillTransfer, DockStyle.Fill, clr: AppProperties.clrTxt);
            IconPictureBox icnInvest = Default.GenIconBox(IconChar.HandHoldingDollar, DockStyle.Fill, clr: AppProperties.clrTxt);

            this.show = false;
            btnShowHideMoney = Default.GenButton("👁️");
            btnShowHideMoney.Click += btnShowHideMoney_Click;
            btnWithdraw = Default.GenButton("Retirar", border: false);
            btnWithdraw.Click += btnWithdraw_Click;
            btnDeposit = Default.GenButton("Depositar", border: false);
            btnDeposit.Click += btnDeposit_Click;
            btnTransfer = Default.GenButton("Transfer", border: false);
            btnTransfer.Click += btnTransfer_Click;
            btnInvest = Default.GenButton("Invertir", border: false);
            btnInvest.Click += btnInvest_Click;
            
            tlpMain.Controls.Add(lblDesc, 1, 2);
            tlpMain.SetColumnSpan(lblDesc, 4);
            tlpMain.Controls.Add(icnMoney, 1, 3);
            tlpMain.SetRowSpan(icnMoney, 3);
            tlpMain.Controls.Add(lblMoney, 2, 3);
            tlpMain.SetColumnSpan(lblMoney, 4);
            tlpMain.SetRowSpan(lblMoney, 2);
            tlpMain.Controls.Add(btnShowHideMoney, 5, 2);
            tlpMain.Controls.Add(lblNum, 2, 5);
            tlpMain.SetColumnSpan(lblNum, 4);
            tlpMain.Controls.Add(btnWithdraw, 7, 2);
            tlpMain.SetColumnSpan(btnWithdraw, 2);
            tlpMain.Controls.Add(icnWithdraw, 9, 2);
            tlpMain.Controls.Add(btnDeposit, 7, 3);
            tlpMain.SetColumnSpan(btnDeposit, 2);
            tlpMain.Controls.Add(icnDeposit, 9, 3);
            tlpMain.Controls.Add(btnTransfer, 7, 4);
            tlpMain.SetColumnSpan(btnTransfer, 2);
            tlpMain.Controls.Add(icnTransfer, 9, 4);
            tlpMain.Controls.Add(btnInvest, 7, 5);
            tlpMain.SetColumnSpan(btnInvest, 2);
            tlpMain.Controls.Add(icnInvest, 9, 5);

            //----------------------------Footer----------------------------
            Label lblTrans = Default.GenLabel("Ultimos 5 movimientos", ContentAlignment.MiddleCenter, AppProperties.fntText);
            Label lblInvs = Default.GenLabel("Ultimas 5 inversiones", ContentAlignment.MiddleCenter, AppProperties.fntText);
            dataTrans = Default2.GenDataGrid(true);
            dataTrans.ScrollBars = ScrollBars.None;
            dataInvs = Default2.GenDataGrid(true);
            dataInvs.ScrollBars = ScrollBars.None;

            btnRefresh = Default.GenButton("🔄 Refrescar");
            btnRefresh.Click += btnRefresh_Click;

            tlpMain.Controls.Add(lblTrans, 1, 7);
            tlpMain.SetColumnSpan(lblTrans, 10);
            tlpMain.Controls.Add(dataTrans, 1, 8);
            tlpMain.SetColumnSpan(dataTrans, 10);
            tlpMain.SetRowSpan(dataTrans, 4);
            tlpMain.Controls.Add(lblInvs, 1, 12);
            tlpMain.SetColumnSpan(lblInvs, 10);
            tlpMain.Controls.Add(dataInvs, 1, 13);
            tlpMain.SetColumnSpan(dataInvs, 10);
            tlpMain.SetRowSpan(dataInvs, 4);
            tlpMain.Controls.Add(btnRefresh, 5, 17);
            tlpMain.SetColumnSpan(btnRefresh, 2);

            //----------------------------FIN CONTENEDOR PRINCIPAL----------------------------

            this.Controls.Add(tlpMain);
        }
    }
}
