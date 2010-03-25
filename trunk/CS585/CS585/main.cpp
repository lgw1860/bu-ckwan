#include <iostream>
#include <string>
#include <cv.h>
#include <highgui.h>

using namespace cv;

#include "CS585Hw4.h"

int main(int argc, char* argv[])
{
	CS585Hw4 homework;

	string menu = "\nImage Registration of Lung Images\n___________________________________"
		"\nSelect a number of landmark points: "
		"\n (a) 15\n (b) 10\n (c) 5\n (d) 3\n (e) 2\n (q) Quit\n"
		"\nOption: ";

	char option = ' ';
	cout << menu;
	cin >> option;

	//keep prompting for option until 'q' entered
	while(option != 'q')
	{
		switch(option)
		{
			case 'a':
				homework.registerImages(15);
				break;
			case 'b':
				homework.registerImages(10);
				break;
			case 'c':
				homework.registerImages(5);
				break;
			case 'd':
				homework.registerImages(3);
				break;
			case 'e':
				homework.registerImages(2);
				break;
			case 'q':
				cout << "\nQuit:\n" << endl;
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
