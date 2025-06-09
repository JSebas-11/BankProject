using DB_BankProject.ModelsDB;
using DB_BankProject.ModelsNative;
using FontAwesome.Sharp;
using Guna.UI2.WinForms;

namespace DB_BankProject.ModulesFrm;

//----------------------------FORMULARIO DEPOSITOS----------------------------
internal class FrmDeposit : Form {
    //----------------------------Atributos----------------------------
    private readonly UserAccount user;
    private readonly DataService data;
    private Guna2Button btnConfirm;
    private Guna2Button btnCancel;
    private Guna2TextBox txbAmount;
    private Guna2TextBox txbPassword;
    private Guna2ComboBox cmbBranch;
    public FrmDeposit(UserAccount user, DataService data){
        this.user = user;
        this.data = data;
        InitAttrs();
        InitCompts();
    }
    //----------------------------BOTONES----------------------------
    private void btnConfirm_Click(object sender, EventArgs e){
        if (CommonFunct.EmptyInputs(txbAmount.Text, txbPassword.Text)){
            MessageBox.Show(this, "No puedes dejar los campos vacios");
            return;
        }
        if (!CommonFunct.CorrectAmount(txbAmount.Text)){
            MessageBox.Show(this, $"-Depositos no pueden ser menores o iguales a 0\n-Limite de dinero por deposito: ${AppProperties.transLimit}");
            return;
        }
        if (!this.data.ExistsPerson(this.user.Number, txbPassword.Text)){
            MessageBox.Show(this, $"Contraseña incorrecta, intenta de nuevo");
            return;
        }
        Tran newTran = new Tran((int)TransType.Deposit, (int)cmbBranch.SelectedItem, this.user.UserId, this.user.UserId, decimal.Parse(txbAmount.Text));
        if (!this.data.AddTrans(newTran)){
            MessageBox.Show(this, "Transaccion fallida, intenta de nuevo");
            return;
        }
        MakeDeposit(); 
    }
    private void btnCancel_Click(object sender, EventArgs e) { this.Close(); }
    private void txb_KeyDown(object sender, KeyEventArgs e){
        if (sender == txbAmount && (e.KeyCode == Keys.Down || e.KeyCode== Keys.Enter)){
            txbPassword.Focus();
        } else if (sender == txbPassword && e.KeyCode == Keys.Up){
            txbAmount.Focus();
        }
    }
    private void txbAmount_KeyPress(object sender, KeyPressEventArgs e){
        if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Down && e.KeyChar != (char)Keys.Enter && e.KeyChar != (char)Keys.Back){
            e.Handled = true;
            MessageBox.Show(this, "Debes ingresar la cantidad a depositar");
        }
    }
    //----------------------------FUNCIONES----------------------------
    private void MakeDeposit() {
        this.user.Deposit(decimal.Parse(txbAmount.Text));
        this.data.ApplyChanges();
        MessageBox.Show(this, "Informacion aprobada, tu transaccion puede tardar unos segundos");
        CommonFunct.RandowWaiting();
        MessageBox.Show(this, "Deposito realizado con exito");
        this.Close();
    }
    //----------------------------INICIALIZACIONES----------------------------
    private void InitAttrs(){
        this.Text = "Deposit Submenu";
        this.ClientSize = AppProperties.szSubScreen;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.TopMost = true;
        this.ControlBox = false;
    }
    private void InitCompts(){
        //----------------------------CONTENEDOR PRINCIPAL----------------------------
        TableLayoutPanel tlpMain = Default.GenTabLayPnl(8, 6, AppProperties.clrTrans, false);
        Guna2GradientPanel pnlGrad = Default.GenGradPnl(AppProperties.clrMainBlue, AppProperties.clrBgBlack);

        pnlGrad.Controls.Add(tlpMain);

        //----------------------------Main----------------------------
        IconPictureBox icn = Default.GenIconBox(IconChar.Bank, DockStyle.Fill, clr: AppProperties.clrTxtGray);
        Label lblAmount = Default.GenLabel("Cuanto quieres depositar?", ContentAlignment.BottomLeft, AppProperties.fntSubTitle);
        Label lblBranch = Default.GenLabel("Desde que sucursal depositaras?", ContentAlignment.BottomLeft, AppProperties.fntSubTitle);
        Label lblPass = Default.GenLabel("Confirma tu contraseña:", ContentAlignment.BottomLeft, AppProperties.fntSubTitle);

        cmbBranch = Default2.GenComBox(AppProperties.branches);

        txbAmount = Default.GenTxtBox("$", 7);
        txbAmount.KeyDown += txb_KeyDown;
        txbAmount.KeyPress += txbAmount_KeyPress;
        txbPassword = Default.GenTxtBox("contraseña", AppProperties.txtInputSize, password: true);
        txbPassword.KeyDown += txb_KeyDown;

        btnCancel = Default.GenButton("Cancelar ✖", fillCol: Color.Red);
        btnCancel.Click += btnCancel_Click;
        btnConfirm = Default.GenButton("Confirmar ✔ ", fillCol: Color.Green);
        btnConfirm.Click += btnConfirm_Click;

        tlpMain.Controls.Add(icn, 0, 0);
        tlpMain.Controls.Add(lblAmount, 1, 0);
        tlpMain.SetColumnSpan(lblAmount, 4);
        tlpMain.Controls.Add(txbAmount, 1, 1);
        tlpMain.SetColumnSpan(txbAmount, 4);
        tlpMain.Controls.Add(lblBranch, 1, 2);
        tlpMain.SetColumnSpan(lblBranch, 4);
        tlpMain.Controls.Add(cmbBranch, 1, 3);
        tlpMain.SetColumnSpan(cmbBranch, 4);
        tlpMain.Controls.Add(lblPass, 1, 4);
        tlpMain.SetColumnSpan(lblPass, 4);
        tlpMain.Controls.Add(txbPassword, 1, 5);
        tlpMain.SetColumnSpan(txbPassword, 4);
        tlpMain.Controls.Add(btnCancel, 1, 7);
        tlpMain.SetColumnSpan(btnCancel, 2);
        tlpMain.Controls.Add(btnConfirm, 3, 7);
        tlpMain.SetColumnSpan(btnConfirm, 2);

        //----------------------------FIN CONTENEDOR PRINCIPAL----------------------------
        this.Controls.Add(pnlGrad);
    }
};

