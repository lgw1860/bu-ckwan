#ifndef CS585HW4
#define CS585HW4

#include <cv.h>
#include <string>
#include <sstream>
#include <cmath>
#include <vector>
#include <iostream>
#include <fstream>

using namespace std;

class CS585Hw4
{
public:
	CS585Hw4()
	{
		//initialize class variables
		image1Landmarks = NULL;
		image2Landmarks = NULL;
		xBar1 = 0.0;
		yBar1 = 0.0;
		xBar2 = 0.0;
		yBar2 = 0.0;
		theta = 0.0;
		x0 = 0.0;
		y0 = 0.0;
		errorSum = 0;

	}//end constructor

	//process images in a folder using the procedure defined by option
	void registerImages(int numLandmarkPoints)
	{
		Mat image1;
		Mat image2;
		Mat image1WithLandmarks;
		Mat image2WithLandmarks;
		Mat image2Transformed;
		Mat errorImage;
		
		//0. Read in images from folders
		image1 = imread("Datasets/Lung/01.jpg");
		image2 = imread("Datasets/Lung/02.jpg");

		//1. Set n corresponding point pairs (landmarks)
		image1.copyTo(image1WithLandmarks);
		image2.copyTo(image2WithLandmarks);
		setLandmarks(image1WithLandmarks,image2WithLandmarks,numLandmarkPoints);

		//2. Compute centroids
		computeCentroid(image1WithLandmarks,*image1Landmarks,xBar1,yBar1);
		computeCentroid(image2WithLandmarks,*image2Landmarks,xBar2,yBar2);
		cout << "Centroid of Image 1: (xBar1, yBar1) = (" << xBar1 << ", " << yBar1 << ")" << endl;
		cout << endl;
		cout << "Centroid of Image 2: (xBar2, yBar2) = (" << xBar2 << ", " << yBar2 << ")" << endl;
		cout << endl;

		//3. Transform point coordinates
		//4. Compute theta
		computeTheta(*image1Landmarks,*image2Landmarks,xBar1,yBar1,xBar2,yBar2,theta);
		cout << "Rotation angle: theta = " << theta << endl;
		cout << endl;

		//5. Compute translation vector: r0 = (x0, y0)
		computeTranslation(xBar1,yBar1,xBar2,yBar2,theta,x0,y0);
		cout << "Translation vector: (x0, y0) = (" << x0 << ", " << y0 << ")" << endl;
		cout << endl;

		//6. Map image 2 to image 1 with warpAffine
		//Combine rotation and translation into one 2x3 matrix as input to warpAffine
		Mat rotTransMat = (Mat_<double>(2,3) << cos(theta), -1*sin(theta), x0, sin(theta), cos(theta), y0);
		warpAffine(image2,image2Transformed,rotTransMat,image2.size(),WARP_INVERSE_MAP);

		//7. Compute error image between image2transformed and image 1
		computeErrorImage(image1,image2Transformed,errorImage,errorSum);
		cout << "Error sum: " << errorSum << endl;
		cout << endl;

		//output processed images to files
		imwrite("Datasets/Lung/01withLandmarks.jpg",image1WithLandmarks);
		imwrite("Datasets/Lung/02withLandmarks.jpg",image2WithLandmarks);
		imwrite("Datasets/Lung/02Transformed.jpg",image2Transformed);
		imwrite("Datasets/Lung/errorImage.jpg",errorImage);

		//show the processed images
		namedWindow("Image 1",CV_WINDOW_AUTOSIZE);		
		namedWindow("Image 2",CV_WINDOW_AUTOSIZE);
		namedWindow("Image 1 With Landmarks",CV_WINDOW_AUTOSIZE);		
		namedWindow("Image 2 With Landmarks",CV_WINDOW_AUTOSIZE);
		namedWindow("Image 2 Transformed",CV_WINDOW_AUTOSIZE);
		namedWindow("Error Image",CV_WINDOW_AUTOSIZE);	

		imshow("Image 1", image1);
		imshow("Image 2", image2);
		imshow("Image 1 With Landmarks", image1WithLandmarks);
		imshow("Image 2 With Landmarks", image2WithLandmarks);
		imshow("Image 2 Transformed", image2Transformed);
		imshow("Error Image", errorImage);
		
		//wait so user has time to look at the images
		waitKey(5000);
		
		//cleanup in prep for the next call to registerImages
		destroyDataStructures();
		cvDestroyAllWindows();

		cout << "Please see Datasets/Lung/ for the output images." << endl;
		cout << endl;
	}//end registerImages


private:
	//convert an integer to a string
	string intToString(int number)
	{
		stringstream ss;
		string s;
		ss << number;
		s.append(ss.str());
		return s;
	}//end intToString()


	
	//convert a double to a string
	string doubleToString(double number)
	{
		stringstream ss;
		string s;
		ss << number;
		s.append(ss.str());
		return s;
	}//end intToString()
		
