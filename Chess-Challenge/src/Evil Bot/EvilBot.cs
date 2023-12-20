using System;
using ChessChallenge.API;
using Stockfish.NET;

public class EvilBot : IChessBot
{
    const int STOCKFISH_LEVEL = 6;

    IStockfish mStockFish;

    public EvilBot()
    {
        Stockfish.NET.Models.Settings stockfishSettings = new Stockfish.NET.Models.Settings();
        stockfishSettings.SkillLevel = STOCKFISH_LEVEL;

        mStockFish = new Stockfish.NET.Stockfish(@"resources/stockfish/stockfish-11-modern", 40, stockfishSettings);
    }

    public Move Think(Board board, Timer timer)
    {
        string fen = board.GetFenString();
        mStockFish.SetFenPosition(fen);
        Console.WriteLine(timer.MillisecondsElapsedThisTurn);
        string bestMove = mStockFish.GetBestMoveTime(GetTime(board, timer));

        return new Move(bestMove, board);
    }

    // Basic time management
    public int GetTime(Board board, Timer timer)
    {
        return 1000; //Math.Min(board.PlyCount * 150 + 100, timer.MillisecondsRemaining / 20);
    }
}