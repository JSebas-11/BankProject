using DB_BankProject.ModelsDB;
using DB_BankProject.ModelsNative;
using DB_BankProject.Properties;
using FontAwesome.Sharp;
using Guna.UI2.WinForms;


namespace DB_BankProject.ModulesFrm {
    internal class FrmUserInvest : Form {
        //----------------------------Atributos----------------------------
        private readonly UserAccount user;
        private readonly DataService data;
        private readonly decimal intRate;
        private Label lblFund;
        private Label lblInvested;
        private Label lblProfit;
        private Guna2TextBox txbAmount;
        private Guna2ComboBox cmbTime;
        private Guna2Button btnRefresh;
        private Guna2Button btnSimulate;
        private Guna2Button btnInvest;
        private Guna2DataGridView dataGrid;
        public FrmUserInvest(UserAccount user, DataService data){
            this.user = user;
            this.data = data;
            this.intRate = AppProperties.interestRate;
            this.InitAttrs();
            this.InitCompts();
            LoadData();
        }
        //----------------------------BOTONES----------------------------
        private void btnRefresh_Click(object sender, EventArgs e){ 
            LoadData();
            lblFund.Text = $"Disponible en cuenta: ${this.user.Funds}";
            lblInvested.Text = $"Total inversiones: ${this.user.InvestedMoney}";
        }
        private void btnSimulate_Click(object sender, EventArgs e){
            if (Empty()) { return; }
            decimal total = CalculateProfit();
            lblProfit.Text = $"Puedes generar:\n${total}\nTotal:\n${decimal.Parse(txbAmount.Text)+total}";
        }
        private void btnInvest_Click(object sender, EventArgs e){
            if (Empty()) { return; }
            if (!this.user.Enough(decimal.Parse(txbAmount.Text))) { 
                MessageBox.Show("No tienes suficiente saldo en cuenta");
                return; 
            }
            decimal amount = decimal.Parse(txbAmount.Text);
            decimal profit = CalculateProfit();
            int months = (int)cmbTime.SelectedItem;
            if (MessageBox.Show(this, $"Vas a invertir ${amount} a {months} meses, generando ${amount+profit}", 
                "Confirmation", MessageBoxButtons.OKCancel) == DialogResult.Cancel) {
                MessageBox.Show("Inversion cancelada");
                return;
            };
            MakeInvest(amount, profit, months);
        }
        private void txbAmount_KeyPress(object sender, KeyPressEventArgs e){
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Enter && e.KeyChar != (char)Keys.Back){
                e.Handled = true;
                MessageBox.Show(this, "Debes ingresar la cantidad a invertir");
            }
        }
        //------------------------------FUNCIONES-------------------------------
        private void MakeInvest(decimal invested, decimal profit, int months){
            Tran newTran = new Tran((int)TransType.Investment, (int)Branches.Digital, this.user.UserId, this.user.UserId, invested);
            Cdt newCdt = new Cdt(this.user.UserId, invested, profit, months);
            if (!this.data.AddTrans(newTran) || !this.data.AddCdt(newCdt)){
                MessageBox.Show(this, "Hubo un fallo en la inversion intenta de nuevo");
                return;
            }
            this.user.Invest(decimal.Parse(txbAmount.Text));
            this.data.ApplyChanges();
            MessageBox.Show(this, "Inversion aprobada, tu transaccion puede tardar unos segundos");
            CommonFunct.RandowWaiting();
            MessageBox.Show(this, "Inversion realizado con exito");
            txbAmount.Text = "";
            lblProfit.Text = "Puedes generar:\n$\nTotal:\n$";
        }
        private void LoadData(){
            dataGrid.DataSource = null;
            dataGrid.DataSource = this.data.GetUserInvest(this.user.Number);
            dataGrid.Refresh();
        }
        private decimal CalculateProfit(){
            return Math.Round(decimal.Parse(txbAmount.Text) * (this.intRate / 100) * (decimal.Parse(cmbTime.Text)*30 / 360), 2);
        }
        private bool Empty(){
            if (CommonFunct.EmptyInput(txbAmount.Text)) {
                MessageBox.Show(this, "Debes ingresar la cantidad a invertir");
                return true;
            };
            return false;
        }
        //----------------------------INICIALIZACIONES----------------------------
        private void InitAttrs(){
            this.Text = "User Home";
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
            tlpMain.BackgroundImage = Resources.userBg;
            tlpMain.BackgroundImageLayout = ImageLayout.Stretch;

            //----------------------------header----------------------------
            IconPictureBox icn = Default.GenIconBox(IconChar.MoneyBillTrendUp, DockStyle.Fill);

            lblFund = Default.GenLabel($"Disponible en cuenta: ${this.user.Funds}", ContentAlignment.MiddleLeft, AppProperties.fntText);
            lblInvested = Default.GenLabel($"Total inversiones: ${this.user.InvestedMoney}", ContentAlignment.MiddleLeft, AppProperties.fntText);

            tlpMain.Controls.Add(lblFund, 1, 0);
            tlpMain.SetColumnSpan(lblFund, 4);
            tlpMain.Controls.Add(lblInvested, 5, 0);
            tlpMain.SetColumnSpan(lblInvested, 4);
            tlpMain.Controls.Add(icn, 9, 0);
            tlpMain.SetRowSpan(icn, 2);

            //----------------------------main----------------------------
            TableLayoutPanel tlpSim = Default.GenTabLayPnl(3, 10, AppProperties.clrMainBlue, false);
            Label lblInv = Default.GenLabel("Tu inversion:", ContentAlignment.MiddleCenter, AppProperties.fntSubTitle);
            Label lblInv2 = Default.GenLabel("Plazo en meses:", ContentAlignment.MiddleCenter, AppProperties.fntSubTitle);
            Label lblInv3 = Default.GenLabel($"Tasa interes: %{this.intRate}", ContentAlignment.MiddleCenter, AppProperties.fntText);
            lblProfit = Default.GenLabel("Puedes generar:\n$\nTotal:\n$", ContentAlignment.MiddleCenter, AppProperties.fntSubTitle);

            txbAmount = Default.GenTxtBox("$", 15);
            txbAmount.FillColor = AppProperties.clrTrans;
            txbAmount.KeyPress += txbAmount_KeyPress;
            cmbTime = Default2.GenComBox(AppProperties.paymenTerm);

            btnRefresh = Default.GenButton("🔄");
            btnRefresh.Click += btnRefresh_Click;
            btnSimulate = Default.GenButton("Simular");
            btnSimulate.Click += btnSimulate_Click;
            btnInvest = Default.GenButton("Invertir");
            btnInvest.Click += btnInvest_Click;

            tlpSim.Controls.Add(lblInv, 0, 0);
            tlpSim.SetColumnSpan(lblInv, 2);
            tlpSim.Controls.Add(lblInv2, 0, 1);
            tlpSim.SetColumnSpan(lblInv2, 2);
            tlpSim.Controls.Add(txbAmount, 1, 0);
            tlpSim.SetColumnSpan(txbAmount, 3);
            tlpSim.Controls.Add(cmbTime, 1, 1);
            tlpSim.SetColumnSpan(cmbTime, 3);
            tlpSim.Controls.Add(lblProfit, 4, 0);
            tlpSim.SetColumnSpan(lblProfit, 3);
            tlpSim.SetRowSpan(lblProfit, 2);
            tlpSim.Controls.Add(lblInv3, 8, 0);
            tlpSim.SetColumnSpan(lblInv3, 2);
            tlpSim.Controls.Add(btnRefresh, 9, 2);
            tlpSim.Controls.Add(btnSimulate, 3, 2);
            tlpSim.SetColumnSpan(btnSimulate, 2);
            tlpSim.Controls.Add(btnInvest, 5, 2);
            tlpSim.SetColumnSpan(btnInvest, 2);
            
            dataGrid = Default2.GenDataGrid(true);

            tlpMain.Controls.Add(tlpSim, 1, 1);
            tlpMain.SetColumnSpan(tlpSim, 8);
            tlpMain.SetRowSpan(tlpSim, 3);
            tlpMain.Controls.Add(dataGrid, 1, 4);
            tlpMain.SetColumnSpan(dataGrid, 8);
            tlpMain.SetRowSpan(dataGrid, 5);
            
            //----------------------------FIN CONTENEDOR PRINCIPAL----------------------------
            this.Controls.Add(tlpMain);
        }
    }
}
