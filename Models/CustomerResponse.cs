namespace FastDeliveryAPI.Models;

public record CustomerReqponse
(   int Id,
    string Name,
    string PhoneNumber,
    string Email,
    string Address,
    bool Status
);