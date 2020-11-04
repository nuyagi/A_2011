using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    GameObject TimerObject;
    TimerScript timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = GameObject.Find("TimerObject").GetComponent<TimerScript>();
    }

    public void StartButtonPressed(){
        timer.begin();
    }

    public void PauseButtonPressed(){
        timer.pause();
    }

    public void StopButtonPressed(){
        timer.cease();
    }

    public void SetActiveTime(int activeTime){
        timer.TimePrimo(activeTime);
    }

    public void SetIntervalTime(int intervalTime){
        timer.TimeSecondo(intervalTime);
    }

    public void SetNumberOfSets(int setsNumber){
        timer.repeat(setsNumber);
    }

    // Update is called once per frame
    void Update()
    {
        //timer.Update();
        /*
        if(timer.timerState != 0){
			if ( timer.isActivePhase ){
				if(timer.activeTimeLeft <= 0){
					//Action of phase changing is written in this section
					//audioSource.PlayOneShot(beep);
					timer.isActivePhase = false;
					timer.activeTimeLeft = timer.time1;
					timer.activeTimeText.text = timer.activeTimeLeft.ToString("00");
					goto campaign;
				}
				if(timer.timerState == 1){
					if(Time.timeScale == 0){
						Time.timeScale = 1.0f;
					}
					timer.activeTimeLeft -= Time.deltaTime; 
					timer.activeTimeText.text = timer.activeTimeLeft.ToString("00");
				}
				if(timer.timerState == 2){
					Time.timeScale = 0;
				}
				timer.activeTimeText.text = timer.activeTimeLeft.ToString("00");
			}
			if ( !timer.isActivePhase ){
				if(timer.intervalTimeLeft <= 0){
					//Action of phase changing is written in this section
					//audioSource.PlayOneShot(beep);
					timer.isActivePhase = true;
					--timer.numberOfSetsLeft;
					timer.intervalTimeLeft = timer.time2;
					timer.setsNumberText.text = timer.numberOfSetsLeft.ToString("00");
					timer.intervalTimeText.text = timer.intervalTimeLeft.ToString("00");
					goto campaign;
				}
				if(timer.timerState == 1){
					if(Time.timeScale == 0){
						Time.timeScale = 1.0f;
					}
					timer.intervalTimeLeft -= Time.deltaTime; 
					timer.intervalTimeText.text = timer.intervalTimeLeft.ToString("00");
				}
				if(timer.timerState == 2){
					Time.timeScale = 0;
				}
				timer.intervalTimeText.text = timer.intervalTimeLeft.ToString("00");
			}
		}

campaign:;
		if(timer.numberOfSetsLeft <= 0){
			//Finishing acctions should be written here
			timer.cease();
		}
        */
    }
    
}
