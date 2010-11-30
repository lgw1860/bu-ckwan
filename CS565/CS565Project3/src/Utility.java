/*
 * Christopher Kwan
 * ckwan (at) cs.bu.edu
 * CS 565 | Terzi | Fall 2010
 * Programming Project 3: Classifying Spam
 */



import java.io.File;
import java.util.ArrayList;

/**
 *
 * @author Chris
 */
public class Utility {

    /**
     * Keep only letters and digits in a string.
     * It is possible for an empty string to be returned.
     * @param word
     * @return
     */
    public static String toAlphaNumeric(String word)
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
        newWord = toAlphaNumeric(word);
        newWord = newWord.toLowerCase();
        stemmer.add(newWord.toCharArray(), newWord.length());
        stemmer.stem();
        return stemmer.toString();
    }

    /**
     * Returns true if the word consists only of digits.
     * @param word
     * @return
     */
    public static boolean isOnlyNumeric(String word)
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
     * Return an ArrayList of all the files in a folder.
     * @param folderPath
     * @return
     */
    public static ArrayList<File> listOfFiles(String folderPath)
    {
        try
        {
            File folder = new File(folderPath);
            File[] files = folder.listFiles();
            ArrayList<File> goodFiles = new ArrayList<File>();
            //only take non-hidden files (not folders)
            for(int i=0; i<files.length; i++)
            {
                File curFile = files[i];
                if(curFile.isFile() && !curFile.isHidden())
                {
                    goodFiles.add(curFile);
                }
            }
            return goodFiles;
        }
        catch(Exception e)
        {
            e.printStackTrace();
            return null;
        }
    }
}
