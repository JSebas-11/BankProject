using DB_BankProject.ModelsDB;
using DB_BankProject.ModelsNative;
using DB_BankProject.Properties;
using FontAwesome.Sharp;
using Guna.UI2.WinForms;
using System.Data;

namespace DB_BankProject.ModulesFrm {
    internal class FrmUserReg : Form {
        //----------------------------Atributos----------------------------
        private readonly UserAccount user;
        private readonly DataService data;
        private readonly DataTable tbData;
        private int transNum;
        public FrmUserReg(UserAccount user, DataService data){
            this.user = user;
            this.data = data;
            this.tbData = this.data.GetUserTrans(this.user.Number);
            this.transNum = tbData.Rows.Count;
            this.InitAttrs();
            this.InitCompts();
        }
        //----------------------------INICIALIZACIONES----------------------------
        private void InitAttrs(){
            this.Text = "User History";
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
            tlpMain.BackgroundImage = Resources.history;
            tlpMain.BackgroundImageLayout = ImageLayout.Stretch;

            //----------------------------Main----------------------------
            IconPictureBox icn = Default.GenIconBox(IconChar.History, DockStyle.Fill, clr: AppProperties.clrTxt);

            Label lblTit = Default.GenLabel($"Cantidad transacciones: {transNum}", ContentAlignment.MiddleLeft, AppProperties.fntTitle);
            Guna2DataGridView dataGrid = Default2.GenDataGrid(true);
            dataGrid.DataSource = tbData; 

            tlpMain.Controls.Add(lblTit, 1, 0);
            tlpMain.SetColumnSpan(lblTit, 6);
            tlpMain.Controls.Add(icn, 9, 0);
            tlpMain.SetRowSpan(icn, 2);
            tlpMain.Controls.Add(dataGrid, 1, 1);
            tlpMain.SetColumnSpan(dataGrid, 8);
            tlpMain.SetRowSpan(dataGrid, 8);

            //----------------------------FIN CONTENEDOR PRINCIPAL----------------------------
            this.Controls.Add(tlpMain);
        }
    }
}
