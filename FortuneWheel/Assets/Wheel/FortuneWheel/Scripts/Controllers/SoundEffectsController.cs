#define MYDEBUG
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public enum SoundEffectsTypes
{
    none,

    Click1,
    
    FortuneWheel1,
    FortuneWheel2,

    CongratulationFortuneWheel,

    Music1
}


public class SoundEffectsController : MonoBehaviour
{
    public bool isMusicEnabled
    {
        get
        {
            return Common.GetBoolFromPlayerPrefs("isMusicEnabled", true);
        }
        set
        {
            PlayerPrefs.SetInt("isMusicEnabled", Convert.ToInt32(value));
            if (value == false)
            {
                this.StopMusic(false);
            }
            else
            {
                StartCoroutine(this.StartMusic());
            }
        }
    }

    public bool isEffectsEnabled // by button
    {
        get
        {
            return Common.GetBoolFromPlayerPrefs("isEffectsEnabled", true);
        }
        set
        {
            PlayerPrefs.SetInt("isEffectsEnabled", Convert.ToInt32(value));
            if (value == false)
            {
                this.StopAllEffects();
            } else
            {
                StartCoroutine( this.StartMusic() );
            }
        }
    }

    private bool _isEffectsAllowed;
    public bool isEffectsAllowed // by system
    {
        set
        {
            this._isEffectsAllowed = value;
            UDebug.Log("[SoundEffectsController] [isEffectsAllowed] value = " + value);
            if (!this._isEffectsAllowed)
            {
                this.StopAllEffects();
                this.StopMusic(false);
            }
        }
    }

    private Dictionary<SoundEffectsTypes, AudioClip>        audioClips          = new Dictionary<SoundEffectsTypes, AudioClip>();
    private Dictionary<SoundEffectsTypes, string>           audioClipsPaths     = new Dictionary<SoundEffectsTypes, string>();
    private Dictionary<SoundEffectsTypes, int>              effectsMaxCount     = new Dictionary<SoundEffectsTypes, int>();
    private Dictionary<SoundEffectsTypes, float>            effectsVolume       = new Dictionary<SoundEffectsTypes, float>();
    private Dictionary<SoundEffectsTypes, List<AudioSource>> effectsInstantiated = new Dictionary<SoundEffectsTypes, List<AudioSource>>();
    private Dictionary<SoundEffectsTypes, AudioSource>                 musicInstantiated   = new Dictionary<SoundEffectsTypes, AudioSource>();
    private SoundEffectsTypes currentMusic = SoundEffectsTypes.none;
    //private Queue toStartMusic = new Queue();
    
    //private bool musicAllowed = true;
    private float pausedMusicTime = 0;

    private static SoundEffectsController instance;
    public static SoundEffectsController Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject();
                instance = go.AddComponent<SoundEffectsController>();
                go.name = "SoundEffectsController";
                go.transform.parent = Common.GetControllerHolder();

                instance._isEffectsAllowed = true; // system allows effects, what about button?
                instance.InitSoundEffectsPaths();

                // COUNT
                instance.effectsMaxCount[SoundEffectsTypes.Click1] = 2;

                instance.effectsMaxCount[SoundEffectsTypes.FortuneWheel1] = 3;
                instance.effectsMaxCount[SoundEffectsTypes.FortuneWheel2] = 3;

                instance.effectsMaxCount[SoundEffectsTypes.CongratulationFortuneWheel] = 1;

                instance.effectsMaxCount[SoundEffectsTypes.Music1] = 1;

