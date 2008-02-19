package simulator;

public class Control {

	private double Lambda = 0.0;
	private double Ts = 0.0;
	private double SimTime = 0;	//max time to run simulation
	
	private String dataTable = "";
	
	private double time;
	private Schedule sched;		//schedule of arrivals/departures
	private Event currentEvent;	//current arr/dep being processed
	private Event arrivalNeedingDeparture;
	
	private int numInQueue = 0;
	private double currentIAT = 0.0;
	private double currentTs = 0.0;
	private double currentTq = 0.0;
	private double currentTw = 0.0;
	private double currentQ = 0.0;
	private double currentW = 0.0;
	
	private int sumInQueue = 0;
	private double sumIAT = 0.0;
	private double sumTs = 0.0;
	private double sumTq = 0.0;
	private double sumTw = 0.0;
	private double sumQ = 0.0;
	private double sumW = 0.0;
	
	private int numRequests = 0;
	
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
				currentTs = randExp(Ts);
				sumTs += currentTs;
				
				
				Event myDeparture = new Event("D", e.getTime()+currentTs);
				sched.add(myDeparture);
				//System.out.println("DDD: " + myDeparture);
				
				//advanceArrival();
				arrivalNeedingDeparture = e;
				
			}
			//schedule next arrival
			currentIAT = randExp(1.0 / Lambda);
			sumIAT += currentIAT;
			
			Event nextArrival = new Event("A", e.getTime()+currentIAT);
			sched.add(nextArrival);
			//System.out.println("AAA: " + nextArrival);
			
		}else if(e.getType() == "D")
		{
			//System.out.println("Departure! @ " + time + "My arrival: " + arrivalNeedingDeparture);
			
			numRequests ++; //a request has finished, increment counter
			
			currentTq = e.getTime() - arrivalNeedingDeparture.getTime();
			sumTq += currentTq;
			
			currentTw = currentTq - currentTs;	//Tq = Tw + Ts
			sumTw += currentTw;
			
			currentQ = Lambda * currentTq;		//q = Lambda * Tq
			sumQ += currentQ;
			
			currentW = Lambda * currentTw;		//w = Lambda * Tw
			sumW += currentW;
			
			dataTable = dataTable + (
			//System.out.println(
					+ cleanDouble(currentIAT) + "\t"
					+ cleanDouble(currentTs) + "\t" 
					+ cleanDouble(arrivalNeedingDeparture.getTime()) + "\t" 
					+ cleanDouble(e.getTime()) + "\t"
					+ cleanDouble(currentTq ) + "\t" 
					+ cleanDouble(currentTw ) + "\t"
					+ (int)(currentQ) + "\t"
					+ (int)(currentW) + "\t"
					+ numInQueue
					+ "\n");
			
			
			if(numInQueue>0)
			{
				numInQueue--;
				//schedule next departure
				//Event nextDeparture = new Event("D", time+1);
				
				if(numInQueue > 0)
				{
				//Stats
				advanceArrival();
				currentTs = randExp(Ts);
				sumTs += currentTs;
				
				//Event nextDeparture = new Event("D", arrivalNeedingDeparture.getTime()+randTime());
				
				//MAX
				
				Event nextDeparture = new Event("D", e.getTime()+currentTs);
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
	
	
	double randExp(double T)
	{
		/* Relationship derivation:
		 * F(U) = U, where 0 <= U <= 1
		 * F(V) = 1 - exp(-lambda*V), where 0<= V <= infinity
		 * Equating the above two, we get:
		 * U = 1 - exp(-lambda*V)
		 * Which leads to the following relationship.
		 * V = - ln(1-U) /lambda
		 */
		double U = Math.random();
		double lambda = 1.0/T;
		double V = ( -1 * (Math.log(1.0 - U)) ) / lambda; 
		return V;
	}
	
	
	private double cleanDouble(double number)
	{
		double cleanNumber = number * 10000.0;
		cleanNumber = ((int)cleanNumber) / 10000.0;
		return cleanNumber;
	}
	
	public String endStats()
	{
		String report = "Means:\n";
		report += cleanDouble( sumIAT / numRequests) + "\t";
		report += cleanDouble( sumTs / numRequests) + "\t";
		report += "\t";
		report += "\t";
		report += cleanDouble( sumTq / numRequests) + "\t";
		report += cleanDouble( sumTw / numRequests) + "\t";
		report += (int)( sumQ / numRequests) + "\t";
		report += (int)( sumW / numRequests) + "\t";
		return report;
	}
	
	public String toString()
	{
		String report = "IAT \tTs \tArr \tDep \tTq \tTw \tq \tw \tn \n";
		report += dataTable;
		report += endStats();
		return report;
	}
	
	public static void main(String[] args)
	{
		Control c = new Control(5.0, 0.15, 10);
		System.out.println(c.Lambda + " " +  c.Ts + " " + c.SimTime);
		
		c.simulate();
		
		System.out.println(c.toString());
		
		//System.out.println(c.randTime());
		
		//System.out.println(c.cleanDouble(55.625));
		//System.out.println(c.cleanDouble(c.randExp(1.0/5.0)));
		
		/*
		double sum = 0.0;
		for(int i=0; i<10; i++)
		{
			double num = c.randExp(0.02);
			System.out.println(num);
			sum = sum + num;
		}
		System.out.println("Actual: " + 0.02);
		System.out.println("Mean: " + sum/1000.0);
		*/
	}
}
