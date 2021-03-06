﻿using UnityEngine;
using System.Collections;

public class RabbitScript : MonoBehaviour 
{
	[Header("Rabbit Attributes")]
	[SerializeField]
	float SPEED = 2f;
	[SerializeField]
	float DEATH_DURATION = 4f;
	bool m_BeginRun = false;

	[Header("Accessor")]
	GameOverScript m_GameOverScript;
	Animator m_Animator;

	void Start()
	{
		m_Animator = transform.GetChild(0).GetComponent<Animator>();
		IsEnteringScene(true);
		m_GameOverScript = GameManager.instance.ReturnGameOverScript();
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
		GameManager.instance.OnLoseLevel();
	}

	public void Bitten()
	{
		FlashCode flash = gameObject.GetComponentInChildren<FlashCode>();
		flash.SetFlashTimer();
		DestroyAfterTime();
	}

	//Consider if we still want this as it causes some conflicts/null reference exceptions later on once destroyed
	public void DestroyAfterTime()
	{
		Invoke("IncreaseLevelCount",4.0f);
		MusicManager.instance.FadeOutMusic();
		Destroy(gameObject, DEATH_DURATION);
	}

	void IncreaseLevelCount()
	{
		if (!GameManager.instance.IsGameOver())
		{
			LevelManager.instance.IncrementLevel();
			GameManager.instance.SetBackgroundLerpPos();
		}
	}

	public void IsEnteringScene(bool choice)
	{
		m_Animator.SetBool("isEntering", choice);
	}
}
