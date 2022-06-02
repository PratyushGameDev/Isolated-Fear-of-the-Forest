using TMPro;
using UnityEngine;

public class Night1 : MonoBehaviour
{
    [SerializeField] bool aNOn = true;
    Animator aN;
    AudioSource aS;
    [SerializeField] GameObject monster;
    [SerializeField] GameObject night1Instructions;
    [SerializeField] public GameObject doomSFXEmpty;
    [SerializeField] public GameObject fadeOut;
    [SerializeField] GameObject fadeOutDeathObject;
    [SerializeField] GameObject bloodOverlay;
    [SerializeField] GameObject radio;
    [SerializeField] GameObject canvas;
    [SerializeField] AudioClip rustle;
    [SerializeField] AudioClip radioSFX;
    [SerializeField] public AudioClip doomSFX;
    [SerializeField] AudioClip loseSFX;
    [SerializeField] public AudioClip winSFX;
    [SerializeField] AudioClip alarmBeep;
    [SerializeField] GameObject timer;
    [SerializeField] GameObject rustleEmpty;
    [SerializeField] GameObject rustleEmpty2;
    [SerializeField] GameObject rustleEmpty3;
    [SerializeField] GameObject rustleEmpty4;
    [SerializeField] GameObject rustleEmpty5;
    [SerializeField] GameObject rustleEmpty6;
    [SerializeField] GameObject rustleEmpty7;
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject loseScreen;
    Vector3 monsterPos;
    int rustle1;
    int rustle2;
    int rustle3;
    int rustle4;
    int rustle5;
    int rustle6;
    int rustle7;
    float time = 0f;
    bool hasAvoidedJumpscareRustle;
    bool makeSureHasAvoidedDoesntTurnFalse = false;
    bool hasSeenNight1Instructions = false;
    bool shouldStartTimer = false;
    [SerializeField] GameObject audioTrackingBeginningTimeline;
    [SerializeField] GameObject car;
    [SerializeField] public bool shouldSkipIntro = false;
    GameControl gC;    

    // Start is called before the first frame update
    void Start()
    {
        aS = GetComponent<AudioSource>();
        aN = GetComponent<Animator>();
        if (shouldSkipIntro == false)
        {
            rustle1 = Random.Range(10 + 30, 30 + 16);
            rustle2 = Random.Range(22 + 30, 30 + 51);
            rustle3 = Random.Range(60 + 30, 30 + 78);
            rustle4 = Random.Range(85 + 30, 30 + 94);
            rustle5 = Random.Range(96 + 30, 30 + 106);
            rustle6 = Random.Range(110 + 30, 30 + 115);
            rustle7 = Random.Range(116 + 30, 30 + 120);
        }                
        Debug.Log(rustle1);
        Debug.Log(rustle2);
        Debug.Log(rustle3);
        Debug.Log(rustle4);
        Debug.Log(rustle5);
        Debug.Log(rustle6);
        Debug.Log(rustle7);
        gC = FindObjectOfType<GameControl>();
    }

    void DebugKeys()
    {
        if (shouldSkipIntro == true)
        {            
            aN.ForceStateNormalizedTime(1);
            audioTrackingBeginningTimeline.SetActive(false);
            fadeOut.GetComponent<Animator>().ForceStateNormalizedTime(1);
            car.SetActive(false);            
            rustle1 = Random.Range(10, 16);
            rustle2 = Random.Range(22, 51);
            rustle3 = Random.Range(60, 78);
            rustle4 = Random.Range(85, 94);
            rustle5 = Random.Range(96, 106);
            rustle6 = Random.Range(110, 115);
            rustle7 = Random.Range(116, 120);          
        }        
    }   

    // Update is called once per frame
    void Update()
    {
        EnablePlayerMovementAfterCutscene();
        RustleTest();
        RunTime();
        Debug.Log(makeSureHasAvoidedDoesntTurnFalse);
        if (loseScreen.active)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if (winScreen.active)
        {
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("win screen should be active");
            aN.enabled = true;
            aN.Play("DeathAnimNight1", 0);
        }
    }

