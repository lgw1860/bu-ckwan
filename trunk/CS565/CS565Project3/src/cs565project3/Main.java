/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package cs565project3;

/**
 *
 * @author Chris
 */
public class Main {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        // TODO code application logic here
        System.out.println("hello world it's me!");

        //Stemmer test
        Stemmer stemmer = new Stemmer();
        String wordsToStem[] = {"algorithm", "algorithmic", "algorithms", "Algorithms", "algo"};
        //String wordsToStem[] = {"truck", "trucker", "truckers", "trucks", "trucking"};
        for (int i=0; i<wordsToStem.length; i++)
        {
            //stem each word
            String curWord = wordsToStem[i].toLowerCase();
            stemmer.add(curWord.toCharArray(), curWord.length());
            stemmer.stem();
            System.out.println("orig: " + wordsToStem[i]);
            System.out.println("\tstemmed: " + stemmer.toString());
        }
    }

}
