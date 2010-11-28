/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package cs565project3;

import java.io.File;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Iterator;
import java.util.Map.Entry;
import java.util.Scanner;
import java.util.Set;
import java.util.regex.Pattern;

/**
 *
 * @author Chris
 */
public class SpamFilter {

    private int numSpamEmails;  //total spam emails trained on
    private int numHamEmails;   //total ham emails trained on
    private HashMap<String, Integer> mapSpam;  //map of words to spam counts
    private HashMap<String, Integer> mapHam;   //map of words to ham counts

    public SpamFilter()
    {
        mapSpam = new HashMap<String, Integer>();
        mapHam = new HashMap<String, Integer>();
    }

    //TODO change this to take in a file
    public void train(String word, boolean isSpam)
    {
        Set<String> set = new HashSet<String>();
        set.add(word);
        train(set,isSpam);
    }

    public static boolean onlyDigits(String word)
    {
        StringBuffer stringBuffer = new StringBuffer();
        char curChar;
        for(int i=0; i<word.length(); i++)
        {
            curChar = word.charAt(i);
            if(!Character.isDigit(curChar))
            {
                stringBuffer.append(curChar);
            }
        }
        return !(stringBuffer.length() > 0);
    }

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

    Set<String> processEmailFile(String filename)
    {
        try
        {
            Set<String> set = new HashSet<String>();
            Scanner scanner = new Scanner(new File(filename));
            scanner.useDelimiter(Pattern.compile("\\s|\\W"));
            int i = 0;
            while(scanner.hasNext())
            {
                String s = scanner.next();
                s = stem(s);

                //ignore words < 3 chars and words that are only numbers
                if(s.length() > 2 && !onlyDigits(s))
                {
                    set.add(s);
                    System.out.println("i: " + i + ": " + s);
                    i++;
                }

            }
            System.out.println("I: " + i);
            return set;
        }
        catch(Exception e)
        {
            e.printStackTrace();
            return null;
        }
    }


    public void train(Set<String> emailWordSet, boolean isSpam)
    {
        //choose the correct map and counter to add to
        HashMap<String, Integer> map; //reference to either mapSpam or mapHam
        if(isSpam)
        {
            map = this.mapSpam;
            this.numSpamEmails++;
        }
        else
        {
            map = this.mapHam;
            this.numHamEmails++;
        }

        //add all words in the email to the map
        Iterator<String> emailIter = emailWordSet.iterator();
        while(emailIter.hasNext())
        {
            String word = emailIter.next();
            if(map.containsKey(word))
            {
                int oldCount = map.get(word);
                map.put(word, oldCount+1);
            }
            else
            {
                map.put(word, 1);
            }
        }
    }

    void test()
    {
//        train("cat", true);
//        train("dog", true);
//        train("cat", false);
//        train("cat", true);
//        train("dog", true);
//        train("cat", true);
//        train("mouse", false);
//        Set<String> testSet = new HashSet<String>();
//        testSet.add("algorithm");
//        testSet.add("algorithms");
//        testSet.add("algorithm");
//        train(testSet,true);

        String filename = "";
        //filename = "2007_12_20071223-151359-customercare@cvs_com-Your_New_Account-1.eml";
        filename = "testalgo.txt";
        train(this.processEmailFile(filename),true);

        filename = "testalgo.txt";
        train(this.processEmailFile(filename),true);

        filename = "testalgo.txt";
        train(this.processEmailFile(filename),false);

        filename = "testturtles.txt";
        train(this.processEmailFile(filename),true);


    }

    public void print()
    {
        System.out.println("num spam: " + this.numSpamEmails);
        System.out.println("num ham: " + this.numHamEmails);

        System.out.println("\nSpam Counts:");
        Iterator<Entry<String,Integer>> iterSpam = this.mapSpam.entrySet().iterator();
        while(iterSpam.hasNext())
        {
            System.out.println(iterSpam.next());
        }

        System.out.println("\nHam Counts:");
        Iterator<Entry<String,Integer>> iterHam = this.mapHam.entrySet().iterator();
        while(iterHam.hasNext())
        {
            System.out.println(iterHam.next());
        }
    }


}
