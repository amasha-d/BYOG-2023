using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
//using Microphone = FrostweepGames.MicrophonePro.Microphone;
//using FrostweepGames.MicrophonePro;

public class PitchAnalyzer : MonoBehaviour
{
    //public AudioMixerGroup mixerGroupMicrophone; // Create an audio mixer group for the microphone
    public float sensitivity = 1000.0f; // Adjust this sensitivity value to fit your needs
    public float loudnessThreshold = 0.1f; // Adjust this threshold value to filter out background noise
    public TextMeshProUGUI pitchText; // Text to display the pitch
    public TextMeshProUGUI deviceNameText; // Text to display the name
    public float smoothingFactor = 0.2f; // Adjust this factor for smoothing (0.0f for no smoothing, 1.0f for no change)

    public float smoothedPitch = 0.0f;

    private AudioSource audioSource;
    private string selectedMicrophone;
    string[] microphones;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(InitializeLoop());
    }

    /*private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        InitializeMicrophone();
    }*/


    IEnumerator InitializeLoop()
    {
        for (int i = 0; i < 7; i++)
        {
            microphones = Microphone.devices;
            yield return new WaitForSeconds(0.1f);
        }
        InitializeMicrophone();

        yield return null;
    }

    void InitializeMicrophone()
    {
        if (microphones.Length > 0)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
			// for webgl we use native audio speaker instead unity audio source due to limitation of unity audio engine
            selectedMicrophone = microphones[0];
            Microphone.Start(Microphone.devices[0], true, 1, 44100, true);
#else
            selectedMicrophone = microphones[0];
            audioSource.clip = Microphone.Start(Microphone.devices[0], true, 1, 44100);
            audioSource.loop = true;
            audioSource.Play();
#endif


            StartCoroutine(UpdateFunc());
        }
        else
        {
            Debug.LogError("No microphone found!");
            deviceNameText.text = "No microphone found!";
        }
    }



        


    IEnumerator UpdateFunc()
    {
        // InitializeMicrophone();
        //microphones = Microphone.devices;
        while (true)
        {
            deviceNameText.text = Microphone.devices[0];

            if (Microphone.IsRecording(Microphone.devices[0]))
            {
                float[] samples = new float[audioSource.clip.samples];
                //float[] samples = new float[128];
                audioSource.clip.GetData(samples, 0);

                float loudness = samples.Select(x => x * x).Sum() / samples.Length;

                if (loudness > loudnessThreshold)
                {
                    float[] spectrum = new float[1024];
                    audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

                    float maxFrequency = 0;
                    float maxSpectrumValue = 0;

                    for (int i = 1; i < spectrum.Length; i++)
                    {
                        if (spectrum[i] > maxSpectrumValue)
                        {
                            maxSpectrumValue = spectrum[i];
                            maxFrequency = i;
                        }
                    }

                    float pitch = maxFrequency * (AudioSettings.outputSampleRate / 2) / spectrum.Length;

                    // Apply exponential moving average filter
                    smoothedPitch = (pitch * smoothingFactor) + (smoothedPitch * (1.0f - smoothingFactor));

                    //Debug.Log("Smoothed Pitch: " + smoothedPitch.ToString("F2") + " Hz");
                    pitchText.text = "Smoothed Pitch: " + smoothedPitch.ToString("F2") + " Hz";

                }
                else
                {
                    smoothedPitch = 0;
                    //Debug.Log("No sound detected");
                    pitchText.text = "No sound detected";
                }
            }
            yield return null;
        }
        
        
    }


}
