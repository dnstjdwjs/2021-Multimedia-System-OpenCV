/* Multimedia System HW #2 
 * - Camera Calibration 및 왜곡보정함수 실습
 * 
 * Name: 전성운
 * Student Number: 2016113294 
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenCvSharp;
using NumSharp;


/* % 본 소스코드 컴파일 및 실행을 위한 사전작업 %
 * 
 * -> 솔루션에 OpenCVSharp의 dll파일을 추가해야 합니다.
 *
 * 1.Shimat의 OpenCVsharp gitHub에서 배포하는 zip파일 (사용할 OpenCVsharp 버전에 맞게) 다운로드
 * 2.OpenCVSharp.dll 등 3가지를 참조관리자로 추가
 * 3.OpenCVsharpExtern.dll은 1번의 /x64에서 복사해서 직접 프로젝트/솔루션/Bin/Debug 폴더에 추가
 * 4.2번까지는 Nuget관리자에서 등록인이 shimat으로 되어있는 OpenCVsharp4를 설치하면되고, 3번은 1번과 더불어서 해줘야합니다. * 
 * 
 * -dll이 과제물 관리중인 GitHub에 올릴때 GitIgnore처리되서 과제환경 pc가 바뀔때마다 다시설치하고 있습니다.
 * 실행해보셔야 된다면 불편을 끼쳐드려 죄송합니다.
 */

