using FontAwesome.Sharp;
using Guna.UI2.WinForms;

namespace DB_BankProject.ModelsNative;

public class PnlDoubleBuff : Panel {
    //Clase panel para optimizar el dibujo de los submenus dinamicos
    public PnlDoubleBuff(){
        this.Padding = new Padding(0);
        this.Margin = new Padding(0);
        this.DoubleBuffered = true;
        this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        this.SetStyle(ControlStyles.UserPaint, true);
        this.UpdateStyles();
    }
}
internal class Default2{ //Clase con generadores de objetos especificos (cards, menus, etc)
    //------------------------------Generador de inputs-------------------------------
    public static TableLayoutPanel GenInputCard(IconChar icon, Guna2TextBox txb, bool large = false){
        TableLayoutPanel tlp = Default.GenTabLayPnl(1, 2, AppProperties.clrBgBlack, true, false);
        tlp.Margin = new Padding(1);
        if (large){
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10f));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 90f));
        } else {
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80f));
        }

        tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        IconPictureBox icn = Default.GenIconBox(icon, DockStyle.Fill, clr: AppProperties.clrTxt);

        tlp.Controls.Add(icn, 0, 0);
        tlp.Controls.Add(txb, 1, 0);

        return tlp;
    }
    //------------------------------Generador de ButtonCard (para Aside)-------------------------------
    public static TableLayoutPanel GenButtonCard(string description, Guna2Button btn, IconChar icon){
        TableLayoutPanel container = Default.GenTabLayPnl(6, 5, AppProperties.clrBgBlue, false);

        IconPictureBox icn = Default.GenIconBox(icon, DockStyle.Fill, clr: AppProperties.clrTxt);
        Label lbl = Default.GenLabel(description, ContentAlignment.MiddleCenter, AppProperties.fntText);

        container.Controls.Add(icn, 0, 1);
        container.SetRowSpan(icn, 4);
        container.Controls.Add(lbl, 1, 1);
        container.SetColumnSpan(lbl, 3);
        container.SetRowSpan(lbl, 4);
        container.Controls.Add(btn, 1, 5);
        container.SetColumnSpan(btn, 3);
        container.SetRowSpan(btn, 2);

        return container;
    }
    //------------------------------Generador de DataGrid-------------------------------
    public static Guna2DataGridView GenDataGrid(bool read, bool multi = false, bool dock = true){
        Guna2DataGridView dataGrid = new Guna2DataGridView(){
            ReadOnly = read,
            MultiSelect = multi,
            Dock = dock ? DockStyle.Fill : DockStyle.None,
            ScrollBars = ScrollBars.Vertical,
            EnableHeadersVisualStyles = false,
            Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.Dark,
            Margin = new Padding(1)
        };
        dataGrid.ThemeStyle.BackColor = AppProperties.clrBgBlue;
        dataGrid.ThemeStyle.HeaderStyle.BackColor = AppProperties.clrMainBlue;
        dataGrid.ThemeStyle.HeaderStyle.ForeColor = AppProperties.clrTxt;
        dataGrid.ThemeStyle.RowsStyle.SelectionBackColor = AppProperties.clrMainBlue;
        dataGrid.ThemeStyle.RowsStyle.SelectionForeColor = AppProperties.clrTxt;
        dataGrid.ColumnHeadersDefaultCellStyle.SelectionBackColor = AppProperties.clrMainBlue;
        return dataGrid;
    }
    //------------------------------Generador de ComboBox-------------------------------
    public static Guna2ComboBox GenComBox(object data, bool dock = true, Color? bgClr = null, Color? txtClr = null, Font? fnt = null){
        Guna2ComboBox cmbx = new Guna2ComboBox(){
            DataSource = data,
            Dock = dock ? DockStyle.Fill : DockStyle.None,
            FillColor = bgClr ?? AppProperties.clrMainBlue,
            BackColor = AppProperties.clrTrans,
            BorderColor = txtClr ?? AppProperties.clrTxtGray,
            ForeColor = txtClr ?? AppProperties.clrTxtGray,
            Font = fnt ?? AppProperties.fntSubTitle,
            BorderRadius = 12,
            BorderThickness = 3,
        };
        cmbx.FocusedState.BorderColor = txtClr ?? AppProperties.clrTxt;
        cmbx.FocusedState.FillColor = AppProperties.clrMainBlue;
        cmbx.FocusedState.ForeColor = AppProperties.clrTxt;

        return cmbx;
    }
}

