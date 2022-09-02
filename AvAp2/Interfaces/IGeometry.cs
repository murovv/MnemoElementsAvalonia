namespace AvAp2.Interfaces
{
    public interface IGeometry
    {
        double CoordinateX2 { get; }
        double CoordinateY2 { get; }
        double LineThickness { get; }
        bool IsDash { get; }
    }
}