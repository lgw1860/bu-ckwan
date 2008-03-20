/**
 * @author Christopher Kwan  ckwan@bu.edu  U37-02-3645
 * @name CS350 HW3 Problem 4 M/M/1 Queue Simulator
 * @date 2-19-2008
 * @class Event.java - Event (Arrival, Departure, Monitor) in Simulation 
 * 			compile:	"javac Event.java"
 * 			run:		"java Event"
 */

//package simulator;

public class Event {

	private double time;	//time to execute
	private String type;	//(A)rrival or (D)eparture or (M)onitor
	private Event next;		//following event, based on time
	private double arr; 		//arrival time
	private double origArr;		//first arrival time for repeat customers
	private boolean repeat; //has the packet already been processed before
	private boolean dropped = false; //was the packet dropped?
	
	public Event(String type, double time)
	{
		this.type = type;
		this.time = time;
		this.arr = -1;
		this.repeat = false;
	}
	
	public Event(String type, double time, double arrival)
	{
		this(type,time);
		this.arr = arrival;
	}
	
	public Event(String type, double time, double arrival, double origArr)
	{
		this(type,time,arrival);
		this.origArr = arrival;
	}
	
	public double getOrigArrival()
	{
		return origArr;
	}
	
	public double getArrival()
	{
		return arr;
	}
	
	public boolean getDropped()
	{
		return dropped;
	}
	
	public void setDropped()
	{
		dropped = true;
	}
	
	public boolean getRepeat()
	{
		return repeat;
	}
	
	public void setRepeat(boolean status)
	{
		repeat = status;
	}
	
	public double getTs()
	{
		return arr;
	}
	
	public double getTime()
	{
		return time;
	}
	
	public String getType()
	{
		return type;
	}

	public boolean hasNext()
	{
		if(next == null){return false;}
		else{return true;}
	}
	
	public Event getNext() {
		return next;
	}

	public void setNext(Event next) {
		this.next = next;
	}
	
	public String toString()
	{
		return(type + " " + time);
	}
	
	public static void main(String[] args)
	{
		Event e1 = new Event("A", 0.5);
		System.out.println(e1.toString());
		
		Event e2 = new Event("D", 1.5);
		System.out.println(e2.toString());
		
		Event e3 = new Event("M", 0.01);
		System.out.println(e3.toString());
	}
}
