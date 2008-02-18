package simulator;

public class Control {

	private double Lambda = 0.0;
	private double Ts = 0.0;
	private int SimTime = 0;	//max time to run simulation
	
	private double time;
	private Schedule sched;		//schedule of arrivals/departures
	private Event currentEvent;	//current arr/dep being processed
	
	
	public Control(double Lambda, double Ts, int SimTime)
	{
		this.Lambda = Lambda;
		this.Ts = Ts;
		this.SimTime = SimTime;
		initialize();
		
	}
	
	public void initialize()
	{
		time = 0;	//initialize time to 0
		
		//schedule and execute first arrival
		sched = new Schedule(new Event("A", 3.6));
		currentEvent = sched.getFirstEvent();
		execute(currentEvent);
	}
	
	
	public void simulate()
	{
		sched.add(new Event("A", 15));
		sched.add(new Event("D", 92.6));
		while(time < SimTime)
		{
			System.out.println("time: " + time);
			//check for nulls
			if(currentEvent.getNext() != null)
			{
				currentEvent = currentEvent.getNext();
				time = currentEvent.getTime();
				execute(currentEvent);
			}
			//!!!!
			time ++;
			//!!!!
		}
		
	}
	
	
	/**
	 * Based on event type
	 * @param e
	 */
	public void execute(Event e)
	{
		if(e.getType() == "A")
		{
			System.out.println("Arrival! @ " + time);
		}else if(e.getType() == "D")
		{
			System.out.println("Departure! @ " + time);
		}
	}
	
	
	
	
	public static void main(String[] args)
	{
		Control c = new Control(5.5, 6.8, 100);
		System.out.println(c.Lambda + " " +  c.Ts + " " + c.SimTime);
		
		c.simulate();
		
	}
}
