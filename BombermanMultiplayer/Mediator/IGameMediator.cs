using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Mediator
{
	public interface IGameMediator
	{
		void Notify(object sender, string eventType);
	}
}
