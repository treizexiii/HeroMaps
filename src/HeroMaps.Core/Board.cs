namespace HeroMaps.Core;

public enum RoomColor
{
    blue,
    red,
    green,
    yellow,
    purple,
    orange,
}

public class Room
{
    public RoomColor Color { get; set; }
    public List<int> Cells { get; set; } = [];

    public Room(RoomColor color)
    {
        Color = color;
    }

    public void AddCell(int cellId)
    {
        if (!Cells.Contains(cellId))
        {
            Cells.Add(cellId);
        }
    }
}

public class Board
{
    public const int DEFAULT_WIDTH = 26;
    public const int DEFAULT_HEIGHT = 19;

    private readonly Cell[,] _grid;

    public List<Room> Rooms { get; private set; } = [];
    public List<int> Corridors { get; private set; } = [];
    public int Width { get; private set; }
    public int Height { get; private set; }

    public Board(int width = DEFAULT_WIDTH, int height = DEFAULT_HEIGHT)
    {
        Width = width;
        Height = height;
        _grid = new Cell[width, height];
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        int cellId = 1;
        for (int row = 0; row < Height; row++)
        {
            for (int col = 0; col < Width; col++)
            {
                _grid[col, row] = new Cell(cellId, row, col);
                cellId++;
            }
        }
    }

    public void CreateRoom(RoomColor color, int startRow, int startCol, int endRow, int endCol)
    {
        var room = new Room(color);
        for (int row = startRow; row <= endRow; row++)
        {
            for (int col = startCol; col <= endCol; col++)
            {
                var cell = GetCell(col, row);

                if (col == startCol)
                {
                    cell.Walls[Directions.left] = true;
                }
                if (col == endCol)
                {
                    cell.Walls[Directions.right] = true;
                }
                if (row == startRow)
                {
                    cell.Walls[Directions.up] = true;
                }
                if (row == endRow)
                {
                    cell.Walls[Directions.down] = true;
                }

                room.AddCell(cell.CellId);
            }
        }

        Rooms.Add(room);
    }

    public Room? GetRoomByColor(RoomColor color)
    {
        return Rooms.FirstOrDefault(r => r.Color == color);
    }

    public IEnumerable<Cell> GetAllCells()
    {
        for (int row = 0; row < Height; row++)
        {
            for (int col = 0; col < Width; col++)
            {
                yield return _grid[col, row];
            }
        }
    }

    public Cell GetCell(int row, int column)
    {
        if (!IsValidPosition(row, column))
        {
            throw new ArgumentOutOfRangeException(
                $"Cell coordinates are out of bounds: row={row}, column={column}"
            );
        }
        return _grid[column, row];
    }

    public Cell GetCell(int cellId)
    {
        var cell = GetAllCells().FirstOrDefault(c => c.CellId == cellId);
        if (cell is null)
        {
            throw new ArgumentOutOfRangeException($"Cell with ID {cellId} does not exist.");
        }
        return cell;
    }

    public void SetCell(Cell cell)
    {
        if (!IsValidPosition(cell.Row, cell.Column))
        {
            throw new ArgumentOutOfRangeException(
                $"Cell coordinates are out of bounds: row={cell.Row}, column={cell.Column}"
            );
        }

        var oldCell = GetCell(cell.Row, cell.Column);
        if (oldCell is not null && oldCell.IsLocked())
        {
            throw new InvalidOperationException(
                $"Cannot modify locked cell at row={cell.Row}, column={cell.Column}"
            );
        }
        _grid[cell.Column, cell.Row] = cell;
    }

    public Cell? GetNext(Cell cell, Directions direction)
    {
        int newRow = cell.Row;
        int newColumn = cell.Column;

        switch (direction)
        {
            case Directions.up:
                newRow--;
                break;
            case Directions.down:
                newRow++;
                break;
            case Directions.left:
                newColumn--;
                break;
            case Directions.right:
                newColumn++;
                break;
        }

        if (IsValidPosition(newRow, newColumn))
        {
            return GetCell(newRow, newColumn);
        }
        return null;
    }

    public void CreateDoor(Cell cell, Directions direction)
    {
        if (!IsValidPosition(cell.Row, cell.Column))
        {
            throw new ArgumentOutOfRangeException(
                $"Cell coordinates are out of bounds: row={cell.Row}, column={cell.Column}"
            );
        }

        var currentCell = GetCell(cell.Row, cell.Column);
        var adjacentCell = GetNext(currentCell, direction);
        if (adjacentCell is null)
        {
            throw new InvalidOperationException(
                $"Cannot create door in direction {direction} from cell at row={cell.Row}, column={cell.Column} because it is out of bounds."
            );
        }

        currentCell.SetDoor(direction);
        adjacentCell.SetDoor(direction.Opposite());
    }

    private bool IsValidPosition(int row, int column)
    {
        return row >= 0 && row < Height && column >= 0 && column < Width;
    }
}
