/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package cs565project3;

import java.util.HashMap;
import java.util.Iterator;
import java.util.Map.Entry;

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
        HashMap<String, Integer> map;

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

    void test()
    {
        train("cat", true);
        train("dog", true);
        train("cat", false);
        train("cat", true);
        train("dog", true);
        train("cat", true);
        train("mouse", false);
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