    private void RunTime()
    {
        timer.GetComponent<Timer>().enabled = true;
        if (night1Instructions.activeInHierarchy && night1Instructions != null)
        {
            Time.timeScale = 0;
        }
        if (shouldStartTimer == true && this.gameObject.GetComponent<CapsuleCollider>().enabled == true) 
        {
            timer.SetActive(true);
            if (shouldSkipIntro == false)
            {
                float timeNeeded = Time.timeSinceLevelLoad - 30;
                timer.GetComponent<TMP_Text>().text = timeNeeded.ToString();
                //CHECK HERE IF FUTURE ME SEES A BUG IN JUMPSCARE TIMER WHILE PLAYTESTING 
                if (Time.timeSinceLevelLoad > 150)
                {
                    if (!aS.isPlaying)
                    {
                        timer.GetComponent<TMP_Text>().text = "2:00";
                        aS.Stop();
                        aS.PlayOneShot(alarmBeep);
                        Invoke(nameof(StopAudioWithEpsilon), 3);
                        Invoke(nameof(Jumpscare), 4f);
                    }
                    bloodOverlay.SetActive(true);
                    Invoke(nameof(TurnFadeOutBackOffLose), 1f);
                    Invoke(nameof(TurnOnLoseScreen), 1f);
                    doomSFXEmpty.GetComponent<AudioSource>().PlayOneShot(doomSFX);
                    this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
                }
            }  
        }
        
    }

    void StopAudioWithEpsilon()
    {
        aS.Stop();
    }

    void RustleTest()
    {
        time += Time.timeSinceLevelLoad;
        if (time >= rustle1 && time <= rustle1 + 4)
        {
            hasAvoidedJumpscareRustle = false;
            Debug.Log("should rustle now");
            AvoidJumpscare();
            Invoke(nameof(TurnBoolTrueToPrepareForNextJumpscare), 5f);
            aS.volume = 0.05f;
            if (!rustleEmpty.GetComponent<AudioSource>().isPlaying && makeSureHasAvoidedDoesntTurnFalse == false)
            {
                if (!rustleEmpty.GetComponent<AudioSource>().isPlaying)
                {
                    rustleEmpty.GetComponent<AudioSource>().PlayOneShot(rustle);
                }
            }
        }
        if (time >= rustle2 && time <= rustle2 + 4)
        {
            hasAvoidedJumpscareRustle = false;
            AvoidJumpscare();
            Invoke(nameof(TurnBoolTrueToPrepareForNextJumpscare), 5f);
            aS.volume = 0.05f;
            if (!rustleEmpty2.GetComponent<AudioSource>().isPlaying && makeSureHasAvoidedDoesntTurnFalse == false)
            {
                if (!rustleEmpty2.GetComponent<AudioSource>().isPlaying)
                {
                    rustleEmpty2.GetComponent<AudioSource>().PlayOneShot(rustle);
                }
            }
        }
        if (time >= rustle3 && time <= rustle3 + 4)
        {
            hasAvoidedJumpscareRustle = false;
            AvoidJumpscare();
            Invoke(nameof(TurnBoolTrueToPrepareForNextJumpscare), 5f);
            aS.volume = 0.05f;
            if (!rustleEmpty3.GetComponent<AudioSource>().isPlaying && makeSureHasAvoidedDoesntTurnFalse == false)
            {
                if (!rustleEmpty3.GetComponent<AudioSource>().isPlaying)
                {
                    rustleEmpty3.GetComponent<AudioSource>().PlayOneShot(rustle);
                }
            }
        }
        if (time >= rustle4 && time <= rustle4 + 4)
        {
            hasAvoidedJumpscareRustle = false;
            AvoidJumpscare();
            Invoke(nameof(TurnBoolTrueToPrepareForNextJumpscare), 5f);
            aS.volume = 0.05f;
            if (!rustleEmpty4.GetComponent<AudioSource>().isPlaying && makeSureHasAvoidedDoesntTurnFalse == false)
            {
                if (!rustleEmpty4.GetComponent<AudioSource>().isPlaying)
                {
                    rustleEmpty4.GetComponent<AudioSource>().PlayOneShot(rustle);
                }
            }
        }
        if (time >= rustle5 && time <= rustle5 + 4)
        {
            hasAvoidedJumpscareRustle = false;
            AvoidJumpscare();
            Invoke(nameof(TurnBoolTrueToPrepareForNextJumpscare), 5f);
            aS.volume = 0.05f;
            if (!rustleEmpty5.GetComponent<AudioSource>().isPlaying && makeSureHasAvoidedDoesntTurnFalse == false)
            {
                if (!rustleEmpty5.GetComponent<AudioSource>().isPlaying)
                {
                    rustleEmpty5.GetComponent<AudioSource>().PlayOneShot(rustle);
                }
            }
        }
        if (time >= rustle6 && time <= rustle6 + 4)
        {
            hasAvoidedJumpscareRustle = false;
            AvoidJumpscare();
            Invoke(nameof(TurnBoolTrueToPrepareForNextJumpscare), 5f);
            aS.volume = 0.05f;
            if (!rustleEmpty6.GetComponent<AudioSource>().isPlaying && makeSureHasAvoidedDoesntTurnFalse == false)
            {
                if (!rustleEmpty6.GetComponent<AudioSource>().isPlaying)
                {
                    rustleEmpty6.GetComponent<AudioSource>().PlayOneShot(rustle);
                }
            }
        }
        if (time >= rustle7 && time <= rustle7 + 4)
        {
            hasAvoidedJumpscareRustle = false;
            AvoidJumpscare();
            Invoke(nameof(TurnBoolTrueToPrepareForNextJumpscare), 5f);
            aS.volume = 0.05f;
            if (!rustleEmpty7.GetComponent<AudioSource>().isPlaying && makeSureHasAvoidedDoesntTurnFalse == false)
            {
                if (!rustleEmpty7.GetComponent<AudioSource>().isPlaying)
                {
                    rustleEmpty7.GetComponent<AudioSource>().PlayOneShot(rustle);
                }
            }
        }
    }

