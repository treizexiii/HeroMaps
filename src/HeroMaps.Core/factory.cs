namespace HeroMaps.Core;

public class BoardFactory
{
    public static Board CreateDefaultBoard()
    {
        var board = new Board();
        return board;
    }

    public static Board CreateBlueRoom(Board board)
    {
        var room = new Room(RoomColor.blue);
        for (int row = 2; row <= 7; row++)
        {
            for (int col = 2; col <= 7; col++)
            {
                var cell = board.GetCell(col, row);
                room.AddCell(cell.CellId);
            }
        }

        board.Rooms.Add(room);

        return board;
    }
}
