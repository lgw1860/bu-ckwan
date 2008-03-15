
/**
 * @author Christopher Kwan  ckwan@bu.edu  U37-02-3645
 * @name CS350 HW3 Problem 4 M/M/1 Queue Simulator
 * @date 2-19-2008
 * @class ControlMM1.java - Controller of Simulator
 * 			compile:	"javac ControlMM1.java"
 * 			run:		"java ControlMM1"
 */

//package simulator;

import java.io.*;

public class ControlMM1 {

	static FileOutputStream fileDataTable;	//a file output object
	static PrintStream psDataTable; //a print stream object

	static FileOutputStream fileMonitorLog;
	static PrintStream psMonitorLog;

	private double Lambda = 0.0;	//mean rate of arrivals
	private double Ts = 0.0;		//mean time of service
	private double SimTime = 0;		//max time to run simulation

	private double monitorInc = 0.1;	//Time between monitor events

	private double time;			//current simulation time
	private Schedule sched;			//schedule of arrivals/departures
	private Event currentEvent;		//current arr/dep being processed
	private Event arrivalNeedingDeparture;	//next departure will be for this arrival

	private int numInQueue = 0;		//number of items currently in queue
	private int numRequests = 0;	//total number of requests served

	//Queue variables
	private double currentIAT = 0.0;
	private double currentTs = 0.0;
	private double currentTq = 0.0;
	private double currentTw = 0.0;
	private double currentQ = 0.0;
	private double currentW = 0.0;

	//cumulative sum of queue variables - for stats
	private double sumIAT = 0.0;
	private double sumTs = 0.0;
	private double sumTq = 0.0;
	private double sumTw = 0.0;
	private double sumQ = 0.0;
	private double sumW = 0.0;

	/*
	 * Construct new M/M/1 simulation.
	 */
	public ControlMM1(double Lambda, double Ts, int SimTime)
	{
		this.Lambda = Lambda;
		this.Ts = Ts;
		this.SimTime = SimTime;
		initialize();
	}

	/*
	 * Initialize system by scheduling first arrival.
	 */
	public void initialize()
	{
		sched = new Schedule(new Event("M", monitorInc));

		/*
		//fill schedule with Monitor events
		for(double i = monitorInc+monitorInc; i<SimTime; i=i+monitorInc)
		{
			//System.out.println(i);
			sched.add(new Event("M", i));
		}
		 */
		time = 0;	//initialize time to 0

		//schedule and execute first arrival
		currentIAT = randExp(1.0 / Lambda);
		sched.add(new Event("A", currentIAT));
		//sched = new Schedule(new Event("A", currentIAT));
		currentEvent = sched.getFirstEvent();
		time = currentEvent.getTime();
		execute(currentEvent);
		arrivalNeedingDeparture = currentEvent;
	}

	/*
	 * Run the simulation until the max simulation time.
	 */
	public void simulate()
	{
		while(time < SimTime)
		{
			if(currentEvent.getNext() != null)
			{
				currentEvent = currentEvent.getNext();
				time = currentEvent.getTime();

				//only execute events scheduled before end of simulation
				if(time < SimTime)
				{
					execute(currentEvent);
				}
			}
		}

	}

	/**
	 * Execute an event based on its type: Arrival, Departure, Monitor
	 */
	public void execute(Event e)
	{
		//Arrival
		if(e.getType() == "A")
		{	
			//System.out.println("\t" + e.toString());

			numInQueue++;

			//if this is only request in queue, schedule its departure
			if(numInQueue == 1)
			{
				currentTs = randExp(Ts);
				sumTs += currentTs;

				Event myDeparture = new Event("D", e.getTime()+currentTs);
				sched.add(myDeparture);

				arrivalNeedingDeparture = e;

			}
			//schedule next arrival
			currentIAT = randExp(1.0 / Lambda);
			sumIAT += currentIAT;

			Event nextArrival = new Event("A", e.getTime()+currentIAT);
			sched.add(nextArrival);

			//Departure
		}else if(e.getType() == "D")
		{
			//System.out.println("\t" + e.toString());

			numRequests ++; //a request has finished, increment counter

			currentTq = e.getTime() - arrivalNeedingDeparture.getTime();
			sumTq += currentTq;

			currentTw = currentTq - currentTs;	//Tq = Tw + Ts
			sumTw += currentTw;

			currentQ = Lambda * currentTq;		//q = Lambda * Tq
			sumQ += currentQ;

			currentW = Lambda * currentTw;		//w = Lambda * Tw
			sumW += currentW;

			//Data table of stats for all requests so far.
			//This is commented because it makes run time very slow.
			//dataTable = dataTable + (
			//System.out.println(
			psDataTable.println(
					+ cleanDouble(time) + " \t"
					//+ cleanDouble(currentIAT) + "\t"
					//+ cleanDouble(currentTs) + "\t" 
					+ cleanDouble(arrivalNeedingDeparture.getTime()) + " \t" 
					+ cleanDouble(e.getTime()) + " \t"
					+ cleanDouble(currentTw ) + " \t"
					+ cleanDouble(currentTq ) + " \t" 
					+ cleanDouble(currentW) + " \t"//(int)(currentW) + "\t"
					+ cleanDouble(currentQ) + " \t"//(int)(currentQ) + "\t"
					+ numInQueue);

			//Don't need to do anything with an empty Queue!
			if(numInQueue>0)
			{
				numInQueue--;

				//schedule next departure
				if(numInQueue > 0)
				{
					advanceArrival();	//the arrival needing departure has been served, so update to next

					currentTs = randExp(Ts);
					sumTs += currentTs;

					//Start of service must wait for both this departure to finish
					//and for the arrival needing departure to actually arrive
					//so take the later of the two
					double actualServiceStart = max(e.getTime(), arrivalNeedingDeparture.getTime());

					Event nextDeparture = new Event("D", actualServiceStart+currentTs);
					sched.add(nextDeparture);
				}//end if
			}//end if
		}
		//Monitor
		else if(e.getType() == "M")
		{
			//System.out.println(numInQueue);

			//System.out.println("\t" + e.toString());

			if(psMonitorLog != null)
			{
				psMonitorLog.println(cleanDouble(e.getTime()) + "\t\t" + numInQueue);
			}


			sched.add(new Event("M", e.getTime()+monitorInc));
		}
	}

