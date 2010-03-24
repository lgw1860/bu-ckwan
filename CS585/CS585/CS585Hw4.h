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

	
	void computeCentroids(vector<Point>& points, int& xBar, int& yBar)
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
			xBar = sumX / numPoints;
			yBar = sumY / numPoints;
		}//end if
	}//end computeCentroids

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
	int xBar1, yBar1, xBar2, yBar2;

};//end class

#endif
