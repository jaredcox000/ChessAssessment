namespace ChessNumbers.Game
   {
   class Piece
      {
      int type;
      int boardX;
      int boardY;

      public Piece (int Type)
         {
         type = Type;
         }

      /// <summary>
      /// Adds the chess piece to the game board.
      /// 
      /// </summary>
      /// <param name="board"></param>
      public void AddToBoard (Board board) 
         {
         board.AddPiece (this);
         }

      public int BoardX
         {
         get { return boardX; }
         }

      public int BoardY
         {
         get { return boardY; }
         }

      /// <summary>
      /// Places the piece on the specifiied position on the board.
      /// 
      /// </summary>
      /// <param name="x"></param>
      /// <param name="y"></param>
      public void SetPosition (int x, int y)
         {
         boardX = x;
         boardY = y;
         }

      public int Type
         {
         get { return type; }
         }
      }
   }
