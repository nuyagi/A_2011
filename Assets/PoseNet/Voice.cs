using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/////消していい//////////

public class Voice : MonoBehaviour {

public AudioClip sound1;
AudioSource audioSource;

 void Start () {
   //Componentを取得
   audioSource = GetComponent<AudioSource>();
 }

 void Update () {
   // 左
  if (Input.GetKey (KeyCode.LeftArrow)) {
   //音(sound1)を鳴らす
    audioSource.PlayOneShot(sound1);
   }
 }
}
/////////////////音声テスト　使わない/////////////////////////