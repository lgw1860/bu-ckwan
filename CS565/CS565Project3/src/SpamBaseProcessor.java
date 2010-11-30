
import java.io.File;
import java.util.Iterator;
import java.util.Set;

/*
 * Christopher Kwan
 * ckwan (at) cs.bu.edu
 * CS 565 | Terzi | Fall 2010
 * Programming Project 3: Classifying Spam
 */


/**
 *
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
    };

    public static void fileToSet(String filepath)
    {
        try
        {
            File file = new File(filepath);
            SpamFilter spamFilter = new SpamFilter();
            Set<String> fileList = spamFilter.processEmailFile(file);

            Iterator<String> iter = fileList.iterator();
            while(iter.hasNext())
            {
                String curString = iter.next();
                System.out.println(curString);
            }
        }
        catch(Exception e)
        {
            e.printStackTrace();
        }
    }

    public static void generateFiles(String sourceFilePath, String destFolderPath)
    {
        try
        {


        }
        catch(Exception e)
        {
            e.printStackTrace();
        }
    }

}
