#include <iostream>
#include <string>
#include <cv.h>
#include <highgui.h>

using namespace cv;

#include "CS585Hw4.h"

int main(int argc, char* argv[])
{
	CS585Hw4 homework;

	string menu = "\n_________________\nSelect an option: "
		"\n (b) Bats\n (c) Cells\n (e) Eyes\n (h) Hands\n (l) Lung\n (q) Quit\n"
		"\nOption: ";

	char option = ' ';
	cout << menu;
	cin >> option;

	//keep prompting for option until 'q' entered
	while(option != 'q')
	{
		switch(option)
		{
			case 'b':
				cout << "\nBats Dataset:\n" << endl;
				homework.processImages("Datasets/Bats", 5, option);
				break;
			case 'c':
				cout << "\nCells Dataset:\n" << endl;
				homework.processImages("Datasets/Cells", 2, option);
				break;
			case 'e':
				cout << "\nEyes Dataset:\n" << endl;
				homework.processImages("Datasets/Eyes", 9, option);
				break;
			case 'h':
				cout << "\nHands Dataset:\n" << endl;
				homework.processImages("Datasets/Hands", 5, option);
				break;
			case 'l':
				cout << "\nLung Dataset:\n" << endl;
				homework.processImages("Datasets/Lung", 1, option);
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
