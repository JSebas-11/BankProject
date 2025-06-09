using DB_BankProject.ModelsNative;
using FontAwesome.Sharp;
using Guna.UI2.WinForms;
using System.Windows.Interop;
using System.Xml.Linq;

namespace DB_BankProject.ModulesFrm;

//----------------------------FORMULARIO CAMBIO DATOS----------------------------
internal class FrmEdit : Form {
    //----------------------------Atributos----------------------------
    private readonly IPerson current;
    private readonly DataService data;
    private Guna2Button btnConfirm;
    private Guna2Button btnCancel;
    private Guna2TextBox txbNum;
    private Guna2TextBox txbName;
    public FrmEdit(IPerson toEdit, DataService data) {
        this.current = toEdit;
        this.data = data;
        InitAttrs();
        InitCompts();
    }
    //----------------------------BOTONES----------------------------
    private void btnConfirm_Click(object sender, EventArgs e){
        if (CommonFunct.EmptyInputs(txbNum.Text, txbName.Text) || !CommonFunct.CorrectNumber(txbNum.Text)) {
            MessageBox.Show(this, "Error:\n-Campo numero debe tener 10 digitos\n-Campos NO puede estar vacio");
            return;
        }
        if (txbNum.Text != current.Number && this.data.ExistsPerson(txbNum.Text)) {
            MessageBox.Show(this, "Ya existe un usuario registrado con ese numero");
            return;
        }
        //Actualizar campos y hacer el commit en la DB
        this.current.Number = txbNum.Text;
        this.current.OwnerName = txbName.Text.ToUpper();
        this.data.ApplyChanges();
        MessageBox.Show(this, "Actualizado con exito");
        this.Close();
    }
    private void btnCancel_Click(object sender, EventArgs e) { this.Close(); }
    private void txbNumber_KeyPress(object sender, KeyPressEventArgs e){
        if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Enter && e.KeyChar != (char)Keys.Down){
            e.Handled = true;
            MessageBox.Show(this, "Debes ingresar tu numero de cuenta");
        }
    }
    //----------------------------INICIALIZACIONES----------------------------
    private void InitAttrs() {
        this.Text = "Edition Menu";
        this.ClientSize = AppProperties.szSubScreen;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.TopMost = true;
        this.ControlBox = false;
    }
    private void InitCompts() {
        //----------------------------CONTENEDOR PRINCIPAL----------------------------
        TableLayoutPanel tlpMain = Default.GenTabLayPnl(8, 6, AppProperties.clrTrans, false);
        Guna2GradientPanel pnlGrad = Default.GenGradPnl(AppProperties.clrMainBlue, AppProperties.clrBgBlack);

        pnlGrad.Controls.Add(tlpMain);

        //----------------------------Main----------------------------
        IconPictureBox icn = Default.GenIconBox(IconChar.UserPen, DockStyle.Fill, clr: AppProperties.clrTxtGray);
        Label lblNum = Default.GenLabel("Modifica el numero:", ContentAlignment.BottomLeft, AppProperties.fntSubTitle);
        Label lblName = Default.GenLabel("Modifica el titular:", ContentAlignment.BottomLeft, AppProperties.fntSubTitle);

        txbNum = Default.GenTxtBox("", 10);
        txbNum.Text = current.Number;
        txbNum.KeyPress += txbNumber_KeyPress;
        txbName = Default.GenTxtBox("", AppProperties.txtInputSize);
        txbName.Text = current.OwnerName;

        btnCancel = Default.GenButton("Cancelar ✖", fillCol: Color.Red);
        btnCancel.Click += btnCancel_Click;
        btnConfirm = Default.GenButton("Confirmar ✔ ", fillCol: Color.Green);
        btnConfirm.Click += btnConfirm_Click;

        tlpMain.Controls.Add(icn, 0, 0);
        tlpMain.Controls.Add(lblNum, 1, 1);
        tlpMain.SetColumnSpan(lblNum, 4);
        tlpMain.Controls.Add(txbNum, 1, 2);
        tlpMain.SetColumnSpan(txbNum, 4);
        tlpMain.Controls.Add(lblName, 1, 3);
        tlpMain.SetColumnSpan(lblName, 4);
        tlpMain.Controls.Add(txbName, 1, 4);
        tlpMain.SetColumnSpan(txbName, 4);
        tlpMain.Controls.Add(btnCancel, 1, 6);
        tlpMain.SetColumnSpan(btnCancel, 2);
        tlpMain.Controls.Add(btnConfirm, 3, 6);
        tlpMain.SetColumnSpan(btnConfirm, 2);

        //----------------------------FIN CONTENEDOR PRINCIPAL----------------------------
        this.Controls.Add(pnlGrad);
    }
};
//----------------------------FORMULARIO CAMBIO CONTRASEÑA----------------------------
internal class FrmChangePass : Form {
    //----------------------------Atributos----------------------------
    private readonly IPerson current;
    private readonly DataService data;
    private Guna2Button btnConfirm;
    private Guna2Button btnCancel;
    private Guna2Button btnShowHide;
    private Guna2TextBox txbOld;
    private Guna2TextBox txbNew;
    public FrmChangePass(IPerson toEdit, DataService data){
        this.current = toEdit;
        this.data = data;
        InitAttrs();
        InitCompts();
    }
    //----------------------------BOTONES----------------------------
    private void btnConfirm_Click(object sender, EventArgs e){
        if (!CommonFunct.CorrectPassword(txbOld.Text) || !CommonFunct.CorrectPassword(txbNew.Text)){
            MessageBox.Show(this, "Error:\nCampo de contraseñas deben tener minimo 4 caracteres y no vacio");
            return;
        }
        //Verificar si la contraseña actual coincide
        if (!this.data.ExistsPerson(current.Number, txbOld.Text)){
            MessageBox.Show(this, "Contraseña actual incorrecta");
            return;
        }
        this.current.HashPassword = BCrypt.Net.BCrypt.HashPassword(txbNew.Text);
        this.data.ApplyChanges();
        MessageBox.Show(this, "Contraseña actualizada con exito");
        this.Close();
    }
    private void btnCancel_Click(object sender, EventArgs e) { this.Close(); }
    private void btnShowHide_Click(object sender, EventArgs e){
        txbOld.UseSystemPasswordChar = !txbOld.UseSystemPasswordChar;
        txbNew.UseSystemPasswordChar = !txbNew.UseSystemPasswordChar;
        btnShowHide.Text = txbOld.UseSystemPasswordChar ? "👁️" : "🔒";
    }
    //----------------------------INICIALIZACIONES----------------------------
    private void InitAttrs(){
        this.Text = "Edition Menu";
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
        IconPictureBox icn = Default.GenIconBox(IconChar.UserPen, DockStyle.Fill, clr: AppProperties.clrTxtGray);
        Label lblOld = Default.GenLabel("Ingresa la contraseña actual:", ContentAlignment.BottomLeft, AppProperties.fntSubTitle);
        Label lblNew = Default.GenLabel("Ingresa la nueva contraseña:", ContentAlignment.BottomLeft, AppProperties.fntSubTitle);

        txbOld = Default.GenTxtBox("Actual", AppProperties.txtInputSize, password: true);
        txbNew = Default.GenTxtBox("Nueva", AppProperties.txtInputSize, password: true);

        btnCancel = Default.GenButton("Cancelar ✖", fillCol: Color.Red);
        btnCancel.Click += btnCancel_Click;
        btnConfirm = Default.GenButton("Confirmar ✔ ", fillCol: Color.Green);
        btnConfirm.Click += btnConfirm_Click;
        btnShowHide = Default.GenButton("👁️");
        btnShowHide.Click += btnShowHide_Click;

        tlpMain.Controls.Add(icn, 0, 0);
        tlpMain.Controls.Add(lblOld, 1, 1);
        tlpMain.SetColumnSpan(lblOld, 4);
        tlpMain.Controls.Add(txbOld, 1, 2);
        tlpMain.SetColumnSpan(txbOld, 4);
        tlpMain.Controls.Add(lblNew, 1, 3);
        tlpMain.SetColumnSpan(lblNew, 4);
        tlpMain.Controls.Add(txbNew, 1, 4);
        tlpMain.SetColumnSpan(txbNew, 4);
        tlpMain.Controls.Add(btnShowHide, 2, 5);
        tlpMain.SetColumnSpan(btnShowHide, 2);
        tlpMain.Controls.Add(btnCancel, 1, 6);
        tlpMain.SetColumnSpan(btnCancel, 2);
        tlpMain.Controls.Add(btnConfirm, 3, 6);
        tlpMain.SetColumnSpan(btnConfirm, 2);

        //----------------------------FIN CONTENEDOR PRINCIPAL----------------------------
        this.Controls.Add(pnlGrad);
    }
};
//----------------------------FORMULARIO BORRAR CUENTA----------------------------
internal class FrmDelete : Form {
    //----------------------------Atributos----------------------------
    private readonly IPerson current;
    private readonly DataService data;
    private Guna2Button btnConfirm;
    private Guna2Button btnCancel;
    public FrmDelete(IPerson toDel, DataService data){
        this.current = toDel;
        this.data = data;
        InitAttrs();
        InitCompts();
    }
    //----------------------------BOTONES----------------------------
    private void btnConfirm_Click(object sender, EventArgs e){
        if (data.DeleteAccount(current.Number)){ MessageBox.Show(this, "Usuario eliminado exitosamente"); } 
        else { MessageBox.Show(this, "Ha habido un error borrando el usuario"); }
        this.Close();
    }
    private void btnCancel_Click(object sender, EventArgs e) { this.Close(); }

