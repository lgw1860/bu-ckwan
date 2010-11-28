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

    /**
     * Convert an email text file into a Set of Strings.
     * @param filename
     * @return
     */
    private Set<String> processEmailFile(String filename)
    {
        try
        {
            Set<String> set = new HashSet<String>();
            Scanner scanner = new Scanner(new File(filename));
            scanner.useDelimiter(Pattern.compile("\\s|\\W"));
            while(scanner.hasNext())
            {
                String s = scanner.next();
                s = Utility.stem(s);
                //ignore words < 3 chars and words that are only numbers
                if(s.length() > 2 && !Utility.isOnlyNumeric(s))
                {
                    set.add(s);
                }
            }
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


    /**
     * Return P(Word | Class), ie the probability of that word given 
     * that its email is in class Spam/Ham.
     * @param word
     * @param isClassSpam
     * @return
     */
    private double probWordClass(String word, boolean isClassSpam)
    {
        //Select the appropriate map and number of emails based on the class
        HashMap<String,Integer> map;
        int numClassEmails = 0;
        if(isClassSpam)
        {
            map = this.mapSpam;
            numClassEmails = this.numSpamEmails;
        }
        else
        {
            map = this.mapHam;
            numClassEmails = this.numHamEmails;
        }

        //Get how many spams/hams this word has appeared in
        int wordClassCount = 0;
        if(map.containsKey(word))
        {
            wordClassCount = map.get(word);
        }
        double probWordClass = 0.0; //P(word | Class)
        //to avoid probabilities of 0, we use Laplacian correction and add 1
        if(wordClassCount <= 0)
        {
            probWordClass = 1.0 / (double)(numClassEmails+1);
        }
        else
        {
            probWordClass = (double)wordClassCount / (double)numClassEmails;
        }
        return probWordClass;
    }

    /**
     * Return P(Word | Spam), ie the probability of that word
     * given that its email is a Spam.
     * @param word
     * @return
     */
    private double probWordSpam(String word)
    {
        return probWordClass(word,true);
    }

    /**
     * Return P(Word | Ham), ie the probability of that word
     * given that its email is a Ham.
     * @param word
     * @return
     */
    private double probWordHam(String word)
    {
        return probWordClass(word,false);
    }

    /**
     * Classify an email given ground truth class to evaluate the classifier.
     * @param emailWordSet
     * @param isSpam
     */
    public void classifyWithGroundTruth(Set<String> emailWordSet, boolean isSpam)
    {
        boolean result = classify(emailWordSet);
        if(isSpam) //email actually is spam
        {
            if(result) //classified as spam
            {
                System.out.println("TRUE POSITIVE");
            }
            else //classified as ham
            {
                System.out.println("FALSE NEGATIVE");
            }
        }
        else //email actually is ham
        {
            if(result) //classified as spam
            {
                System.out.println("FALSE POSITIVE");
            }
            else //classifed as ham
            {
                System.out.println("TRUE NEGATIVE");
            }
        }
    }

    /**
     * Return true if the email is classified as Spam, false if Ham.
     * @param emailWordSet
     * @return
     */
    public boolean classify(Set<String> emailWordSet)
    {
        //prevent against probabilities of 0 and dividing by 0
        if(this.numSpamEmails <=0)
        {
            this.numSpamEmails = 1;
        }
        if(this.numHamEmails <= 0)
        {
            this.numHamEmails = 1;
        }
        
        double totalEmails = this.numSpamEmails + this.numHamEmails;
        double probSpam = (double)this.numSpamEmails/totalEmails; //P(Spam)
        double probHam = (double)this.numHamEmails/totalEmails; //P(Ham)
        System.out.println("\nprobSpam: " + probSpam);
        System.out.println("probHam: " + probHam);

        double probAllWordsSpam = 1.0; //Product sum of P(Word_i | Spam)
        double probAllWordsHam = 1.0; //Product sum of P(Word_i | Ham)

        //for all words
        Iterator<String> iterEmail = emailWordSet.iterator();
        while(iterEmail.hasNext())
        {
            String word = iterEmail.next();
            System.out.println("\t" + word + ": spam: " + probWordSpam(word)
                    + " ham: " + probWordHam(word));
            //compute the prob of the word and incorporate it in product sum
            probAllWordsSpam = probAllWordsSpam * probWordSpam(word);
            probAllWordsHam = probAllWordsHam * probWordHam(word);
        }

        System.out.println("probAllWordsSpam: " + probAllWordsSpam);
        System.out.println("probAllWordsHam: " + probAllWordsHam);

        //ProdSum[P(Word_i | Spam)] * P(Spam) <- we only look at the numerator
        double probSpamWordNumer = probAllWordsSpam * probSpam;
        System.out.println("prob that email is SPAM is: " + probSpamWordNumer);

        //ProdSum[P(Word_i | Ham)] * P(Ham) <- we only look at the numerator
        double probHamWordNumer = probAllWordsHam * probHam;
        System.out.println("prob that email is HAM is: " + probHamWordNumer);

        //condition is > not >= b/c in case they are equal, it is better to let
        //a spam through than to falsely classify a ham as spam
        if(probSpamWordNumer > probHamWordNumer)
        {
            System.out.println("SPAM!!!");
            return true;
        }
        else
        {
            System.out.println("HAM~~~");
            return false;
        }
    }

    public void classify(String word)
    {
        Set<String> set = new HashSet<String>();
        set.add(word);
        classify(set);
    }

    public void test()
    {
        classify("test"); //testing dividing by 0
        
        String filename = "";
        //filename = "2007_12_20071223-151359-customercare@cvs_com-Your_New_Account-1.eml";
        filename = "testalgo.txt";
        //filename = "testturtles.txt";
        train(this.processEmailFile(filename),true);

        filename = "testturtles.txt";
        //filename = "testalgo.txt";
        classifyWithGroundTruth(this.processEmailFile(filename),true);
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
