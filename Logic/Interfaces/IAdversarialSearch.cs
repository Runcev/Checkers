namespace Logic.Interfaces
{
    public interface IAdversarialSearch<in S, out A> where A : class
    {
        A MakeDecision(S state);
    }
}