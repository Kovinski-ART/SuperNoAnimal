
using System.Collections.Generic;

public class PlayerAbilityFactory
{
	PlayerStateMachine _context;

	Dictionary<PlayerStates, PlayerBaseState> _states = new Dictionary<PlayerStates, PlayerBaseState>();

	public PlayerAbilityFactory(PlayerStateMachine currentContext)
	{
		_context = currentContext;

	}
}
