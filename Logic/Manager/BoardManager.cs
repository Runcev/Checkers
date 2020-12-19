using System.Collections.Generic;
using System.Linq;
using Shared.Models;
using Shared.Models.Enums;

namespace Logic.Manager
{
    public class BoardManager
    {
        private readonly GameInfoData _gameInfoData;

        public BoardManager(GameInfoData gameInfoData)
        {
            _gameInfoData = gameInfoData;
        }

        public Square GetSquare(int position)
        {
            return _gameInfoData.Board.FirstOrDefault(square => square.Position == position);
        }

        public List<Square> GetPlayerSquares(Player player)
        {
            return _gameInfoData.Board.Where(square => square.Color == player).ToList();
        }
        
        
    }
}