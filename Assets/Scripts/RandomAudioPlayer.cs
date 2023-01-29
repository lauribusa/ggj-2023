using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomAudioPlayer : MonoBehaviour
{
	#region Exposed

	[SerializeField]
	private AudioClip[] _audioClips;
	
	#endregion


	#region Unity API
	
    private void Awake() => _audioSource = GetComponent<AudioSource>();

    #endregion


    #region Utils

    public void Play()
    {
        int randomInt = Random.Range(0, _audioClips.Length);
        _audioSource.PlayOneShot(_audioClips[randomInt]);
    }

	#endregion


	#region Private and Protected

	private AudioSource _audioSource;
	
	#endregion
}