using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Logic.Game;
using Logic.Interfaces;
using Shared.Models;
using Shared.Models.Enums;

namespace Logic.Algorithm
{
     public class IterativeAlphaBeta : IAdversarialSearch<MapState, MoveAction>
    {
        private readonly IGame<MapState, MoveAction, Player> _game;
        private readonly double _possibleTime;

        public IterativeAlphaBeta(IGame<MapState, MoveAction, Player> game, double possibleTime)
        {
            _possibleTime = possibleTime;
            _timer.AutoReset = false;
            _timer.Elapsed += (_, _) => _isTimeElapsed = true;
            _game = game;
        }

        private bool _isTimeElapsed = false;
        private readonly Timer _timer = new Timer();
        
        public MoveAction MakeDecision(MapState state)
        {
            // var actions = state.GetActions();
            // return state.GetActions()[0];
            
            var player = _game.GetPlayer(state);
            var results = _game.GetActions(state);

            _timer.Interval = _possibleTime / 2 / results.Count;

            var newResults = new List<(MoveAction action, double value)>();
            foreach (var action in results)
            {
                var s = _game.GetResult(state, action);
                var p = _game.GetPlayer(s);
                _isTimeElapsed = false;
                _timer.Start();
                double value = AlphaBeta(s, p, double.NegativeInfinity, double.PositiveInfinity);
                _timer.Stop();

                newResults.Add((action, value));
            }
            
            return player is Player.Red
                ? GetMaxValueAction(newResults, state)
                : GetMinValueAction(newResults, state);
            
        }
        

        private MoveAction GetMaxValueAction(List<(MoveAction action, double value)> actionValues, MapState state)
        {
            var max = actionValues.Max(av => av.value);

            return actionValues
                .Where(av => av.value == max)
                .OrderByDescending(av => _game.GetResult(state, av.action).Eval()).First().action;
        }
        
        private MoveAction GetMinValueAction(List<(MoveAction action, double value)> actionValues, MapState state)
        {
            var min = actionValues.Min(av => av.value);

            return actionValues
                .Where(av => av.value == min)
                .OrderByDescending(av => _game.GetResult(state, av.action).Eval()).Last().action;
        }

        private int AlphaBeta(MapState state, Player player, double alpha, double beta)
        {
            if (_isTimeElapsed || _game.IsTerminal(state))
            {
                return Eval(state, player);
            }

            int value;

            if (player == Player.Red)
            {
                value = int.MinValue;
                foreach (var action in _game.GetActions(state))
                {
                    var s = _game.GetResult(state, action);
                    var p = _game.GetPlayer(s);
                    value = Math.Max(value, AlphaBeta(s, p, alpha, beta));
                    alpha = Math.Max(alpha, value);
                    if (alpha >= beta)
                    {
                        return value;
                    }
                }

                return value;
            }

            value = int.MaxValue;
            foreach (var action in _game.GetActions(state))
            {
                var s = _game.GetResult(state, action);
                var p = _game.GetPlayer(s);
                value = Math.Min(value, AlphaBeta(s, p, alpha, beta));
                beta = Math.Min(beta, value);
                if (beta <= alpha)
                {
                    return value;
                }
            }

            return value;
        }

        private int Eval(MapState state, Player player)
        {
            return _game.IsTerminal(state) ? _game.GetUtility(state, player) : state.Eval();
        }
    }      
}
