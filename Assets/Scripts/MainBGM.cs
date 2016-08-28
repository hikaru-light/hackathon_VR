using UnityEngine;
using System.Collections;

public class MainBGM : MonoBehaviour {
	private AudioClip audioSE = null;
	private AudioSource audioSource;

	void Start () {
		audioSource = GetComponent<AudioSource>();
		audioSource.Play ();
	}
}
