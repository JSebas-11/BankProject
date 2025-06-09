namespace DB_BankProject.ModelsNative {
    public static class AppProperties {
        public static List<Branches> branches = new List<Branches> { 
            Branches.Medellin, Branches.Itagui, Branches.Envigado, Branches.Bello, Branches.Sabaneta, 
            Branches.LaEstrella, Branches.Aguadas, Branches.Segovia, Branches.Digital };
        public static List<int> paymenTerm = new List<int>() { 1, 3, 6, 12, 18, 24 };
        public static int txtInputSize = 24;
        public static decimal interestRate = 9.25m;
        public static decimal transLimit = 2000000.00m;
        public static Size szScreen = new Size(1280, 680);
        public static Size szSubScreen = new Size((int)(szScreen.Width*.4), (int)(szScreen.Height*.6));
        public static Font fntTitle = new Font("Segoe UI", 22, FontStyle.Bold);
        public static Font fntSubTitle = new Font("Segoe UI", 14, FontStyle.Bold);
        public static Font fntText = new Font("Segoe UI", 10, FontStyle.Regular);
        public static Color clrMainBlue = Color.FromArgb(2, 63, 129);
        public static Color clrBgBlack = Color.FromArgb(10, 15, 25);
        public static Color clrBgBlue = Color.FromArgb(35, 40, 50);
        public static Color clrTxt = Color.FromArgb(245, 245, 250);
        public static Color clrTxtGray = Color.FromArgb(176, 190, 197);
        public static Color clrHover = Color.FromArgb(55, 60, 70);
        public static Color clrTrans = Color.Transparent;
    }
}
