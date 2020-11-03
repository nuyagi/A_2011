using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;

public class timer : MonoBehaviour
{
 public Text activeTimeText, intervalTimeText, setsNumberText;
 public float activeTimeInput, intervalTimeInput;
 public float time1, time2, activeTimeLeft, intervalTimeLeft;
 public int timerState = 0,num, setNumberInput, numout;
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
		numout = num;
		setsNumberText.text = numout.ToString("00");
	}
		
	

    void Update()
    {
		if(timerState != 0){
			if ( isActivePhase ){
				if(activeTimeLeft <= 0){
					//Action of phase changing is written in this section
					audioSource.PlayOneShot(beep);
					//EditorApplication.Beep();
					isActivePhase = false;
					activeTimeLeft = time1;
					activeTimeText.text = activeTimeLeft.ToString("00");
					goto campaign;
				}
				if(timerState == 1){
					activeTimeLeft -= Time.deltaTime; 
				}
				if(timerState == 2){
					while(timerState == 2){
						if(timerState != 2){
							break;
						}
					}
				}
				activeTimeText.text = activeTimeLeft.ToString("00");
			}
			if ( !isActivePhase ){
				if(intervalTimeLeft <= 0){
					//Action of phase changing is written in this section
					audioSource.PlayOneShot(beep);
					//EditorApplication.Beep();
					isActivePhase = true;
					--numout;
					intervalTimeLeft = time2;
					setsNumberText.text = numout.ToString("00");
					intervalTimeText.text = intervalTimeLeft.ToString("00");
					goto campaign;
				}
				if(timerState == 1){
					intervalTimeLeft -= Time.deltaTime; 
				}
				if(timerState == 2){
					while(timerState == 2){
						if(timerState != 2){
							break;
						}
					}
				}
				intervalTimeText.text = intervalTimeLeft.ToString("00");
			}
		}

campaign:;
		if(numout <= 0){
			//Finishing acctions should be written here
			cease();
		}
    }

    public void begin(){
		timerState = 1;
		isActivePhase = true;
    }

    public void cease(){
		timerState = 0;
		numout = num;
		activeTimeLeft = time1;
		intervalTimeLeft = time2;
		setsNumberText.text = numout.ToString("00");
		activeTimeText.text = activeTimeLeft.ToString("00");
		intervalTimeText.text = intervalTimeLeft.ToString("00");
	}

    public void pause(){
		if(timerState == 2){timerState = 1;}else{timerState = 2;}
    }
}
