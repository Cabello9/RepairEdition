using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSounds : MonoBehaviour
{
	public AudioManager audioManager;

	public void DoCrash()
	{
		audioManager.PlayCrashSound();
	}

	public void DoGnomeTalk()
	{
		audioManager.PlayGnomeTalkSound();
	}

	public void DoSolidSnake()
	{
		audioManager.PlaySolidSnakeSound();
	}
}
