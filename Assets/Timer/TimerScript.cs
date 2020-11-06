using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;

public class TimerScript : MonoBehaviour
{
 public Text activeTimeText, intervalTimeText, setsNumberText;
 public float activeTimeInput, intervalTimeInput;
 public float time1, time2, activeTimeLeft, intervalTimeLeft;
 public int timerState = 0,num, setNumberInput, numberOfSetsLeft;
 public bool isActivePhase = true;
 public AudioClip beep;
 AudioSource audioSource;

    void Start()
    {
		activeTimeText.text = "30";
		intervalTimeText.text = "10";
		setsNumberText.text = "3";
		time1 = 30;
		time2 = 10;
		activeTimeLeft = 30;
		intervalTimeLeft = 10;
		num = 3;
		setNumberInput	= 3;
		
    }

	public void TimePrimo(int activeTimeInput){
		activeTimeLeft = activeTimeInput;
		time1 = activeTimeInput;
		activeTimeText.text = activeTimeLeft.ToString("00");
	}

	public void TimeSecondo(int intervalTimeInput){
		intervalTimeLeft = intervalTimeInput;
		time2 = intervalTimeInput;
		intervalTimeText.text = intervalTimeLeft.ToString("00");
	}
	
	public void repeat(int setNumberInput){
		num = setNumberInput;
		numberOfSetsLeft = num;
		setsNumberText.text = numberOfSetsLeft.ToString("00");
	}

	public void begin(){
		timerState = 1;
		isActivePhase = true;
    }

    public void cease(){
		timerState = 0;
		numberOfSetsLeft = num;
		activeTimeLeft = time1;
		intervalTimeLeft = time2;
		setsNumberText.text = numberOfSetsLeft.ToString("00");
		activeTimeText.text = activeTimeLeft.ToString("00");
		intervalTimeText.text = intervalTimeLeft.ToString("00");
	}

    public void pause(){
		if(timerState == 2){timerState = 1;}
		else if(timerState == 1){timerState = 2;}
		else{timerState = 0;}
    }
	
	

    void Update()
    {
		/*
		if(timerState != 0){
			if ( isActivePhase ){
				if(activeTimeLeft <= 0){
					//Action of phase changing is written in this section
					//audioSource.PlayOneShot(beep);
					isActivePhase = false;
					activeTimeLeft = time1;
					activeTimeText.text = activeTimeLeft.ToString("00");
					goto campaign;
				}
				if(timerState == 1){
					if(Time.timeScale == 0){
						Time.timeScale = 1.0f;
					}
					activeTimeLeft -= Time.deltaTime; 
					activeTimeText.text = activeTimeLeft.ToString("00");
				}
				if(timerState == 2){
					Time.timeScale = 0;
				}
				activeTimeText.text = activeTimeLeft.ToString("00");
			}
			if ( !isActivePhase ){
				if(intervalTimeLeft <= 0){
					//Action of phase changing is written in this section
					//audioSource.PlayOneShot(beep);
					isActivePhase = true;
					--numberOfSetsLeft;
					intervalTimeLeft = time2;
					setsNumberText.text = numberOfSetsLeft.ToString("00");
					intervalTimeText.text = intervalTimeLeft.ToString("00");
					goto campaign;
				}
				if(timerState == 1){
					if(Time.timeScale == 0){
						Time.timeScale = 1.0f;
					}
					intervalTimeLeft -= Time.deltaTime; 
					intervalTimeText.text = intervalTimeLeft.ToString("00");
				}
				if(timerState == 2){
					Time.timeScale = 0;
				}
				intervalTimeText.text = intervalTimeLeft.ToString("00");
			}
		}

campaign:;
		if(numberOfSetsLeft <= 0){
			//Finishing acctions should be written here
			cease();
		}
		*/
    }
	

    
}