	//output landmarks to console and mark them on the image
	void outputLandmarks(Mat& dst, vector<Point>& landmarkPts)
	{
		for(int i=0;i<(int)landmarkPts.size();i++)
		{
			cout << "(" << landmarkPts[i].x << "," << landmarkPts[i].y << ") ";
			
			//draw green circles over the landmarks in the image
			circle(dst,landmarkPts[i],2,Scalar(0,255,0),2);

		}//end for
		cout << endl;
	}//end outputLandmarks

	
	//set n corresponding point pairs (landmarks) in two images
	//these values have been picked by me and hardcoded into the method
	//n presets: 1, 2, 5, 10, 15 
	void setLandmarks(Mat& dst1, Mat& dst2, int numLandmarks)
	{
		//initialize data structures for storing landmarks
		image1Landmarks = new vector<Point>();
		image2Landmarks = new vector<Point>();

		//note: there are no 'breaks' intentionally (except for the last case)
		//so that cases fall through and all the points get added
		switch(numLandmarks)
		{
			case 15:
				//top white oval
				image1Landmarks->push_back(Point(253,114));
				image2Landmarks->push_back(Point(241,124));
				//middle dark spot
				image1Landmarks->push_back(Point(238,252));
				image2Landmarks->push_back(Point(243,262));
				//bottom middle circle
				image1Landmarks->push_back(Point(232,333));
				image2Landmarks->push_back(Point(255,342));
				//bottom left squiggle
				image1Landmarks->push_back(Point(56,298));
				image2Landmarks->push_back(Point(82,342));
				//bottom right squiggle
				image1Landmarks->push_back(Point(434,285));
				image2Landmarks->push_back(Point(448,277));
			case 10:
				//top white oval
				image1Landmarks->push_back(Point(218,104));
				image2Landmarks->push_back(Point(205,117));
				//middle dark spot
				image1Landmarks->push_back(Point(241,228));
				image2Landmarks->push_back(Point(244,240));
				//bottom middle circle
				image1Landmarks->push_back(Point(232,313));
				image2Landmarks->push_back(Point(253,321));
				//bottom left squiggle
				image1Landmarks->push_back(Point(30,255));
				image2Landmarks->push_back(Point(47,306));
				//bottom right squiggle
				image1Landmarks->push_back(Point(443,252));
				image2Landmarks->push_back(Point(457,242));
			case 5:
				//bottom left squiggle
				image1Landmarks->push_back(Point(29,266));
				image2Landmarks->push_back(Point(52,316));
				//bottom right squiggle
				image1Landmarks->push_back(Point(441,270));
				image2Landmarks->push_back(Point(455,259));
			case 3:
				//middle dark spot
				image1Landmarks->push_back(Point(241,239));
				image2Landmarks->push_back(Point(247,254));
			case 2:
				//top white oval
				image1Landmarks->push_back(Point(232,112));
				image2Landmarks->push_back(Point(224,122));
				//bottom middle circle
				image1Landmarks->push_back(Point(233,323));
				image2Landmarks->push_back(Point(255,332));
				break;
			default:
				cout << "Not a valid number of landmarks.\n" << endl;
		}
		
		//output a list of the landmarks
		cout << "Image 1 Landmarks (" << image1Landmarks->size() << "):" << endl;
		this->outputLandmarks(dst1,*image1Landmarks);
		cout << endl;

		cout << "Image 2 Landmarks (" << image2Landmarks->size() << "):" << endl;
		this->outputLandmarks(dst2,*image2Landmarks);
		cout << endl;

	}//end setLandmarks

	
	//compute the coordinates of the centroid of a set of points and mark it on the image
	void computeCentroid(Mat& dst, vector<Point>& points, double& xBar, double& yBar)
	{
		int sumX = 0;
		int sumY = 0;

		int numPoints = 0;
		numPoints = (int)points.size();

		for(int i=0; i<numPoints; i++)
		{
			sumX += points[i].x;
			sumY += points[i].y;
		}//end for

		if(numPoints > 0)
		{
			xBar = (double)sumX / (double)numPoints;
			yBar = (double)sumY / (double)numPoints;
		}//end if

		//draw a red circle over the centroid in the image
		circle(dst,Point((int)xBar,(int)yBar),2,Scalar(0,0,255),2);

	}//end computeCentroid

	
	//compute the angle, theta, in order to transform point coordinates
	void computeTheta(vector<Point>& img1Pts, vector<Point>& img2Pts, 
		double& xBar1, double& yBar1, double& xBar2, double& yBar2, double& theta)
	{
								//l = left = 1, and r = right = 2
		double numer = 0.0;		//SUM { y'_r,i * x'_l,i - x'_r,i * y'_l,i }
		double denom = 0.0;		//SUM { x'_r,i * x'_l,i + y'_r,i * y'_l,i }
		double tanTheta = 0.0;	//numer / denom

		double xPrime1 = 0.0;	//x'_l,i = x_l,i - xBar_l
		double yPrime1 = 0.0;	//y'_l,i = y_l,i - yBar_l
		double xPrime2 = 0.0;	//x'_r,i = x_r,i - xBar_r
		double yPrime2 = 0.0;	//y'_r,i = y_r,i - yBar_r


		//assume that img1 and img2 have same number of points
		int numPoints = (int)img1Pts.size();

		for(int i=0; i<numPoints; i++)
		{
			xPrime1 = img1Pts[i].x - xBar1;
			yPrime1 = img1Pts[i].y - yBar1;
			xPrime2 = img2Pts[i].x - xBar2;
			yPrime2 = img2Pts[i].y - yBar2;

			numer += yPrime2 * xPrime1 - xPrime2 * yPrime1;
			denom += xPrime2 * xPrime1 + yPrime2 * yPrime1;
		}//end for

		tanTheta = numer / denom;
		theta = atan(tanTheta);
	}//end computeTheta
	
