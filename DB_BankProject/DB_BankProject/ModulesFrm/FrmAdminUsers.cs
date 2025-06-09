using DB_BankProject.ModelsDB;
using DB_BankProject.ModelsNative;
using DB_BankProject.Properties;
using FontAwesome.Sharp;
using Guna.UI2.WinForms;
using Microsoft.VisualBasic.ApplicationServices;
using System.Data;


namespace DB_BankProject.ModulesFrm {
    internal class FrmAdminUsers : Form {
        //----------------------------Atributos----------------------------
        private readonly DataService data;
        private Guna2Button btnUpdate;
        private Guna2Button btnDelete;
        private Guna2Button btnRefresh;
        private Guna2DataGridView dataUsers;
        public FrmAdminUsers(DataService data){
            this.data = data;
            this.InitAttrs();
            this.InitCompts();
            LoadData();
        }
        //----------------------------BOTONES----------------------------
        private void btnUpdate_Click(object sender, EventArgs e){
            UserAccount? user = GetSelectedUser();
            if (user == null){ return; }
            new FrmEdit((IPerson)user, this.data).ShowDialog();
        }
        private void btnDelete_Click(object sender, EventArgs e){
            UserAccount? user = GetSelectedUser();
            if (user == null) { return; }
            new FrmDelete((IPerson)user, this.data).ShowDialog();
        }
        private void btnRefresh_Click(object sender, EventArgs e){
            LoadData();
        }
        //------------------------------FUNCIONES-------------------------------
        private void LoadData(){
            dataUsers.DataSource = null;
            dataUsers.DataSource = this.data.GetUsers().
                                    Select(u => new {u.UserId, u.Number, u.OwnerName, u.Funds, u.InvestedMoney, u.HashPassword}).
                                    ToList();
            dataUsers.Refresh();
        }
        private UserAccount? GetSelectedUser(){
            if (dataUsers.SelectedRows.Count == 1){
                return data.GetUser(dataUsers.SelectedRows[0].Cells["Number"].Value.ToString());
            }
            return null;
        }
        //----------------------------INICIALIZACIONES----------------------------
        private void InitAttrs(){
            this.Text = "Admin Users";
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
            TableLayoutPanel tlpMain = Default.GenTabLayPnl(12, 10, AppProperties.clrTrans, false);
            tlpMain.BackgroundImage = Resources.userBg;
            tlpMain.BackgroundImageLayout = ImageLayout.Stretch;

            //----------------------------Header----------------------------
            Label lblTit = Default.GenLabel("Gestion de usuarios", ContentAlignment.MiddleCenter, AppProperties.fntTitle);
            Label lblDesc = Default.GenLabel("Elimina o actualiza campos permitidos de los usuarios", 
                ContentAlignment.MiddleCenter, AppProperties.fntText, fontColor: AppProperties.clrTxt);
            IconPictureBox icnHead = Default.GenIconBox(IconChar.Sliders, DockStyle.Fill, clr: AppProperties.clrTxt);

            tlpMain.Controls.Add(lblTit, 3, 0);
            tlpMain.SetColumnSpan(lblTit, 4);
            tlpMain.Controls.Add(lblDesc, 3, 1);
            tlpMain.SetColumnSpan(lblDesc, 4);
            tlpMain.Controls.Add(icnHead, 9, 0);
            tlpMain.SetRowSpan(icnHead, 2);

            //----------------------------Main----------------------------
            dataUsers = Default2.GenDataGrid(true);

            btnRefresh = Default.GenButton("🔄 Refrescar");
            btnRefresh.Click += btnRefresh_Click;

            tlpMain.Controls.Add(btnRefresh, 7, 2);
            tlpMain.SetColumnSpan(btnRefresh, 2);
            tlpMain.Controls.Add(dataUsers, 1, 3);
            tlpMain.SetColumnSpan(dataUsers, 8);
            tlpMain.SetRowSpan(dataUsers, 6);

            //----------------------------Footer----------------------------
            btnUpdate = Default.GenButton("📝 Actualizar");
            btnUpdate.Click += btnUpdate_Click;
            btnDelete = Default.GenButton("🗑️ Eliminar");
            btnDelete.Click += btnDelete_Click;
            
            tlpMain.Controls.Add(btnUpdate, 1, 10);
            tlpMain.SetColumnSpan(btnUpdate, 3);
            tlpMain.Controls.Add(btnDelete, 6, 10);
            tlpMain.SetColumnSpan(btnDelete, 3);

            //----------------------------FIN CONTENEDOR PRINCIPAL----------------------------
            this.Controls.Add(tlpMain);
        }
    }
}
