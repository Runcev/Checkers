using System.Collections.Generic;
using Logic.Interfaces;
using Shared.Models;
using Shared.Models.Enums;

namespace Logic.Game
{
    public class CheckersGame : IGame<MapState, MoveAction, Player>
    {
        private readonly MapState _initialState;

        public CheckersGame(GameInfoData initialState)
        {
            _initialState = new MapState(initialState);
        }

        public MapState GetInitialState() => _initialState;

        public Player GetPlayer(MapState state) => state.GetPlayer();

        public List<MoveAction> GetActions(MapState state) => state.GetActions();

        public MapState GetResult(MapState state, MoveAction action)
        {
            var result = (MapState)state.Clone();
            result.Move(action);
            return result;
        }

        public bool IsTerminal(MapState state) => state.IsTerminal();

        public int GetUtility(MapState state, Player player)
        {
            return state.GetUtility(player);
        }
    }
}