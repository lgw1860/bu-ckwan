/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package cs565project3;

import java.io.*;
import java.util.Scanner;
/**
 *
 * @author Chris
 */
public class EmailCleaner {
    public static String stem(String word)
    {
        Stemmer stemmer = new Stemmer();
        stemmer.add(word.toCharArray(), word.length());
        stemmer.stem();
        return stemmer.toString();
    }

    //read in text
    //find first empty line
    //create new file
    public static void parseFile(String filename)
    {
        try
        {
            Scanner scanner = new Scanner(new File(filename));
            while(scanner.hasNext())
            {
                String s = scanner.next();
//
//                Stemmer stemmer = new Stemmer();
//                stemmer.add(s.toCharArray(), s.length());
//                stemmer.stem();
//                s = stemmer.toString();
//                //String newWord = stemmer.toString();
//                //System.out.println(newWord);
                s = stem(s);
                System.out.println(s);
            }
        }
        catch(Exception e)
        {
            e.printStackTrace();
        }
        
    }

    public static void readFile(String filename)
    {
        try
        {
            BufferedReader reader = new BufferedReader(new FileReader(filename));
            String s;
            while((s = reader.readLine()) != null)
            {
                System.out.println(s);
            }
            reader.close();
        }
        catch(Exception e)
        {
            System.out.println("EXCEPTION !");
            e.printStackTrace();
        }
    }
}
