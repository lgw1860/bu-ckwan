
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;
import java.util.ArrayList;
import java.util.Iterator;
import java.util.Scanner;
import java.util.Set;
import java.util.regex.Pattern;

/*
 * Christopher Kwan
 * ckwan (at) cs.bu.edu
 * CS 565 | Terzi | Fall 2010
 * Programming Project 3: Classifying Spam
 */


/**
 * Ad-hoc class to convert modified (by me) SpamBase datasets back into email
 * files so they can be used with SpamFilter.
 * @author Chris
 */
public class SpamBaseProcessor {

    //words used as features in the spambase dataset
    public static String[] names = {
        "make"         ,
        "address"      ,
        "all"          ,
        "3d"           ,
        "our"          ,
        "over"         ,
        "remove"       ,
        "internet"     ,
        "order"        ,
        "mail"         ,
        "receive"      ,
        "will"         ,
        "people"       ,
        "report"       ,
        "addresses"    ,
        "free"         ,
        "business"     ,
        "email"        ,
        "you"          ,
        "credit"       ,
        "your"         ,
        "font"         ,
        "000"          ,
        "money"        ,
        "hp"           ,
        "hpl"          ,
        "george"       ,
        "650"          ,
        "lab"          ,
        "labs"         ,
        "telnet"       ,
        "857"          ,
        "data"         ,
        "415"          ,
        "85"           ,
        "technology"   ,
        "1999"         ,
        "parts"        ,
        "pm"           ,
        "direct"       ,
        "cs"           ,
        "meeting"      ,
        "original"     ,
        "project"      ,
        "re"           ,
        "edu"          ,
        "table"        ,
        "conference"   ,
        ";"            ,
        "("            ,
        "["            ,
        "!"            ,
        "$"            ,
        "#"            
    };
    
    /**
     * Generate a new email file for every row in the SpamBase CSV file.
     * @param filepath
     */
    public static void GenerateEmailFilesFromCSV(String filepath, String destFolderPath)
    {
        try
        {
            File file = new File(filepath);
            Scanner scanner = new Scanner(file);
            scanner.useDelimiter("\n");
            int numForNextFile = 0; //file name for the next file to be created
            while(scanner.hasNext()) //each line of the csv file becomes a new file
            {
                String s = scanner.next();
                String[] vals = s.split(",");
                listToFile(vals,numForNextFile,destFolderPath);
                numForNextFile++;
            }
        }
        catch(Exception e)
        {
            e.printStackTrace();
        }
    }

    /**
     * Convert a list of doubles (a row in spambase.data)
     * into an email text file so it can be processed by SpamFilter.
     * @param list
     * @param fileNum
     */
    private static void listToFile(String[] listWordFreq, int fileNum, String destFolderPath)
    {
        StringBuffer sb = new StringBuffer();
        for(int i=0; i<listWordFreq.length; i++)
        {
            double curWordFreq = Double.parseDouble(listWordFreq[i]);
            if(curWordFreq > 0)//word[i] is in the email
            {
                //find the associated name and add it to the email
                sb.append(names[i] + " ");
            }
        }
        
        //System.out.println(sb.toString());
        try
        {
            String filename = destFolderPath + "/" + fileNum + ".spambaseemail";
            BufferedWriter writer = new BufferedWriter(
                new FileWriter(filename));
            writer.write(sb.toString());
            writer.flush();
            writer.close();
        }
        catch(Exception e)
        {
            e.printStackTrace();
        }
    }
}
