using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMahagers : MonoBehaviour
{
	[SerializeField] private bool OnPlayer;
	[SerializeField] private GameObject startingPlayerPosition;

	[SerializeField] public List<PlayerInput> players;
	[SerializeField] public List<ThirdPersonController> thirdPersonControllers;
	[SerializeField] private List<Transform> startingPosition;

	[SerializeField]
	private CinemachineTargetGroup group;

	PlayerInputManager inputManager;

	private void OnValidate()
	{
		//inputManager = GetComponent<PlayerInputManager>();

		if (OnPlayer && startingPlayerPosition.activeSelf == false)
		{
			startingPlayerPosition.SetActive(true);

			group.AddMember(startingPlayerPosition.transform, 1, 2);
		}
		else if (!OnPlayer && startingPlayerPosition.activeSelf == true)
		{
			startingPlayerPosition.SetActive(false);
			group.RemoveMember(startingPlayerPosition.transform);
		}
	}

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