    void TurnBoolTrueToPrepareForNextJumpscare()
    {
        hasAvoidedJumpscareRustle = true;
        makeSureHasAvoidedDoesntTurnFalse = false;
    }

    void AvoidJumpscare()
    {
        if (makeSureHasAvoidedDoesntTurnFalse == false)
        {            
            Debug.Log("played rustle");
            if (Input.GetKeyDown(KeyCode.Mouse1) && makeSureHasAvoidedDoesntTurnFalse == false)
            {
                hasAvoidedJumpscareRustle = true;
                makeSureHasAvoidedDoesntTurnFalse = true;
                
                Debug.Log("averted jumpscare?");
                if (!radio.GetComponent<AudioSource>().isPlaying)
                {
                    //radio.GetComponent<AudioSource>().Stop();
                    radio.GetComponent<AudioSource>().PlayOneShot(radioSFX);
                }
                else if (radio.GetComponent<AudioSource>().isPlaying)
                {
                    radio.GetComponent<AudioSource>().Stop();
                    radio.GetComponent<AudioSource>().PlayOneShot(radioSFX);
                }
                return;
            }
            if (makeSureHasAvoidedDoesntTurnFalse == true)
            {
                Invoke(nameof(TurnOffAudioSourcesAfterAvoidingJumpscare), 4.1f);
            }
            Invoke(nameof(TurnOffRustleAfterPlayedOnce), 1.2f);
        }
        Invoke(nameof(JumpscareAfterNotPlayingAudio), 4f);
    }

    void TurnOffAudioSourcesAfterAvoidingJumpscare()
    {
        //rustleEmpty.GetComponent<AudioSource>().enabled = false;
        radio.GetComponent<AudioSource>().enabled = false;
        Invoke(nameof(TurnOnAudiosourcesAfterAvoidedJumpscare), 1f);
    }
    void TurnOnAudiosourcesAfterAvoidedJumpscare()
    {
        //rustleEmpty.GetComponent<AudioSource>().enabled = true;
        radio.GetComponent<AudioSource>().enabled = true;
    }

