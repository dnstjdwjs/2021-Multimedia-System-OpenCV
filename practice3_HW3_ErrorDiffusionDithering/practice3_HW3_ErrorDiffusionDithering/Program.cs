/* Multimedia System HW #3 
 * - 오차확산 디더링(Error diffusion dithering) 실습
 * 
 * Name: 전성운
 * Student Number: 2016113294 
 */


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenCvSharp;

namespace practice3_HW3_ErrorDiffusionDithering
{
    class Program
    {
        static void Main(string[] args)
        {

            // -------------------------------------------------------------- 입출력 경로설정
            string root_path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.Parent.FullName +
               "\\Image Resource\\";
            string save_path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.Parent.FullName +
                "\\for HW\\hw3_output\\";
            string image_name1 = "hw3_lena\\lena.tif";
            string image_name2 = "conv_Image01.jpg";

            // -------------------------------------------------------------- 이미지 필드 선언 
            Mat img_in_BGR = new Mat { };
            Mat img_in_gray = new Mat { };
            Mat img_out_gray = new Mat { };
            Mat[] list_img_split = new Mat[3];
            Mat[] list_img_merge = new Mat[3];
            Mat img_out_BGR = new Mat { };

            try
            {
                // -------------------------------------------------------------- 파일 입출력
                img_in_BGR = Cv2.ImRead(root_path + image_name2, ImreadModes.Color); // 디폴트값: BGR 순서로 읽는다.

                // -------------------------------------------------------------- 흑백 변환
                img_in_gray = img_in_BGR.CvtColor(ColorConversionCodes.BGR2GRAY);
                // img_in_gray.Type() -> 0 -> CV_8U -> byte (0~255)
            }
            catch (OpenCVException e)
            {
                throw new NotImplementedException();
                // TODO: 예외처리 익숙해질때까진 asssert로 실수줄이기.
                throw;
            }
            finally
            {
                Console.WriteLine("root_path: " + root_path);
                Cv2.ImShow("before_Gray", img_in_gray);
            }
            // -------------------------------------------------------------- 알고리즘 적용
            img_out_gray = ErrorDiffusion(img_in_gray);

            // -------------------------------------------------------------- 흑백 완료. 잠시 대기.          
            Cv2.ImShow("After_Gray", img_out_gray);
            Console.WriteLine("그레이 스케일 디더링 완료. 컬러채널 분할 및 디더링 진행을 위해 아무 키 입력.");
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();

            // -------------------------------------------------------------- 채널 분할 -> 각 분할 이미지에 알고리즘 적용 -> 재합성
            // !! OpenCV의 기본 채널 순서: B, G, R 
            list_img_split = Cv2.Split(img_in_BGR);

            list_img_merge[0] = ErrorDiffusion(list_img_split[0]);
            list_img_merge[1] = ErrorDiffusion(list_img_split[1]);
            list_img_merge[2] = ErrorDiffusion(list_img_split[2]);

            Cv2.Merge(list_img_split, img_out_BGR);

            // -------------------------------------------------------------- 출력 테스트 
            Cv2.ImShow("After_BGR", img_out_BGR);
            //Cv2.ImShow("blue", list_img_split[0]);
            //Cv2.ImShow("green", list_img_split[1]);
            //Cv2.ImShow("red", list_img_split[2]);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();

            // -------------------------------------------------------------- 결과물 저장
            Cv2.ImWrite(save_path + "dithered_gray_" + image_name2, img_out_gray);
            Console.WriteLine("저장완료: " + save_path + " dithered_gray_" + image_name2);
            Cv2.ImWrite(save_path + "dithered_color_" + image_name2, img_out_BGR);
            Console.WriteLine("저장완료: " + save_path + " dithered_color_" + image_name2);
            Cv2.WaitKey(0);
        }


        private static Mat ErrorDiffusion(Mat img_in)
        {

            // -------------------------------------------------------------- 필요한 변수 생성
            img_in.ConvertTo(img_in, MatType.CV_64F);

            var mat3 = new Mat<double>(img_in);
            var indexer_in = mat3.GetIndexer();

            double error;
            double new_pixel;
            double old_pixel;
            // -------------------------------------------------------------- 데이터 직접접근을 통한 처리.
            for (int y = 0; y < img_in.Rows - 1; y++)
            {
                for (int x = 1; x < img_in.Cols - 1; x++)
                {
                    // 1. 양자화
                    old_pixel = indexer_in[y, x]; // TODO: 깊은/얕은 복사 유무 확인해둘것.
                    new_pixel = (double)find_closest_color(old_pixel);
                    indexer_in[y, x] = new_pixel;

                    // 2. 오차확산
                    error = old_pixel - new_pixel;

                    indexer_in[y, x + 1] += (double)Math.Round(error * 7.0 / 16.0);
                    indexer_in[y + 1, x - 1] += (double)Math.Round(error * 3.0 / 16.0);
                    indexer_in[y + 1, x] += (double)Math.Round(error * 5.0 / 16.0);
                    indexer_in[y + 1, x + 1] += (double)Math.Round(error * 1.0 / 16.0);

                }
            }

            // 의문점: 필터 적용범위를 줄이는 대신 출력 이미지 크기를 좌,우,하단 1열/1행씩 늘리면?

            // ------------------------------------- double(CV_64F)로 변환했던 엔트리타입 byte(CV_8U)로 되돌리기
            img_in.ConvertTo(img_in, MatType.CV_8U);

            return img_in;
        }
        private static byte find_closest_color(double color_in)
        {
            //byte color_out;
            //color_out = (byte)(((byte)Math.Round(color_in / 255.0)) * 255);
            //return color_out;

            byte color_out;
            if (color_in > 127)
            {
                color_out = 255;
            }
            else
            {
                color_out = 0;
            }
            return color_out;
        }
    }
}

