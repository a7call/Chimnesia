using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Linq;
using DG.Tweening;

public class EffectManager : Singleton<EffectManager>
{
	public AudioMixerGroup mixerGroup;

	public Sound[] soundFxBank;

	public void Play(string sound, GameObject gameObjectSource, float volumeScale = 1)
	{
		Sound s = Array.Find(soundFxBank, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		if (s.source == null)
		{
			s.source = gameObjectSource.AddComponent(typeof(AudioSource)) as AudioSource;
			s.source.clip = s.clip;
			s.source.loop = s.loop;
			s.source.volume = s.volume;
			s.source.outputAudioMixerGroup = s.mixerGroup;
			s.source.pitch = s.pitch;
		}

		s.source.Play();
	}

	public void Stop(string sound)
    {
		var sources = GetComponents<AudioSource>().ToList();
        sources.ForEach(x =>
        {
			var sou = Array.Find(soundFxBank, item => item.name == sound);
			print(x);
			if (sound == null)
				return;
				
			if (x.clip == sou.clip)
            {
				var volume = x.volume;
				x.DOFade(0, 1).OnComplete(() =>
				{
					x.Stop();
					x.DOFade(volume, 0.1f);
				});
			}

		});


    }
}
