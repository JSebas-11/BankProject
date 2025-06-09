using DB_BankProject.ModelsNative;
using DB_BankProject.Properties;
using FontAwesome.Sharp;
using Guna.UI2.WinForms;
using System.Drawing.Drawing2D;

namespace DB_BankProject.ModulesFrm {
    internal class FrmLogin : Form {
        //----------------------------Atributos----------------------------
        private readonly DataService data;
        private IPerson? current;
        private Guna2Button btnLog;
        private Guna2Button btnSign;
        private Guna2Button btnExit;
        private Guna2Button btnShowHide;
        private Guna2TextBox txbNumber;
        private Guna2TextBox txbPassword;
        private Guna2MessageDialog msg;

        public FrmLogin(DataService data){
            this.data = data;
            this.InitAttrs();
            this.InitCompts();
        }
        //----------------------------BOTONES----------------------------
        private void btnLog_Click(object sender, EventArgs e) {
            if (CommonFunct.EmptyInputs(txbNumber.Text, txbPassword.Text) 
                || !CommonFunct.CorrectInputs(txbNumber.Text, txbPassword.Text)) { 
                msg.Show("Verifica los inputs:\n-Campos numero y contraseña no pueden estar vacios" +
                    "\n-Campo numero debe tener 10 digitos\n-Campo contraseña debe tener minimo 4 caracteres");
                return;
            }

            current = data.GetPerson(txbNumber.Text, txbPassword.Text);
            if (current == null){ msg.Show("El usuario no existe, intenta de nuevo"); }
            else {
                msg.Show($"Bienvenido {current.OwnerName} ({current.PersonType})");
                CleanInputs();
                this.Hide();
                if (current.PersonType == PersonType.Admin){
                    new FrmAdminMain(this, this.current, this.data).Show();
                } else if (current.PersonType == PersonType.User){
                    new FrmUserMain(this, this.current, this.data).Show();
                }
            }
        }
        private void btnSign_Click(object sender, EventArgs e) {
            this.Hide();
            CleanInputs();
            new FrmSign(this, this.data).Show();
        }
        private void btnExit_Click(object sender, EventArgs e) {
            Application.Exit();
            this.data.Dispose();
        }
        private void btnShowHide_Click(object sender, EventArgs e) {
            txbPassword.UseSystemPasswordChar = !txbPassword.UseSystemPasswordChar;
            btnShowHide.Text = txbPassword.UseSystemPasswordChar ? "👁️" : "🔒";
        }
        private void txb_KeyDown(object sender, KeyEventArgs e) {
            if (sender == txbNumber && (e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter)){
                txbPassword.Focus();
            } else if (sender == txbPassword && e.KeyCode == Keys.Up){
                txbNumber.Focus();
            }
        }
        private void txbNumber_KeyPress(object sender, KeyPressEventArgs e){
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Enter && e.KeyChar != (char)Keys.Down){
                e.Handled = true; //Bloquear letras que no sean numeros
                msg.Show("Debes ingresar tu numero de cuenta");
            }
        }
        //------------------------------FUNCIONES-------------------------------
        private void CleanInputs() {
            txbNumber.Text = "";
            txbPassword.Text = "";
            txbPassword.UseSystemPasswordChar = true;
            btnShowHide.Text = "👁️";
        }
        //----------------------------INICIALIZACIONES----------------------------
        private void InitAttrs(){
            this.Text = "Login";
            this.ClientSize = AppProperties.szScreen;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.msg = new Guna2MessageDialog(){
                Style = MessageDialogStyle.Dark,
                Caption = "Informacion",
                Buttons = MessageDialogButtons.OK,
                Icon = MessageDialogIcon.Information,
                Parent = this
            };
            //Liberar contexto en caso de cerrar
            this.FormClosed += (s, e) => {
                this.data.Dispose();
                Application.Exit();
            };
        }
        private void InitCompts(){
            //----------------------------CONTENEDOR PRINCIPAL----------------------------
            TableLayoutPanel tlpMainContainer = Default.GenTabLayPnl(10, 12, AppProperties.clrTrans, false);
            TableLayoutPanel tlpBorder = Default.GenTabLayPnl(1, 2, AppProperties.clrMainBlue, true);
            tlpBorder.BackgroundImage = Resources.backGround;
            tlpBorder.BackgroundImageLayout = ImageLayout.Stretch;
            Guna2GradientPanel pnlDarkBg = Default.GenGradPnl(AppProperties.clrMainBlue, AppProperties.clrBgBlack);

            //----------------------------main1 (inputs)----------------------------
            TableLayoutPanel tlpLog = Default.GenTabLayPnl(7, 7, AppProperties.clrTrans, false);

            Label lblLog = Default.GenLabel("👋 Bienvenid@", ContentAlignment.BottomCenter, AppProperties.fntTitle);

            //-------------------------tlp input number-------------------------
            TableLayoutPanel tlpNumber = Default.GenTabLayPnl(1, 2, AppProperties.clrBgBlack, true, false);
            tlpNumber.Margin = new Padding(1);
            tlpNumber.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
            tlpNumber.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60f));
            tlpNumber.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            TableLayoutPanel tlpFlag = Default.GenTabLayPnl(1, 2, AppProperties.clrTrans, false);
            Guna2PictureBox pbx = Default.GenImgBox(Resources.colombiaFlag);
            Label lblflag = Default.GenLabel("+57", ContentAlignment.MiddleCenter, AppProperties.fntSubTitle);

            tlpFlag.Controls.Add(pbx, 0, 0);
            tlpFlag.Controls.Add(lblflag, 1, 0);

            txbNumber = Default.GenTxtBox("Numero celular", 10);
            txbNumber.KeyPress += txbNumber_KeyPress;
            txbNumber.KeyDown += txb_KeyDown;

            tlpNumber.Controls.Add(tlpFlag, 0, 0);
            tlpNumber.Controls.Add(txbNumber, 1, 0);

            //-------------------------tlp input password-------------------------
            TableLayoutPanel tlpPass = Default.GenTabLayPnl(1, 2, AppProperties.clrBgBlack, true, false);
            tlpPass.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75.0f));
            tlpPass.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25.0f));
            tlpPass.Margin = new Padding(1);

            txbPassword = Default.GenTxtBox("Contraseña", AppProperties.txtInputSize, password: true);
            txbPassword.KeyDown += txb_KeyDown;

            btnShowHide = Default.GenButton("👁️");
            btnShowHide.Click += btnShowHide_Click;

            tlpPass.Controls.Add(txbPassword, 0, 0);
            tlpPass.Controls.Add(btnShowHide, 1, 0);
            
            btnLog = Default.GenButton("➡️ Iniciar Sesion");
            btnLog.Click += btnLog_Click;

            IconPictureBox icn = Default.GenIconBox(IconChar.BuildingColumns, DockStyle.Fill);
            //--------------------------------------------------
            tlpLog.Controls.Add(lblLog, 1, 0);
            tlpLog.SetColumnSpan(lblLog, 5);
            tlpLog.Controls.Add(tlpNumber, 1, 2);
            tlpLog.SetColumnSpan(tlpNumber, 5);
            tlpLog.Controls.Add(tlpPass, 1, 3);
            tlpLog.SetColumnSpan(tlpPass, 5);
            tlpLog.Controls.Add(btnLog, 2, 5);
            tlpLog.SetColumnSpan(btnLog, 3);
            tlpLog.Controls.Add(icn, 0, 6);
            
            //----------------------------main2 (buttons)----------------------------
            TableLayoutPanel tlpInfo = Default.GenTabLayPnl(8, 7, AppProperties.clrTrans, false);

            Label lblInfotit = Default.GenLabel("💰 Nequi a la mano", ContentAlignment.MiddleCenter,
                new Font(AppProperties.fntTitle.FontFamily, AppProperties.fntTitle.Size, FontStyle.Underline), fontColor: AppProperties.clrTxtGray);

            Guna2PictureBox img = Default.GenImgBox(Resources.security);
            
            Label lblInfo = Default.GenLabel("Inicia sesión para disfrutar de todos nuestros servicios.\n Tu informacion esta " +
                "protegida con los mas altos estandares de seguridad.", ContentAlignment.MiddleCenter, AppProperties.fntText);

            btnSign = Default.GenButton("📝 Registrar");
            btnSign.Click += btnSign_Click;
            btnExit = Default.GenButton("🚪 Salir");
            btnExit.Click += btnExit_Click;

            tlpInfo.Controls.Add(lblInfotit, 1, 0);
            tlpInfo.SetColumnSpan(lblInfotit, 5);
            tlpInfo.Controls.Add(img, 2, 1);
            tlpInfo.SetColumnSpan(img, 3);
            tlpInfo.SetRowSpan(img, 3);
            tlpInfo.Controls.Add(lblInfo, 2, 4);
            tlpInfo.SetColumnSpan(lblInfo, 3);
            tlpInfo.SetRowSpan(lblInfo, 2);
            tlpInfo.Controls.Add(btnSign, 1, 6);
            tlpInfo.SetColumnSpan(btnSign, 2);
            tlpInfo.Controls.Add(btnExit, 4, 6);
            tlpInfo.SetColumnSpan(btnExit, 2);

            //----------------------------FIN CONTENEDOR PRINCIPAL----------------------------
            tlpBorder.Controls.Add(tlpLog, 0, 0);
            tlpBorder.Controls.Add(tlpInfo, 1, 0);

            tlpMainContainer.Controls.Add(tlpBorder, 1, 1);
            tlpMainContainer.SetColumnSpan(tlpBorder, 10);
            tlpMainContainer.SetRowSpan(tlpBorder, 8);

            pnlDarkBg.Controls.Add(tlpMainContainer);
            this.Controls.Add(pnlDarkBg);
        }
    }
}
