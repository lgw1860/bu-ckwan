#ifndef CS585HW4
#define CS585HW4

#include <cv.h>
#include <string>
#include <sstream>
#include <cmath>
#include <stack>
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
		connCompStack = NULL;
		regionsVector = NULL;
	}//end constructor

	//process images in a folder using the procedure defined by option
	void processImages(string folderName, int numImages, char option)
	{
		for (int imageNum = 1; imageNum <= numImages; imageNum++)
		{
			Mat image;
			Mat binaryImage;
			Mat processedImage;
			
			//reading in images from a folder
			string filename;
			filename.append(folderName);
			filename.append("/");
			filename.append("0");
			filename.append(intToString(imageNum));
			
			//binary image filename - just add a b
			string binaryFilename;
			binaryFilename.append(filename);	
			binaryFilename.append("b.jpg");
			
			//output textfile filename - add .txt
			string outputTXTFilename;
			outputTXTFilename.append(filename);
			outputTXTFilename.append(".txt");

			filename.append(".jpg");
			image = imread(filename);

			//show the original image
			namedWindow(filename,CV_WINDOW_AUTOSIZE);
			imshow(filename, image);
		
			//convert to binary image
				//createBinaryImage(image,binaryImage);
			//use pre-converted (Irfanview) image instead
			binaryImage = imread(binaryFilename);
			
			//show binary image
			namedWindow(binaryFilename,CV_WINDOW_AUTOSIZE);
			imshow(binaryFilename, binaryImage);

			//connected component labeling algorithm to determine regions
			this->connectedComponents(binaryImage, binaryImage);

			//compute quantities for regions
			//need to use original BGR image so we can color regions different colors
			this->computeQuantities(image, processedImage, outputTXTFilename);

			//different functions depending on which dataset(option) you are analyzing
			switch(option)
			{
				case 'b':
					cout << "Bats function:\n" << endl;
					this->functionBats(processedImage,processedImage);
					break;
				case 'c':
					cout << "Cells function:\n" << endl;
					this->functionCells(processedImage,processedImage);
					break;
				case 'e':
					cout << "Eyes function:\n" << endl;
					this->functionEyes(processedImage,processedImage);
					break;
				case 'h':
					cout << "Hands function:\n" << endl;
					this->functionHands(processedImage,processedImage);
					break;
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
			this->cleanup();

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
		
		//close all the windows in prep for the next call
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
	

	//create a binary image from an image using average brightness value to threshold
	void createBinaryImage(Mat& src, Mat& dst)
	{
		src.copyTo(dst);

		//convert image to grayscale
		cvtColor(src,dst,CV_BGR2GRAY);

		//find average brightness value
		int numPixels = dst.cols * dst.rows;
		int sumBrightness = 0;
		double avgBrightness = 0;
		int currentBrightness = 0;

		for(int x=0; x<dst.cols; x++)
		{
			for(int y=0; y<dst.rows; y++)
			{
				currentBrightness = dst.data[y*dst.step+dst.channels()*x];
				sumBrightness += currentBrightness;
			}//end for y
		}//end for x

		avgBrightness = (double)sumBrightness / (double)numPixels;

		threshold(dst,dst,avgBrightness,255,THRESH_BINARY);
	}//end createBinaryImage


	
	//connected components labeling algorithm
	//iterative with a stack (recursive would overflow the callstack)
	void connectedComponents(Mat& src, Mat& dst)
	{
		src.copyTo(dst);

		//allocate data structures
		this->connCompStack = new stack<Point>();
		this->regionsVector = new vector< vector<Point>* >();

		int currentRegionNum = 0;			//current region we are adding to

		//go through all pixels in image
		for(int x=0; x<dst.cols; x++)
		{
			for(int y=0; y<dst.rows; y++)
			{
				//if pixel has not been assigned to a region yet
				if(dst.data[y*dst.step+dst.channels()*x] == this->UNMARKED_VAL)
				{
					//it will be the first pixel in a new region

					//allocate a new vector for the region
					vector<Point>* vecToAdd = new vector<Point>();
					this->regionsVector->push_back(vecToAdd);

					//mark the pixel
					dst.data[y*dst.step+dst.channels()*x] = this->MARKED_VAL;

					//insert the pixel into the vector for the region
					this->regionsVector->at(currentRegionNum)->push_back(Point(x,y));

					//push the pixel onto the stack
					this->connCompStack->push(Point(x,y));

					//grab the rest of the pixels in the region!
					while(!(this->connCompStack->empty()))
					{
						//peek and pop off the top of the stack
						Point top = this->connCompStack->top();
						this->connCompStack->pop();

						//add all of top's neighbors onto the stack (using N8 connectedness)
						for(int neighX = top.x-1; neighX <= top.x+1; neighX++)
						{
							for(int neighY = top.y-1; neighY <= top.y+1; neighY++)
							{
								//check that neighbor coordinates is within bounds of image
								//and is unmarked (not added to a region yet)
								if(
									(neighX >= 0 && neighX < dst.cols)
									&&
									(neighY >= 0 && neighY < dst.rows)
									&&
									(dst.data[neighY*dst.step+dst.channels()*neighX] == this->UNMARKED_VAL)
									)
								{
									//mark the neighbor
									dst.data[neighY*dst.step+dst.channels()*neighX] = this->MARKED_VAL;

									//insert the neighbor into the vector for the region
									this->regionsVector->at(currentRegionNum)->push_back(Point(neighX,neighY));

									//push the pixel onto the stack
									this->connCompStack->push(Point(neighX,neighY));
								}//end if
							}//end for neighY
						}//end for neighX
					}//end while

					//at this point the stack should be empty
					//and we have added all the pixels in the current region
					//so increment for the next region
					currentRegionNum++;
				}//end if

				//else the pixel is background or already marked

			}//end for y
		}//end for x
	}//end connectedComponents()
	

	//compute quantities and label regions with different colors and numbers
	void computeQuantities(Mat& src, Mat& dst, string filename)
	{
		src.copyTo(dst);

		//report of quantities for each region, will get written to a file
		string outputFilename = filename;
		string outputReport = "#:\tArea:\tOrientation:\tCircularity:\t"
			"Hu 1:\tHu 2:\tHu 3:\tHu 4:\tHu 5:\tHu 6:\tHu 7:\t\n";

		//initialize quantity vectors to size of regionVector
		areaVector = new vector<int>(this->regionsVector->size());
		orientationVector = new vector<double>(this->regionsVector->size());
		circularityVector = new vector<double>(this->regionsVector->size());
		xBarVector = new vector<double>(this->regionsVector->size());
		yBarVector = new vector<double>(this->regionsVector->size());

		//variables for current region being looked at
		vector<Point> *curRegion = NULL;
		int currentX = 0;
		int currentY = 0;
		int currentBlue = 0;
		int currentGreen = 0;
		int currentRed = 0;
		int area = 0;
		int sumX = 0;
		int sumY = 0;
		double xBar = 0.0;
		double yBar = 0.0;

		//variables for computing a, b, c for orientation and circularity:
		double a = 0.0;
		double b = 0.0;
		double c = 0.0;
		double tan2Alpha = 0.0;
		double alpha = 0.0;
		double EMin = 0.0; //used for circularity
		double EMax = 0.0; //used for circularity
		double sqrtThing = 0.0; //used for circularity: sqrt(b^2 + (a-c)^2)
		double circularity = 0.0;

		int sumXSquared = 0;
		int sumYSquared = 0;
		int sumXY = 0;

		for(int curRegionNum = 0; curRegionNum < (int)this->regionsVector->size(); curRegionNum++)
		{
			curRegion = this->regionsVector->at(curRegionNum);

			Point *curRegionPoints;
			curRegionPoints = new Point[curRegion->size()];
			
			//randomly generate the color of curRegion
			currentBlue = rand() % 256;
			currentGreen = rand() % 256;
			currentRed = rand() % 256;

			//reset orientation variables:
			sumX = 0;
			sumY = 0;
			sumXSquared = 0;
			sumYSquared = 0;
			sumXY = 0;

			//iterate through all points of currentRegion
			for(int curRegionIndex = 0; curRegionIndex < (int)curRegion->size(); curRegionIndex++)
			{
				currentX = curRegion->at(curRegionIndex).x;
				currentY = curRegion->at(curRegionIndex).y;

				sumX += currentX;
				sumY += currentY;
				sumXSquared += (currentX * currentX);
				sumYSquared += (currentY * currentY);
				sumXY += (currentX * currentY);

				//color the current pixel the region's color
				dst.data[currentY*dst.step+dst.channels()*currentX+0] = currentBlue;
				dst.data[currentY*dst.step+dst.channels()*currentX+1] = currentGreen;
				dst.data[currentY*dst.step+dst.channels()*currentX+2] = currentRed;
			}//end for curRegionIndex

			//compute area = # pixels in region = size of the vector
			area = (int)curRegion->size();
			areaVector->at(curRegionNum) = area;

			//centroid coordinates
			xBar = (double)sumX / (double)area;
			yBar = (double)sumY / (double)area;
			xBarVector->at(curRegionNum) = xBar;
			yBarVector->at(curRegionNum) = yBar;

			//compute orientation
			a = (double)((sumXSquared) - (2*xBar*sumX) + (xBar*xBar*area));
			c = (double)((sumYSquared) - (2*yBar*sumY) + (yBar*yBar*area));
			b = 2.0 * (double)((sumXY) - (yBar*sumX) - (xBar*sumY) + (xBar*yBar*area));

			if(a != c)
			{
				tan2Alpha = b / (a - c);
				alpha = 0.5 * atan(tan2Alpha); //angle in radians
			}//end if
			else
			{
				tan2Alpha = -1;
				alpha = -1;
			}//end else

			orientationVector->at(curRegionNum) = alpha;

			//compute circularity
			sqrtThing = sqrt((b*b + (a-c)*(a-c)));
			EMin = ((a+c)/2.0) - ((a-c)/2.0)*(1*(a-c)/sqrtThing) - 0.5*b*(1*b/sqrtThing);
			EMax = ((a+c)/2.0) - ((a-c)/2.0)*(-1*(a-c)/sqrtThing) - 0.5*b*(-1*b/sqrtThing);
			circularity = EMin / EMax;
			circularityVector->at(curRegionNum) = circularity;

			//compute hu moments
			Mat grayDst;
			cvtColor(dst,grayDst,CV_BGR2GRAY);
			Moments imgMoments = moments(grayDst,true);
			double huMoments[7];
			HuMoments(imgMoments, huMoments);
			
			//draw region number on centroid of region if region is more than a few pixels
			if(area > 2)
			{
				putText(dst,intToString(curRegionNum),Point((int)xBar,(int)yBar),FONT_HERSHEY_PLAIN,0.5,Scalar(0,0,0));
			}
			
			
			//add quantities to output report:
			//region #:
			outputReport.append(intToString(curRegionNum));			
			outputReport.append("\t");
			
			//area:
			outputReport.append(intToString(area));
			outputReport.append("\t");
			
			//orientation:
			outputReport.append(doubleToString(alpha));
			outputReport.append("\t");
			
			//circularity:
			outputReport.append(doubleToString(circularity));
			outputReport.append("\t");
			
			//Hu moments:
			for(int i=0; i<7; i++)
			{
				outputReport.append(doubleToString(huMoments[i]));
				outputReport.append("\t");
			}
			
			outputReport.append("\n");

		}//end for curRegionNum

		//write quantities to output file
		cout << "\nOutput file: " << outputFilename << "\n" << endl;
		ofstream outputFile;
		outputFile.open(outputFilename.data());
		outputFile << outputReport;
		outputFile.close();

	}//end computeQuantities()

	//Estimate the direction of motion of the bats
	void functionBats(Mat& src, Mat& dst)
	{
		src.copyTo(dst);

		for(int curRegionNum = 0; curRegionNum < (int)this->regionsVector->size(); curRegionNum++)
		{
			double xBar = this->xBarVector->at(curRegionNum);
			double yBar = this->yBarVector->at(curRegionNum);
			double alpha = this->orientationVector->at(curRegionNum);
			
			//draw lines perpendicular to the orientation of each bat
			//because orientation is based on the wingspan
			if(alpha != -1)
			{
				line(dst,Point((int)xBar,(int)yBar),Point((int)(xBar + 50*cos(alpha - 3.14/2.0)),(int)(yBar + 50*sin(alpha - 3.14/2.0))),Scalar(0,255,0));
			}
		}//end for curRegionNum
	}//end functionBats


	//Estimate the direction of motion of the cells
	void functionCells(Mat& src, Mat& dst)
	{
		src.copyTo(dst);

		int cellAreaThresh = 100;

		for(int curRegionNum = 0; curRegionNum < (int)this->regionsVector->size(); curRegionNum++)
		{
			double xBar = this->xBarVector->at(curRegionNum);
			double yBar = this->yBarVector->at(curRegionNum);
			double alpha = this->orientationVector->at(curRegionNum);
			double area = this->areaVector->at(curRegionNum);
			
			//draw lines along the orientation of the cells
			if(area > cellAreaThresh && alpha != -1)
			{
				line(dst,Point((int)xBar,(int)yBar),Point((int)(xBar + 50*cos(alpha)),(int)(yBar + 50*sin(alpha))),Scalar(0,255,0),2);
			}
		}//end for curRegionNum
	}//end functionCells


	//Find the irises or pupils in the eye images
	void functionEyes(Mat& src, Mat& dst)
	{
		src.copyTo(dst);

		int maxAreaIndex = 0;

		for(int curRegionNum = 0; curRegionNum < (int)this->regionsVector->size(); curRegionNum++)
		{
			double xBar = this->xBarVector->at(curRegionNum);
			double yBar = this->yBarVector->at(curRegionNum);
			double area = this->areaVector->at(curRegionNum);
			
			//the eye has the most area in the image
			//the centroid is approximately where the iris and pupil are
			if(this->areaVector->at(curRegionNum) > this->areaVector->at(maxAreaIndex))
			{
				maxAreaIndex = curRegionNum;
			}

		}//end for curRegionNum

		//circle the eye
		double xBar = this->xBarVector->at(maxAreaIndex);
		double yBar = this->yBarVector->at(maxAreaIndex);
		double maxArea = this->areaVector->at(maxAreaIndex);
		circle(dst,Point((int)xBar,(int)yBar),(int)(sqrt(maxArea)/4.0),Scalar(0,255,0),2);

	}//end functionEyes


	//Choose the open hand in the hand images
	void functionHands(Mat& src, Mat& dst)
	{
		src.copyTo(dst);

		double handAreaThresh = 30000.0;

		for(int curRegionNum = 0; curRegionNum < (int)this->regionsVector->size(); curRegionNum++)
		{
			double xBar = this->xBarVector->at(curRegionNum);
			double yBar = this->yBarVector->at(curRegionNum);
			double area = this->areaVector->at(curRegionNum);
			
			//the open hand has a greater area than the closed hand or pen
			//and is around a certain size (above an area threshold)
			if(area > handAreaThresh)
			{
				circle(dst,Point((int)xBar,(int)yBar),(int)sqrt(area),Scalar(0,255,0),2);
			}

		}//end for curRegionNum
	}//end functionHands


	//Find the two tumors in the picture of the lung (on the right)
	void functionLung(Mat& src, Mat& dst)
	{
		src.copyTo(dst);

		double tumorCircleThresh = 0.6;

		for(int curRegionNum = 0; curRegionNum < (int)this->regionsVector->size(); curRegionNum++)
		{
			double xBar = this->xBarVector->at(curRegionNum);
			double yBar = this->yBarVector->at(curRegionNum);
			double area = this->areaVector->at(curRegionNum);
			double circularity = this->circularityVector->at(curRegionNum);
			
			//tumors are circular in shape
			//look for regions above a circularity threshold
			//and circle them
			if(circularity > tumorCircleThresh)
			{
				circle(dst,Point((int)xBar,(int)yBar),(int)sqrt(area),Scalar(0,255,0));
			}

		}//end for curRegionNum
	}//end functionLung


	//cleanup for next dataset, free allocated memory for data structures
	void cleanup()
	{
		//clean up stack
		delete(this->connCompStack);
		
		//clean up each vector in regionsVector
		if(regionsVector != NULL)
		{
			for(int i=0; i<(int)regionsVector->size(); i++)
			{
				delete ((vector<Point>*)regionsVector->at(i));
			}
		}
		//clean up regionsVector itself
		delete regionsVector;

		//clean up quantity vectors
		delete areaVector;
		delete orientationVector;
		delete circularityVector;
		delete xBarVector;
		delete yBarVector;

	}//end cleanup

	
	//Class variables:

	//stack for connectedComponents()
	stack<Point> *connCompStack;	
	
	//regions of the image
	//each internal vector is a region of Points
	vector< vector<Point>* > *regionsVector;
	vector<int> *areaVector;
	vector<double> *orientationVector;
	vector<double> *circularityVector;
	vector<double> *xBarVector;
	vector<double> *yBarVector;

	//brightness values for connectedComponents()
	static const int BACKGROUND_VAL = 0;	//pixel is part of background
	static const int UNMARKED_VAL = 255;	//pixel has not been assigned a region yet
	static const int MARKED_VAL = 128;		//pixel has already been assigned to a region

};//end class

#endif
