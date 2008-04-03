
/**
 * @author Christopher Kwan  ckwan@bu.edu  U37-02-3645
 * @name CS350 HW3 Problem 4 M/M/1 Queue Simulator
 * @date 2-19-2008
 * @class Schedule.java - Schedule of Events for Simulator 
 * 			compile:	"javac Schedule.java"
 * 			run:		"java Schedule"
 */

//package simulator;

public class Schedule {

	private int length;
	private Event firstEvent;
	private Event lastEvent;
	
	public Schedule(Event first)
	{
		firstEvent = first;
		lastEvent = first;
		length = 1;
	}
	
	public void addRandom(Event newEvent)
	{
		lastEvent.setNext(newEvent);
	}
	
	/**
	 * Add newEvent to list based on its time.
	 * @param newEvent
	 */
	public void add(Event newEvent)
	{	
		//adding new event to beginning
		if(newEvent.getTime() < firstEvent.getTime())
		{
			newEvent.setNext(firstEvent);
			firstEvent = newEvent;
		}else
		{
			Event prev = firstEvent;
			Event current = firstEvent;
			for(int i=1; i<=length; i++)
			{
				//adding new event in middle
				if(current.getNext() != null)
				{
					current = current.getNext();
					if(newEvent.getTime() < current.getTime())
					{
						newEvent.setNext(current);
						prev.setNext(newEvent);
						break;
					}else
					{
						prev = current;
					}		
				//adding new event to end
				}else
				{
					lastEvent.setNext(newEvent);
					lastEvent = newEvent;
				}//end else
			}//end for
		}//end else
		
		length++;	//make sure to increment!
	}//end add
	
	public String toString()
	{
		String report = "";
		Event temp = firstEvent;
		for(int i=1; i<=length; i++)
		{
			report += "(" + temp.toString() + ") -> ";
			temp = temp.getNext();
		}
		
		report += "\nFirst Event: " + firstEvent;
		report += "\nLast Event: " + lastEvent;
		
		return report;
	}
	
	public static void main(String[] args)
	{
		Schedule s = new Schedule(new Event("D", 8));
		s.add(new Event("A", 5.5));
		s.add(new Event("D", 18.09));
		s.add(new Event("A", 6));
		s.add(new Event("X", 8.5));
		
		System.out.println(s.toString());
	}

	public Event getFirstEvent() {
		return firstEvent;
	}
	
}
