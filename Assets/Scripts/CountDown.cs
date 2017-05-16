﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CountDown : MonoBehaviour 
{
	float m_Timer;
	[SerializeField]
	Text m_Text;
	[SerializeField]
	AudioSource m_AudioSource;
	[SerializeField]
	MusicManager m_MusicManager;

	void Start () 
	{
		m_Timer = 3f;
	}

	void Update () 
	{
		m_Timer -= Time.deltaTime;
		m_Text.text = m_Timer.ToString("#");
		if (m_Timer <= 0)
		{
			//Start music
			m_AudioSource.Play();
			m_MusicManager.SetCanInstantiateTrue();
			Destroy(this.gameObject);
		}
	}
}
