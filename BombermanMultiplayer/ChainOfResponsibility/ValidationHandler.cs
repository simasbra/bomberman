using BombermanMultiplayer.Commands.Interface;

namespace BombermanMultiplayer.ChainOfResponsibility
{
    /// <summary>
    /// Base class for validation chain. Each handler validates one aspect and forwards if valid.
    /// Returns true when validation passes through the chain.
    /// </summary>
    public abstract class ValidationHandler
    {
        protected ValidationHandler _nextHandler;

        public ValidationHandler SetNext(ValidationHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        public virtual bool Validate(ICommand command, Game game)
        {
            if (_nextHandler != null)
            {
                return _nextHandler.Validate(command, game);
            }

            return true;
        }
    }
}
