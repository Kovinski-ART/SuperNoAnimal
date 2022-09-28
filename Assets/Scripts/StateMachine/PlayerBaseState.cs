
using UnityEngine;

public abstract class PlayerBaseState
{
	private bool _isRootState = false;
	private string _name = "Defaut";
	private PlayerStateMachine _ctx;
	private PlayerStateFactory _factory;
	private PlayerBaseState _currentSubState;
	private PlayerBaseState _currentSuperState;

	protected bool IsRootState { set { _isRootState = value; } }
	protected PlayerStateMachine Ctx { get { return _ctx; } }
	protected PlayerStateFactory Factory { get { return _factory; } }

	public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
	{
		_ctx = currentContext;
		_factory = playerStateFactory;
	}
	public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory, string newname)
	{
		_ctx = currentContext;
		_factory = playerStateFactory;
		_name = newname;
	}


	public abstract void EnterState();
	public abstract void UpdateState();
	public abstract void ExitState();
	public abstract void CheckSwithStates();
	public abstract void InitializeSubState();

	public void UpdateStates()
	{
		UpdateState();
		if (_currentSubState != null)
		{
			_currentSubState.UpdateState();
		}
	}

	public void ExitStates()
	{
		ExitState();
		if (_currentSubState != null)
		{
			_currentSubState.ExitState();
		}
	}
	protected void SwitchState(PlayerBaseState newState)
	{
		ExitStates();

		newState.EnterState();

		if (_isRootState)
		{
			_ctx.CurrentState = newState;
		}
		else if (_currentSuperState != null)
		{
			_currentSuperState.SetSubState(newState);
		}
	}
	protected void SetSuperState(PlayerBaseState newSuperState)
	{
		Debug.Log($"Set Super State : " + newSuperState._name);
		_currentSuperState = newSuperState;
	}
	protected void SetSubState(PlayerBaseState newSubState)
	{
		Debug.Log($"Set Sub State : " + newSubState._name);
		_currentSubState = newSubState;
		newSubState.SetSuperState(this);
	}

	protected void SwitchSubState(PlayerBaseState newSubState)
	{
		if (_currentSubState != null)
		{
			_currentSubState.ExitState();
		}
		newSubState.EnterState();
		_currentSubState = newSubState;
		newSubState.SetSuperState(this);

	}
}
