/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package cs565project3;

import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;
import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Iterator;
import java.util.Map.Entry;
import java.util.Random;
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

    private StringBuffer strBufResults; //results of classification on all files
    private StringBuffer strBufStats; //statistics about the classifier

    //for the ROC curve: a list for each email:
    //the classifier's probability for that email being Spam
    //and whether the email is actually Spam
    private ArrayList<DoubleBooleanPair> listROCPoints;

    /**
     * Class so you can return both a double and a boolean at once.
     * Used for the classifier and for generating ROC curves.
     */
    public class DoubleBooleanPair implements Comparable<DoubleBooleanPair>
    {
        private double probability = 0.0;
        private boolean isSpam = false;

        public DoubleBooleanPair()
        {
            
        }

        public DoubleBooleanPair(double prob, boolean isS)
        {
            this.probability = prob;
            this.isSpam = isS;
        }

        public double getProbability()
        {
            return this.probability;
        }

        public void setProbability(double prob)
        {
            this.probability = prob;
        }

        public boolean getIsSpam()
        {
            return this.isSpam;
        }

        public void setIsSpam(boolean isS)
        {
            this.isSpam = isS;
        }

        @Override public String toString()
        {
            return ("<" + this.probability + ", " + this.isSpam + ">");
        }

        public int compareTo(DoubleBooleanPair o) {
            //the pair with the smaller probability is smaller
            if(this.probability < o.getProbability())
            {
                return -1;
            }
            //if probs are the same, if this has false
            //and o has true, this is smaller
            else if(this.probability == o.getProbability())
            {
                if(!this.isSpam && o.getIsSpam())
                {
                    return -1;
                }
                //both prob and bool are equal
                else if(this.isSpam == o.getIsSpam())
                {
                    return 0;
                }
                else //isSpam && !o.isSpam
                {
                    return 1;
                }
            }
            else //this probability is greater
            {
                return 1;
            }
        }
    }

    public SpamFilter()
    {
        mapSpam = new HashMap<String, Integer>();
        mapHam = new HashMap<String, Integer>();

        strBufResults = new StringBuffer();
        strBufResults.append("File,Probability,Actual,Classified,Observation\n");
        
        strBufStats = new StringBuffer();
        strBufStats.append("Measure,Value\n");

        listROCPoints = new ArrayList<DoubleBooleanPair>();
    }

    //TODO change this to take in a file
    public void train(String word, boolean isSpam)
    {
        Set<String> set = new HashSet<String>();
        set.add(word);
        train(set,isSpam);
    }

    /**
     * Process an email text file into a Set of Strings.
     * @param file
     * @return
     */
    private Set<String> processEmailFile(File file)
    {
        try
        {
            Set<String> set = new HashSet<String>();
            Scanner scanner = new Scanner(file);
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
        //grab classification results from the classifier
        DoubleBooleanPair pairFromClassify = classify(emailWordSet);
        boolean result = pairFromClassify.getIsSpam();

        if(isSpam) //email actually is spam
        {
            //System.out.println("Actual: Spam");
            this.strBufResults.append("SPAM,");
            if(result) //classified as spam
            {
                this.truePositives++;
                //System.out.println("Classified: Spam");
                this.strBufResults.append("SPAM,");
                //System.out.println("TRUE POSITIVE");
                this.strBufResults.append("TP\n");
            }
            else //classified as ham
            {
                this.falseNegatives++;
                //System.out.println("Classified: Ham");
                this.strBufResults.append("HAM,");
                //System.out.println("FALSE NEGATIVE");
                this.strBufResults.append("FN\n");
            }
        }
        else //email actually is ham
        {
            //System.out.println("Actual: Ham");
            this.strBufResults.append("HAM,");
            if(result) //classified as spam
            {
                this.falsePositives++;
                //System.out.println("Classified: Spam");
                this.strBufResults.append("SPAM,");
                //System.out.println("FALSE POSITIVE");
                this.strBufResults.append("FP\n");
            }
            else //classifed as ham
            {
                this.trueNegatives++;
                //System.out.println("Classified: Ham");
                this.strBufResults.append("HAM,");
                //System.out.println("TRUE NEGATIVE");
                this.strBufResults.append("TN\n");
            }
        }
        //System.out.println("---");

        //create a pair of <classifier probability, actual class>
        //for use in creating the ROC curve
        DoubleBooleanPair pairForRoc = new DoubleBooleanPair();
        pairForRoc.setIsSpam(isSpam);
        pairForRoc.setProbability(pairFromClassify.getProbability());
        this.listROCPoints.add(pairForRoc);
        //System.out.println(pairForRoc);
    }

    /**
     * Return <true if the email is classified as Spam, false if Ham,
     * classifier's probility for the email>
     * @param emailWordSet
     * @return
     */
    public DoubleBooleanPair classify(Set<String> emailWordSet)
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
        //System.out.println("\nnumSpam: " + this.numSpamEmails);
        //System.out.println("numHam: " + this.numHamEmails);
        //System.out.println("probSpam: " + probSpam);
        //System.out.println("probHam: " + probHam);

        double probAllWordsSpam = 1.0; //Product sum of P(Word_i | Spam)
        double probAllWordsHam = 1.0; //Product sum of P(Word_i | Ham)

        //for all words
        Iterator<String> iterEmail = emailWordSet.iterator();
        while(iterEmail.hasNext())
        {
            String word = iterEmail.next();
            //System.out.println("\t" + word + ": spam: " + probWordSpam(word)
            //        + " ham: " + probWordHam(word));

            //compute the prob of the word and incorporate it in product sum
            probAllWordsSpam = probAllWordsSpam * probWordSpam(word);
            probAllWordsHam = probAllWordsHam * probWordHam(word);
        }
        //System.out.println("probAllWordsSpam: " + probAllWordsSpam);
        //System.out.println("probAllWordsHam: " + probAllWordsHam);

        //ProdSum[P(Word_i | Spam)] * P(Spam) <- we only look at the numerator
        double probSpamWordNumer = probAllWordsSpam * probSpam;
        //System.out.println("prob that email is SPAM is: " + probSpamWordNumer);
        
        //ProdSum[P(Word_i | Ham)] * P(Ham) <- we only look at the numerator
        double probHamWordNumer = probAllWordsHam * probHam;
        //System.out.println("prob that email is HAM is: " + probHamWordNumer);
        
        //TODO return a probability

        //condition is > not >= b/c in case they are equal, it is better to let
        //a spam through than to falsely classify a ham as spam
        if(probSpamWordNumer > probHamWordNumer) //classified as spam
        {
            //System.out.println("SPAM!!!");
            //System.out.println("Prob: " + probSpamWordNumer);
            this.strBufResults.append(probSpamWordNumer + ",");
            //return true;
            return new DoubleBooleanPair(probSpamWordNumer,true);
        }
        else //classified as ham
        {
            //System.out.println("HAM~~~");
            //System.out.println("Prob: " + probHamWordNumer);
            this.strBufResults.append(probHamWordNumer + ",");
            //return false;
            return new DoubleBooleanPair(probHamWordNumer,false);
        }
    }

    public void classify(String word)
    {
        Set<String> set = new HashSet<String>();
        set.add(word);
        classify(set);
    }


    private ArrayList<ArrayList<File>> listSpamBuckets;
    private ArrayList<ArrayList<File>> listHamBuckets;

    /**
     * Randomly place spams and hams into k buckets for k-fold cross validation.
     * @param k
     * @param spamFolderPath
     * @param hamFolderPath
     */
    private void randomlyPartitionEmails(int k, String spamFolderPath, String hamFolderPath)
    {
        //get number of files in spam and ham folders
        ArrayList<File> listSpamFiles = Utility.listOfFiles(spamFolderPath);
        ArrayList<File> listHamFiles = Utility.listOfFiles(hamFolderPath);

        int spamFolderCount = listSpamFiles.size();
        int hamFolderCount = listHamFiles.size();

        int spamBucketSize = (int)Math.ceil((double)spamFolderCount / (double)k);
        int hamBucketSize = (int)Math.ceil((double)hamFolderCount / (double)k);

        //create k spam and ham buckets
        listSpamBuckets = new ArrayList<ArrayList<File>>(k);
        listHamBuckets = new ArrayList<ArrayList<File>>(k);
        for(int i=0; i<k; i++)
        {
            listSpamBuckets.add(new ArrayList<File>(spamBucketSize));
            listHamBuckets.add(new ArrayList<File>(hamBucketSize));
        }

        //for each spam email file
        //add it to a random bucket
        Iterator<File> iterSpam = listSpamFiles.iterator();
        while(iterSpam.hasNext())
        {
            File curFile = iterSpam.next();
            boolean fileAddedInBucket = false;
            while(!fileAddedInBucket)
            {
                Random rand = new Random();
                int bucketIndex = rand.nextInt(k);
                ArrayList<File> curBucket = listSpamBuckets.get(bucketIndex);
                if(curBucket.size() < spamBucketSize)
                {
                    curBucket.add(curFile);
                    fileAddedInBucket = true;
                }
            }
        }//end while spam

        //for each ham email file
        //add it to a random bucket
        Iterator<File> iterHam = listHamFiles.iterator();
        while(iterHam.hasNext())
        {
            File curFile = iterHam.next();
            boolean fileAddedInBucket = false;
            while(!fileAddedInBucket)
            {
                Random rand = new Random();
                int bucketIndex = rand.nextInt(k);
                ArrayList<File> curBucket = listHamBuckets.get(bucketIndex);
                if(curBucket.size() < hamBucketSize)
                {
                    curBucket.add(curFile);
                    fileAddedInBucket = true;
                }
            }
        }//end while ham
    }

    /**
     * Reset classifier by removing all data from training.
     */
    private void resetClassifier()
    {
        this.mapSpam.clear();
        this.mapHam.clear();
        this.numSpamEmails = 0;
        this.numHamEmails = 0;
    }



    public void crossValidate(int k, String spamFolderPath, String hamFolderPath)
    {

        //randomly partition all emails into k buckets
        this.randomlyPartitionEmails(k, spamFolderPath, hamFolderPath);

        //for each of k buckets in listBuckets
        //for each file in the bucket

        //for all k buckets
        //train on all k-1 buckets and classify the last one
        for(int i=0; i<k; i++)
        {
            //for each bucket != i
            for(int j=0; j<k; j++)
            {
                if(j != i)
                {
                    //train spams
                    //for each file in the bucket
                    ArrayList<File> curListSpam = this.listSpamBuckets.get(j);
                    Iterator<File> iterSpam = curListSpam.iterator();
                    while(iterSpam.hasNext())
                    {
                        File curFileSpam = iterSpam.next();
                        this.train(this.processEmailFile(curFileSpam), true);
                    }//end while

                    //train hams
                    //for each file in the bucket
                    ArrayList<File> curListHam = this.listHamBuckets.get(j);
                    Iterator<File> iterHam = curListHam.iterator();
                    while(iterHam.hasNext())
                    {
                        File curFileHam = iterHam.next();
                        this.train(this.processEmailFile(curFileHam), false);
                    }//end while
                }//end if j != i
            }//end for j

            //use remaining bucket for testing
            
            //test spam
            ArrayList<File> testListSpam = this.listSpamBuckets.get(i);
            Iterator<File> iterTestSpam = testListSpam.iterator();
            while(iterTestSpam.hasNext())
            {
                File testFileSpam = iterTestSpam.next();
                //System.out.println("File: " + testFileSpam);
                this.strBufResults.append(testFileSpam + ",");
                this.classifyWithGroundTruth(this.processEmailFile(testFileSpam),true);
            }

            //test ham
            ArrayList<File> testListHam = this.listHamBuckets.get(i);
            Iterator<File> iterTestHam = testListHam.iterator();
            while(iterTestHam.hasNext())
            {
                File testFileHam = iterTestHam.next();
                //System.out.println("File: " + testFileHam);
                this.strBufResults.append(testFileHam + ",");
                this.classifyWithGroundTruth(this.processEmailFile(testFileHam),false);
            }

            //reset classifier for the next round
            this.resetClassifier();
        }//end for i

        //TODO
        //output prob | actual class to text file
        //compute all those fun stats
        this.addStatsToStringBuffer();
        this.writeStringBuffersToFiles();
        this.outputROCPointsForExcel();
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
        this.crossValidate(10, "testdata/spam", "testdata/ham");
    }

    public void writeStringBuffersToFiles()
    {
        //System.out.println(this.strBufResults.toString());
        try
        {
            String filename = "results.csv";
            BufferedWriter writer = new BufferedWriter(
                    new FileWriter(filename));
            writer.write(this.strBufResults.toString());
            writer.flush();
            writer.close();
            System.out.println("See <" + filename + "> for classifier results on all emails.");
        }
        catch(Exception e)
        {
            e.printStackTrace();
        }

        //System.out.println(this.strBufStats.toString());
        try
        {
            String filename = "stats.csv";
            BufferedWriter writer = new BufferedWriter(
                    new FileWriter(filename));
            writer.write(this.strBufStats.toString());
            writer.flush();
            writer.close();
            System.out.println("See <" + filename + "> for classifier statistics.");
        }
        catch(Exception e)
        {
            e.printStackTrace();
        }
    }

    /**
     * Output statistics for the classifier.
     */
    public void addStatsToStringBuffer() {
        //System.out.println("TP: " + this.truePositives);
        this.strBufStats.append("True Positives," + this.truePositives + "\n");

        //System.out.println("FP: " + this.falsePositives);
        this.strBufStats.append("False Positives," + this.falsePositives + "\n");

        //System.out.println("TN: " + this.trueNegatives);
        this.strBufStats.append("True Negatives," + this.trueNegatives + "\n");

        //System.out.println("FN: " + this.falseNegatives);
        this.strBufStats.append("False Negatives," + this.falseNegatives + "\n");

        //System.out.println("Accuracy: " + this.accuracy());
        this.strBufStats.append("Accuracy," + this.accuracy() + "\n");

        //System.out.println("Precision: " + this.precision());
        this.strBufStats.append("Precision," + this.precision() + "\n");

        //System.out.println("Recall: " + this.recall());
        this.strBufStats.append("Recall," + this.recall() + "\n");

        //System.out.println("F-measure: " + this.fMeasure());
        this.strBufStats.append("F-measure," + this.fMeasure() + "\n");

        //System.out.println("True Positive Rate: " + this.truePositiveRate());
        this.strBufStats.append("True Positive Rate," + this.truePositiveRate() + "\n");

        //System.out.println("False Positive Rate: " + this.falsePositiveRate());
        this.strBufStats.append("False Positive Rate," + this.falsePositiveRate() + "\n");
    }

    public void outputROCPointsForExcel()
    {
        //sort points by probability in descending order
        //Collections.sort(this.listROCPoints);

        DoubleBooleanPair a = new DoubleBooleanPair(0.5,true);
        DoubleBooleanPair b = new DoubleBooleanPair(0.5,false);
        System.out.println(a);
        System.out.println(b);
        System.out.println(a.compareTo(b));

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

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        System.out.println("Please wait...");
        SpamFilter spamFilter = new SpamFilter();
        spamFilter.test();
    }
    
}
