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

    public void classify(String word)
    {
        Set<String> set = new HashSet<String>();
        set.add(word);
        classify(set);
    }
    public void classify(Set<String> emailWordSet)
    {
        double totalEmails = this.numSpamEmails + this.numHamEmails;
        double probSpam = (double)this.numSpamEmails/totalEmails; //P(C_Spam)
        double probHam = (double)this.numHamEmails/totalEmails; //P(C_Ham)
        System.out.println("\nprobSpam: " + probSpam);
        System.out.println("probHam: " + probHam);

        double probAllWordsSpam = 1.0;
        System.out.print("probAllWordsSpam = ");

        //for all words
        Iterator<String> iterEmail = emailWordSet.iterator();
        while(iterEmail.hasNext())
        {
            String word = iterEmail.next();
            System.out.print(word + ": ");
            int wordSpamCount = 0;
            if(this.mapSpam.containsKey(word))
            {
                wordSpamCount = this.mapSpam.get(word);
            }

            double probWordSpam = 0.0; //P(word | C_Spam)
            double numer = 0.0; //numerator for P(word | C_Spam)
            double denom = 0.0; //denominator for P(word | C_Spam)
            //to avoid probabilities of 0, we use Laplacian correction and add 1
            if(wordSpamCount <= 0)
            {
                numer = 1.0;
                denom = (double)(this.numSpamEmails+1);
            }
            else
            {
                numer = (double)wordSpamCount;
                denom = (double)this.numSpamEmails;
            }

            probWordSpam = numer / denom;
            System.out.print(probWordSpam + " * ");
            probAllWordsSpam = probAllWordsSpam * probWordSpam;
        }//end while
        //end for

        System.out.println(" = " + probAllWordsSpam);

        //P(word | C_Spam) * P(C_Spam) <- we only look at the numerator
        double probSpamWordNumer = probAllWordsSpam * probSpam;
        System.out.println("prob that email is SPAM is: " + probSpamWordNumer);
    }

    public void test()
    {
        classify("test"); //testing dividing by 0
        
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

        train("bababa goose", false);
        //train("bababa goose", true);

        //classify("algorithm");
        //classify("bababa goose");
        //classify("nullo");

        filename = "testalgo.txt";
        Set<String> testSet = this.processEmailFile(filename);
        System.out.println(testSet);
        classify(testSet);

        classify("algorithm");
        classify("bababa goose");

        //filename = "2007_12_20071223-151359-customercare@cvs_com-Your_New_Account-1.eml";
        //train(this.processEmailFile(filename),true);

        //classify(this.processEmailFile(filename));
        //train("smtp",false);
        classify("smtp");
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
