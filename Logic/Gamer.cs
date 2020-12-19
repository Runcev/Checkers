using System;
using System.Collections.Generic;
using System.Drawing;
using Shared.Models;
using Shared.Models.Enums;

namespace Logic
{
    public class Gamer
    {
        private const int MinRow = 0;
        private const int MaxRow = 7;
        private const int MinCol = 0;
        private const int MaxCol = 3;
        private GameInfoData GameInfo { get; set; }
        private Player Player { get; set; }
        
        private List<Square> OurSquare { get; set; }

        public Gamer(GameInfoData gameInfoData, Player player)
        {
            GameInfo = gameInfoData;
            Player = player;
            foreach (var square in gameInfoData.Board)
            {
                if (square.Color == Player)
                {
                    OurSquare.Add(square);
                }
            }
        }

        public void Play(GameInfoData gameInfoData)
        {
            if (gameInfoData.IsStarted && gameInfoData.Status == "Success")
            {
                
            }
        }

        public List<Square> GetPossibleMoves(GameInfoData gameInfoData)
        {
            var possibleMoves = new List<Square>();
            foreach (var square in OurSquare)
            {
                if (square.Column == 0)
                {
                    
                }
            }
            return possibleMoves;
        }
    

        /*
        private List<Tuple<List<Square>, String>> GetPossibleBoardStates(List<Square> boardState, bool red)
        {
            var possibleBoardStates = new List<Tuple<List<Square>, string>>();
            var capture = false;

            foreach (var square in boardState)
            {
                if (isTimeOut())
                    break;
                if (square.Color == Player.Red && red)
                    FindMove(boardState, possibleBoardStates, capture, square, true, "");
                else if (square.Color != Player.Red && !red)
                    FindMove(boardState, possibleBoardStates, capture, square, false, "");
            }

            return possibleBoardStates;
        }



        private void FindMove(List<Square> boardState, List<Tuple<List<Square>, String>> possibleBoardStates, bool capture, Square square, bool isRed, string currentMoves ){
            var auxBoard = new bool[33]; // True - RED, False - Black, Null - Empty
            foreach (var s in boardState)
            {
                auxBoard[s.Position] = s.Color == Player.Red;
            }

            if (square.King)
                if (isRed)
                    FindMovesLeft(boardState, possibleBoardStates, auxBoard, capture, square, true, currentMoves);
                else
                    FindMovesRight(boardState, possibleBoardStates, auxBoard, capture, square, false, currentMoves);
            if (isRed)
                FindMovesRight(boardState, possibleBoardStates, auxBoard, capture, square, true, currentMoves);
            else
                FindMovesLeft(boardState, possibleBoardStates, auxBoard, capture, square, false, currentMoves);
        }


        private void FindMovesRight(
            List<Square> boardState, List<Tuple<List<Square>, String>> possibleBoardStates, bool[] auxBoard,
            bool capture, Square cell,
            bool isRed, String currentMove
        ) 
        {
            var row = cell.Row;
            var col = cell.Column;
            var pos = cell.Position;
            var rowEven = row % 2 == 0;

            int topRight = getTopRightPosition(pos, rowEven);
            int bottomRight = getBottomRightPosition(pos, rowEven);

            if (row < MaxRow - 1 && col > MinCol)
            {
                int topTopRight = getTopRightPosition(topRight, !rowEven);
                maybeCapture(boardState, possibleBoardStates, auxBoard,
                    capture, pos, topRight, topTopRight, isRed, currentMove, cell.King);
            }

            if (row < MaxRow - 1 && col < MaxCol)
            {
                int bottomBottomRight = getBottomRightPosition(bottomRight, !rowEven);
                maybeCapture(boardState, possibleBoardStates, auxBoard,
                    capture, pos, bottomRight, bottomBottomRight, isRed, currentMove, cell.King);
            }

            if (!capture && row != MaxRow)
                maybeMove(boardState, possibleBoardStates, auxBoard,
                    isRed, col, pos, rowEven, topRight, bottomRight);
        }


        private void FindMovesLeft(
            List<Square> boardState, List<Tuple<List<Square>, String>> possibleBoardStates, bool[] auxBoard,
            bool capture, Square cell,
            bool isRed, String currentMove
        )
        {
            int row = cell.Row;
            int col = cell.Column;
            int pos = cell.Position;
            bool rowEven = row % 2 == 0;

            int topLeft = getTopLeftPosition(pos, rowEven);
            int bottomLeft = getBottomLeftPosition(pos, rowEven);

            if (row > MinRow + 1 && col > MinCol)
            {
                int topTopLeft = getTopLeftPosition(topLeft, !rowEven);
                maybeCapture(boardState, possibleBoardStates, auxBoard,
                    capture, pos, topLeft, topTopLeft, isRed, currentMove, cell.King);
            }

            if (row > MinRow + 1 && col < MaxCol)
            {
                int bottomBottomLeft = getBottomLeftPosition(bottomLeft, !rowEven);
                maybeCapture(boardState, possibleBoardStates, auxBoard,
                    capture, pos, bottomLeft, bottomBottomLeft, isRed, currentMove, cell.King);
            }

            if (!capture && row != MinRow)
                maybeMove(boardState, possibleBoardStates, auxBoard,
                    isRed, col, pos, rowEven, topLeft, bottomLeft);
        }*/
    }
}