    //----------------------------INICIALIZACIONES----------------------------
    private void InitAttrs(){
        this.Text = "Edition Menu";
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
        IconPictureBox icn = Default.GenIconBox(IconChar.UserXmark, DockStyle.Fill, clr: AppProperties.clrTxtGray);
        Label lblDesc = Default.GenLabel($"Estas seguro que deseas eliminar cuenta?\n{current.Number}\n{current.OwnerName}", 
            ContentAlignment.MiddleCenter, AppProperties.fntTitle);

        btnCancel = Default.GenButton("Cancelar ✖", fillCol: Color.Green);
        btnCancel.Click += btnCancel_Click;
        btnConfirm = Default.GenButton("Confirmar ✔ ", fillCol: Color.Red);
        btnConfirm.Click += btnConfirm_Click;

        tlpMain.Controls.Add(icn, 0, 0);
        tlpMain.Controls.Add(lblDesc, 1, 0);
        tlpMain.SetColumnSpan(lblDesc, 4);
        tlpMain.SetRowSpan(lblDesc, 6);
        tlpMain.Controls.Add(btnCancel, 1, 6);
        tlpMain.SetColumnSpan(btnCancel, 2);
        tlpMain.Controls.Add(btnConfirm, 3, 6);
        tlpMain.SetColumnSpan(btnConfirm, 2);

        //----------------------------FIN CONTENEDOR PRINCIPAL----------------------------
        this.Controls.Add(pnlGrad);
    }
};