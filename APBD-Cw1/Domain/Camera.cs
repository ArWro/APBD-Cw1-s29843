namespace APBD_Cw1.Domain;

public sealed class Camera : Equipment
{
    public Camera(string id, string name, int megapixels, bool interchangeableLens)
        : base(id, name)
    {
        Megapixels = megapixels;
        InterchangeableLens = interchangeableLens;
    }

    public int Megapixels { get; }
    public bool InterchangeableLens { get; }

    public override string ToString()
        => base.ToString() + $" | MPix: {Megapixels} | Wymienna optyka: {(InterchangeableLens ? "tak" : "nie")}";
}