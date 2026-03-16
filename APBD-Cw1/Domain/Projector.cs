namespace APBD_Cw1.Domain;

public sealed class Projector : Equipment
{
    public Projector(string id, string name, int brightnessLumens, bool supportsHdmi)
        : base(id, name)
    {
        BrightnessLumens = brightnessLumens;
        SupportsHdmi = supportsHdmi;
    }

    public int BrightnessLumens { get; }
    public bool SupportsHdmi { get; }

    public override string ToString()
        => base.ToString() + $" | Jasność: {BrightnessLumens} lm | HDMI: {(SupportsHdmi ? "tak" : "nie")}";
}