	//compute the translation vector r0 = (x0, y0) given angle and centroids
	void computeTranslation(double& xBar1, double& yBar1, double& xBar2, double& yBar2, 
		double& theta, double& x0, double& y0)
	{
		//r0		= rBar_r - R*rBar_l
		//(x0,y0)	= (xBar_r, yBar_r) - R*(xBar_l, yBar_l)
		/* R =	[cosTheta	-sinTheta]
				[sinTheta	cosTheta ] */

		double cosTheta = cos(theta);
		double sinTheta = sin(theta);

		x0 = xBar2 - (cosTheta * xBar1 - sinTheta * yBar1);
		y0 = yBar2 - (sinTheta * xBar1 + cosTheta * yBar1);
	}//end computeTranslation
	
	//compute the error image of two images
	//assume both src Matrices are the same size
	void computeErrorImage(Mat& src1, Mat& src2, Mat& dst, int& errorSum)
	{
		//convert both images to grayscale so we only have to compare one channel
		Mat src1Gray;
		Mat src2Gray;

		cvtColor(src1,src1Gray,CV_BGR2GRAY);
		cvtColor(src2,src2Gray,CV_BGR2GRAY);

		src1Gray.copyTo(dst);

		int curImg1Val = 0;	//current pixel in image1 being compared
		int curImg2Val = 0;	//current pixel in image2 being compared
		int sumAbsDiff = 0;	//sum of all the absolute pixel intensity differences

		//compare all pixels in both images and create error image
		for(int x=0; x<src1.cols; x++)
		{
			for(int y=0; y<src1.rows; y++)
			{
				curImg1Val = src1Gray.data[y*src1Gray.step+src1Gray.channels()*x];
				curImg2Val = src2Gray.data[y*src2Gray.step+src2Gray.channels()*x];

				//error = absolute difference btn intensities in img2 and img1
				//lighter pixels mean higher error
				dst.data[y*dst.step+dst.channels()*x] = abs(curImg2Val - curImg1Val);
				sumAbsDiff += abs(curImg2Val - curImg1Val);
			}//end for y
		}//end for x

		errorSum = sumAbsDiff;
	}//end computeErrorImage
	
	//free allocated memory for data structures
	void destroyDataStructures()
	{
		//delete landmark vectors
		delete image1Landmarks;
		delete image2Landmarks;

	}//end destroyDataStructures

	
	//Class variables:
	vector<Point> *image1Landmarks;
	vector<Point> *image2Landmarks;
	double xBar1, yBar1, xBar2, yBar2;
	double theta;
	double x0, y0;
	int errorSum;

};//end class

#endif
