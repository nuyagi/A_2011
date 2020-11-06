using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace TensorFlowLite
{

    /// <summary>
    /// Pose Estimation Example
    /// https://www.tensorflow.org/lite/models/pose_estimation/overview
    /// </summary>
    public class PoseNet : BaseImagePredictor<float>
    {

        //閾値
        private static double threshould = 0.5f;
        public enum Part
        {
            NOSE,
            LEFT_EYE,
            RIGHT_EYE,
            LEFT_EAR,
            RIGHT_EAR,
            LEFT_SHOULDER,
            RIGHT_SHOULDER,
            LEFT_ELBOW,
            RIGHT_ELBOW,
            LEFT_WRIST,
            RIGHT_WRIST,
            LEFT_HIP,
            RIGHT_HIP,
            LEFT_KNEE,
            RIGHT_KNEE,
            LEFT_ANKLE,
            RIGHT_ANKLE
        }

        public static readonly Part[,] Connections = new Part[,]
        {
            // HEAD
            { Part.LEFT_EAR, Part.LEFT_EYE },
            { Part.LEFT_EYE, Part.NOSE },
            { Part.NOSE, Part.RIGHT_EYE },
            { Part.RIGHT_EYE, Part.RIGHT_EAR },
            // BODY
            { Part.LEFT_HIP, Part.LEFT_SHOULDER },
            { Part.LEFT_ELBOW, Part.LEFT_SHOULDER },
            { Part.LEFT_ELBOW, Part.LEFT_WRIST },
            { Part.LEFT_HIP, Part.LEFT_KNEE },
            { Part.LEFT_KNEE, Part.LEFT_ANKLE },
            { Part.RIGHT_HIP, Part.RIGHT_SHOULDER },
            { Part.RIGHT_ELBOW, Part.RIGHT_SHOULDER },
            { Part.RIGHT_ELBOW, Part.RIGHT_WRIST },
            { Part.RIGHT_HIP, Part.RIGHT_KNEE },
            { Part.RIGHT_KNEE, Part.RIGHT_ANKLE },
            { Part.LEFT_SHOULDER, Part.RIGHT_SHOULDER },
            { Part.LEFT_HIP, Part.RIGHT_HIP }
        };

        [System.Serializable]
        public struct Result
        {
            public Part part;
            public float confidence;
            public float x;
            public float y;
        }


        Result[] results = new Result[17];

        float[,,] outputs0; // heatmap
        float[,,] outputs1; // offset

        // float[] outputs2 = new float[9 * 9 * 32]; // displacement fwd
        // float[] outputs3 = new float[9 * 9 * 32]; // displacement bwd

        public PoseNet(string modelPath) : base(modelPath)
        {
            var odim0 = interpreter.GetOutputTensorInfo(0).shape;
            var odim1 = interpreter.GetOutputTensorInfo(1).shape;
            outputs0 = new float[odim0[1], odim0[2], odim0[3]];
            outputs1 = new float[odim1[1], odim1[2], odim1[3]];

        }

        public override void Invoke(Texture inputTex)
        {
            // const float OFFSET = 128f;
            // const float SCALE = 1f / 128f;
            // ToTensor(inputTex, input0, OFFSET, SCALE);
            ToTensor(inputTex, input0);

            interpreter.SetInputTensorData(0, input0);
            interpreter.Invoke();
            interpreter.GetOutputTensorData(0, outputs0);
            interpreter.GetOutputTensorData(1, outputs1);
            // not using
            // interpreter.GetOutputTensorData(2, outputs2);
            // interpreter.GetOutputTensorData(3, outputs3);
        }

        public Result[] GetResults()
        {
            // Name alias
            float[,,] scores = outputs0;
            float[,,] offsets = outputs1;
            float stride = scores.GetLength(0) - 1;

            ApplySigmoid(scores);
            var argmax = ArgMax2D(scores);

            // Add offsets
            for (int part = 0; part < results.Length; part++)
            {
                ArgMaxResult arg = argmax[part];
                Result res = results[part];

                float offsetX = offsets[arg.y, arg.x, part + results.Length];
                float offsetY = offsets[arg.y, arg.x, part];
                res.x = ((float)arg.x / stride * width + offsetX) / width;
                res.y = ((float)arg.y / stride * height + offsetY) / height;
                res.confidence = arg.score;
                res.part = (Part)part;

                results[part] = res;
            }

            return results;
        }

        static void ApplySigmoid(float[,,] arr)
        {
            int rows = arr.GetLength(0); // y
            int cols = arr.GetLength(1); // x
            int parts = arr.GetLength(2);
            // simgoid to get score
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    for (int part = 0; part < parts; part++)
                    {
                        arr[y, x, part] = MathTF.Sigmoid(arr[y, x, part]);
                    }
                }
            }
        }

        struct ArgMaxResult
        {
            public int x;
            public int y;
            public float score;
        }

        static ArgMaxResult[] argMaxResults;
        static ArgMaxResult[] ArgMax2D(float[,,] scores)
        {
            int rows = scores.GetLength(0); //y
            int cols = scores.GetLength(1); //x
            int parts = scores.GetLength(2);

            // Init with minimum float
            if (argMaxResults == null)
            {
                argMaxResults = new ArgMaxResult[parts];
            }
            for (int i = 0; i < parts; i++)
            {
                argMaxResults[i].score = float.MinValue;
            }

            // ArgMax
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    for (int part = 0; part < parts; part++)
                    {
                        float current = scores[y, x, part];
                        if (current > argMaxResults[part].score)
                        {
                            argMaxResults[part] = new ArgMaxResult()
                            {
                                x = x,
                                y = y,
                                score = current,
                            };
                        }
                    }
                }
            }
            return argMaxResults;
        }


        /////////////// add function ///////////////
        public static double GetAngular(double ax,double ay,double bx,double by,double cx,double cy)
        {
            // 内積から角度を計算
            double vectorA_x = ax - bx;
            double vectorA_y = ay - by;
            double vectorC_x = cx - bx;
            double vectorC_y = cy - by;
            // caluculate inner product
            double naiseki = vectorA_x * vectorC_x + vectorA_y * vectorC_y;
            double normAB = Math.Sqrt(Math.Pow(vectorA_x,2) + Math.Pow(vectorA_y,2));
            double normCB = Math.Sqrt(Math.Pow(vectorC_x,2) + Math.Pow(vectorC_y,2));
            double angular = Math.Acos(naiseki / normAB / normCB) * 180 / 3.141;
            
            return angular;
        }

        public static double CheckCrunchi(Result[] a)
        {
            // a reult型　の結果データ
            // クランチ 右尻12 右ひざ14 右足首16
            // 左尻　11 左膝14　左足首15
            // 0.5f しきい値
            // 足の角度が 100 ~ 80 = 1 110 ~ 70 = 0.8 
            double angle = -1;
            if (a[13].confidence > 0.5f && a[14].confidence > 0.5f && a[16].confidence > 0.5f){
                angle = GetAngular(a[12].x,a[12].y,a[14].x,a[14].y,a[16].x,a[16].y);
            }
            else if (a[11].confidence > 0.5f && a[13].confidence > 0.5f && a[15].confidence > 0.5f){
                angle = GetAngular(a[11].x,a[11].y,a[13].x,a[13].y,a[15].x,a[15].y);
            }
            else {
                //体が写ってないとき
                angle = -1;
            }

            // 体が写って無いときは０
            // 足の角度が 100 ~ 80 = 1 110 ~ 70 = 0.8　それ以外　0.5
            if (angle > 110) return 0.5f;
            else if (angle > 100) return 0.8f;
            else if (angle < 70 && angle > 0) return -0.5f;
            else if (angle < 80 && angle > 0) return -0.8f;
            else if (angle>=80 && angle <= 100) return 1;
            else return 0;
        }
        public static int CountCrunchi(Result[] a){
            //左肩　5　左尻11
            //右肩　６　右尻１２
            // 肩が上がってる１　下がってる　－１　そもそも見えない　０
            // 座標20　で感知
            double threshould = 0.5f;
            if (a[5].confidence > threshould && a[11].confidence > threshould)
            {
                if(a[5].x > (a[11].x+0.2)){
                    return 1;
                }
                else{
                    return -1;
                }
            }
            else if (a[6].confidence > threshould && a[12].confidence > threshould)
            {
                if(a[6].x > (a[12].x+0.2)){
                    return 1;
                }
                else{
                    return -1;
                }
            }
            else{
                return 0;
            }
        }
        public static int CountNose(Result[] a){
            //テスト用
            if (a[0].confidence > threshould)
            {
                if(a[0].x > 0.5){
                    return 1;
                }
                else{
                    return -1;
                }
            }
            else{
                return 0;
            }
        }
        public static double CheckSquat(Result[] a)
        {
            // a reult型　の結果データ
            // スクワット 右尻12 右ひざ14 右足首16
            // 左尻　11 左膝13　左足首15
            // 0.5f しきい値
            // x座標の差を見る 
            double difference = -1;
            if (a[12].confidence > 0.5f && a[14].confidence > 0.5f && a[16].confidence > 0.5f){
                difference = System.Math.Abs(a[14].x-a[16].x);
            }
            else if (a[11].confidence > 0.5f && a[13].confidence > 0.5f && a[15].confidence > 0.5f){
                difference = System.Math.Abs(a[13].x-a[15].x);
            }
            else {
                //体が写ってないとき
                difference = -1;
            }

            // 体が写って無いときは-1
            // ずれが許容範囲　１　大きすぎ　０
            if (difference > 0.2) return 0;
            else if (difference <= 0.2  && difference >= 0) return 1;
            else return -1;
        }
        public static int CountSquat(Result[] a){
            //左尻11 左ひざ13
            //右尻１２　右ひざ14
            // 腰が下がってる１　あがってる　－１　そもそも見えない　０
            // 座標20　で感知
            double threshould = 0.5f;
            if (a[11].confidence > threshould && a[13].confidence > threshould)
            {
                if(a[11].y < (a[13].y + 0.2)){
                    return 1;
                }
                else{
                    return -1;
                }
            }
            else if (a[12].confidence > threshould && a[14].confidence > threshould)
            {
                if(a[12].y < (a[14].y + 0.2)){
                    return 1;
                }
                else{
                    return -1;
                }
            }
            else{//写ってない
                return 0;
            }
        }
        ///////////////////////////////////////////////////////////////////

    }
}
