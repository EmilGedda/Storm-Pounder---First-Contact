using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Storm_Pounder___First_Contact.Core.Event
{
	public class GameEventArgs : EventArgs
	{
		public GameTime Time { get; private set; }

		public GameEventArgs(GameTime gameTime)
		{
			Time = gameTime;
		}

		public new static GameEventArgs Empty
		{
			get { return new GameEventArgs(new GameTime()); }
		}
		 
	}
}
