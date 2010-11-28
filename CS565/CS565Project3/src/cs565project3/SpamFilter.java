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

    Integer numSpamEmails;
    Integer numHamEmails;
    HashMap<String, Integer> mapSpamCount;
    HashMap<String, Integer> mapHamCount;

    public SpamFilter()
    {
        numSpamEmails = new Integer(0);
        numHamEmails = new Integer(0);
        mapSpamCount = new HashMap<String, Integer>();
        mapHamCount = new HashMap<String, Integer>();
    }

    void train(String word, boolean isSpam)
    {
        Integer numEmails;
        HashMap<String, Integer> mapCount;

        if(isSpam)
        {
            //numEmails = numSpamEmails;
            mapCount = mapSpamCount;
        }
        else
        {
            //numEmails = numHamEmails;
            mapCount = mapHamCount;
        }

        if(mapCount.containsKey(word))
        {
            int oldCount = mapCount.get(word);
            mapCount.put(word, oldCount+1);
        }
        else
        {
            mapCount.put(word, 1);
        }
        //numEmails = new Integer(99);
        if(isSpam)
        {
            this.numSpamEmails++;
        }
        else
        {
            this.numHamEmails++;
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
    }

    void print()
    {
        System.out.println("num spam: " + this.numSpamEmails);
        System.out.println("num ham: " + this.numHamEmails);

        System.out.println("\nSpam Counts:");
        Iterator<Entry<String,Integer>> iterSpam = this.mapSpamCount.entrySet().iterator();
        while(iterSpam.hasNext())
        {
            System.out.println(iterSpam.next());
        }

        System.out.println("\nHam Counts:");
        Iterator<Entry<String,Integer>> iterHam = this.mapHamCount.entrySet().iterator();
        while(iterHam.hasNext())
        {
            System.out.println(iterHam.next());
        }
    }


}
