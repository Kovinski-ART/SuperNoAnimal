using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMahagers : MonoBehaviour
{
	[SerializeField] public List<PlayerInput> players;
	[SerializeField] public List<ThirdPersonController> thirdPersonControllers;
	[SerializeField] private List<Transform> startingPosition;

	[SerializeField]
	private CinemachineTargetGroup group;

	PlayerInputManager inputManager;
	// Start is called before the first frame update
	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	private void Awake()
	{
		inputManager = GetComponent<PlayerInputManager>();
	}

	// Update is called once per frame
	void Update()
	{


	}

	private void AddPlayer(PlayerInput player)
	{
		players.Add(player);
		var tpcont = player.GetComponent<ThirdPersonController>();
		thirdPersonControllers.Add(tpcont);
		tpcont.MoveStaert(startingPosition[players.Count - 1].position);

		group.AddMember(tpcont.CinemachineCameraTarget.transform, 1, 2);
		//Transform playerParent = ;
		//player.transform.Translate(startingPosition[players.Count - 1].position);
		//Debug.Log("playerParent " + " = " + startingPosition[players.Count - 1].position);
	}

	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	private void OnEnable()
	{
		inputManager.onPlayerJoined += AddPlayer;
	}

	/// <summary>
	/// This function is called when the behaviour becomes disabled or inactive.
	/// </summary>
	private void OnDisable()
	{
		inputManager.onPlayerJoined -= AddPlayer;
	}
}
