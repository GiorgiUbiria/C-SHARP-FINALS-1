using Newtonsoft.Json;

namespace Finals;

public class CardDetails
{
    [JsonProperty("CardNumber")]
    private string CardNumber { get; set; }
    [JsonProperty("ExpirationDate")]
    private string ExpirationDate { get; set; }
    [JsonProperty("CVC")]
    private string CVC { get; set; }
    [JsonProperty("PinCode")]
    private string PinCode { get; set; }

    public CardDetails(string cardNumber, string expirationDate, string cvc, string pinCode)
    {
        CardNumber = cardNumber;
        ExpirationDate = expirationDate;
        CVC = cvc;
        PinCode = pinCode;
    }

    public void setPinCode(string newPinCode)
    {
        this.PinCode = newPinCode;
    }

    public string getPinCode()
    {
        return this.PinCode;
    }
    public string getExpirationDate()
    {
        return this.ExpirationDate;
    }
    public string getCardNumber()
    {
        return this.CardNumber;
    }

    public string getCvc()
    {
        return this.CVC;
    }
}