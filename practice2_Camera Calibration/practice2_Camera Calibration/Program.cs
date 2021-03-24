/* * Multimedia System HW #2
 * 
 * Name: 전성운
 * Student Number: 2016113294 
 */

using System;
using OpenCvSharp;
using System.Collections.Generic;

/* 솔루션에 OpenCVSharp의 dll파일을 추가.
 *
 * 1.해당 gitHub에서 배포하는 zip파일 다운로드
 * 2.OpenCVSharp.dll 등 3가지 참조관리자로 추가
 * 3.OpenCVsharpExtern.dll은 직접 Debug 폴더에 추가
 */

namespace practice2_Camera_Calibration
{
    class Program
    {
        static void Main(string[] args)
        {
            //이미지 입력 경로 : 폴더 + 이미지경로
            string root_path = "D:\\성운\\2021년도 1학기 경북대\\멀티미디어 시스템\\OpenCV실습용 체스판사진 10장\\";
            string image_path = null;

            //이미지 입력 리스트
            int LENGTH = 10;
            List<Mat> img_list = new List<Mat>();

            List<InputArray> input_list = new List<InputArray>();

            foreach (Mat img in img_list)
            {
                input_list.Add(img);
            }
            

            OutputArray corners = OutputArray;
            
            
            //

            //체스판 찾기-인자로 넣을 값과 객체 선언
            Size patternsize = new Size(5,6); //현재 임의 입력. 측정해둘 필요있음


            //


            for (int i = 0; i < LENGTH; i++)
            {                
                if (img_list.Count == 0)
                {
                    Console.WriteLine("Could not read the image: " + root_path + image_path);
                    return;
                }
                image_path = String.Format("conv_image{0:D2}.jpg", i + 1); //conv_image01.jpg부터 10번째 이미지까지

                img_list.Add(Cv2.ImRead(root_path + image_path, ImreadModes.Color));
            }
                        
            foreach (Mat img in img_list)
            {
                if (image_path != null)
                {
                    Cv2.ImShow(image_path, img);
                }
            }

            for (int i = 0; i < LENGTH; i++)
            {
                Cv2.FindChessboardCorners(, patternsize, corners, ChessboardFlags.AdaptiveThresh | ChessboardFlags.NormalizeImage);
                Cv2.CornerSubPix(img_list);
                Cv2.DrawChessboardCorners();
                Cv2.CalibrateCamera();
            }

            int k = Cv2.WaitKey(0); //입력 있을때까지 대기. 이 구문이 없을시 창이 바로 종료.

            return;
        }
    }
}

/* 실습에 사용한 스마트폰 카메라 스펙: 
 * Pixel size: 1.4[µm]
 * 
 * Scale값: 1/(1.4 [µm]) = (1/1.4)*1000 [pixel/mm] = 714.285714...(285714/285714/285714/28571)
 */



///입출력 코드 스니펫

/*파일 출력
Mat img = Cv2.ImRead(root_path + image_path, ImreadModes.Color);

if (img.Empty())
{
    Console.WriteLine("Could not read the image: " + root_path + image_path);
    return;
}

Cv2.ImShow("Display window", img);
*/

/* 파일 저장
int k = Cv2.WaitKey(0);

if (k == 's')
{
    Cv2.ImWrite("저장할파일명.포맷", img);
}
*/
