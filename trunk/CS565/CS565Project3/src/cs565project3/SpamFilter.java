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

    private int truePositives = 0;
    private int falsePositives = 0;
    private int trueNegatives = 0;
    private int falseNegatives = 0;

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
                this.truePositives++;
                System.out.println("TRUE POSITIVE");
            }
            else //classified as ham
            {
                this.falseNegatives++;
                System.out.println("FALSE NEGATIVE");
            }
        }
        else //email actually is ham
        {
            if(result) //classified as spam
            {
                this.falsePositives++;
                System.out.println("FALSE POSITIVE");
            }
            else //classifed as ham
            {
                this.trueNegatives++;
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



    public void crossValidate(int k, String spamFolderPath, String hamFolderPath)
    {
        //create k buckets

        //get number of files in spam and ham folders
        int spamFolderCount = Utility.numFilesInFolder(spamFolderPath);
        int hamFolderCount = Utility.numFilesInFolder(hamFolderPath);

        int spamBucketSize = (int)Math.ceil((double)spamFolderCount / (double)k);
        int hamBucketSize = (int)Math.ceil((double)hamFolderCount / (double)k);





    }


    /**
     * Return the accuracy of the classifier.
     * Accuracy = TP + TN) / (TP+TN+FP+FN).
     * @return
     */
    public double accuracy()
    {
        double acc = (double)(this.truePositives + this.trueNegatives) /
                (double)(this.truePositives + this.trueNegatives +
                this.falsePositives + this.falseNegatives);
        return acc;
    }

    /**
     * Return the precision of the classifier.
     * Precision = TP / (TP + FP)
     * @return
     */
    public double precision()
    {
        double pre = (double)this.truePositives /
                (double)(this.truePositives + this.falsePositives);
        return pre;
    }

    /**
     * Return the recall of the classifier.
     * Recall = TP / (TP + FN)
     * @return
     */
    public double recall()
    {
        double rec = (double)this.truePositives /
                (double)(this.truePositives + this.falseNegatives);
        return rec;
    }

    /**
     * Return the F-measure of the classifier.
     * F-measure = (2*rec*pre) / (rec + pre)
     * F-measure = 2TP / (2TP + FP + FN)
     * @return
     */
    public double fMeasure()
    {
        double f = 2.0*this.truePositives /
                (2.0*this.truePositives +
                this.falsePositives + this.falseNegatives);
        return f;
    }

    /**
     * Return the True Positive Rate of the classifier.
     * TPR = TP / (TP + FN)
     * @return
     */
    public double truePositiveRate()
    {
        double tpr = (double)this.truePositives /
                (double)(this.truePositives + this.falseNegatives);
        return tpr;
    }

    /**
     * Return the False Positive Rate of the classifier.
     * FPR = FP / (FP + TN)
     * @return
     */
    public double falsePositiveRate()
    {
        double fpr = (double)this.falsePositives /
                (double)(this.falsePositives + this.trueNegatives);
        return fpr;
    }

    public void test()
    {
        classify("test"); //testing dividing by 0
        
        String filename = "";
        //filename = "2007_12_20071223-151359-customercare@cvs_com-Your_New_Account-1.eml";
        filename = "testalgo.txt";
        //filename = "testturtles.txt";
        train(this.processEmailFile(filename),true);

        filename = "testalgo.txt";
        train(this.processEmailFile(filename),true);

        filename = "testalgo.txt";
        //filename = "testturtles.txt";
        train(this.processEmailFile(filename),true);

        //filename = "testturtles.txt";
        filename = "testalgo.txt";
        classifyWithGroundTruth(this.processEmailFile(filename),true);

        classifyWithGroundTruth(this.processEmailFile(filename),true);

        Utility.numFilesInFolder("testdata/spam");

        this.crossValidate(4, "testdata/spam", "testdata/ham");
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

        System.out.println("TP: " + this.truePositives);
        System.out.println("FP: " + this.falsePositives);
        System.out.println("TN: " + this.trueNegatives);
        System.out.println("FN: " + this.falseNegatives);

        //for testing
        //to delete
//        this.truePositives = 150;
//        this.trueNegatives = 250;
//        this.falsePositives = 60;
//        this.falseNegatives = 40;

        this.truePositives = 250;
        this.trueNegatives = 200;
        this.falsePositives = 5;
        this.falseNegatives = 45;

        System.out.println("Accuracy: " + this.accuracy());
        System.out.println("Precision: " + this.precision());
        System.out.println("Recall: " + this.recall());
        System.out.println("F-measure: " + this.fMeasure());
        System.out.println("True Positive Rate: " + this.truePositiveRate());
        System.out.println("False Positive Rate: " + this.falsePositiveRate());
    }
    
}