//----------------------------FORMULARIO RETIROS----------------------------
internal class FrmWithDraw : Form {
    //----------------------------Atributos----------------------------
    private readonly UserAccount user;
    private readonly DataService data;
    private Guna2Button btnConfirm;
    private Guna2Button btnCancel;
    private Guna2TextBox txbAmount;
    private Guna2TextBox txbPassword;
    private Guna2ComboBox cmbBranch;
    public FrmWithDraw(UserAccount user, DataService data){
        this.user = user;
        this.data = data;
        InitAttrs();
        InitCompts();
    }
    //----------------------------BOTONES----------------------------
    private void btnConfirm_Click(object sender, EventArgs e){
        if (CommonFunct.EmptyInputs(txbAmount.Text, txbPassword.Text)){
            MessageBox.Show(this, "No puedes dejar los campos vacios");
            return;
        }
        if (!CommonFunct.CorrectAmount(txbAmount.Text) || !this.user.Enough(decimal.Parse(txbAmount.Text))){
            MessageBox.Show(this, $"-Retiros no pueden ser mayores a tu saldo en cuenta\n-Limite de dinero por retiro: ${AppProperties.transLimit}");
            return;
        }
        if (!this.data.ExistsPerson(this.user.Number, txbPassword.Text)){
            MessageBox.Show(this, $"Contraseña incorrecta, intenta de nuevo");
            return;
        }
        Tran newTran = new Tran((int)TransType.Withdrawal, (int)cmbBranch.SelectedItem, this.user.UserId, this.user.UserId, decimal.Parse(txbAmount.Text));
        if (!this.data.AddTrans(newTran)){
            MessageBox.Show(this, "Transaccion fallida, intenta de nuevo");
            return;
        }
        MakeWithdraw();
    }
    private void btnCancel_Click(object sender, EventArgs e) { this.Close(); }
    private void txb_KeyDown(object sender, KeyEventArgs e){
        if (sender == txbAmount && (e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter)){
            txbPassword.Focus();
        }
        else if (sender == txbPassword && e.KeyCode == Keys.Up){
            txbAmount.Focus();
        }
    }
    private void txbAmount_KeyPress(object sender, KeyPressEventArgs e){
        if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Down && e.KeyChar != (char)Keys.Enter && e.KeyChar != (char)Keys.Back){
            e.Handled = true;
            MessageBox.Show(this, "Debes ingresar la cantidad a depositar");
        }
    }
    //----------------------------FUNCIONES----------------------------
    private void MakeWithdraw(){
        this.user.WithDraw(decimal.Parse(txbAmount.Text));
        this.data.ApplyChanges();
        MessageBox.Show(this, "Informacion aprobada, tu transaccion puede tardar unos segundos");
        CommonFunct.RandowWaiting();
        MessageBox.Show(this, "Retiro realizado con exito");
        this.Close();
    }
    //----------------------------INICIALIZACIONES----------------------------
    private void InitAttrs(){
        this.Text = "WithDraw Submenu";
        this.ClientSize = AppProperties.szSubScreen;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.TopMost = true;
        this.ControlBox = false;
    }
    private void InitCompts(){
        //----------------------------CONTENEDOR PRINCIPAL----------------------------
        TableLayoutPanel tlpMain = Default.GenTabLayPnl(8, 6, AppProperties.clrTrans, false);
        Guna2GradientPanel pnlGrad = Default.GenGradPnl(AppProperties.clrMainBlue, AppProperties.clrBgBlack);

        pnlGrad.Controls.Add(tlpMain);

        //----------------------------Main----------------------------
        IconPictureBox icn = Default.GenIconBox(IconChar.Bank, DockStyle.Fill, clr: AppProperties.clrTxtGray);
        Label lblAmount = Default.GenLabel("Cuanto quieres retirar?", ContentAlignment.BottomLeft, AppProperties.fntSubTitle);
        Label lblBranch = Default.GenLabel("En cual sucursal retiraras?", ContentAlignment.BottomLeft, AppProperties.fntSubTitle);
        Label lblPass = Default.GenLabel("Confirma tu contraseña:", ContentAlignment.BottomLeft, AppProperties.fntSubTitle);

        cmbBranch = Default2.GenComBox(AppProperties.branches);

        txbAmount = Default.GenTxtBox($"Saldo: ${this.user.Funds}", 7);
        txbAmount.KeyDown += txb_KeyDown;
        txbAmount.KeyPress += txbAmount_KeyPress;
        txbPassword = Default.GenTxtBox("contraseña", AppProperties.txtInputSize, password: true);
        txbPassword.KeyDown += txb_KeyDown;

        btnCancel = Default.GenButton("Cancelar ✖", fillCol: Color.Red);
        btnCancel.Click += btnCancel_Click;
        btnConfirm = Default.GenButton("Confirmar ✔ ", fillCol: Color.Green);
        btnConfirm.Click += btnConfirm_Click;

        tlpMain.Controls.Add(icn, 0, 0);
        tlpMain.Controls.Add(lblAmount, 1, 0);
        tlpMain.SetColumnSpan(lblAmount, 4);
        tlpMain.Controls.Add(txbAmount, 1, 1);
        tlpMain.SetColumnSpan(txbAmount, 4);
        tlpMain.Controls.Add(lblBranch, 1, 2);
        tlpMain.SetColumnSpan(lblBranch, 4);
        tlpMain.Controls.Add(cmbBranch, 1, 3);
        tlpMain.SetColumnSpan(cmbBranch, 4);
        tlpMain.Controls.Add(lblPass, 1, 4);
        tlpMain.SetColumnSpan(lblPass, 4);
        tlpMain.Controls.Add(txbPassword, 1, 5);
        tlpMain.SetColumnSpan(txbPassword, 4);
        tlpMain.Controls.Add(btnCancel, 1, 7);
        tlpMain.SetColumnSpan(btnCancel, 2);
        tlpMain.Controls.Add(btnConfirm, 3, 7);
        tlpMain.SetColumnSpan(btnConfirm, 2);

        //----------------------------FIN CONTENEDOR PRINCIPAL----------------------------
        this.Controls.Add(pnlGrad);
    }
};

