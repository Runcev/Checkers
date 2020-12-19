using System.Collections.Generic;
using GameSolver.Game;
using PacMan.Entities;
using PacMan.Players;

namespace PacMan.SkovorodaGame
{
    public class PacmanGame : IGame<MapState, MoveAction, Player>
    {
        private readonly MapState _initialState;

        public PacmanGame(int level)
        {
            _initialState = new MapState(level);
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