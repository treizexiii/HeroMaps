namespace HeroMaps.Core;

public class BoardFactory
{
    public static Board CreateDefaultBoard()
    {
        var board = new Board();

        board.CreateRoom(RoomColor.blue, 2, 2, 7, 7);
        board.CreateRoom(RoomColor.red, 2, 7, 20, 23);
        return board;
    }
}