	/*
	 * Helper - return maximum of two doubles
	 */
	private double max(double a, double b)
	{
		if(a > b)
		{
			return a;
		}
		else
		{
			return b;
		}
	}

	/*
	 * Helper - advance the pointer to next arrival that needs a departure
	 */
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

	/*
	 * Helper - Exponential distribution around mean T
	 * Maps uniform random to exponential random
	 */
	private double randExp(double T)
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

	/*
	 * Helper - cleans a double for formatting
	 * Truncates all decimals except for 4 places
	 */
	private double cleanDouble(double number)
	{
		double cleanNumber = number * 10000.0;
		cleanNumber = ((int)cleanNumber) / 10000.0;
		return cleanNumber;
	}

	/*
	 * Comparison report of statistics through data and through calculation
	 */
	public String endStats()
	{
		String report = "";
		//	"\nIAT   \tTs     \tArr   \tDep   \tTq     " +
		// "\tTw     \tq \tw \tn \n";
		
		report += "\nUsing equations (analytically):\n";
		//report += "\n\nMeans from calculation of parameters:\n";
		report += "IAT: " + cleanDouble( 1.0 / Lambda) + " \t";
		report += "Ts: " + cleanDouble( Ts ) + " \t";
		//report += "\t";
		//report += "\t\t\t";

		//Calculations from parameters
		double Rho = Lambda*Ts;				//Rho = Lambda / Mew, Rho = Lambda * Ts
		double myQ = Rho / (1.0 - Rho);		//q = Rho / (1 - Rho)
		double myTq = myQ / Lambda; 		//q = Lambda * Tq, Tq = q / Lambda
		double myTw = myTq - Ts;			//Tq = Tw + Ts, Tw = Tq - Ts
		double myW = Lambda * myTw;			//w = Lambda * Tw

		report += "Tw: "+ cleanDouble(myTw) + " \t"; 
		report += "Tq: " + cleanDouble(myTq) + " \t";
		report += "w: " + cleanDouble(myW) + " \t";//(int)myW + "\t";
		report += "q: " + cleanDouble(myQ) + " \t";//(int)myQ + "\t";
		
		
		report += "\n\nUsing simulation:\n";
		//report += "\nMeans from data:\n";
		report += "IAT: " + cleanDouble( sumIAT / numRequests) + " \t";
		report += "Ts: " + cleanDouble( sumTs / numRequests) + " \t";
		//report += "\t";
		//report += "\t\t\t";
		report += "Tw: " + cleanDouble( sumTw / numRequests) + " \t";
		report += "Tq: " + cleanDouble( sumTq / numRequests) + " \t";
		report += "w: " + cleanDouble( sumW / numRequests) + " \t";//(int)( sumW / numRequests) + "\t";
		report += "q: " + cleanDouble( sumQ / numRequests) + " \t";//(int)( sumQ / numRequests) + "\t";
		
		
		return report;
	}

	public String toString()
	{
		String report = "";
		//report += "IAT \tTs \tArr \tDep \tTq \tTw \tq \tw \tn \n";
		//report += dataTable;
		report += endStats();
		return report;
	}


	public void run()
	{
		//System.out.println("Lambda: " + Lambda + "\t" 
		//		+  "Ts: " + Ts + "\t" 
		//		+  "Simulation Time: " + SimTime);
		
		//System.out.println("\nPlease see DataTable.txt and MonitorLog.txt");
		System.out.println("\nPlease wait...");

		try
		{
			fileDataTable = new FileOutputStream("DataTable.txt");
			fileMonitorLog = new FileOutputStream("MonitorLog.txt");

			//Connect print stream to output stream
			psDataTable = new PrintStream(fileDataTable);
			psMonitorLog = new PrintStream(fileMonitorLog);

			psMonitorLog.println("Time \tNumber in Queue");

			psDataTable.println("Lambda: " + Lambda + "\n" 
					+  "Ts: " + Ts + "\n" 
					+  "Simulation Time: " + SimTime + "\n");

			psDataTable.println("t    \tArr   \tDep   \tTw     " +
			"\tTq     \tw   \tq   \tn \n");

			simulate();

			//ps.println (report);

			psDataTable.println(endStats());

			psMonitorLog.close();

			psDataTable.close();

		}
		catch (Exception e)
		{
			System.err.println("Error.");
		}

		System.out.println(toString());
		
	}

	public static void main(String[] args)
	{
		ControlMM1 c = new ControlMM1(5, 0.15, 1000);
		//ControlMM1 c = new ControlMM1(100, 0.0085, 100);
		//ControlMM1 c = new ControlMM1(100, 0.002, 100);
		//ControlMM1 c = new ControlMM1(30, 0.03, 100);

		System.out.println("Lambda: " + c.Lambda + "\n" 
				+  "Ts: " + c.Ts + "\n" 
				+  "Simulation Time: " + c.SimTime);
		
		c.run();

		
	}
}
