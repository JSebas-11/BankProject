using DB_BankProject.ModelsNative;
using DB_BankProject.Properties;
using FontAwesome.Sharp;
using Guna.UI2.WinForms;

namespace DB_BankProject.ModulesFrm {
    internal class FrmAdminHistory : Form {
        //----------------------------Atributos----------------------------
        private readonly DataService data;
        private Guna2TextBox txbSearch;
        private Guna2ComboBox cmbFields;
        private Guna2Button btnSearch;
        private Guna2DataGridView dataTrans;
        private Guna2DataGridView dataInvest;
        public FrmAdminHistory(DataService data){
            this.data = data;
            this.InitAttrs();
            this.InitCompts();
            LoadData("All");
        }
        //----------------------------BOTONES----------------------------
        private void btnSearch_Click(object sender, EventArgs e){
            if (CommonFunct.EmptyInput(txbSearch.Text) && cmbFields.SelectedValue.ToString() != "All"){
                MessageBox.Show(this, "Input no puede estar vacio con filtro seleccionado");
                return;
            }
            LoadData(cmbFields.SelectedValue.ToString());
        }
        private void cmbFields_SelectedValueChanged(object sender, EventArgs e){
            txbSearch.Text = "";
            if (cmbFields.SelectedValue.ToString() == "Number"){ txbSearch.PlaceholderText = "numero usuario:"; } 
            else if (cmbFields.SelectedValue.ToString() == "Name"){ txbSearch.PlaceholderText = "nombre usuario:"; }
            else { txbSearch.PlaceholderText = "usuario"; }
        }
        //------------------------------FUNCIONES-------------------------------
        private void LoadData(string by){
            dataTrans.DataSource = null;
            dataInvest.DataSource = null;
            switch (by){
                case "All":
                    dataTrans.DataSource = this.data.ExecuteQuery("SELECT * FROM Trans");
                    dataInvest.DataSource = this.data.ExecuteQuery("SELECT * FROM CDT");
                    break;
                case "Number":
                    dataTrans.DataSource = this.data.GetUserTrans(txbSearch.Text);
                    dataInvest.DataSource = this.data.GetUserInvest(txbSearch.Text);
                    break;
                case "Name":
                    dataTrans.DataSource = this.data.GetUserTrans(txbSearch.Text, true);
                    dataInvest.DataSource = this.data.GetUserInvest(txbSearch.Text, true);
                    break;
                default:
                    break;
            }
            dataTrans.Refresh();
            dataInvest.Refresh();
        }
        //----------------------------INICIALIZACIONES----------------------------
        private void InitAttrs(){
            this.Text = "Admin Users History";
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
            TableLayoutPanel tlpMain = Default.GenTabLayPnl(10, 16, AppProperties.clrTrans, false);
            tlpMain.BackgroundImage = Resources.history;
            tlpMain.BackgroundImageLayout = ImageLayout.Stretch;

            //----------------------------Header----------------------------
            Label lblTit = Default.GenLabel("Datos Transacciones/Inversiones", ContentAlignment.MiddleCenter, AppProperties.fntTitle);
            Label lblDescOrd = Default.GenLabel("Para ordenar presiona sobre la columna deseada", ContentAlignment.MiddleLeft, AppProperties.fntSubTitle);
            Label lblDescSer = Default.GenLabel("Busca x usuario por:", ContentAlignment.MiddleLeft, AppProperties.fntSubTitle);
            IconPictureBox icnHead = Default.GenIconBox(IconChar.SackDollar, DockStyle.Fill, clr: AppProperties.clrTxt);
            IconPictureBox icnsort = Default.GenIconBox(IconChar.Sort, DockStyle.Fill, clr: AppProperties.clrTxt);
            IconPictureBox icnSearch = Default.GenIconBox(IconChar.SearchPlus, DockStyle.Fill, clr: AppProperties.clrTxt);

            btnSearch = Default.GenButton("Aplicar ⇅");
            btnSearch.Click += btnSearch_Click;
            cmbFields = Default2.GenComBox(new List<string>() { "All", "Number", "Name" });
            cmbFields.SelectedValueChanged += cmbFields_SelectedValueChanged;
            txbSearch = Default.GenTxtBox("usuario", AppProperties.txtInputSize);

            tlpMain.Controls.Add(lblTit, 2, 0);
            tlpMain.SetColumnSpan(lblTit, 12);
            tlpMain.Controls.Add(icnHead, 15, 0);
            tlpMain.SetRowSpan(icnHead, 2);
            tlpMain.Controls.Add(icnsort, 1, 1);
            tlpMain.Controls.Add(lblDescOrd, 2, 1);
            tlpMain.SetColumnSpan(lblDescOrd, 10);
            tlpMain.Controls.Add(icnSearch, 1, 2);
            tlpMain.Controls.Add(lblDescSer, 2, 2);
            tlpMain.SetColumnSpan(lblDescSer, 4);
            tlpMain.Controls.Add(cmbFields, 6, 2);
            tlpMain.SetColumnSpan(cmbFields, 3);
            tlpMain.Controls.Add(txbSearch, 9, 2);
            tlpMain.SetColumnSpan(txbSearch, 3);
            tlpMain.Controls.Add(btnSearch, 12, 2);
            tlpMain.SetColumnSpan(btnSearch, 3);

            //----------------------------Main----------------------------
            dataTrans = Default2.GenDataGrid(true);
            dataInvest = Default2.GenDataGrid(true);
            Label lblTrans = Default.GenLabel("Transacciones", ContentAlignment.TopCenter, AppProperties.fntTitle);
            Label lblInv = Default.GenLabel("Inversiones", ContentAlignment.TopCenter, AppProperties.fntTitle);

            tlpMain.Controls.Add(dataTrans, 1, 3);
            tlpMain.SetColumnSpan(dataTrans, 7);
            tlpMain.SetRowSpan(dataTrans, 6);
            tlpMain.Controls.Add(dataInvest, 8, 3);
            tlpMain.SetColumnSpan(dataInvest, 7);
            tlpMain.SetRowSpan(dataInvest, 6);
            tlpMain.Controls.Add(lblTrans, 2, 9);
            tlpMain.SetColumnSpan(lblTrans, 5);
            tlpMain.Controls.Add(lblInv, 9, 9);
            tlpMain.SetColumnSpan(lblInv, 5);

            //----------------------------FIN CONTENEDOR PRINCIPAL----------------------------
            this.Controls.Add(tlpMain);
        }
    }
}
