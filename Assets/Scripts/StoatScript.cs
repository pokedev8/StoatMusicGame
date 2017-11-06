﻿using UnityEngine;
using System.Collections;

public class StoatScript : MonoBehaviour 
{
	[Header ("Stoat Attributes")]
	[SerializeField]
	float m_MoveAmount = 1f;
	[SerializeField]
	float m_MoveDuration = 4f;
	float DELAY_BITE_DURATION = 2.5f;
	Vector2 m_StartPos;
	Vector2 m_CurrentPos;
	Vector2 m_EndPos;

	[Header ("Accessor")]
	[SerializeField]
	GameObject m_MusicManagerObject;
	Animator m_Animator;
	[SerializeField]
	RabbitScript m_RabbitScript;
	[SerializeField]
	GameObject m_RabbitPrefab;
	GameObject m_CurrentRabbit;
	TouchPanel m_TouchPanel;
	GameManager m_GameManager;

	[Header ("Stoat Move Attributes")]
	[SerializeField]
	float[] m_Level01MoveTimeArr = new float[]{32f, 68f};
	Timer m_StoatTimer = new Timer();
	int m_TotalStepTowardsRabbit = 0;

	void Start()
	{
		m_CurrentPos = transform.position;
		m_StartPos = m_CurrentPos;
		m_EndPos = transform.position;
		m_Animator = gameObject.transform.GetChild(0).GetComponent<Animator>();	
		FindRabbit();
		m_TouchPanel = GameObject.FindGameObjectWithTag("TouchPanel").GetComponent<TouchPanel>();
		m_GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}

	void Update()
	{
		//Timer is ticking, move closer to rabbit as long as the current array counter is less than 2 and if music time is less than the value in the 
		//m_Level01TimerArray[ ] , then set the lerp position and increment m_ArrayCounter
		m_StoatTimer.Update(Time.deltaTime);
		MoveStoat();
		if (m_TotalStepTowardsRabbit < 2)
		{
			if (MusicManager.instance.GetCurrentMusicTime() > m_Level01MoveTimeArr[m_TotalStepTowardsRabbit])
			{
				SetLerpPositions();
				m_TotalStepTowardsRabbit++;
			}
		}

		if (!m_CurrentRabbit && !LevelManager.instance.IsGameOver())
		{
			FindRabbit();
		}
	}

	public void MoveStoat() 
	{
		transform.position = Vector2.Lerp(m_CurrentPos, m_EndPos, (m_MoveDuration - m_StoatTimer.GetTimer()) / m_MoveDuration);
	}

	public void SetLerpPositions()
	{
		m_CurrentPos = transform.position;
		m_EndPos = new Vector2(transform.position.x + m_MoveAmount, transform.position.y);
		m_StoatTimer.SetTimer(m_MoveDuration);
	}

	IEnumerator Bite()
	{
		yield return new WaitForSeconds(DELAY_BITE_DURATION);
		m_Animator.SetTrigger("isBiting");
		yield return new WaitForSeconds(0.5f);
		m_RabbitScript.Bitten();
	}

	public void FindRabbit()
	{
		if (m_CurrentRabbit == null)
		{
			m_CurrentRabbit = GameObject.FindGameObjectWithTag("Rabbit");
			m_RabbitScript = m_CurrentRabbit.GetComponentInChildren<RabbitScript>();
		}
		else
		{
			print("can't find rabbit yet");
		}
	}

	public void EndOfLevel()
	{
		SetLerpPositions();
		StartCoroutine("Bite");
	}

	public void ReturnStartLerpPos()
	{
		m_CurrentPos = transform.position;
		m_EndPos = new Vector2(m_StartPos.x, m_StartPos.y);
		m_StoatTimer.SetTimer(m_MoveDuration);
	}

	public void StopMoving()
	{
		m_Animator.SetTrigger("isStop");
	}
}