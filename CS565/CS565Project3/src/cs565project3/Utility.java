/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package cs565project3;

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
}
