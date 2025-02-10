using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private float lengthOfFade = 2.0f;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioClip[] sfxAudioClips;
    public static AudioManager Instance;

    [System.Serializable]
    public class SceneAudioSetup
    {
        public AudioClip[] audioClips;
    }

    private SceneAudioSetup sceneAudioSetups;

    private AudioSource[] audioSources;
    private bool _introHasPlayed = false, stopMusic = false;

    private void Awake()
    {
        // Make AudioManager a Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }

        sceneAudioSetups ??= new SceneAudioSetup
        {
            audioClips = new AudioClip[]
            {
                LoadAudioClip("Audio/Music/ambient-piano-logo-165357"),
                LoadAudioClip("Audio/Music/classical-piano-by-alkan-chemin-de-fer-112453"),
                LoadAudioClip("Audio/Music/classical-piano-by-chopin-prelude-op-28-no-16-119741"),
                LoadAudioClip("Audio/Music/classical-piano-by-chopin-prelude-op-28-no-22-119782"),
                LoadAudioClip("Audio/Music/classical-piano-by-chopin-sonate-no-02-b-min-op-35-mov-02-119788"),
                LoadAudioClip("Audio/Music/classical-piano-by-chopin-sonate-no-02-b-min-op-35-mov-04-119904"),
                LoadAudioClip("Audio/Music/classical-piano-by-chopin-sonate-op-35-4-satz-119907")
            },
        };
        StartCoroutine(PlayMultipleTracks(sceneAudioSetups.audioClips));
    }
    
    // Get & Set Volume methods
    #region VolumeControl
    public void SetVolume(float linearVolume)
    {
        // Convert linear volume (0-1) to dB
        float dbValue = LinearToDecibel(linearVolume);
        audioMixer.SetFloat("MasterVolume", dbValue);
    } 
    
    public float GetCurrentVolume()
    {
        float dbValue;
        audioMixer.GetFloat("MasterVolume", out dbValue);
        return DecibelToLinear(dbValue); // Return linear value for UI
    } 

    public void SetBkgdVolume(float linearVolume) // Set Background Volume
    {
        // Convert linear volume (0-1) to dB
        float dbValue = LinearToDecibel(linearVolume);
        audioMixer.SetFloat("BackgroundVolume", dbValue);
    }
    
    public float GetBkgdVolume() // Get Background Volume
    {
        float dbValue;
        audioMixer.GetFloat("BackgroundVolume", out dbValue);
        return DecibelToLinear(dbValue); // Return linear value for UI
    }

    public void SetSFXVolume(float linearVolume) // Set SFX Volume
    {
        // Convert linear volume (0-1) to dB
        float dbValue = LinearToDecibel(linearVolume);
        audioMixer.SetFloat("SFXVolume", dbValue);
    }
    
    public float GetSFXVolume() // Set SFX Volume
    {
        float dbValue;
        audioMixer.GetFloat("SFXVolume", out dbValue);
        return DecibelToLinear(dbValue); // Return linear value for UI
    }


    private float LinearToDecibel(float input)
    {
        // Ensure input is within the valid range
        input = Mathf.Clamp(input, 0f, 10f);

        // Apply logarithmic scaling
        float logValue = Mathf.Log(input + 0.01f); // Adding 0.01 to avoid log(0)

        // Scale the logarithmic value to the range -80 to 20
        float minLog = Mathf.Log(1f); // log of 0 + 1
        float maxLog = Mathf.Log(11f); // log of 10 + 1
        float scaledValue = -80f + (logValue - minLog) * (100f) / (maxLog - minLog);

        return scaledValue;
    }

    private float DecibelToLinear(float input)
    {
        if (input <= -80f)
            return 0f; // Silence
       
        // Ensure input is within the valid range
        input = Mathf.Clamp(input, -80f, 20f);

        // Normalize the input to a 0-1 range
        float normalizedInput = (input - (-80f)) / (20f - (-80f));

        // Apply exponential scaling (inverse of logarithm)
        float expValue = Mathf.Exp(normalizedInput * Mathf.Log(11f)) - 1f;

        // Scale the exponential value to the range 0-10
        float scaledValue = expValue;

        return scaledValue;
    }
    #endregion

    private AudioClip LoadAudioClip(string path)
    {
        AudioClip clip = UnityEngine.Resources.Load<AudioClip>(path);
        if (clip == null)
        {
            Debug.LogError(
                $"Failed to load audio clip: {path}. Make sure the file exists in a Resources folder and the path is correct.");
        }
        return clip;
    }

    private void InitializeAudioSources()
    {
        audioSources = new AudioSource[3];
        for (int i = 0; i < 2; i++)  // Background AudioSources
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
            audioSources[i].loop = true;
            audioSources[i].playOnAwake = false;
            audioSources[i].spatialBlend = 1f; // 3D sound
            
            // assign the audioSource to the AudioMixer
            audioSources[i].outputAudioMixerGroup = audioMixer.FindMatchingGroups("Background")[0];
        }
        audioSources[2] = gameObject.AddComponent<AudioSource>();  // SFX AudioSource
        audioSources[2].loop = false;
        audioSources[2].playOnAwake = false;
        // assign the audioSource to the AudioMixer
        audioSources[2].outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
    }

    private IEnumerator PlayMultipleTracks(AudioClip[] clips)
    {
        int index = 0;
        int channel = 0;
        int oldChannel = 0;
        while (!stopMusic)
        {
            float fadeDuration = Time.time + lengthOfFade;
            audioSources[channel].clip = clips[index];
            audioSources[channel].loop = false;
            audioSources[channel].Play();
            yield return new WaitForSeconds(audioSources[0].clip.length - lengthOfFade);
            index = (index + 1) % clips.Length; // Loops through 0 and Length - 1
            oldChannel = channel;
            channel = (channel + 1) % 2; // Flips between 0 and 1
            audioSources[channel].clip = clips[index];
            audioSources[channel].loop = false; 
            audioSources[channel].Play();
            while (fadeDuration > Time.time)
            {
                audioSources[oldChannel].volume = Mathf.Lerp(1.0f, 0.0f, (fadeDuration - Time.time) / lengthOfFade);
                audioSources[channel].volume = Mathf.Lerp(0.0f, 1.0f, (fadeDuration - Time.time) / lengthOfFade);
                yield return null;
                audioSources[oldChannel].Stop();
                audioSources[oldChannel].clip = null;
            }
            audioSources[oldChannel].volume = 0.0f;
            audioSources[channel].volume = 1.0f;
            
            // Optionally, yield return null here to allow other processes to run during loop.
            // This isn't strictly necessary but good practice in some cases.
            yield return null;
            if (!_introHasPlayed)
            {
                _introHasPlayed = true;
                // Remove the first item from the clips array
                clips = clips.Skip(1).ToArray();
                index = 0;
                channel = 0;
            }
        }
    }
    
    public void PlaySFX(int index)
    {
        if (index >= 0 && index < audioSources.Length)
        {
            audioSources[2].clip = sfxAudioClips[index];
            audioSources[2].Play();
        }
    }

    public void StopTheMusic()
    {
        stopMusic = true;
    }
}