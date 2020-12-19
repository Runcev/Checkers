namespace Logic.Game
{
    public enum MoveActionEnum
    {
        ForwardRight,
        ForwardLeft,
        BackLeft,
        BackRight
        
    }

    public class MoveAction
    {
        public MoveAction(MoveActionEnum action)
        {
            Action = action;
        }

        public MoveActionEnum Action { get; }
    }
}