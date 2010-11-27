/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package cs565project3;

import java.io.*;
import java.util.Scanner;
import java.util.Collections;
import java.util.*;
import java.util.regex.Pattern;
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
     * Keep only letters, digits, HTML brackets in a string.
     * It is possible for an empty string to be returned.
     * @param word
     * @return
     */
    public static String onlyLettersDigits(String word)
    {
        StringBuffer stringBuffer = new StringBuffer();
        char curChar;
        for(int i=0; i<word.length(); i++)
        {
            curChar = word.charAt(i);
            if(Character.isLetter(curChar) || Character.isDigit(curChar))
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
            //TreeSet<String> list = new TreeSet<String>();
            HashSet<String> list = new HashSet<String>();
            Scanner scanner = new Scanner(new File(filename));
            scanner.useDelimiter(Pattern.compile("\\s|\\W"));
            int i = 0;
            while(scanner.hasNext())
            {
                String s = scanner.next();
                s = stem(s);
                list.add(s);
                System.out.println("i: " + i + ": " + s);
                i++;
            }
            System.out.println("I: " + i);
            
            Iterator<String> iter = list.iterator();
            int j=0;
            String curString;
            while(iter.hasNext())
            {
                curString = iter.next();
                System.out.println("j: " + j + ": " + curString);
                j++;
            }
            System.out.println("J: " + j);
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
