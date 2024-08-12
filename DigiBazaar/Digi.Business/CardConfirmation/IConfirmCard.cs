namespace Digi.Business.CardConfirmation;

public interface IConfirmCard
{
    bool Confirm(string NameSurname,string CardNumber, int Cvv, int ExpirationYear, int ExpirationMonth);
}