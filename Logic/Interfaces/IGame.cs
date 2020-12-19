using System.Collections.Generic;

namespace Logic.Interfaces
{
    public interface IGame<S,A,P>
    {
        S GetInitialState();
        P GetPlayer(S state);
        List<A> GetActions(S state);
        S GetResult(S state, A action);
        bool IsTerminal(S state);
        int GetUtility(S state, P player);
    }
}