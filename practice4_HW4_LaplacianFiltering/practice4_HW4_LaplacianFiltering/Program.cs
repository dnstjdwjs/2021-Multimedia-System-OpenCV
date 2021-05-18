/* Multimedia System HW #4
 * - 라플라시안 필터를 통한 경계선 검출 실습
 * (Edge Detection by laplacian filtering)
 * 
 * Name: 전성운
 * Student Number: 2016113294 
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using OpenCvSharp;      // TODO: opencvsharp 라이브러리 세팅

namespace practice4_HW4_LaplacianFiltering
{
    class Program
    {
        static void Main(string[] args)
        {
            // -------------------------------------------------------------- 입출력 폴더 경로설정
            string root_path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName +
               "\\Image Resource\\hw4_input\\";
            string save_path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName +
                "\\for HW\\hw4_output\\";
            // -------------------------------------------------------------- 입력이미지 이름 설정
            //string image_name1 = "<lena 이미지>";
            string image_name1 = "lena.tif";
            //string image_name2 = "<도로 표지판 영상>";
            string image_name2 = "road_sign.png";
            //string image_name3 = "<차선이 보이는 도로 영상>";
            string image_name3 = "highway.jpg";

            Console.WriteLine("{0}", root_path);
            Console.WriteLine("{0}", save_path);

            // -------------------------------------------------------------- 함수 인자 선언
            double[] sigmaX = new double[3] { 2.0, 3.0, 5.0 };

            // -------------------------------------------------------------- 입출력 테스트 (통과)
            //Mat test = new Mat { };
            //test = Cv2.ImRead(root_path + image_name1, ImreadModes.Grayscale);
            //Cv2.ImShow("image", test);
            //Cv2.ImWrite(save_path + "testOut.png", test);


            // -------------------------------------------------------------- 영상읽기: 색상모드를 회색조로
            // -------------------------------------------------------------- 입력 이미지 1~3 입력행렬
            Mat[] input_list = new Mat[3];
            input_list[0] = Cv2.ImRead(root_path + image_name1, ImreadModes.Grayscale);
            input_list[1] = Cv2.ImRead(root_path + image_name2, ImreadModes.Grayscale);
            input_list[2] = Cv2.ImRead(root_path + image_name3, ImreadModes.Grayscale);

            //Console.WriteLine(input_list[0].Type().ToString()); // 결과: CV_8UC1

            // -------------------------------------------------------------- 출력 행렬 선언
            Mat[] out_list = new Mat[3];
            out_list[0] = new Mat(size: input_list[0].Size(), MatType.CV_8UC1);
            out_list[1] = new Mat(size: input_list[0].Size(), MatType.CV_8UC1);
            out_list[2] = new Mat(size: input_list[0].Size(), MatType.CV_8UC1);

            Mat[] out_list_2 = new Mat[3];
            out_list_2[0] = new Mat(size: input_list[0].Size(), MatType.CV_8UC1);
            out_list_2[1] = new Mat(size: input_list[0].Size(), MatType.CV_8UC1);
            out_list_2[2] = new Mat(size: input_list[0].Size(), MatType.CV_8UC1);

            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    // -------------------------------------------------------------- 가우시안 필터 (smoothing, blurring & noise filtering)
                    Cv2.GaussianBlur(input_list[i], out_list[i], new Size(5, 5), sigmaX[j], 0, BorderTypes.Default);

                    Cv2.ImShow(i.ToString() + " after Gaussian", out_list[i]);

                    // -------------------------------------------------------------- 라플라시안 필터 적용
                    out_list[i].ConvertTo(out_list[i], rtype: MatType.CV_32FC1);
                    Cv2.Laplacian(out_list[i], out_list[i], MatType.CV_32FC1, ksize: 5 , scale: 1, delta: 0, BorderTypes.Default);
                    Cv2.ImShow(i.ToString() + " after lap", out_list[i]);

                    //Cv2.ConvertScaleAbs(out_list[i], out_list[i], alpha: 1, beta: 0);
                    //zerocrossing dectection 이전에 본함수 사용시 음수영역과 실수부등 uchar 영역 외의 값이 손실.


                    // -------------------------------------------------------------- ZeroCrossing 함수 적용
                    out_list[i].ConvertTo(out_list_2[i], MatType.CV_32F);
                    out_list[i].ConvertTo(out_list[i], MatType.CV_8UC1);
                    Cv2.ImShow(i.ToString() + " after 32f convert", out_list_2[i]);
                    Cv2.ImShow(i.ToString() + " after 8u convert", out_list[i]);
                    FindZeroCrossings(out_list_2[i], out_list[i]);


                    Cv2.ImShow(i.ToString() + " after zerocrossing detection", out_list[i]);

                    Console.WriteLine(out_list[i].Type().ToString());
                    Cv2.WaitKey(0);

                    // -------------------------------------------------------------- 지정된 출력폴더에 처리된 이미지 저장
                    Cv2.ImWrite(save_path + "image " + i.ToString() + "_sigma " + sigmaX[j].ToString() + ".png", out_list[i]);

                }

            }

        }

        unsafe static void FindZeroCrossings(Mat inputarray_, Mat outputarray_)
        {
            int image_rows = inputarray_.Rows;
            int image_channels = inputarray_.Channels();
            int values_on_each_row = inputarray_.Cols * image_channels;
            float laplacian_threshold = 100.0f;

            // Find Zero Crossings
            for (int row = 1; row < image_rows; row++)
            {
                float* prev_row_pixel = (float*)inputarray_.Ptr(row - 1).ToPointer() + 1;
                float* curr_row_pixel = (float*)inputarray_.Ptr(row).ToPointer();
                byte* output_pixel = (byte*)outputarray_.Ptr(row).ToPointer() + 1;
                for (int column = 1; column < values_on_each_row; column++)
                {
                    float prev_value_on_row = *curr_row_pixel;
                    curr_row_pixel++;
                    float curr_value = *curr_row_pixel;
                    float prev_value_on_column = *prev_row_pixel;
                    float difference = 0.0f;
                    if (((curr_value > 0) && (prev_value_on_row < 0)) ||
                        ((curr_value < 0) && (prev_value_on_row > 0)))
                        difference = Math.Abs(curr_value - prev_value_on_row);
                    if ((((curr_value > 0) && (prev_value_on_column < 0)) ||
                         ((curr_value < 0) && (prev_value_on_column > 0))) &&
                        (Math.Abs(curr_value - prev_value_on_column) > difference))
                        difference = Math.Abs(curr_value - prev_value_on_column);

                    // C# & .Net framework 9.0버전 이후부터만 삼항연산자 적용가능
                    // output_pixel = (difference > laplacian_threshold) ? 255 : 0;
                    if (difference > laplacian_threshold)
                    {
                        *output_pixel = 255;
                    }
                    else
                    {
                        *output_pixel = 0;
                    }

                    // (int) ((100 * difference) / laplacian_threshold);

                    prev_row_pixel++;
                    output_pixel++;
                }
            }
        }


    }
}
 