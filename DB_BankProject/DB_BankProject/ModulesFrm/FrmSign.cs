using DB_BankProject.ModelsNative;
using DB_BankProject.ModelsDB;
using DB_BankProject.Properties;
using FontAwesome.Sharp;
using Guna.UI2.WinForms;

namespace DB_BankProject.ModulesFrm {
    internal class FrmSign : Form {
        //------------------------------ATRIBUTOS-------------------------------
        private readonly DataService data;
        private readonly FrmLogin logRef;
        private Guna2Button btnConfirm;
        private Guna2Button btnBack;
        private Guna2Button btnShowHide;
        private Guna2TextBox txbName;
        private Guna2TextBox txbNumber;
        private Guna2TextBox txbPassword;
        private Label lblName;
        private Label lblNumber;
        private Guna2MessageDialog msg;
        public FrmSign(FrmLogin logReference, DataService data){
            this.data = data;
            this.logRef = logReference;
            InitAttrs();
            InitCompts();
        }
        //------------------------------BOTONES-------------------------------
        private void btnConfirm_Click(object sender, EventArgs e){
            if (CommonFunct.EmptyInputs(txbNumber.Text, txbName.Text, txbPassword.Text)
                || !CommonFunct.CorrectInputs(txbNumber.Text, txbPassword.Text)) {
                msg.Show("Verifica los inputs:\n-Campos nombre, numero, contraseña son obligatorios" +
                    "\n-Campo numero debe tener 10 digitos\n-Campo contraseña debe tener minimo 4 caracteres");
                return;
            }
            if (data.ExistsPerson(txbNumber.Text)){
                msg.Show("Ya existe un usuario registrado con ese numero");
            } else {
                string message = data.AddAccount(new UserAccount(txbName.Text, txbNumber.Text, txbPassword.Text))
                                ? "Usuario registrado" : "Hubo un error creando el usuario, intenta de nuevo";
                msg.Show(message);
            }
        }
        private void btnBack_Click(object sender, EventArgs e){
            this.Close();
            this.logRef.Show();
        }
        private void btnShowHide_Click(object sender, EventArgs e){
            txbPassword.UseSystemPasswordChar = !txbPassword.UseSystemPasswordChar;
            btnShowHide.Text = txbPassword.UseSystemPasswordChar ? "👁️" : "🔒";
        }
        private void txb_KeyDown(object sender, KeyEventArgs e){
            if (sender == txbName && (e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter)){
                txbNumber.Focus();
            } else if (sender == txbNumber) {
                if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter){
                    txbPassword.Focus();
                } else if (e.KeyCode == Keys.Up){
                    txbName.Focus();
                }
            } else if (sender == txbPassword && e.KeyCode == Keys.Up){
                txbNumber.Focus();
            }
        }
        private void txbNumber_KeyPress(object sender, KeyPressEventArgs e){
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Enter && e.KeyChar != (char)Keys.Down){
                e.Handled = true;
                msg.Show("Debes ingresar tu numero de cuenta");
            }
        }
        private void txb_Leave(object sender, EventArgs e){
            lblName.Text = txbName.Text.ToUpper();
            lblNumber.Text = txbNumber.Text.ToUpper();
        }
        //------------------------------INICIALIZACIONES-------------------------------
        private void InitAttrs(){
            this.Text = "Sign";
            this.ClientSize = AppProperties.szScreen;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.ControlBox = false;
            this.msg = new Guna2MessageDialog(){
                Style = MessageDialogStyle.Dark,
                Caption = "Informacion",
                Buttons = MessageDialogButtons.OK,
                Icon = MessageDialogIcon.Information,
                Parent = this
            };
        }
        private void InitCompts(){
            //----------------------------CONTENEDOR PRINCIPAL----------------------------
            TableLayoutPanel tlpMainContainer = Default.GenTabLayPnl(10, 12, AppProperties.clrTrans, false);
            TableLayoutPanel tlpBorder = Default.GenTabLayPnl(1, 2, AppProperties.clrMainBlue, true);
            tlpBorder.BackgroundImage = Resources.backGround;
            tlpBorder.BackgroundImageLayout = ImageLayout.Stretch;
            Guna2GradientPanel pnlDarkBg = Default.GenGradPnl(AppProperties.clrMainBlue, AppProperties.clrBgBlack);

            //----------------------------Main 1 (inputs)----------------------------
            TableLayoutPanel tlpInputs = Default.GenTabLayPnl(7, 7, AppProperties.clrTrans, false);

            Label lblTit = Default.GenLabel("Registrate en minutos y empieza a disfrutar de nuestros servicios " +
                "seguros y confiables, tus finanzas agradeceran", ContentAlignment.MiddleCenter, AppProperties.fntSubTitle);

            txbName = Default.GenTxtBox("Nombre titular", AppProperties.txtInputSize);
            txbName.KeyDown += txb_KeyDown;
            txbName.Leave += txb_Leave;
            TableLayoutPanel tlpName = Default2.GenInputCard(IconChar.AddressBook, txbName);
            
            txbNumber = Default.GenTxtBox("Numero cuenta", 10);
            txbNumber.KeyDown += txb_KeyDown;
            txbNumber.KeyPress += txbNumber_KeyPress;
            txbNumber.Leave += txb_Leave;
            TableLayoutPanel tlpNumber = Default2.GenInputCard(IconChar.Hashtag, txbNumber);
            
            txbPassword = Default.GenTxtBox("Contraseña", AppProperties.txtInputSize, password: true);
            txbPassword.KeyDown += txb_KeyDown;
            TableLayoutPanel tlpPass = Default2.GenInputCard(IconChar.Key, txbPassword);

            btnShowHide = Default.GenButton("👁️");
            btnShowHide.Click += btnShowHide_Click;

            IconPictureBox icnFooter = Default.GenIconBox(IconChar.Signature, DockStyle.Fill);

            tlpInputs.Controls.Add(lblTit, 1, 0);
            tlpInputs.SetColumnSpan(lblTit, 5);
            tlpInputs.SetRowSpan(lblTit, 2);
            tlpInputs.Controls.Add(tlpName, 1, 2);
            tlpInputs.SetColumnSpan(tlpName, 5);
            tlpInputs.Controls.Add(tlpNumber, 1, 3);
            tlpInputs.SetColumnSpan(tlpNumber, 5);
            tlpInputs.Controls.Add(tlpPass, 1, 4);
            tlpInputs.SetColumnSpan(tlpPass, 5);
            tlpInputs.Controls.Add(btnShowHide, 3, 5);
            tlpInputs.Controls.Add(icnFooter, 0, 6);

            //----------------------------Main 2 (btns)----------------------------
            TableLayoutPanel tlpInfo = Default.GenTabLayPnl(7, 7, AppProperties.clrTrans, false);

            Label lblData = Default.GenLabel("Datos finales", ContentAlignment.MiddleCenter, AppProperties.fntTitle);
            Label lbl1 = Default.GenLabel("Tu cuenta quedara a nombre de:", ContentAlignment.BottomLeft, AppProperties.fntSubTitle);
            lblName = Default.GenLabel("", ContentAlignment.MiddleCenter, AppProperties.fntTitle);
            Label lbl2 = Default.GenLabel("Numero vinculado:", ContentAlignment.BottomLeft, AppProperties.fntSubTitle);
            lblNumber = Default.GenLabel("", ContentAlignment.MiddleCenter, AppProperties.fntTitle);

            btnConfirm = Default.GenButton("🆗 Validar");
            btnConfirm.Click += btnConfirm_Click;
            btnBack = Default.GenButton("↩️ Volver");
            btnBack.Click += btnBack_Click;

            tlpInfo.Controls.Add(lblData, 2, 0);
            tlpInfo.SetColumnSpan(lblData, 3);
            tlpInfo.Controls.Add(lbl1, 1, 1);
            tlpInfo.SetColumnSpan(lbl1, 5);
            tlpInfo.Controls.Add(lblName, 1, 2);
            tlpInfo.SetColumnSpan(lblName, 5);
            tlpInfo.Controls.Add(lbl2, 1, 3);
            tlpInfo.SetColumnSpan(lbl2, 5);
            tlpInfo.Controls.Add(lblNumber, 1, 4);
            tlpInfo.SetColumnSpan(lblNumber, 5);
            tlpInfo.Controls.Add(btnConfirm, 1, 5);
            tlpInfo.SetColumnSpan(btnConfirm, 2);
            tlpInfo.Controls.Add(btnBack, 4, 5);
            tlpInfo.SetColumnSpan(btnBack, 2);
            
            //----------------------------FIN CONTENEDOR PRINCIPAL----------------------------
            tlpBorder.Controls.Add(tlpInputs, 0, 0);
            tlpBorder.Controls.Add(tlpInfo, 1, 0);

            tlpMainContainer.Controls.Add(tlpBorder, 1, 1);
            tlpMainContainer.SetColumnSpan(tlpBorder, 10);
            tlpMainContainer.SetRowSpan(tlpBorder, 8);

            pnlDarkBg.Controls.Add(tlpMainContainer);
            this.Controls.Add(pnlDarkBg);
        }
    }
}
