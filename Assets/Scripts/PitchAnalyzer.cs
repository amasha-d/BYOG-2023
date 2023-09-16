using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
//using Microphone = FrostweepGames.MicrophonePro.Microphone;

public class PitchAnalyzer : MonoBehaviour
{
    public AudioMixerGroup mixerGroupMicrophone; // Create an audio mixer group for the microphone
    public float sensitivity = 1000.0f; // Adjust this sensitivity value to fit your needs
    public float loudnessThreshold = 0.1f; // Adjust this threshold value to filter out background noise
    //public Text pitchText; // Text to display the pitch
    public float smoothingFactor = 0.2f; // Adjust this factor for smoothing (0.0f for no smoothing, 1.0f for no change)

    public float smoothedPitch = 0.0f;

    private AudioSource audioSource;
    private string selectedMicrophone;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        InitializeMicrophone();
    }




    void InitializeMicrophone()
    {
        string[] microphones = Microphone.devices;

        if (microphones.Length > 0)
        {
            selectedMicrophone = microphones[0]; // Choose the first available microphone
            audioSource.outputAudioMixerGroup = mixerGroupMicrophone;
            audioSource.clip = Microphone.Start(selectedMicrophone, true, 1, AudioSettings.outputSampleRate);
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("No microphone found!");
        }
    }



        


    void Update()
    {
        if (Microphone.IsRecording(selectedMicrophone))
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

                Debug.Log("Smoothed Pitch: " + smoothedPitch.ToString("F2") + " Hz");
            }
            else
            {
                smoothedPitch = 0;
                Debug.Log("No sound detected");
            }
        }
    }


}