namespace practice2_Camera_Calibration
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 잡 주석 1
            //// ------------------------- 이미지 입력 경로 설정

            ///*
            // * 본 코드에서 사용하는 경로는 C# 프로젝트 폴더 상위폴더(GitHub repository)에 존재함
            // */

            //string root_path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName +
            //    "\\Image Resource\\";
            //Console.WriteLine(root_path);

            //// ------------------------- 입력 경로내 전체 파일이름 배열에 할당
            ///
            //// ------------------------- 캘리브레이션을 위한 이미지(Mat) 배열 할당 

            // ------------------------- 카메라 캘리브레이션 함수 호출
            #endregion

            //CalibrateCamera();
            CalibrateCamera_numpy();

            #region 잡 주석 2
            /*  Mat[] 배열로 이미지 목록을 주었다.
             *  무엇을 하는 함수인가?
             *  체스판 찾고, 코너 조정하고, 코너위치를 그린 뒤, 카메라 캘리브레이션해서 출력
             *  위의 과정을 10장 전체에 대해서 반복하여 결과값(intrinsic,렌즈왜곡,보정오차,R벡터, T벡터)을 전부 출력,저장 ("보정결과.txt")

            // ------------------------- 사용한 카메라(갤럭시 S8 후면)의 CMOS 스펙조사하여 Focal length 계산

            /* 실습에 사용한 스마트폰 카메라 스펙: 
             * Pixel size: 1.4[µm]
             * 
             * Scale값: 1/(1.4 [µm]) = (1/1.4)*1000 [pixel/mm] = 714.285714...(285714/285714/285714/28571)
             * (사진 해상도를 줄여서 사용했기 때문에 그 부분도 고려해야함...이 부분 추가요망.)          
             */



            /*
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
            */
            #endregion

            int k = Cv2.WaitKey(0); //입력 있을때까지 대기. 이 구문이 없을시 창이 바로 종료.

            return;
        }
        static void CalCalibration_2(Mat[] images)
        {
        }
        /*
        static void CalCalibration(Mat[] images)
        {
            // 참조한 소스코드: https://github.com/shimat/opencvsharp/issues/132
            // 대략적인 입출력 형태만 참조하였고 실제 사용함수나 자료구조는 변경하였습니다.

            // --------------------------------------------------- 함수인자로 넣을 필드선언
            Size imageSize = images[0].Size();
            Size patternSize = new Size(5, 6);
            Size regionSize = new Size(4, 4);
            Size rectangleSize = new Size(20f, 20f);

            var imagePoints = new List<Mat<Point2d>>();
            var objectPoints = new List<Point3d>();

            var objectPoint = new List<Point3d>();
            for (int i = 0; i < patternSize.Height; i++)
            {
                for (int j = 0; j < patternSize.Width; j++)
                {
                    objectPoint.Add(new Point3d(j * rectangleSize.Width,
                        i * rectangleSize.Height, 0.0F));
                }
            }

            for (int i = 0; i < images.Length; i++)
            {

                Mat currentMat = images[i];

                Mat<Point2d> corners = new Mat<Point2d>();
                bool isFoundChessboard = Cv2.FindChessboardCorners(currentMat, patternSize, OutputArray.Create(corners));
                if (!isFoundChessboard)
                {
                    continue;
                }

                bool isCornerSubpix = Cv2.Find4QuadCornerSubpix(currentMat, corners, regionSize);
                if (!isCornerSubpix)
                {
                    continue;
                }

                imagePoints.Add(corners);
                MatOfPoint3d objectPointMat = new MatOfPoint3d(patternSize.Width * patternSize.Height, 1,
                    objectPoint.ToArray());
                objectPoints.Add(objectPointMat);
            }

            var cameraMatrix = new double[3, 3];
            var distCoeffs = new double[8];
            Vec3d[] rvecs, tvecs;

            Cv2.CalibrateCamera(objectPoints, imagePoints, imageSize, cameraMatrix, distCoeffs, out rvecs, out tvecs);
        }
        */
        static void CalibrateCamera_numpy()
        {
            var nd = np.arange(0, 5, 1);
            Console.WriteLine(nd.ToString() + "\n---------------------\n");

            Console.WriteLine(n1.ToString() + "\n---------------------\n");


            //    NDArray objp = np.zeros((5 * 6, 3), np.float32);

            //    var nd = np.arange(0, 5, 1).mgrid(np.arange(0,6,1));
            //    Console.WriteLine(nd.ToString() + "\n---------------------\n");

            //    //var n1 = (np.transpose(nd.Item1, null), np.transpose(nd.Item2, null));
            //    var n1 = np.concatenate(nd.Item1);
            //    Console.WriteLine(n1.ToString() + "\n---------------------\n");

            //    //var n2 = np.concatenate((n1.Item1, n1.Item2), 0);
            //    var n2 = n1.transpose(null);
            //    Console.WriteLine(n2.ToString() + "\n---------------------\n");

            //    var n3 = n2.reshape((-1, 2));
            //    Console.WriteLine(n3.ToString() + "\n---------------------\n");

            Console.Read();
            
        }
        static void CalibrateCamera()
        {
            int ImageNum;
            const int PatRow = 5;
            const int PatCol = 6;
            const int PatSize = PatRow * PatCol;
            int AllPoints; // = ImageNum * PatSize;
            const float ChessSize = 30.0f;

            string root_path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName +
                "\\Image Resource\\";
            string save_path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName +
                "\\Output Xml\\";
            Console.WriteLine(root_path);
            string[] imageArr = Directory.GetFiles(root_path);
            ImageNum = imageArr.Length;
            AllPoints = ImageNum * PatSize;

            Mat[] srcgray = new Mat[ImageNum];

            for (int i = 0; i < ImageNum; i++)
            {
                srcgray[i] = new Mat(imageArr[i], ImreadModes.Grayscale);
            }

            Point3f[,,] objects = new Point3f[ImageNum, PatRow, PatCol];

            for (int i = 0; i < ImageNum; i++)
            {
                for (int j = 0; j < PatRow; j++)
                {
                    for (int k = 0; k < PatCol; k++)
                    {
                        objects[i, j, k] = new Point3f
                        {
                            X = j * ChessSize,
                            Y = k * ChessSize,
                            Z = 0.0f //3차원 공간상에 2차원 이미지를 붙여놓고 찍었으니 Z축(이미지 뒤편으로 나가는)이 0이 아닌 코너는 없다.
                        };
                    }
                }
            }
            //Mat objectPoints = new Mat(AllPoints, 3, MatType.F32C1, objects);
            Mat[] objectPoints = Enumerable.Repeat<Mat>(new Mat(AllPoints, 3, MatType.CV_32FC1, objects), ImageNum).ToArray<Mat>();
            // --> 기존 OpenCVsharp에서는 좌표값 행렬을 뭉쳐서 하나의 행렬로 만들었지만 버전업되면서 vec혹은 리스트형태로 바뀜. 그렇게 할당해주는 구문.

            Size patternSize = new Size(PatCol, PatRow);

            int foundNum = 0;
            List<Point2f> allCorners = new List<Point2f>(AllPoints);
            int[] pointCountsValue = new int[ImageNum];
            using (Window window = new Window("Calibration", WindowFlags.AutoSize))
            {
                for (int i = 0; i < ImageNum; i++)
                {
                    Point2f[] corners;
                    bool found = Cv2.FindChessboardCorners(srcgray[i], patternSize, out corners, ChessboardFlags.AdaptiveThresh | ChessboardFlags.NormalizeImage);
                    Console.WriteLine("{0:D2}...", i);
                    if (found)
                    {
                        Console.WriteLine("ok");
                        foundNum++;
                    }
                    else
                    {
                        Console.WriteLine("fail");
                    }

                    Cv2.CornerSubPix(srcgray[i], corners, new Size(3, 3), new Size(-1, -1), new TermCriteria(CriteriaTypes.Eps | CriteriaTypes.MaxIter, 30, 0.001));
                    Cv2.DrawChessboardCorners(srcgray[i], patternSize, corners, found);
                    pointCountsValue[i] = corners.Length;

                    window.ShowImage(srcgray[i]);
                    Cv2.WaitKey(0);

                    allCorners.AddRange(corners);
                }
                if (foundNum != ImageNum)
                {
                    Console.WriteLine("error flag 1 (JSW)");
                }
            }

            Mat[] imagePoints = Enumerable.Repeat<Mat>(new Mat(AllPoints, 1, MatType.CV_32FC2, allCorners.ToArray()), ImageNum).ToArray<Mat>();
            //Mat pointCounts = new Mat(ImageNum, 1, MatType.CV_32SC1, pointCountsValue);

            Mat cameraMatrix = new Mat(3, 3, MatType.CV_64FC1);
            Mat distCoeffs = new Mat(1, 5, MatType.CV_64FC1);
            //Mat rotation = new Mat(ImageNum, 3, MatType.CV_64FC1);
            Mat[] rotation = Enumerable.Repeat<Mat>(new Mat(1, 3, MatType.CV_64FC1), ImageNum).ToArray<Mat>();
            Mat[] translation = Enumerable.Repeat<Mat>(new Mat(1, 3, MatType.CV_64FC1), ImageNum).ToArray<Mat>();
            Mat[] rotation_3x3 = Enumerable.Repeat<Mat>(new Mat(3, 3, MatType.CV_64FC1), ImageNum).ToArray<Mat>();

            Cv2.CalibrateCamera(objectPoints, imagePoints, srcgray[0].Size(), cameraMatrix, distCoeffs, out rotation, out translation, CalibrationFlags.None, new TermCriteria((CriteriaTypes.Eps | CriteriaTypes.MaxIter), 30, 0.001));
            //

            // ----------------------------------------------------------- #| 보정오차 계산 |#

            double error = 0.0f;
            double tot_error = 0.0f;

            Mat[] imagePoints_ = (Mat[])imagePoints.Clone();

            for (int i = 0; i < ImageNum; i++)
            {
                Cv2.ProjectPoints(objectPoints[i], rotation[i], translation[i], cameraMatrix, distCoeffs, imagePoints_[i]);
                error = Cv2.Norm(imagePoints[i], imagePoints_[i], NormTypes.L2) / (double)
                    imagePoints.Length;
                tot_error += error;
            }
            //Console.WriteLine(string.Format("Error_rate = {0}",tot_error / objectPoints.Length));
            Console.WriteLine(tot_error.ToString());


            // ------------------------- 상기 값들 이용하여 왜곡보정 함수 적용 & 적용 전,후 이미지 한 쌍을 창에 띄우기.
            Mat src = srcgray[3];
            Mat dst = srcgray[3].EmptyClone();

            Console.WriteLine(cameraMatrix.Dump());
            Console.WriteLine(distCoeffs.Dump());

            Cv2.Undistort(src, dst, cameraMatrix, distCoeffs, null);

            //Cv2.ImShow("Before", src);
            using (Window window1 = new Window("before", WindowFlags.AutoSize),
                window2 = new Window("after", WindowFlags.AutoSize))
            {
                window1.ShowImage(src);
                window2.ShowImage(dst);
                Cv2.WaitKey(0);
            }
                       
            Console.Read();


                


            // ----------------------------------------------->로드리게스 함수로 회전행렬 표현방식 변환
                for (int i = 0; i < ImageNum; i++)
            {
                Cv2.Rodrigues(rotation[i], rotation_3x3[i], null);
            }

            //Console.WriteLine("case start -1" + rotation[0].Dump() + "case end");
            //Console.WriteLine("case start -2" + rotation_3x1[0].Dump() + "case end");

            //// !! Mat 타입을 제대로 텍스트 출력하기 위해서는 Dump()함수를 이용하면 된다.
            //// 이걸 몰라서 Xml 입출력의 문제인줄 알고 8시간 삽질을...

            ///* Cv2.Rodrigues();
            // * r ,t 벡터가 다1x3, 1x3으로  나오는데 이 함수를 적용해서 r을 3x3으로 바꿔주면 된다.
            // */

            for (int i = 0; i < ImageNum; i++)
            {
                using (var fs = new FileStorage(save_path + string.Format("camera{0}.xml", i), FileStorage.Modes.Write, null))
                {
                    fs.Write("intrinsic", cameraMatrix);
                    fs.Write("rotation", rotation_3x3[i]);
                    fs.Write("translation", translation[i]);
                    fs.Write("distortion", distCoeffs);
                }
                /*
                 * 파일 작업을 했으면 반드시 메모리버퍼를 해제해주어야한다.
                 * 아니면 이렇게 using을 이용해서 알아서 관리되도록 하거나..
                 */
            }

            foreach (Mat img in srcgray)
            {
                img.Dispose(); 
            }


            //// ---------------------------저장해놓은 xml 읽기 (텍스트로)
            //// 書き込んだファイルを表示
            //Console.WriteLine(File.ReadAllText(save_path + "camera0.xml"));


            // --------------------------- 저장해놓은 xml 열어서 데이터 읽기

            for (int i = 0; i < ImageNum; i++)
            {
                using (var fs = new FileStorage(save_path + string.Format("camera{0}.xml", i), FileStorage.Modes.Read, null))
                {
                    FileNode fn1 = fs["intrinsic"];
                    Console.WriteLine("카메라 내부값" + fn1.ReadMat().Dump() + "case end");
                    
                    fn1.Dispose();

                    FileNode fn2 = fs["rotation"];
                    Console.WriteLine("rotation" + fn2.ReadMat().Dump() + "case end");
                    fn2.Dispose();

                    FileNode fn3 = fs["translation"];
                    Console.WriteLine("translation" + fn3.ReadMat().Dump() + "case end");
                    fn3.Dispose();

                    FileNode fn4 = fs["렌즈왜곡값"];
                    Console.WriteLine("렌즈왜곡값" + fn4.ReadMat().Dump() + "case end");
                    fn4.Dispose();
                }
            }
            

            /*
             * fs["R"] >> R;                                      // Read cv::Mat
        fs["T"] >> T;
        fs["MyData"] >> m;                                 // Read your own structure_
        cout << endl
            << "R = " << R << endl;
        cout << "T = " << T << endl << endl;
        cout << "MyData = " << endl << m << endl << endl;

            */

            Console.Read();
        }
    }
}

/* 실습에 사용한 스마트폰 카메라 스펙: 
 * Pixel size: 1.4[µm]
 * 
 * Scale값: 1/(1.4 [µm]) = (1/1.4)*1000 [pixel/mm] = 714.285714...(285714/285714/285714/28571)
 */

/* 파일입출력용 상대경로값들.
$(ProjectDir)

: .vcproj 파일의 경로



$(SolutionDir)

: .sln 파일의 경로
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
