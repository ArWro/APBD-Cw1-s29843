namespace APBD_Cw1.Services;

public class IdGenerator
{
    private int _equipmentCounter = 1;
    private int _userCounter = 1;
    private int _rentalCounter = 1;

    public string NextEquipmentId() => $"EQ-{_equipmentCounter++:D3}";
    public string NextUserId() => $"USR-{_userCounter++:D3}";
    public string NextRentalId() => $"RNT-{_rentalCounter++:D3}";
}