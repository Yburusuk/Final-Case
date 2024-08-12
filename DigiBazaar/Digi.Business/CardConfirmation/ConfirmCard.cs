using Digi.Base.Response;

namespace Digi.Business.CardConfirmation;

public class ConfirmCard : IConfirmCard
{
    public ConfirmCard()
    {
    }

    public bool Confirm(string NameSurname, string CardNumber, int Cvv, int ExpirationYear, int ExpirationMonth)
    {
        int currentMonth = DateTime.Now.Month;
        int currentYear = DateTime.Now.Year;
        
        if (NameSurname is null) return false;
        
        if (CardNumber.Length != 16) return false;
        
        if (Cvv.ToString().Length != 3) return false;
        
        if (ExpirationYear.ToString().Length != 4) return false;
        
        if (ExpirationMonth.ToString().Length != 2) return false;
        
        if (ExpirationYear < currentYear || (ExpirationYear == currentYear && ExpirationMonth < currentMonth)) return false;

        return true;
    }
}