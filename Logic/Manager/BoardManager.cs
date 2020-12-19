using System.Collections.Generic;
using System.Linq;
using Shared.Models;
using Shared.Models.Enums;

namespace Logic.Manager
{
    public static class BoardManager
    {
        public static Square GetSquare(this GameInfoData gameInfoData, int position)
        {
            return gameInfoData.Board.FirstOrDefault(square => square.Position == position);
        }

        public static List<Square> GetPlayerSquares(this GameInfoData gameInfoData, Player player)
        {
            return gameInfoData.Board.Where(square => square.Color == player).ToList();
        }
    }
}