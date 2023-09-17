using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;
using TMPro;

using Microphone = FrostweepGames.MicrophonePro.Microphone;
using System.Collections;

//namespace FrostweepGames.MicrophonePro.Examples

public class LeapSync_Example : MonoBehaviour
{
#if !UNITY_WEBGL || UNITY_EDITOR
		public AudioSource _audioSource;
#endif

    public float sensitivity = 1000.0f; // Adjust this sensitivity value to fit your needs
    public float loudnessThreshold = 0.1f; // Adjust this threshold value to filter out background noise
    public TextMeshProUGUI pitchText; // Text to display the pitch
    public TextMeshProUGUI deviceNameText; // Text to display the name
    public float smoothingFactor = 0.2f; // Adjust this factor for smoothing (0.0f for no smoothing, 1.0f for no change)

    public float smoothedPitch = 0.0f;



    public Button startRecord,
					  stopRecord;

		private int _sampleRate = 44100;

		private int _recordingTime = 1;


		private void Start()
		{
        //startRecord.onClick.AddListener(StartRecordHandler);
            StartRecordHandler();
			//stopRecord.onClick.AddListener(StopRecordHandler);
			startRecord.interactable = false;
			stopRecord.interactable = false;

            //Microphone.PermissionChangedEvent += PermissionChangedEvent;
        }

        private void OnDestroy()
        {
            Microphone.PermissionChangedEvent -= PermissionChangedEvent;
        }

        private void PermissionChangedEvent(bool granted)
        {
            Debug.Log($"Permission state changed on: {granted}");
        }

        private void StartRecordHandler()
        {
            if (Microphone.devices.Length == 0)
                return;
#if UNITY_WEBGL && !UNITY_EDITOR
			// for webgl we use native audio speaker instead unity audio source due to limitation of unity audio engine
            Microphone.Start(Microphone.devices[0], true, _recordingTime, _sampleRate, true);
#else
			_audioSource.clip = Microphone.Start(Microphone.devices[0], true, _recordingTime, _sampleRate);
            _audioSource.loop = true;
            _audioSource.Play();
#endif
            startRecord.interactable = false;
			stopRecord.interactable = false;

        StartCoroutine(UpdateFunc());
		}

		private void StopRecordHandler()
		{
			if (Microphone.devices.Length == 0)
				return;

            Microphone.End(Microphone.devices[0]);
#if !UNITY_WEBGL || UNITY_EDITOR
            _audioSource.Stop();
#endif
			startRecord.interactable = true;
			stopRecord.interactable = false;
		}

    IEnumerator UpdateFunc()
    {
        while (Microphone.devices.Length > 0)
        {
            deviceNameText.text = Microphone.devices[0];

            //if (Microphone.devices.Length > 0)
            {
                float[] samples = new float[_audioSource.clip.samples];
                //float[] samples = new float[128];
                _audioSource.clip.GetData(samples, 0);

                float loudness = samples.Select(x => x * x).Sum() / samples.Length;

                if (loudness > loudnessThreshold)
                {
                    float[] spectrum = new float[1024];
                    _audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

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

        deviceNameText.text = "out of while";
        pitchText.text = "out of while";
    }

	}



