namespace Finals;

public class CardDetails
{
    public string CardNumber { get; set; }
    public string ExpirationDate { get; set; }
    public string CVC { get; set; }
    public string PinCode { get; set; }

    public CardDetails(string cardNumber, string expirationDate, string cvc, string pinCode)
    {
        CardNumber = cardNumber;
        ExpirationDate = expirationDate;
        CVC = cvc;
        PinCode = pinCode;
    }
}