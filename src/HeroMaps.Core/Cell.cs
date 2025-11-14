namespace HeroMaps.Core;

public class Cell
{
    public int CellId { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
    public CellContent Content { get; set; } = new CellContent();
    public string Zone { get; set; } = string.Empty;
    public bool Locked { get; private set; } = false;

    public Dictionary<Directions, bool> Doors { get; set; } =
        new()
        {
            { Directions.up, false },
            { Directions.down, false },
            { Directions.left, false },
            { Directions.right, false },
        };

    public Dictionary<Directions, bool> Walls { get; set; } =
        new()
        {
            { Directions.up, false },
            { Directions.down, false },
            { Directions.left, false },
            { Directions.right, false },
        };

    public Cell(int Id, int row, int column)
    {
        CellId = Id;
        Row = row;
        Column = column;
    }

    public bool IsLocked()
    {
        return Locked;
    }

    public void Lock()
    {
        Locked = true;
    }

    public void Unlock()
    {
        Locked = false;
    }

    public bool HasDoor(Directions direction)
    {
        return Doors.ContainsKey(direction) && Doors[direction];
    }

    public bool HasWall(Directions direction)
    {
        return Walls.ContainsKey(direction) && Walls[direction];
    }

    public void SetWall(Directions direction)
    {
        if (Walls.ContainsKey(direction))
        {
            Walls[direction] = true;
        }
    }

    public void RemoveWall(Directions direction)
    {
        if (Walls.ContainsKey(direction))
        {
            Walls[direction] = false;
        }
    }

    public void SetDoor(Directions direction)
    {
        if (Doors.ContainsKey(direction))
        {
            Doors[direction] = true;
        }
    }

    public void RemoveDoor(Directions direction)
    {
        if (Doors.ContainsKey(direction))
        {
            Doors[direction] = false;
        }
    }

    public void AddContent(CellContent content)
    {
        Content = content;
    }

    public void RemoveContent()
    {
        Content = new CellContent();
    }

    public override string ToString()
    {
        return $"Cell({Row}, {Column}): Content={Content.Content} ({Content.ContentType}), Zone={Zone}, Doors=[up={HasDoor(Directions.up)}, down={HasDoor(Directions.down)}, left={HasDoor(Directions.left)}, right={HasDoor(Directions.right)}]";
    }

    public string ToJson()
    {
        return $"{{\"row\":{Row},\"column\":{Column},\"content\":\"{Content.Content}\",\"contentType\":\"{Content.ContentType}\",\"zone\":\"{Zone}\",\"doors\":{{\"up\":{HasDoor(Directions.up).ToString().ToLower()},\"down\":{HasDoor(Directions.down).ToString().ToLower()},\"left\":{HasDoor(Directions.left).ToString().ToLower()},\"right\":{HasDoor(Directions.right).ToString().ToLower()}}}}}";
    }
}
