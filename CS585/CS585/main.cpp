#include <iostream>
#include <string>
#include <cv.h>
#include <highgui.h>

using namespace cv;

#include "CS585Hw4.h"

int main(int argc, char* argv[])
{
	CS585Hw4 homework;

	string menu = "\nImage Registration\n__________________\nSelect an option: "
		"\n (l) Lung\n (q) Quit\n"
		"\nOption: ";

	char option = ' ';
	cout << menu;
	cin >> option;

	//keep prompting for option until 'q' entered
	while(option != 'q')
	{
		switch(option)
		{
			case 'l':
				cout << "\nLung Dataset:\n" << endl;
				homework.processImages("Datasets/Lung", 2, option);
				break;
			case 'q':
				cout << "\nQuit:\n" << endl;
				break;
			case 't':
				cout << "\nTest Dataset:\n" << endl;
				homework.processImages("Datasets/Test", 4, option);
				break;
			default:
				cout << "\nPlease select a different option.\n" << endl;
		}

		//prompt for option again
		cout << menu;
		cin >> option;
	}

    return 0;
}//end main
