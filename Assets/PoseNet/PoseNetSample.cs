using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TensorFlowLite;

public class PoseNetSample : MonoBehaviour
{
    [SerializeField, FilePopup("*.tflite")] string fileName = "posenet_mobilenet_v1_100_257x257_multi_kpt_stripped.tflite";
    [SerializeField] RawImage cameraView = null;
    [SerializeField, Range(0f, 1f)] float threshold = 0.5f;
    [SerializeField, Range(0f, 1f)] float lineThickness = 0.5f;

    WebCamTexture webcamTexture;
    PoseNet poseNet;
    Vector3[] corners = new Vector3[4];
    PrimitiveDraw draw;

    public PoseNet.Result[] results;

    //////////////////////////////////////
    // 音声出力用///////
    public AudioClip sound1;
    // 1 あし　たかい　２　あしひくい 3なにも写っていない
    public AudioClip sound2;
    public AudioClip sound3;
    AudioSource audioSource;
    ///////////////////////
    // 平均用カウンター counter
    // flame counter %60 で割る
    ulong counter=0;
    //ulong flamecounter = 0;
    double score = 0;
    //double[] ScoreList
    //スコア用
    public GameObject score_object = null;
    double  totalscore = 0;
    bool FlagStartStop = false;
    ///////////////////////////////////////

    //
    void Start()
    {
        string path = Path.Combine(Application.streamingAssetsPath, fileName);
        poseNet = new PoseNet(path);
        /////////////////////////////////////
        //audio Componentを取得 足した
        audioSource = GetComponent<AudioSource>();
        //flag 初期化
        FlagStartStop = false;

        /////////////////////////////////////
        // Init camera
        string cameraName = WebCamUtil.FindName();
        webcamTexture = new WebCamTexture(cameraName, 640, 480, 30);
        webcamTexture.Play();
        cameraView.texture = webcamTexture;

        draw = new PrimitiveDraw()
        {
            color = Color.green,
        };
    }

    void OnDestroy()
    {
        webcamTexture?.Stop();
        poseNet?.Dispose();
        draw?.Dispose();
    }

    ////////////////////////
    public void OnClick() {
        if (FlagStartStop==false){
            FlagStartStop = true;
        }
        else {
            FlagStartStop = false;
        }
    }
    ///////////////////////////

    void Update()
    {
        poseNet.Invoke(webcamTexture);
        results = poseNet.GetResults();

        cameraView.material = poseNet.transformMat;
        // cameraView.texture = poseNet.inputTex;
        //////////////// add (sugawara)//////////////
        score = PoseNet.CheckCrunchi(results);
        //Debug.Log(score);
        if (counter < 120){
            counter += 1;
        }
        // score ! = 1 なら発音
        if (counter >= 120){
            counter = 0;
            if (score ==1){
                // 何もしない
            }
            else if(score>0){
            audioSource.PlayOneShot(sound1);
            }
            else if (score<0) {
                audioSource.PlayOneShot(sound2);
            }
            else {
                audioSource.PlayOneShot(sound3);
            }
        }
        //スコアを加算
        if (FlagStartStop){
            if (counter == 60 || counter == 120){
                totalscore+=score;
                //totalscore +=1;
            }
        }
        Text score_text = score_object.GetComponent<Text> ();
        // テキストの表示を入れ替える
        score_text.text = "Score:" +totalscore;
        ///////////////////////////////////////////
        DrawResult();
    }

    void DrawResult()
    {
        var rect = cameraView.GetComponent<RectTransform>();
        rect.GetWorldCorners(corners);
        Vector3 min = corners[0];
        Vector3 max = corners[2];

        var connections = PoseNet.Connections;
        int len = connections.GetLength(0);
        for (int i = 0; i < len; i++)
        {
            var a = results[(int)connections[i, 0]];
            var b = results[(int)connections[i, 1]];
            if (a.confidence >= threshold && b.confidence >= threshold)
            {
                draw.Line3D(
                    MathTF.Lerp(min, max, new Vector3(a.x, 1f - a.y, 0)),
                    MathTF.Lerp(min, max, new Vector3(b.x, 1f - b.y, 0)),
                    lineThickness
                );
            }
        }

        draw.Apply();
    }
}
