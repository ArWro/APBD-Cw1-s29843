namespace APBD_Cw1.Domain;

public sealed class Laptop : Equipment
{
    public Laptop(string id, string name, string processor, int ramGb)
        : base(id, name)
    {
        Processor = processor;
        RamGb = ramGb;
    }

    public string Processor { get; }
    public int RamGb { get; }

    public override string ToString()
        => base.ToString() + $" | CPU: {Processor} | RAM: {RamGb} GB";
}