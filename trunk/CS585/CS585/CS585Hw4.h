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
		//initialize data structures

	}//end constructor

	//process images in a folder using the procedure defined by option
	void processImages(string folderName, int numImages, char option)
	{
		//1. get n corresponding point pair landmarks
		getLandmarks();

		//2. compute centroids
		computeCentroids(*image1Landmarks,xBar1,yBar1);
		cout << "xBar1: " << xBar1 << endl;
		cout << "yBar1: " << yBar1 << endl;

		cout << endl;

		computeCentroids(*image2Landmarks,xBar2,yBar2);
		cout << "xBar2: " << xBar2 << endl;
		cout << "yBar2: " << yBar2 << endl;

		//3. Transform point coordinates
		//4. Compute theta
		computeTheta(*image1Landmarks,*image2Landmarks,xBar1,yBar1,xBar2,yBar2,theta);
		cout << "theta: " << theta << endl;

		//5. Compute translation vector: r0 = (x0, y0)
		computeTranslation(xBar1,yBar1,xBar2,yBar2,theta,x0,y0);
		cout << "x0: " << x0 << endl;
		cout << "y0: " << y0 << endl;


		//6. Map image 2 to image 1 with warpAffine
		//Combine rotation and translation into one 2x3 matrix
		//Mat rotTrans = Mat(2,3,CV_32FC1);

		//from: http://opencv.willowgarage.com/documentation/cpp/basic_structures.html?#mat
		//create 2x3 double-precision matrix
		//Mat rotTrans = (Mat_<double>(2,3) << cos(theta), -1*sin(theta), x0, sin(theta), cos(theta), y0);

		//Mat rotTrans = (Mat_<double>(2,3) << 1, 2, 3, 4, 5, 6);


		/*
		double a[] = { 1,  2,  3,  4,
               5,  6,  7,  8,
               9, 10, 11, 12 };

CvMat Ma=cvMat(3, 4, CV_64FC1, a);
		*/


		double a[] = { 1,  2,  3,  4,
               5,  6,  7,  8,
               9, 10, 11, 12 };

		//CvMat rotTrans=cvMat(3, 4, CV_64FC1, a);

		//cout << cvmGet(&rotTrans,0,1) << endl;

		Mat rotTrans = Mat(3,4,CV_32FC1, a);
		
		for(int x=0; x<rotTrans.cols; x++)
		{
			for(int y=0; y<rotTrans.rows; y++)
			{
				//cout << rotTrans.data[y*rotTrans.step+rotTrans.channels()*x] << endl;
				//cout << rotTrans(y,x) << endl;
				//cout << cvmGet(&rotTrans,x,y) << endl;

				cout << rotTrans.at<double>(x,y) << endl;

			}//end for y
			cout << endl;
		}//end for x
		
		//cout << sizeof(double) << endl;

		for (int imageNum = 1; imageNum <= numImages; imageNum++)
		{
			Mat image;
			Mat processedImage;
			
			//reading in images from a folder
			string filename;
			filename.append(folderName);
			filename.append("/");
			filename.append("0");
			filename.append(intToString(imageNum));
			
			//output textfile filename - add .txt
			string outputTXTFilename;
			outputTXTFilename.append(filename);
			outputTXTFilename.append(".txt");

			filename.append(".jpg");
			image = imread(filename);

			//show the original image
			namedWindow(filename,CV_WINDOW_AUTOSIZE);
			imshow(filename, image);

			//copy from image to processedImage
			image.copyTo(processedImage);

			//different functions depending on which dataset(option) you are analyzing
			switch(option)
			{
				case 'l':
					cout << "Lung function:\n" << endl;
					this->functionLung(processedImage,processedImage);
					break;
				case 'q':
					cout << "Quit\n" << endl;
					break;
				case 't':
					cout << "Test\n" << endl;
					break;
				default:
					cout << "Not a valid option.\n" << endl;
			}

			//cleanup data structures in preparation for another dataset
			//this->cleanup();

			//write output images to same folder
			string outname;
			outname.append(folderName);
			outname.append("/");
			outname.append("out");
			outname.append("0");
			outname.append(intToString(imageNum));
			outname.append(".jpg");

			cout << "Output image: " << outname << endl;
			imwrite(outname,processedImage);

			//show the processed image
			namedWindow(outname,CV_WINDOW_AUTOSIZE);
			imshow(outname, processedImage);

			//wait 1 sec btn each image so you can actually see it
			waitKey(1000);
		}//end for
		
		//cleanup in prep for the next call to processImages
		destroyDataStructures();
		cvDestroyAllWindows();

		cout << endl;

	}//end processImages


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
		
	//print out points in a vector
	void printVector(vector<Point>& vec)
	{
		for(int i=0;i<(int)vec.size();i++)
		{
			cout << vec[i].x << "," << vec[i].y << endl;
		}
	}


	
	void getLandmarks()
	{
		image1Landmarks = new vector<Point>();
		image2Landmarks = new vector<Point>();

		//hard coded values for testing

		/*
		image1Landmarks->push_back(Point(90,90));
		image2Landmarks->push_back(Point(90,90));
		*/

		
		image1Landmarks->push_back(Point(232,112));
		image2Landmarks->push_back(Point(224,122));

		image1Landmarks->push_back(Point(241,239));
		image2Landmarks->push_back(Point(247,254));

		image1Landmarks->push_back(Point(233,323));
		image2Landmarks->push_back(Point(255,332));

		image1Landmarks->push_back(Point(29,266));
		image2Landmarks->push_back(Point(52,316));

		image1Landmarks->push_back(Point(441,270));
		image2Landmarks->push_back(Point(455,259));
		


		cout << "Image 1 Landmarks:" << endl;
		this->printVector(*image1Landmarks);
		cout << endl;

		cout << "Image 2 Landmarks:" << endl;
		this->printVector(*image2Landmarks);
		cout << endl;
	}

	
	void computeCentroids(vector<Point>& points, double& xBar, double& yBar)
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
	}//end computeCentroids

	
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
	

	void computeTranslation(double& xBar1, double& yBar1, double& xBar2, double& yBar2, 
		double& theta, double& x0, double& y0)
	{
		//r0		= rBar_r - R*rBar_l
		//(x0,y0)	= (xBar_r, yBar_r) - R*(xBar_l, yBar_l)
		/* R =	[cosTheta	-sinTheta]
				[sinTheta	cosTheta ] */

		double cosTheta = cos(theta);
		double sinTheta = sin(theta);
		//double x0 = 0.0;
		//double y0 = 0.0;

		x0 = xBar2 - (cosTheta * xBar1 - sinTheta * yBar1);
		y0 = yBar2 - (sinTheta * xBar1 + cosTheta * yBar1);
	}//end computeTranslation



	//user warpAffine instead!
	void mapImage(Mat& src, Mat& dst, double& theta, double& x0, double& y0)
	{
		src.copyTo(dst);

		for(int x=0; x<dst.cols; x++)
		{
			for(int y=0; y<dst.rows; y++)
			{
				//dst.data[y*dst.step+dst.channels()*x] 
				//dst.data[y*dst.step+dst.channels()*x]
			}//end for y
		}//end for x

	}//end mapImage

	//Find the two tumors in the picture of the lung (on the right)
	void functionLung(Mat& src, Mat& dst)
	{
		src.copyTo(dst);

	}//end functionLung


	
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

};//end class

#endif
