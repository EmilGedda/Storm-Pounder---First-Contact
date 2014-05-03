using System;
using System.Reflection;
using Storm_Pounder___First_Contact.Core.Event;

namespace Storm_Pounder___First_Contact.Core
{
    public partial class Level
    {
        public Level()
        {
        }

		public event EventHandler<GameEventArgs> Start;
	    public event EventHandler<GameEventArgs> End;

	    protected virtual void OnStart(GameEventArgs e)
	    {
		    EventHandler<GameEventArgs> handler = Start;
		    if (handler != null) handler(this, e);
	    }
	    protected virtual void OnEnd(GameEventArgs e)
	    {
		    EventHandler<GameEventArgs> handler = End;
		    if (handler != null) handler(this, e);
	    }
    }
}