    void JumpscareAfterNotPlayingAudio()
    {
        if (makeSureHasAvoidedDoesntTurnFalse == true)
        {
            if (hasAvoidedJumpscareRustle == false)
            {
                hasAvoidedJumpscareRustle = true;
            }
        }        
        if (hasAvoidedJumpscareRustle == false)
        {
            Jumpscare();
            bloodOverlay.SetActive(true);
            Invoke(nameof(TurnFadeOutBackOffLose), 1f);
            Invoke(nameof(TurnOnLoseScreen), 1f);
            doomSFXEmpty.GetComponent<AudioSource>().PlayOneShot(doomSFX);
            this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
    }

    void TurnOffRustleAfterPlayedOnce()
    {
        rustleEmpty.GetComponent<AudioSource>().enabled = false;
    }

    void EnablePlayerMovementAfterCutscene()
    {
        if (aNOn == false)
        {
            aN.enabled = false;
            if (night1Instructions != null)
            {
                night1Instructions.SetActive(true);
            }            
        }
        else if (aNOn == true)
        {
            aN.enabled = true;
        }
        if (night1Instructions != null)
        {
            Invoke(nameof(PlayerHasSeenInstructions), 5f);
            NightOneInstructionsOff();
            shouldStartTimer = true;
            if (!aS.isPlaying && night1Instructions.active == true)
            {
                aS.Stop();
                Invoke(nameof(PlayDoomSFX), 0.1f);
                Invoke(nameof(StopMusic), 7);
            }
            if (Input.GetKeyDown(KeyCode.Mouse1) && night1Instructions != null)
            {
                Destroy(night1Instructions);
                Time.timeScale = 1;
            }
        }
    }

    private void PlayerHasSeenInstructions()
    {
        hasSeenNight1Instructions = true;
    }

    void NightOneInstructionsOff()
    {
        if (hasSeenNight1Instructions == true)
        {
            Invoke(nameof(PlayGame), .1f);
        }
    }

    void PlayGame()
    {
        //canvas.SetActive(false);
        shouldStartTimer = true;
    }

    void PlayDoomSFX()
    {
        if (!doomSFXEmpty.GetComponent<AudioSource>().isPlaying)
        {
            doomSFXEmpty.GetComponent<AudioSource>().PlayOneShot(doomSFX);
        }        
    }

    void StopMusic()
    {
        doomSFXEmpty.SetActive(false);
    }

    void OnCollisionEnter(Collision other)
    {
        CheckIfOnTrail(other);
        if (other.gameObject.tag == "House")
        {
            Debug.Log("youve won");
            aS.Stop();
            this.gameObject.GetComponent<Rigidbody>().useGravity = false;
            this.gameObject.GetComponent<GameControl>().enabled = false;
            this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            Destroy(monster);
            gC.walkingEmpty.SetActive(false);
            fadeOut.GetComponent<Animator>().Play("DeathFadeOut", 0);
            rustle1 = 500;
            rustle2 = 500;
            rustle3 = 500;
            rustle4 = 500;
            rustle5 = 500;
            rustle6 = 500;
            rustle7 = 500;
            Invoke(nameof(TurnFadeOutBackOffWin), 1f);
            timer.SetActive(false);
            this.gameObject.GetComponent<Animator>().enabled = true;
            aN.Play("TurnOffControlToEnableCursor", 0);
            doomSFXEmpty.GetComponent<AudioSource>().PlayOneShot(doomSFX);
        }
    }

    private void TurnFadeOutBackOffWin()
    {
        fadeOut.SetActive(false);
        winScreen.SetActive(true);
        if (!aS.isPlaying)
        {
            aS.PlayOneShot(winSFX);
        }
    }

    void CheckIfOnTrail(Collision other)
    {
        if (other.gameObject.tag != "Trail" && other.gameObject.tag != "House")
        {
            //isOnTrail = false;            
            Jumpscare();
            bloodOverlay.SetActive(true);
            Invoke(nameof(TurnFadeOutBackOffLose), 1f);
            Invoke(nameof(TurnOnLoseScreen), 1f);
            doomSFXEmpty.GetComponent<AudioSource>().PlayOneShot(doomSFX);
            this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
    }

    private void TurnOnLoseScreen()
    {
        loseScreen.SetActive(true);
    }

    void TurnFadeOutBackOffLose()
    {
        fadeOut.SetActive(false);
    }

    void Jumpscare()
    {
        //monsterPos = monster.gameObject.transform.position;
        aNOn = true;
        GetComponent<Animator>().enabled = true;
        aN.enabled = true;
        Debug.Log("dead");
        JumpscareMonster();
        aN.Play("DeathAnimNight1", 0);
        this.GetComponent<Rigidbody>().isKinematic = true;
        timer.SetActive(false);
        gC.walkingEmpty.SetActive(false);
        rustle1 = 500;
        rustle2 = 500;
        rustle3 = 500;
        rustle4 = 500;
        rustle5 = 500;
        rustle6 = 500;
        rustle7 = 500;
        gC.flashlight.SetActive(false);
        fadeOutDeathObject.SetActive(true);
        Invoke(nameof(TurnOffFadeOutDeathAfterEnabling), 1f);
        if (!aS.isPlaying)
        {
            aS.PlayOneShot(loseSFX);
        }
    }

    void JumpscareMonster()
    {
        monster.SetActive(true);
        //monster.gameObject.transform.position = monsterPos;
    }
    void TurnOffFadeOutDeathAfterEnabling()
    {
        Destroy(fadeOutDeathObject);
        loseScreen.SetActive(true);
    }
}