//----------------------------FORMULARIO TRANSFERENCIAS----------------------------
internal class FrmTransfer : Form {
    //----------------------------Atributos----------------------------
    private readonly UserAccount user;
    private readonly DataService data;
    private Guna2Button btnConfirm;
    private Guna2Button btnCancel;
    private Guna2TextBox txbTargetNum;
    private Guna2TextBox txbAmount;
    private Guna2TextBox txbPassword;
    private Guna2ComboBox cmbBranch;
    public FrmTransfer(UserAccount user, DataService data){
        this.user = user;
        this.data = data;
        InitAttrs();
        InitCompts();
    }
    //----------------------------BOTONES----------------------------
    private void btnConfirm_Click(object sender, EventArgs e){
        if (CommonFunct.EmptyInputs(txbAmount.Text, txbTargetNum.Text, txbPassword.Text)){
            MessageBox.Show(this, "No puedes dejar los campos vacios");
            return;
        }
        if (!CommonFunct.CorrectAmount(txbAmount.Text) || !this.user.Enough(decimal.Parse(txbAmount.Text))){
            MessageBox.Show(this, $"-Transferencias no pueden ser mayores a tu saldo en cuenta\n-Limite de dinero por transferencia: ${AppProperties.transLimit}");
            return;
        }
        if (!this.data.ExistsPerson(txbTargetNum.Text)){
            MessageBox.Show(this, $"Usuario destino no encontrado ({txbTargetNum.Text})");
            return;
        }
        if (!this.data.ExistsPerson(this.user.Number, txbPassword.Text)){
            MessageBox.Show(this, "Contraseña incorrecta, intenta de nuevo");
            return;
        }
        Tran newTran = new Tran(
            (int)TransType.Transfer, (int)cmbBranch.SelectedItem, this.user.UserId, 
            this.data.GetUserID(txbTargetNum.Text), decimal.Parse(txbAmount.Text)
        );
        if (!this.data.AddTrans(newTran)){
            MessageBox.Show(this, "Transaccion fallida, intenta de nuevo");
            return;
        }
        MakeTransfer();
    }
    private void btnCancel_Click(object sender, EventArgs e) { this.Close(); }
    private void txb_KeyPress(object sender, KeyPressEventArgs e){
        if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Down && e.KeyChar != (char)Keys.Enter && e.KeyChar != (char)Keys.Back){
            e.Handled = true;
            if (sender == txbAmount){ MessageBox.Show(this, "Debes ingresar la cantidad a depositar"); }
            else if (sender == txbTargetNum){ MessageBox.Show(this, "Debes ingresar el numero de la persona destino"); }
        }
    }
    //----------------------------FUNCIONES----------------------------
    private void MakeTransfer(){
        this.user.Transfer(decimal.Parse(txbAmount.Text), this.data.GetUser(txbTargetNum.Text));
        this.data.ApplyChanges();
        MessageBox.Show(this, "Informacion aprobada, tu transaccion puede tardar unos segundos");
        CommonFunct.RandowWaiting();
        MessageBox.Show(this, "Transferencia realizado con exito");
        this.Close();
    }
    //----------------------------INICIALIZACIONES----------------------------
    private void InitAttrs(){
        this.Text = "Transfer Submenu";
        this.ClientSize = AppProperties.szSubScreen;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.TopMost = true;
        this.ControlBox = false;
    }
    private void InitCompts(){
        //----------------------------CONTENEDOR PRINCIPAL----------------------------
        TableLayoutPanel tlpMain = Default.GenTabLayPnl(10, 6, AppProperties.clrTrans, false);
        Guna2GradientPanel pnlGrad = Default.GenGradPnl(AppProperties.clrMainBlue, AppProperties.clrBgBlack);

        pnlGrad.Controls.Add(tlpMain);

        //----------------------------Main----------------------------
        IconPictureBox icn = Default.GenIconBox(IconChar.Bank, DockStyle.Fill, clr: AppProperties.clrTxtGray);
        Label lblAmount = Default.GenLabel("Cantidad a transferir", ContentAlignment.BottomLeft, AppProperties.fntText);
        Label lblReceiver = Default.GenLabel("Numero destinatario", ContentAlignment.BottomLeft, AppProperties.fntText);
        Label lblBranch = Default.GenLabel("A que sucursal envias?", ContentAlignment.BottomLeft, AppProperties.fntSubTitle);
        Label lblPass = Default.GenLabel("Confirma tu contraseña:", ContentAlignment.BottomLeft, AppProperties.fntSubTitle);

        cmbBranch = Default2.GenComBox(AppProperties.branches);

        txbAmount = Default.GenTxtBox($"${this.user.Funds}", 7);
        txbAmount.KeyPress += txb_KeyPress;
        txbTargetNum = Default.GenTxtBox("Numero", 10);
        txbTargetNum.KeyPress += txb_KeyPress;
        txbPassword = Default.GenTxtBox("contraseña", AppProperties.txtInputSize, password: true);

        btnCancel = Default.GenButton("Cancelar ✖", fillCol: Color.Red);
        btnCancel.Click += btnCancel_Click;
        btnConfirm = Default.GenButton("Confirmar ✔ ", fillCol: Color.Green);
        btnConfirm.Click += btnConfirm_Click;

        tlpMain.Controls.Add(icn, 0, 0);
        tlpMain.Controls.Add(lblAmount, 1, 1);
        tlpMain.SetColumnSpan(lblAmount, 2);
        tlpMain.Controls.Add(lblReceiver, 3, 1);
        tlpMain.SetColumnSpan(lblReceiver, 2);
        tlpMain.Controls.Add(txbAmount, 1, 2);
        tlpMain.SetColumnSpan(txbAmount, 2);
        tlpMain.Controls.Add(txbTargetNum, 3, 2);
        tlpMain.SetColumnSpan(txbTargetNum, 2);
        tlpMain.Controls.Add(lblBranch, 1, 3);
        tlpMain.SetColumnSpan(lblBranch, 4);
        tlpMain.Controls.Add(cmbBranch, 1, 4);
        tlpMain.SetColumnSpan(cmbBranch, 4);
        tlpMain.Controls.Add(lblPass, 1, 5);
        tlpMain.SetColumnSpan(lblPass, 4);
        tlpMain.Controls.Add(txbPassword, 1, 6);
        tlpMain.SetColumnSpan(txbPassword, 4);
        tlpMain.Controls.Add(btnCancel, 1, 8);
        tlpMain.SetColumnSpan(btnCancel, 2);
        tlpMain.Controls.Add(btnConfirm, 3, 8);
        tlpMain.SetColumnSpan(btnConfirm, 2);

        //----------------------------FIN CONTENEDOR PRINCIPAL----------------------------
        this.Controls.Add(pnlGrad);
    }
};