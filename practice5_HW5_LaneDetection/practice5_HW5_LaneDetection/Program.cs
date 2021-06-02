/* Multimedia System HW #5
 * - 허프변환을 통한 차선 인식 및 표시 
 * (Lane Detection with Hough Transform)
 * 
 * Name: 전성운
 * Student Number: 2016113294 
 */

/* - 과제수행간 메모 * 
 * 1.파일 입출력경로, 임계값 등 핵심요소가 아닌것들은 하드코딩 무방.
 * 2.핵심 알고리즘 실행영역을 강조위해 간결한 코드작성 혹은 적절한 주석 첨삭.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using OpenCvSharp;      // TODO: opencvsharp 라이브러리 세팅


namespace practice5_HW5_LaneDetection
{
    class Program
    {

        static void Main(string[] args)
        {
            // ----------------------------------------------------- 1.기본 입출력 경로 설정

            string root_path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName +
                  "\\Image Resource\\hw5_input\\";
            string save_path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName +
                    "\\for HW\\hw5_output\\";

            string folder_in = "testvideo2";
            // "testvideo1""testvideo2"
            // image names in each folder is "Left_0.bmp" ~ "Left_99.bmp"

            string folder_out = "test_output_2\\";
            // "test_output_1""test_output_2" 
            // let's set img names in each folder "Lined_0.bmp" ~ "Lined_99.bmp" 

            int imgCount = 100;
            // ----------------------------------------------------- 2.폴더내 이미지 긁어오기.
            string[] filelist = new string[imgCount];
            for (int i = 0; i < 100; i++)
            {
                filelist[i] = Directory.GetFiles(root_path + folder_in, "*_" + i.ToString() + ".bmp")[0];
            }
            foreach (string item in filelist)
            {
                Console.WriteLine(item);
            }
            imgCount = filelist.Length;
            Console.WriteLine("Total image: {0}", imgCount);

            Console.ReadKey(); // pause

            //녹화
            VideoWriter merge = new VideoWriter(save_path + folder_out + "Recordtest2.avi", FourCC.XVID, 10, new Size(800, 600), true);

            // ----------------------------------------------------- 3.영상 한장마다 입력-처리-출력(화면,비디오,이미지) 반복.
            for (int i = 0; i < imgCount; i++)
            {
                Mat imgIn = new Mat();
                Mat imgOut = new Mat();
                imgIn = Cv2.ImRead(filelist[i], ImreadModes.AnyColor);
                // CV_8UC3 -> byte타입 3채널
                Console.WriteLine(filelist[i]);
                imgOut = imgIn.Clone();

                Mat gray = new Mat();
                Cv2.CvtColor(imgIn, gray, ColorConversionCodes.BGR2GRAY);
                // CV_8UC1 -> byte타입 1채널

                Mat Canny = new Mat();
                Cv2.Canny(gray, Canny, 50, 200, apertureSize: 3, true);

                // ----------------------------------------------------- 차선 픽셀 탐색,저장
                Mat EdgesL = new Mat(imgIn.Rows, imgIn.Cols, MatType.CV_8UC1, 0);
                Mat EdgesR = new Mat(imgIn.Rows, imgIn.Cols, MatType.CV_8UC1, 0);
                int center_X = 300; // 0~799
                int top_Y = 599 - 170; // 0~600, 
                int Gap = 3;
                findEdge(Canny, EdgesL, center_X, top_Y, Gap, 0);
                findEdge(Canny, EdgesR, center_X, top_Y, Gap, 1);

                //Cv2.WaitKey(0);

                // ----------------------------------------------------- 직선 검출
                LineSegmentPolar[] lineL;
                LineSegmentPolar[] lineR;

                int houghthresh = 15;
                // 기본 임계값은 테스트를 통해 임의로 설정.
                // 필요시 Cv2.kmeans() 알고리즘 등으로 테스트 데이터에 대한 적절한 임계값 찾아볼 수 있을 것으로 예상   

                for (; ; )
                {
                    lineL = Cv2.HoughLines(EdgesL, 0.2, Cv2.PI / 180, houghthresh);
                    // 최소 한개의 직선이 나올때까지 임계값감소.                 
                    if (lineL.Length > 0)
                        break;
                    else
                        houghthresh--;
                }

                houghthresh = 13;
                for (; ; )
                {
                    lineR = Cv2.HoughLines(EdgesR, 0.2, Cv2.PI / 180, houghthresh);
                    if (lineR.Length > 0)
                        break;
                    else
                        houghthresh--;
                }
                Console.WriteLine("검출된 갯수: {0},{1}", lineL.Length, lineR.Length);
                // 인자:  (출력, rho 해상도, theta 해상도( PI/180 rad = 1 degree), 임계값(vote의 최소치))

                int x1 = Convert.ToInt32(lineL[0].XPosOfLine(599)); // 좌측차선 화면 하단 절편
                int x2 = Convert.ToInt32(lineR[0].XPosOfLine(599)); // 우측차선 화면 하단 절편

                int ptL1 = Convert.ToInt32(lineL[0].YPosOfLine(x1));
                int ptL2 = Convert.ToInt32(lineL[0].YPosOfLine(x2));
                int ptR1 = Convert.ToInt32(lineR[0].YPosOfLine(x1));
                int ptR2 = Convert.ToInt32(lineR[0].YPosOfLine(x2));

                // lineL 직선의 방정식
                float lineL1 = (float)(ptL2 - ptL1) / (float)(x2 - x1);
                float lineL2 = ptL2 - lineL1 * x2;
                // lineR 직선의 방정식
                float lineR1 = (float)(ptR2 - ptR1) / (float)(x2 - x1);
                float lineR2 = ptR2 - lineR1 * x2;
                // 교점(소실점?) 계산
                Point ptBanish = new Point();
                ptBanish.X = (int)((lineR2 - lineL2) / (lineL1 - lineR1));
                ptBanish.Y = (int)(lineL1 * ptBanish.X + lineL2);

                // Console.WriteLine("교점: {0}", ptBanish.ToString());

                int lineCount = 1;
                for (int k = 0; k < lineCount; k++)
                {
                    // Console.WriteLine(lineL[k].ToString());
                    int y1 = Convert.ToInt32(lineL[k].YPosOfLine(x1));
                    int y2 = Convert.ToInt32(lineL[k].YPosOfLine(ptBanish.X));

                    // 좌측차선 그리기 : 좌하단 절편부터 소실점까지
                    Cv2.Line(imgOut, x1, y1, ptBanish.X, y2, Scalar.Red, 2, default, 0);
                }

                for (int k = 0; k < lineCount; k++)
                {
                    // Console.WriteLine(lineR[k].ToString());
                    int y1 = Convert.ToInt32(lineR[k].YPosOfLine(ptBanish.X));
                    int y2 = Convert.ToInt32(lineR[k].YPosOfLine(x2));

                    // 우측차선 그리기 : 소실점부터 우하단 절편까지
                    Cv2.Line(imgOut, ptBanish.X, y1, x2, y2, Scalar.Red, 2, default, 0);
                }

                // 소실점 그리기
                Cv2.Circle(imgOut, ptBanish, 8, Scalar.LightYellow, 2);

                // Cv2.ImShow("img", imgOut);
                // Cv2.WaitKey(0);
                string filename_out = "output_" + i.ToString() + ".bmp";
                Cv2.ImWrite(save_path + folder_out + filename_out, imgOut);


                Console.WriteLine("image {0} complete!", i.ToString());

                // 동영상 프레임으로 출력
                merge.Write(imgOut);

            }
            // 녹화 종료
            merge.Release();
            // 프로그램 종료 대기
            Console.WriteLine("아무 키나 눌러 프로그램 종료...");
            Console.ReadKey();
        }

        // 주어진 중앙좌표에서부터 좌우측차선의 픽셀을 검출
        // center_X : 중앙좌표 위치
        // top_Y : (이미지 높이) - (탐색할 y범위)
        // gap : 탐색시 y 간격 (between scanlines)
        // option - 0:좌측차선,1:우측차선 
        private static void findEdge(Mat canny, Mat edges, int center_X, int top_Y, int gap, int option)
        {
            if (option == 0)
            {
                // find Left
                for (int i = top_Y - 1; i < canny.Rows; i += gap)
                {
                    for (int j = center_X; j >= 0; j--)
                    {
                        if (canny.Get<byte>(i, j) == 255)
                        {
                            edges.Set<byte>(i, j, 255);
                            break;
                        }
                    }
                }
            }
            else if (option == 1)
            {
                //find Right
                for (int i = top_Y - 1; i < canny.Rows; i += gap)
                {
                    for (int j = center_X; j <= canny.Cols; j++)
                    {
                        if (canny.Get<byte>(i, j) == 255)
                        {
                            edges.Set<byte>(i, j, 255);
                            break;
                        }
                    }
                }
            }
        }
    }
}