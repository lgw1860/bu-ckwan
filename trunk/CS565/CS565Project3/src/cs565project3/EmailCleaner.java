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
    /**
     * Stems a word using the Porter stemming algorithm.
     * You should also check if the stemmed word is empty.
     * @param word
     * @return
     */
    public static String stem(String word)
    {
        Stemmer stemmer = new Stemmer();
        String newWord = word;
        newWord = onlyLettersDigits(word);
        newWord = newWord.toLowerCase();
        stemmer.add(newWord.toCharArray(), newWord.length());
        stemmer.stem();
        return stemmer.toString();
    }

    /**
     * Keep only letters and digits in a string.
     * It is possible for an empty string to be returned.
     * @param word
     * @return
     */
    public static String onlyLettersDigits(String word)
    {
        StringBuffer stringBuffer = new StringBuffer();
        for(int i=0; i<word.length(); i++)
        {
            if(Character.isLetter(word.charAt(i)) ||
                    Character.isDigit(word.charAt(i)))
            {
                stringBuffer.append(word.charAt(i));
            }
        }
        return stringBuffer.toString();
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
                //s = onlyLettersDigits(s);
                s = stem(s);
                if(!s.isEmpty())
                {
                    System.out.println(s);
                }
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
