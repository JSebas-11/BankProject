namespace DB_BankProject.ModelsNative;
internal class CommonFunct {
    //Funciones comunes en entradas de datos comunes en todos los forms
    public static bool TransLimit(decimal amount){ return amount > AppProperties.transLimit; }
    public static void RandowWaiting(){
        Thread.Sleep(new Random().Next(1000, 5000));
    }
    public static bool EmptyInput(string input){ return string.IsNullOrWhiteSpace(input.Trim()); }
    public static bool EmptyInputs(string input1, string input2){
        if (string.IsNullOrWhiteSpace(input1.Trim()) || string.IsNullOrWhiteSpace(input2.Trim())){
            return true;
        }
        return false;
    }
    public static bool EmptyInputs(string input1, string input2, string input3){
        if (string.IsNullOrWhiteSpace(input1.Trim()) 
            || string.IsNullOrWhiteSpace(input2.Trim())
            || string.IsNullOrWhiteSpace(input3.Trim())){
            return true;
        }
        return false;
    }
    public static bool CorrectInputs(string number, string password){
        if (number.Trim().Length != 10 || password.Trim().Length < 4){
            return false;
        }
        return true;
    }
    public static bool CorrectNumber(string number){
        if (number.Trim().Length != 10 || string.IsNullOrWhiteSpace(number.Trim())){
            return false;
        }
        return true;
    }
    public static bool CorrectAmount(string number){
        if (!decimal.TryParse(number, out decimal num)) {
            return false;
        };
        if (num <= 0 || TransLimit(num)){
            return false;
        }
        return true;
    }
    public static bool CorrectPassword(string password){
        if (password.Trim().Length < 4 || string.IsNullOrWhiteSpace(password.Trim())){
            return false;
        }
        return true;
    }
}

