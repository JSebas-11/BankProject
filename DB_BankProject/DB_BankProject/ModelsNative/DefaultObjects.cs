using FontAwesome.Sharp;
using Guna.UI2.WinForms;
using System.Drawing.Drawing2D;

namespace DB_BankProject.ModelsNative;

 public enum PersonType { Admin, User }
 public enum TransType { Withdrawal = 1, Deposit, Transfer, Investment }
 public enum InvestStatus { Cancelled = 1, Active, Completed }
 public enum Branches { Medellin = 1, Itagui, Envigado, Bello, Sabaneta, LaEstrella, Aguadas, Segovia, Digital }
 public class Default { //Clase con generadores de objetos generales (botones, txbs, etc)
     //------------------------------Generador de TableLayoutPanel-------------------------------
     public static TableLayoutPanel GenTabLayPnl(int rows, int cols, Color bgColor, bool border, bool styles = true, bool dock = true){
         TableLayoutPanel tlp = new TableLayoutPanel(){
             RowCount = rows,
             ColumnCount = cols,
             BackColor = bgColor,
             Dock = dock ? DockStyle.Fill : DockStyle.None,
             CellBorderStyle = border ? TableLayoutPanelCellBorderStyle.Inset : TableLayoutPanelCellBorderStyle.None,
             Margin = new Padding(0),
             Padding = new Padding(0)
         };

         if (styles){
             float rowPercen = (float)100 / rows;
             float colPercen = (float)100 / cols;

             for (int i = 0; i < rows; i++) { tlp.RowStyles.Add(new RowStyle(SizeType.Percent, rowPercen)); }
             for (int i = 0; i < cols; i++) { tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, colPercen)); }
         }

         return tlp;
     }
     //------------------------------Generador de panel gradiente-------------------------------
     public static Guna2GradientPanel GenGradPnl(Color clr1, Color clr2, bool dock = true){
         return new Guna2GradientPanel(){
             Dock = dock ? DockStyle.Fill : DockStyle.None,
             FillColor = clr1,
             FillColor2 = clr2,
             GradientMode = LinearGradientMode.Vertical,
             Padding = new Padding(0),
             Margin = new Padding(0)
         };
     }
     //------------------------------Generador de Label-------------------------------
     public static Label GenLabel(string text, ContentAlignment align, Font font, bool dock = true, Color? fontColor = null){
         return new Label(){
             Text = text,
             Font = font,
             TextAlign = align,
             Dock = dock ? DockStyle.Fill : DockStyle.None,
             ForeColor = fontColor ?? AppProperties.clrTxt,
             BackColor = AppProperties.clrTrans
         };
     }
     //------------------------------Generador de Button-------------------------------
     public static Guna2Button GenButton(string text, bool dock = true, Font font = null, Color? txtCol = null, Color? fillCol = null, bool border = true){
         Guna2Button newBtn = new Guna2Button(){
             Text = text,
             Font = font ?? AppProperties.fntSubTitle,
             ForeColor = txtCol ?? AppProperties.clrTxt,
             FillColor = fillCol ?? AppProperties.clrBgBlack,
             BackColor = AppProperties.clrTrans,
             BorderColor = txtCol ?? AppProperties.clrTxt,
             Dock = dock ? DockStyle.Fill : DockStyle.None,
             Cursor = Cursors.Hand,
         };
        if (border){
            newBtn.BorderThickness = 3;
            newBtn.BorderRadius = 12;
        }
        newBtn.HoverState.FillColor = AppProperties.clrHover;
         newBtn.HoverState.ForeColor = AppProperties.clrTxtGray;
         newBtn.HoverState.BorderColor = AppProperties.clrTxtGray;

         return newBtn;
     }
     //------------------------------Generador de TextBox-------------------------------
     public static Guna2TextBox GenTxtBox(string placeholder, int len, Font? font = null, bool password = false, bool align = true, bool dock = true){
         Guna2TextBox txb = new Guna2TextBox(){
             PlaceholderText = placeholder,
             MaxLength = len,
             TextAlign = align ? HorizontalAlignment.Center : HorizontalAlignment.Left,
             Font = font ?? AppProperties.fntSubTitle,
             Dock = dock ? DockStyle.Fill : DockStyle.None,
             UseSystemPasswordChar = password,
             Padding = new Padding(0),
             Margin = new Padding(0),
             BackColor = AppProperties.clrBgBlack,
             FillColor = AppProperties.clrBgBlack,
             PlaceholderForeColor = AppProperties.clrTxtGray,
             ForeColor = AppProperties.clrTxt,
             BorderColor = AppProperties.clrTxtGray
         };
         txb.HoverState.BorderColor = AppProperties.clrTxt;
         txb.HoverState.PlaceholderForeColor = AppProperties.clrTxt;
         txb.FocusedState.BorderColor = AppProperties.clrTxt;
         txb.FocusedState.PlaceholderForeColor = AppProperties.clrTxt;

         return txb;
     }
     //------------------------------Generador de PictureBox (img)-------------------------------
     public static Guna2PictureBox GenImgBox(Bitmap img, bool dock = true){
         return new Guna2PictureBox(){
             Image = img,
             Dock = dock ? DockStyle.Fill : DockStyle.None,
             SizeMode = PictureBoxSizeMode.StretchImage,
             BackColor = AppProperties.clrTrans,
             BorderRadius = 10,
             Padding = new Padding(0),
             Margin = new Padding(0)
         };
     }
     //------------------------------Generador de PictureBox (iconos)-------------------------------
     public static IconPictureBox GenIconBox(IconChar icon, DockStyle? dock = null, int? size = null, Color? clr = null){
         return new IconPictureBox(){
             IconChar = icon,
             Dock = dock ?? DockStyle.Fill,
             IconColor = clr ?? AppProperties.clrMainBlue,
             IconSize = size ?? (int)AppProperties.fntTitle.Size,
             IconFont = IconFont.Solid,
         };
     }
}

