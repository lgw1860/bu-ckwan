/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package cs565project3;

import java.io.*;

/**
 *
 * @author Chris
 */
public class Main {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        // TODO code application logic here
        System.out.println("hello world it's me!");

        //Stemmer test
        Stemmer stemmer = new Stemmer();
        String wordsToStem[] = {"algorithm", "algorithmic", "greetings", "algorithms", "Algorithms", "algo"};
        //String wordsToStem[] = {"truck", "trucker", "truckers", "trucks", "trucking"};
        for (int i=0; i<wordsToStem.length; i++)
        {
            //stem each word
//            String curWord = wordsToStem[i].toLowerCase();
//            stemmer.add(curWord.toCharArray(), curWord.length());
//            stemmer.stem();
            System.out.println("orig: " + wordsToStem[i]);
            System.out.println("\tstemmed: " + EmailCleaner.stem(wordsToStem[i]));
            //System.out.println("\tstemmed: " + stemmer.toString());
        }

        //file io test
        System.out.println("Working dir " + System.getProperty("user.dir"));
        String workDir = System.getProperty("user.dir");
        File dir = new File(workDir);
        File[] children = dir.listFiles();
        if(children == null)
        {
            System.out.println("no children");
        }
        else
        {
            for(int i=0; i<children.length; i++)
            {
                System.out.print("name: " + children[i].getName());
                
                if(children[i].isHidden())
                {
                    System.out.print(" is hidden and");
                }

                if(children[i].isFile())
                {
                    System.out.print(" is a file\n");
                }
                else if(children[i].isDirectory())
                {
                    System.out.print(" is a directory\n");
                }
                else
                {
                    System.out.print(" is neither!");
                }

            }//end for
        }//end else

        //EmailCleaner test
        //String filename = "testalgo.txt";
        String filename = "0001.ea7e79d3153e7469e7a9c3e0af6a357e";
        //EmailCleaner.readFile(filename);

        EmailCleaner.parseFile(filename);

    }//end Main

}
