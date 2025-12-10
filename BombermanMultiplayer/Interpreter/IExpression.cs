using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Interpreter
{
	public interface IExpression
	{
		void Interpret(GameCommandContext context);
	}
}