                // VOLUME
                instance.effectsVolume[SoundEffectsTypes.none] = 0.8f;
                instance.effectsVolume[SoundEffectsTypes.FortuneWheel1] = 0.3f;
                instance.effectsVolume[SoundEffectsTypes.FortuneWheel2] = 0.2f;
            }
            return instance;
        }
    } // SoundEffectsController

    void Awake()
    {
#if MYDEBUG
        UDebug.Log("SoundEffectsController :: Awake");
#endif
    }

    private void InitSoundEffectsPaths()
    {
        this.audioClipsPaths[SoundEffectsTypes.Click1] = "Sound/UI/Click1";

        this.audioClipsPaths[SoundEffectsTypes.FortuneWheel1] = "Sound/UI/FortuneWheel1";
        this.audioClipsPaths[SoundEffectsTypes.FortuneWheel2] = "Sound/UI/FortuneWheel2";

        this.audioClipsPaths[SoundEffectsTypes.CongratulationFortuneWheel] = "Sound/UI/CongratulationFortuneWheel";

        this.audioClipsPaths[SoundEffectsTypes.Music1] = "Sound/BackgroundMusic1";
    } // InitSoundEffectsPaths

    public void AddAudioClip(SoundEffectsTypes type, AudioClip clip)
    {
        this.audioClips[type] = clip;
    }

    private IEnumerator StartMusic()
    {
        UDebug.Log("[StartMusic] " + this.currentMusic);
        if (!this._isEffectsAllowed || !this.isMusicEnabled)
        {
            yield break;
        }
        if (this.currentMusic == SoundEffectsTypes.none)
        {
            yield break;
        }

        UDebug.Log("[StartMusic] this.pausedMusicTime = " + this.pausedMusicTime);
        if (this.musicInstantiated.ContainsKey(this.currentMusic))
        {
            this.musicInstantiated[this.currentMusic].time = this.pausedMusicTime;
            this.musicInstantiated[this.currentMusic].Play();
            this.musicInstantiated[this.currentMusic].mute = false;

            float volume = 1;
            if (this.effectsVolume != null && this.effectsVolume.ContainsKey(this.currentMusic))
            {
                volume = this.effectsVolume[this.currentMusic];
            }
            float stepDelta = volume / 10.0f;
            for (int i = 1; i <= 10; i++)
            {
                this.musicInstantiated[this.currentMusic].volume = i * stepDelta;
                yield return new WaitForSeconds(0.1f);
            }
            this.musicInstantiated[this.currentMusic].volume = volume;
        } else
        {
            SoundEffectsTypes toPlay = this.currentMusic;
            this.currentMusic = SoundEffectsTypes.none;
            this.PlayMusic(toPlay);
        }
    } // StartMusic

    public void StopMusic(bool fade = true)
    {
        this.StopMusicFade(fade);
    }

    public void StopMusic(AudioSource source)
    {
        StartCoroutine(this.StopMusicFade(source));
    }

    private void StopMusicFade(bool fade = true)
    {
#if MYDEBUG
        UDebug.Log("SoundEffectsController :: StopMusicFade");
#endif
        if (this.musicInstantiated.ContainsKey(this.currentMusic))
        {
            this.pausedMusicTime = this.musicInstantiated[this.currentMusic].time;
        }

        UDebug.Log("this.musicInstantiated.Count = " + this.musicInstantiated.Count);
        foreach (KeyValuePair<SoundEffectsTypes, AudioSource> entry in this.musicInstantiated)
        {
#if MYDEBUG
            UDebug.Log("SoundEffectsController :: Stop [" + entry.Key + "]");
#endif
            if (entry.Value.isPlaying)
            {
                if (fade)
                {
                    StartCoroutine(this.StopMusicFade(entry.Value));
                } else
                {
                    entry.Value.Stop();
                    entry.Value.volume = 1.0f;
                }
            }
        }
    }

    private IEnumerator StopMusicFade(AudioSource source)
    {
#if MYDEBUG
        UDebug.Log("SoundEffectsController :: StopMusicFade - source");
#endif
        if (source != null)
        {
            if (source.volume > 0)
            {
                for (int i = 9; i > 0; i--)
                {
                    if (source != null)
                    {
                        source.volume = i*0.1f;
                    }
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
        if (source != null)
        {
            source.Stop();
            source.volume = 1.0f;
        }
        yield return new WaitForEndOfFrame();
    }


    public void PlayMusic(SoundEffectsTypes name, float volume = 100)
    {
#if MYDEBUG
        UDebug.Log("SoundEffectsController :: PlayMusic [name=" + name + "]");
#endif
        if (this._isEffectsAllowed && this.isMusicEnabled)
		{
            StartCoroutine(this.SwitchMusic(name, volume));
        } else
        {
            this.currentMusic = name;
            this.pausedMusicTime = 0;
        }
    } // PlayMusic

    public void SetMusicVolume(float percents)
    {
        if (this.currentMusic == SoundEffectsTypes.none)
        {
            return;
        }
        float volume = 1;
        if (this.effectsVolume != null && this.effectsVolume.ContainsKey(this.currentMusic))
        {
            volume = this.effectsVolume[this.currentMusic];
        }
        volume = volume / 100.0f * percents;
        if (this.musicInstantiated.ContainsKey(this.currentMusic))
        {
            this.musicInstantiated[this.currentMusic].volume = volume;
        }
    } // SetMusicVolume

    public void PlayEffectDelay(SoundEffectsTypes effect, float time)
    {
        if (!this.isEffectsEnabled || !this._isEffectsAllowed)
        {
            return;
        }
        StartCoroutine(this.WaitForEffect(effect, time));
    }

    private IEnumerator WaitForEffect(SoundEffectsTypes effect, float time)
    {
        yield return new WaitForSeconds(time);
        this.PlayEffect(effect, false);
    }

    public void PlayEffect(SoundEffectsTypes effect)
    {
        this.PlayEffect(effect, false);
    }

    public void PlayEffect(SoundEffectsTypes effect, bool loop, bool fadeIn = false)
    {
        if (!this.isEffectsEnabled || !this._isEffectsAllowed)
        {
            return;
        }
        if (!this.effectsInstantiated.ContainsKey(effect))
        {
            this.effectsInstantiated[effect] = new List<AudioSource>();
        }
        // find free AudioSource
        int freeAudioSourceIndex = -1;
        for (int i = 0; i < this.effectsInstantiated[effect].Count; i++)
        {
            if (this.effectsInstantiated[effect][i] != null)
            {
                if (!this.effectsInstantiated[effect][i].isPlaying)
                {
                    freeAudioSourceIndex = i;
                    break;
                }
            }
        } // for

        if (!this.effectsMaxCount.ContainsKey(effect))
        {
            UDebug.LogWarning("SoundEffectsController :: effectsMaxCount need record [" + effect.ToString() + "]");
        }

        if (freeAudioSourceIndex == -1 && this.effectsInstantiated[effect].Count < this.effectsMaxCount[effect])
        {
            // no free AudioSources and AudioSources count < MaxCount
            // Instantiate new AudioSource
            freeAudioSourceIndex = this.CreateNewAudioSourceEffect(effect); // + volume inside
        }

        if (freeAudioSourceIndex != -1)
        {
            // turn ON Effect
            AudioSource aSource = this.effectsInstantiated[effect][freeAudioSourceIndex];
            float volume = 1;
            if (this.effectsVolume != null && this.effectsVolume.ContainsKey(effect))
            {
                volume = this.effectsVolume[effect];
            }
            if (loop)
            {
                aSource.loop = true;
            } else
            {
                aSource.loop = false;
            }
            aSource.pitch = 1;
            aSource.Play();
            if (fadeIn)
            {
                StartCoroutine(this.EffectFadeInIE(aSource, volume));
            }
        }
    } // PlayEffect

    public void PlayEffect(SoundEffectsTypes effect, float volume, float pitch)
    {
        if (!this.isEffectsEnabled || !this._isEffectsAllowed)
        {
            return;
        }
        if (!this.effectsInstantiated.ContainsKey(effect))
        {
            this.effectsInstantiated[effect] = new List<AudioSource>();
        }
        // find free AudioSource
        int freeAudioSourceIndex = -1;
        for (int i = 0; i < this.effectsInstantiated[effect].Count; i++)
        {
            if (this.effectsInstantiated[effect][i] != null)
            {
                if (!this.effectsInstantiated[effect][i].isPlaying)
                {
                    freeAudioSourceIndex = i;
                    break;
                }
            }
        } // for

        if (!this.effectsMaxCount.ContainsKey(effect))
        {
            UDebug.LogWarning("SoundEffectsController :: effectsMaxCount need record [" + effect.ToString() + "]");
        }

        if (freeAudioSourceIndex == -1 && this.effectsInstantiated[effect].Count < this.effectsMaxCount[effect])
        {
            // no free AudioSources and AudioSources count < MaxCount
            // Instantiate new AudioSource
            freeAudioSourceIndex = this.CreateNewAudioSourceEffect(effect); // + volume inside
        }

        if (freeAudioSourceIndex != -1)
        {
            // turn ON Effect
            AudioSource aSource = this.effectsInstantiated[effect][freeAudioSourceIndex];
            aSource.pitch = pitch;
            aSource.volume = volume;
            aSource.loop = false;
            aSource.Play();
            
        }
    } // PlayEffect

    public void StopEffect(SoundEffectsTypes effect)
    {
        UDebug.Log("[SoundEffectsController] [StopEffect] " + effect.ToString());
        if (this.effectsInstantiated.ContainsKey(effect) && this.effectsInstantiated[effect].Count > 0 && this.effectsInstantiated[effect][0] != null)
        {
            for (int i = 0; i < this.effectsInstantiated[effect].Count; i++)
            {
                if (this.effectsInstantiated[effect][i].isPlaying)
                {
                    this.effectsInstantiated[effect][i].Stop();
                }
                else
                {
                    //DebugMy.LogWarning("Effect [" + effect.ToString() + "] not playing (do not need stop)");
                }
            } // for
        }
        else
        {
            //DebugMy.LogWarning("Effect [" + effect.ToString() + "] not found in Instantiated for STOPPING");
        }
    } // StopEffect

    public void StopAllEffects()
    {
        UDebug.Log("[SoundEffectsController] [StopAllEffects]");
        if (this.effectsInstantiated == null)
        {
            return;
        }
        UDebug.Log("[SoundEffectsController] [StopAllEffects] this.effectsInstantiated.Count = " + this.effectsInstantiated.Count);
        foreach (KeyValuePair<SoundEffectsTypes, List<AudioSource>> entry in this.effectsInstantiated)
        {
            for (int i = 0; i < entry.Value.Count; i++)
            {
                if (entry.Value[i].isPlaying)
                {
                    entry.Value[i].Stop();
                }
            }
        }
    } // StopAllEffects

    public void StopEffectFadeOut(SoundEffectsTypes effect)
    {
        UDebug.Log("[SoundEffectsController] [StopEffectFadeOut] " + effect.ToString());
        if (this.effectsInstantiated.ContainsKey(effect) && this.effectsInstantiated[effect].Count > 0 && this.effectsInstantiated[effect][0] != null)
        {
            for (int i = 0; i < this.effectsInstantiated[effect].Count; i++)
            {
                if (this.effectsInstantiated[effect][i].isPlaying)
                {
                    StartCoroutine(this.EffectFadeOutIE(this.effectsInstantiated[effect][i]));
                }
                else
                {
                    //DebugMy.LogWarning("Effect [" + effect.ToString() + "] not playing (do not need stop)");
                }
            } // for
        }
        else
        {
            //DebugMy.LogWarning("Effect [" + effect.ToString() + "] not found in Instantiated for STOPPING");
        }
    }

    private IEnumerator EffectFadeOutIE(AudioSource effectAU)
    {
        int steps = 10;
        float stepDelta = effectAU.volume / (float)steps;
        for (int i = 0; i < steps; i++)
        {
            effectAU.volume -= stepDelta;
            yield return new WaitForSeconds(0.05f);
        }
        effectAU.Stop();
    }

    private IEnumerator EffectFadeInIE(AudioSource effectAU, float targetVolume = 1)
    {
        int steps = 10;
        float stepDelta = targetVolume / (float)steps;
        effectAU.volume = 0;
        for (int i = 0; i < steps; i++)
        {
            effectAU.volume += stepDelta;
            yield return new WaitForSeconds(0.05f);
        }
        effectAU.volume = targetVolume;
    }

    private IEnumerator SwitchMusic(SoundEffectsTypes name, float volume = 100)
    {
#if MYDEBUG
        UDebug.Log("SoundEffectsController :: SwitchMusic [name=" + name + "]");
#endif
        if (this.currentMusic == name)
        {
            float v = 1;
            if (this.effectsVolume != null && this.effectsVolume.ContainsKey(name))
            {
                v = this.effectsVolume[name];
            }
            if (volume != 100)
            {
                v = v / 100.0f * volume;
            }
            this.musicInstantiated[name].volume = v;
            // DO NOT SWITCH
            yield break;
        }

        this.pausedMusicTime = 0;
        if (this.currentMusic != SoundEffectsTypes.none && this.musicInstantiated.ContainsKey(this.currentMusic))
        {
            if (this.musicInstantiated[this.currentMusic].volume > 0 && !this.musicInstantiated[this.currentMusic].mute)
            {
#if MYDEBUG
                UDebug.Log("Disable (fadeout) [" + this.currentMusic + "] volume = " + this.musicInstantiated[this.currentMusic].volume);
#endif
                float stepDelta = this.musicInstantiated[this.currentMusic].volume / 10.0f;
                for (int i = 1; i <= 10; i++)
                {
                    this.musicInstantiated[this.currentMusic].volume -= i * stepDelta;
                    yield return new WaitForSeconds(0.1f);
                }
                this.currentMusic = SoundEffectsTypes.none;
            }
        }
        // stop all music
        foreach (KeyValuePair<SoundEffectsTypes, AudioSource> entry in this.musicInstantiated)
        {
            entry.Value.Stop();
            entry.Value.mute = true;
        }

        // fadein new music
        if (!this.musicInstantiated.ContainsKey(name))
        {
            this.CreateNewAudioSourceMusic(name);
        }

        if (this._isEffectsAllowed && this.musicInstantiated.ContainsKey(name))
        {
            this.musicInstantiated[name].volume = 0;
            this.musicInstantiated[name].mute = false;
            this.musicInstantiated[name].loop = true;
            this.musicInstantiated[name].playOnAwake = false;

            this.musicInstantiated[name].Play();
            float v = 1;
            if (this.effectsVolume != null && this.effectsVolume.ContainsKey(name))
            {
                v = this.effectsVolume[name];
            }
            if (volume != 100)
            {
                v = v / 100.0f * volume;
            }
            float stepDelta = v / 10.0f;
            for (int i = 1; i <= 10; i++)
            {
                this.musicInstantiated[name].volume = i * stepDelta;
                yield return new WaitForSeconds(0.1f);
            }
            this.currentMusic = name;
        }
    } // SwitchMusic

    private int CreateNewAudioSourceEffect(SoundEffectsTypes effect)
    {
        if (this.effectsInstantiated[effect].Count >= this.effectsMaxCount[effect])
        {
            return -1; // limit
        }
        GameObject go = new GameObject();
        go.name = effect.ToString();
        go.transform.parent = transform;
        AudioSource aSource = go.AddComponent<AudioSource>();
        // setup AudioSource

        // add clip
        //MyDebugMy.Log(GVars.soundEffects[effect]);
        aSource.clip = this.GetAudioClip(effect);//GVars.allAudioClips[effect];
        if (this.effectsVolume.ContainsKey(effect))
        {
            aSource.volume = this.effectsVolume[effect];
        }
        this.effectsInstantiated[effect].Add(aSource);
        return this.effectsInstantiated[effect].Count - 1;
    } // CreateNewAudioSourceEffect

    private AudioClip GetAudioClip(SoundEffectsTypes clipName)
    {
        if (this.audioClips.ContainsKey(clipName))
        {
            return this.audioClips[clipName];
        }
        // try to get clip from resources
        if (this.audioClipsPaths.ContainsKey(clipName))
        {
            AudioClip clip = (AudioClip)LinkedResourcesManager.Instance.GetLinkedObjectByFullPath(this.audioClipsPaths[clipName]);
            if (clip != null)
            {
                return clip;
            }
            UDebug.LogError("[SoundEffectsController] [GetAudioClip] cannot get LinkedObject by path '" + this.audioClipsPaths[clipName] + "'");
        }
        UDebug.LogError("[SoundEffectsController] [GetAudioClip] cannot find AudioClip '" + clipName + "'");
        return null;
    } // GetAudioClip

    
    private void CreateNewAudioSourceMusic(SoundEffectsTypes name)
    {
        GameObject go = new GameObject();
        go.name = name.ToString();
        go.transform.parent = transform;
        AudioSource aSource = go.AddComponent<AudioSource>();
        // setup AudioSource

        // add clip
        //MyDebugMy.Log(GVars.soundEffects[effect]);
        aSource.clip = this.GetAudioClip(name);
        this.musicInstantiated[name] = aSource;
    } // CreateNewAudioSourceMusic
    
    
} // SoundEffectsController
