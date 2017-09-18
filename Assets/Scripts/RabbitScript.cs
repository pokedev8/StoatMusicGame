﻿using UnityEngine;
using System.Collections;

public class RabbitScript : MonoBehaviour 
{
	[SerializeField]
	float SPEED = 0.05f;
	[SerializeField]
	float DEATH_DURATION = 4f;

	[Header("Accessor")]
	[SerializeField]
	GameObject m_GameOverTextObject;
	GameOverScript m_GameOverScript;
	Animator m_Animator;

	bool m_BeginRun = false;

	void Start()
	{
		m_GameOverScript = m_GameOverTextObject.GetComponent<GameOverScript>();
		m_Animator = transform.GetChild(0).GetComponent<Animator>();
	}

	void Update()
	{
		if (m_BeginRun)
		{	
			transform.Translate(Vector2.right * Time.deltaTime * SPEED);
		}
	}

	public void RunAway()
	{
		m_Animator.SetTrigger("isRunning");
		m_BeginRun = !m_BeginRun;
		DestroyAfterTime();
		m_GameOverScript.SetLoseTextActive();
	}

	public void Bitten()
	{
		FlashCode flash = gameObject.GetComponentInChildren<FlashCode>();
		flash.SetFlashTimer();
	}

	//Consider if we still want this as it causes some conflicts/null reference exceptions later on once destroyed
	public void DestroyAfterTime()
	{
		Destroy(gameObject, DEATH_DURATION);
	}
}
