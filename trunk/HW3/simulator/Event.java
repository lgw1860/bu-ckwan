package simulator;

public class Event {

	private double time;	//time to execute
	private String type;	//(A)rrival or (D)eparture
	private Event next;		//following event, based on time
	
	public Event(String type, double time)
	{
		this.type = type;
		this.time = time;
	}
	
	public double getTime()
	{
		return time;
	}
	
	public String getType()
	{
		return type;
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
}
