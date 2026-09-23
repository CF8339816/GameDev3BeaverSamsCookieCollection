using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

#region coder & project
/// <summary>
/// NSCC GAME2065/4087/Game Development III(B)/Cameron,Jordan
/// Jam 1 :Beaver Sam's Cookie Cruncher
/// team: Chris French, Roman Zhurakhov, Myranda Roy
/// Coder current script: Chris French Second Year NSCC Game Programming 
/// Additions / annotations:
/// 
/// </summary>
#endregion

public class AddAudio: MonoBehaviour
{// Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("audio clip")]
    [SerializeField] public AudioClip cookieNomAudio;// defines clip 
    [SerializeField] public AudioClip BossFightAudio;// defines clip 
    [SerializeField] public AudioClip MenuAudio;// defines clip 
    [SerializeField] public AudioClip GameLossAudio;// defines clip 
    [SerializeField] public AudioClip StageAudio;// defines clip 
    [SerializeField] public AudioClip MiscAudio;
    private AudioSource source;// defines audio source
    private static AudioSource currentlyPlayingSource; //defines any currently playing for audio checks
    [SerializeField] private float startingVolume = .35f;


    [Header("UI Elements")]
    [SerializeField] public Slider volumeSlider ; // defines  the slider ui element being used

    private void Awake()
    {
        GameObject sliderObject = GameObject.Find("Volume");
        if (sliderObject != null)
        {
            volumeSlider = sliderObject.GetComponent<Slider>();
        }

        if (source == null) source = GetComponent<AudioSource>();
        if (source == null) source = gameObject.AddComponent<AudioSource>();

        source.spatialBlend = 0f;
        source.playOnAwake = false;
        source.mute = false;
        source.clip = MiscAudio;
    }
    private void Start()
    {
        StartupAudio();
        OnMenu();
    }
    public void StartupAudio()
    {
        source.volume = startingVolume; // Safely sets 0.35f fallback

        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;

            
            if (volumeSlider.value != 1f && volumeSlider.value != 0f)// If another script already adjusted the slider from 1.0, match it
            {
                source.volume = volumeSlider.value;
            }
            else
            {
                volumeSlider.value = source.volume; // Handshake 0.35f to UI
            }

            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        }
    }
        
    public void OnVolumeChanged(float newVolume)
    {
        if (source != null)  //allose for volumme to be changedc based on slider valuie during play 
        {
            source.volume = newVolume;
        }
    }

    public void OnNom()
    {
        if (volumeSlider != null && source != null)// presets volume per new govenrences above
        {
            source.volume = volumeSlider.value;
        }

        if (source != null && source.clip != null)
        {
            source.PlayOneShot(cookieNomAudio, 1.0f);//plays defined audio clip on ente of collider zone
        }
    }

    public void OnMenu()
    {
        if (volumeSlider != null && source != null)// presets volume per new govenrences above
        {
            source.volume = volumeSlider.value;
        }

        if (source != null && source.clip != null)
        {
            source.PlayOneShot(MenuAudio, 1.0f);//plays defined audio clip on ente of collider zone
        }
    }

    public void OnStage()
    {
        if (volumeSlider != null && source != null)// presets volume per new govenrences above
        {
            source.volume = volumeSlider.value;
        }

        if (source != null && source.clip != null)
        {
            source.PlayOneShot(StageAudio, 1.0f);//plays defined audio clip on ente of collider zone
        }
    }

    public void OnBoss()
    {
        if (volumeSlider != null && source != null)// presets volume per new govenrences above
        {
            source.volume = volumeSlider.value;
        }

        if (source != null && source.clip != null)
        {
            source.PlayOneShot(BossFightAudio, 1.0f);//plays defined audio clip on ente of collider zone
        }
    }


}
