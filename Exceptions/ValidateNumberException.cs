namespace FastDeliveryAPI.Exceptions;

public class ValideteNumberException : ApplicationException
{
    public ValideteNumberException(string numero) : base ($"{numero} Only 8 digits are allowed.")
    {

    }
}