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

	}//end constructor

	//process images in a folder using the procedure defined by option
	void processImages(string folderName, int numImages, char option)
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

		//1. Get n corresponding point pairs (landmarks)
		getLandmarks();
		image1.copyTo(image1WithLandmarks);
		image2.copyTo(image2WithLandmarks);

		//2. Compute centroids
		computeCentroid(*image1Landmarks,xBar1,yBar1);
		computeCentroid(*image2Landmarks,xBar2,yBar2);
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
		Mat rotTransMat = (Mat_<double>(2,3) << cos(theta), -1*sin(theta), x0, sin(theta), cos(theta), y0); //for testing
		
		/*
		for(int row=0; row<rotTransMat.rows; row++)
		{
			for(int col=0; col<rotTransMat.cols; col++)
			{
				//at uses row,col coords, not x,y coords!
				cout << rotTransMat.at<double>(row,col) << endl;
			}//end for col
			cout << endl;
		}//end for row
		*/

		warpAffine(image2,image2Transformed,rotTransMat,image2.size(),WARP_INVERSE_MAP);

		//7. Compute error image between image2transformed and image 1
		computeErrorImage(image1,image2Transformed,errorImage);

		//8. Output parameters to textfile


/*
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
		*/


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
			cout << "(" << vec[i].x << "," << vec[i].y << ") ";
		}//end for
		cout << endl;
	}//end printVector

	
	//get n corresponding point pairs (landmarks) in two images
	//these values have been picked by me and hardcoded into the method
	void getLandmarks()
	{
		//initialize data structures for storing landmarks
		image1Landmarks = new vector<Point>();
		image2Landmarks = new vector<Point>();

		//case
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
		
		//output a list of the landmarks
		cout << "Image 1 Landmarks (" << image1Landmarks->size() << "):" << endl;
		this->printVector(*image1Landmarks);
		cout << endl;

		cout << "Image 2 Landmarks (" << image2Landmarks->size() << "):" << endl;
		this->printVector(*image2Landmarks);
		cout << endl;
	}//end getLandmarks

	
	//compute the coordinates of the centroid of a set of points
	void computeCentroid(vector<Point>& points, double& xBar, double& yBar)
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
	void computeErrorImage(Mat& src1, Mat& src2, Mat& dst)
	{
		//convert both images to grayscale so we only have to compare one channel
		Mat src1Gray;
		Mat src2Gray;

		cvtColor(src1,src1Gray,CV_BGR2GRAY);
		cvtColor(src2,src2Gray,CV_BGR2GRAY);

		src1Gray.copyTo(dst);

		int curImg1Val = 0;	//current pixel in image1 being compared
		int curImg2Val = 0;	//current pixel in image2 being compared

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
			}//end for y
		}//end for x
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

};//end class

#endif
