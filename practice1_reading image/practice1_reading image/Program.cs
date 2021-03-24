/* * Multimedia System HW #1
 * 
 * Name: 전성운
 * Student Number: 2016113294 
 */

using System;
using OpenCvSharp;

//솔루션에 OpenCVSharp의 dll파일을 추가하거나 Nuget라이브러리 관리자를 통해 설치.

namespace Multimedia_System_OpenCV_Practice1
{
    class Program
    {
        static void Main(string[] args)
        {
            string root_path = "D:\\성운\\2021년도 1학기 경북대\\멀티미디어 시스템\\HW #1 제출물\\OpenCV실습용 체스판사진 10장\\";
            string image_path;

            int LENGTH = 10;
            Mat[] img = new Mat[LENGTH];

            for (int i = 0; i < 10; i++)
            {
                image_path = String.Format("conv_image{0:D2}.jpg", i + 1); //conv_image01.jpg부터 10번째 이미지까지

                img[i] = Cv2.ImRead(root_path + image_path, ImreadModes.Color);

                if (img[i].Empty())
                {
                    Console.WriteLine("Could not read the image: " + root_path + image_path);
                    return;
                }

                Cv2.ImShow(String.Format("Display window{0:D2}", i + 1), img[i]);
            }

            int k = Cv2.WaitKey(0); //입력 있을때까지 대기. 이 구문이 없을시 창이 바로 종료.

            return;
        }
    }
}


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
