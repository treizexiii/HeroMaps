namespace HeroMaps.Core;

public static class DirectionsExtensions
{
    public static Directions Opposite(this Directions direction)
    {
        return direction switch
        {
            Directions.up => Directions.down,
            Directions.down => Directions.up,
            Directions.left => Directions.right,
            Directions.right => Directions.left,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), "Invalid direction"),
        };
    }
}
