package simulator;

public class Control {

	private double Lambda = 0.0;
	private double Ts = 0.0;
	private double SimTime = 0;	//max time to run simulation
	
	private double time;
	private Schedule sched;		//schedule of arrivals/departures
	private Event currentEvent;	//current arr/dep being processed
	private Event arrivalNeedingDeparture;
	
	private int numInQueue = 0;
	
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
		time = currentEvent.getTime();
		execute(currentEvent);
		arrivalNeedingDeparture = currentEvent;
	}
	
	
	public void simulate()
	{
		//sched.add(new Event("A", 6.7));
		//sched.add(new Event("D", 3.7));
		while(time < SimTime)
		{
			//System.out.println("time: " + time);
			//System.out.println("n: " + numInQueue + "\n");
			//System.out.println(numInQueue);
			
			
			//check for nulls
			if(currentEvent.getNext() != null)
			{
				currentEvent = currentEvent.getNext();
				time = currentEvent.getTime();
				
//				currentEvent may have been scheduled for past SimTime
				if(time < SimTime)
				{
					execute(currentEvent);
				}
			}
			//!!!!
			//time ++;
			//!!!!
		}
		
	}
	
	public double randTime()
	{
		double time = Math.random();
		time = ( (int)(time*100+5) ) / 100.00;
		return time;
	}
	
	/**
	 * Based on event type
	 * @param e
	 */
	public void execute(Event e)
	{
		if(e.getType() == "A")
		{
			//System.out.println("Arrival! @ " + time);
			
			numInQueue++;
			if(numInQueue == 1)
			{
				Event myDeparture = new Event("D", cleanDouble(e.getTime()+randTime()));
				sched.add(myDeparture);
				//System.out.println("DDD: " + myDeparture);
				
				//advanceArrival();
				arrivalNeedingDeparture = e;
				
			}
			//schedule next arrival
			Event nextArrival = new Event("A", cleanDouble( e.getTime()+randTime() ));
			sched.add(nextArrival);
			//System.out.println("AAA: " + nextArrival);
			
		}else if(e.getType() == "D")
		{
			//System.out.println("Departure! @ " + time + "My arrival: " + arrivalNeedingDeparture);
			System.out.println(111 + "\t" + 555 + "\t" + arrivalNeedingDeparture.getTime() + "\t" + e.getTime() + "\t"
					+ cleanDouble( (e.getTime() - arrivalNeedingDeparture.getTime()) ) );
			if(numInQueue>0)
			{
				numInQueue--;
				//schedule next departure
				//Event nextDeparture = new Event("D", time+1);
				
				if(numInQueue > 0)
				{
				//Stats
				advanceArrival();
				//Event nextDeparture = new Event("D", arrivalNeedingDeparture.getTime()+randTime());
				Event nextDeparture = new Event("D", cleanDouble( e.getTime()+randTime() ));
				//Event nextDeparture = new Event("D", e.getTime()+randTime());
				//System.out.println("DD: " + nextDeparture + "arrivalNeeding: " + arrivalNeedingDeparture);
				
				sched.add(nextDeparture);
				}
				//advanceArrival();
				
				//System.out.println("DDD: " + nextDeparture);
				//numInQueue--;
			
			}
			
		}
	}
	
	
	private void advanceArrival()
	{
		if(arrivalNeedingDeparture != null && arrivalNeedingDeparture.getNext() != null)
		{
			if(arrivalNeedingDeparture.getNext().getType() == "A")
			{
				arrivalNeedingDeparture = arrivalNeedingDeparture.getNext();
			}else
			{
				while(arrivalNeedingDeparture.getNext().getType() != "A")
				{
					arrivalNeedingDeparture = arrivalNeedingDeparture.getNext();
				}
				arrivalNeedingDeparture = arrivalNeedingDeparture.getNext();
			}
		}
	}
	
	private double cleanDouble(double number)
	{
		double cleanNumber = number * 100.0;
		cleanNumber = ((int)cleanNumber) / 100.0;
		return cleanNumber;
	}
	
	public static void main(String[] args)
	{
		Control c = new Control(5.5, 6.8, 10);
		//System.out.println(c.Lambda + " " +  c.Ts + " " + c.SimTime);
		
		System.out.println("IAT\tTs\tArr\tDep\tTq");
		
		c.simulate();
		//System.out.println(c.randTime());
		
		System.out.println(c.cleanDouble(55.625));
	}
}
