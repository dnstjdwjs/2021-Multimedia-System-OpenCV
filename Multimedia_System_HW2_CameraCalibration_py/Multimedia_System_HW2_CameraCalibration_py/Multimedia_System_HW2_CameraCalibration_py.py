
import numpy as np
import cv2
import glob

# termination criteria
criteria = (cv2.TERM_CRITERIA_EPS + cv2.TERM_CRITERIA_MAX_ITER, 30, 0.001)

# prepare object points, like (0,0,0), (1,0,0), (2,0,0) ....,(6,5,0)
objp = np.zeros((5*6,3), np.float32)
objp[:,:2] = np.mgrid[0:5,0:6].T.reshape(-1,2)

# Arrays to store object points and image points from all the images.
objpoints = [] # 3d point in real world space
imgpoints = [] # 2d points in image plane.


images = glob.glob('../../Image Resource/conv_*.jpg')

# 보정결과 출력할 파일 세팅

f = open( '../../for HW/보정결과.txt', 'w')

i=0
for fname in images:
    img = cv2.imread(fname)
    gray = cv2.cvtColor(img,cv2.COLOR_BGR2GRAY)

    # Find the chess board corners
    ret, corners = cv2.findChessboardCorners(gray, (5,6),None)
    
    # If found, add object points, image points (after refining them)
    if (ret == False) | (i>10):
        continue
    print("Start processing Image %s..." % str(i))
    objpoints.append(objp)

    corners2 = cv2.cornerSubPix(gray,corners,(11,11),(-1,-1),criteria)
    imgpoints.append(corners2)

    # Draw and display the corners
    img = cv2.drawChessboardCorners(gray, (5,6), corners2,ret)
        
    cv2.imshow(f'img {i+1}',gray)
    cv2.waitKey(0)

    # Cam calibration
    ret, mtx, dist, rvecs, tvecs = cv2.calibrateCamera(objpoints, imgpoints, gray.shape[::-1],None,None)

    ## Undistortion    

    img = cv2.imread('../../Image Resource/conv_image{0:02d}.jpg'.format(i+1))
    h,  w = img.shape[:2]
    newcameramtx, roi=cv2.getOptimalNewCameraMatrix(mtx,dist,(w,h),1,(w,h))

    # undistort
    dst = cv2.undistort(img, mtx, dist, None, newcameramtx)

    # crop the image
    x,y,w,h = roi
    dst = dst[y:y+h, x:x+w]
    cv2.imwrite('../../Image Resource/undistorted_image{0:02d}.jpg'.format(i+1),dst)
    
    ## show & compare Before/After
    cv2.namedWindow('Before', cv2.WINDOW_AUTOSIZE)
    cv2.imshow('Before', img)
    cv2.moveWindow('Before',10,10)                    
               

    cv2.namedWindow('After', cv2.WINDOW_AUTOSIZE)
    dst = cv2.resize(dst,dsize = tuple(np.flip(img.shape[:2])), interpolation = cv2.INTER_CUBIC )
        
    cv2.imshow('After', dst)
    cv2.moveWindow('After',700,10)        
 
    cv2.waitKey(0)
    cv2.destroyAllWindows()
                
    i += 1

## 보정결과 출력

# 내부 값
print("\nintrinsic"+str(mtx))
f.write("intrinsic\n"+str(mtx))

# 렌즈왜곡값
print("\ndistCoeffs"+str(dist))
f.write("\n\ndistCoeffs"+str(dist))

# mean_error
tot_error = 0
for i in range(len(objpoints)):
    imgpoints2, _ = cv2.projectPoints(objpoints[i], rvecs[i], tvecs[i], mtx, dist)
    error = cv2.norm(imgpoints[i],imgpoints2, cv2.NORM_L2)/len(imgpoints2)
    tot_error += error

print( "\nmean error: ", tot_error/len(objpoints), '\n')
f.write( "\n\nmean error: "+ str(tot_error/len(objpoints))+'\n')

# 외부 값(회전,변환)
for i in range(10):  

    # R vector를 3x1 에서 3x3로 역산
    # 사용하지 않는 jacob을 받는 이유: 형이 자동으로 지정되어 rodri에 (arr,arr)형태로 들어가기 때문에 이상한 출력을 얻게 됨
    rodri,jacob = cv2.Rodrigues(rvecs[i], dst= None ,jacobian = None)

    print('\nrvec %d\n'%(i+1)+str(rodri))
    # print(str(rvecs[i]))
    f.write('\n\nrvec %d\n'%(i+1)+str(rodri))
    # f.write(str(rvecs[i]))
    print(('tvec %d\n'%(i+1)) + str(tvecs[i]))
    # print(str(tvecs[i]))
    f.write('\ntvec %d\n'%(i+1) + str(tvecs[i]))
    # f.write(str(tvecs[i]))
         
# 보정결과 출력 파일
f.close()

cv2.waitKey(0)
cv2.destroyAllWindows()
