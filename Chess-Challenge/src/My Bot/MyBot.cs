using ChessChallenge.API;

public class MyBot : IChessBot
{
    const int QUEEN_WEIGHT = 1000;
    const int ROOK_WEIGHT = 525;
    const int BISHOP_WEGIHT = 350;
    const int KNIGHT_WEIGHT = 350;
    const int PAWN_WEIGHT = 100;
    const int MOBILITY_WEIGHT = 25;
    bool white;
    public Move Think(Board board, Timer timer)
    {
        white = board.IsWhiteToMove;
        int bestscore = int.MinValue;
        Move bestmove = new();
        foreach(Move move in board.GetLegalMoves())
        {
            board.MakeMove(move);
            int eval = Trace(board, 3);
            board.UndoMove(move);
            if(eval > bestscore)
            {
                bestscore = eval;
                bestmove = move;
            }
        }
        return bestmove;
    }

    public int Trace(Board board, int depth)
    {
        if (depth == 0)
            return Evaluate(board);
        int bestscore = (board.IsWhiteToMove == white) ? int.MinValue : int.MaxValue;

        foreach(Move move in board.GetLegalMoves())
        {
            board.MakeMove(move);
            int eval = Trace(board, depth - 1);
            board.UndoMove(move);
            if(board.IsWhiteToMove == white && eval > bestscore)
            {
                bestscore = eval;
            }

            else if (board.IsWhiteToMove != white && eval < bestscore)
            {
                bestscore = eval;
            }
        }
        return bestscore;
    }
    public int Evaluate(Board board)
    {
        if (board.IsInCheckmate())
            return int.MaxValue - 1;
        if (board.IsDraw())
            return 0;
        int selfpieceindex = board.IsWhiteToMove ? 0 : 6;
        int enemypieceindex = board.IsWhiteToMove ? 6 : 0;
        PieceList[] pl = board.GetAllPieceLists();
        int materialscore = QUEEN_WEIGHT *
            (pl[4 + selfpieceindex].Count - pl[4 + enemypieceindex].Count)
            + ROOK_WEIGHT *
            (pl[3 + selfpieceindex].Count - pl[3 + enemypieceindex].Count)
             + BISHOP_WEGIHT *
            (pl[2 + selfpieceindex].Count - pl[2 + enemypieceindex].Count)
            + KNIGHT_WEIGHT *
            (pl[1 + selfpieceindex].Count - pl[1 + enemypieceindex].Count)
            + PAWN_WEIGHT *
            (pl[selfpieceindex].Count - pl[enemypieceindex].Count);

        int mobilityscore = MOBILITY_WEIGHT * (board.GetLegalMoves().Length - SquaresAttacked(board));

        return (materialscore + mobilityscore);

    }

    public int SquaresAttacked(Board board)
    {
        int count = 0;
        for(int i = 0; i < 64; i++)
        {
            count += board.SquareIsAttackedByOpponent(new Square(i)) ? 1 : 0;
        }
        return count;
    }
}
