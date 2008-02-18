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
		
	}
	
	/**
	 * Based on event type
	 * @param e
	 */
	public void execute(Event e)
	{
		if(e.getType() == "A")
		{
			System.out.println("Arrival!");
		}else if(e.getType() == "D")
		{
			System.out.println("Departure!");
		}
	}
	
	public static void main(String[] args)
	{
		Control c = new Control(5.5, 6.8, 100);
		System.out.println(c.Lambda + " " +  c.Ts + " " + c.SimTime);
		//c.doStuff();
		
		Schedule s = new Schedule(new Event("A", 5.5));
		s.add(new Event("A", 8.8));
		
		c.currentEvent = s.getFirstEvent();
		c.execute(c.currentEvent);
		
		/*
		for(int i=0; i<10; i++)
		{
			double time = Math.random() * 100;
			int tempTime = (int)(time);
			time = (double)tempTime / 100.00;
			
			Event e = new Event("J", time);
			s.add(e);
		}
		*/
		
		System.out.println(s.toString());
		
		
	}
}
