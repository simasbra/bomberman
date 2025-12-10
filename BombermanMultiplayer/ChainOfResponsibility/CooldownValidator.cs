using BombermanMultiplayer.Commands.Interface;
using System;
using System.Collections.Generic;

namespace BombermanMultiplayer.ChainOfResponsibility
{
    /// <summary>
    /// Validates cooldown - prevents command spam
    /// Fifth validator: Sufficient time must have passed since last command
    /// </summary>
    public class CooldownValidator : ValidationHandler
    {
        private Dictionary<byte, DateTime> _lastCommandTime = new Dictionary<byte, DateTime>();
        private readonly int _cooldownMs;

        public CooldownValidator(int cooldownMs = 100)
        {
            _cooldownMs = cooldownMs;
        }

        public override bool Validate(ICommand command, Game game)
        {
            byte playerNum = command.PlayerNumber;
            DateTime now = DateTime.Now;

            // Check if player has previous command
            if (_lastCommandTime.ContainsKey(playerNum))
            {
                TimeSpan timeSinceLastCommand = now - _lastCommandTime[playerNum];
                
                if (timeSinceLastCommand.TotalMilliseconds < _cooldownMs)
                {
                    double remaining = _cooldownMs - timeSinceLastCommand.TotalMilliseconds;
                    Console.WriteLine($"[5. Cooldown] X REJECTED - Cooldown active ({remaining:F0}ms / {_cooldownMs}ms)");
                    return false;
                }
            }

            // Update last command time
            _lastCommandTime[playerNum] = now;

            Console.WriteLine($"[5. Cooldown] + PASSED - Cooldown ready ({_cooldownMs}ms threshold)");
            Console.WriteLine($"=== Validation Result: ALL PASSED + ===\n");
            
            return base.Validate(command, game);
        }
    }
}
