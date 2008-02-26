
/**
 * @author Christopher Kwan  ckwan@bu.edu  U37-02-3645
 * @name CS350 HW4 Problem 6 Part 1 M/M/1/K Queue Simulator
 * @date 2-26-2008
 * @class ControlMM1K.java - Controller of SimulatorMM1K
 * 			compile:	"javac ControlMM1K.java"
 * 			run:		"java ControlMM1K"
 */

//package simulator;

import java.io.*;
import java.util.*;

public class ControlMM1K {

	static FileOutputStream fileDataTable;	//a file output object
	static PrintStream psDataTable; //a print stream object

	static FileOutputStream fileMonitorLog;
	static PrintStream psMonitorLog;

	private int K = 0;				//maximum size of queue
	private double Lambda = 0.0;	//mean rate of arrivals
	private double Ts = 0.0;		//mean time of service
	private double SimTime = 0;		//max time to run simulation

	private double monitorInc = 0.1;	//Time between monitor events

	private boolean isBusy = false;

	private double time;			//current simulation time
	private Schedule sched;			//schedule of arrivals/departures
	private Event currentEvent;		//current arr/dep being processed
	private Event arrivalNeedingDeparture;	//next departure will be for this arrival
	private boolean arrivalNeedingRejected = false;

	private int numInQueue = 0;		//number of items currently in queue
	private int numAccepted = 0;	//total number of requests served

	private int numRejected = 0;

	private LinkedList<Double> qList;
	private LinkedList<Double> TqList;



	//Queue variables
	private double currentIAT = 0.0;
	private double currentTs = 0.0;
	private double currentTq = 0.0;
	private double currentTw = 0.0;
	private double currentRho = 0.0;
	private double currentQ = 0.0;
	private double currentW = 0.0;

	//cumulative sum of queue variables - for stats
	private double sumIAT = 0.0;
	private double sumTs = 0.0;
	private double sumTq = 0.0;
	private double sumTw = 0.0;
	private double sumRho = 0.0;
	private double sumQ = 0.0;
	private double sumW = 0.0;

	/*
	 * Construct new M/M/1 simulation.
	 */
	public ControlMM1K(int K, double Lambda, double Ts, int SimTime)
	{
		this.K = K;
		this.Lambda = Lambda;
		this.Ts = Ts;
		this.SimTime = SimTime;
		qList = new LinkedList<Double>();
		TqList = new LinkedList<Double>();
		initialize();
	}

	/*
	 * Initialize system by scheduling first arrival.
	 */
	public void initialize()
	{
		sched = new Schedule(new Event("M", monitorInc));
		time = 0;	//initialize time to 0

		//schedule and execute first arrival
		currentIAT = randExp(1.0 / Lambda);
		sched.add(new Event("A", currentIAT));

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

			//if the queue is full, the request is rejected
			if(numInQueue < K)
			{
				numInQueue++;
				arrivalNeedingRejected = false;
			}
			else
			{
				numRejected++;
				arrivalNeedingRejected = true;
			}

			//if this is only request in queue, schedule its departure
			if(numInQueue == 1)
			{
				isBusy = true;

				currentTs = randExp(Ts);
				sumTs += currentTs;


				Event myDeparture = new Event("D", e.getTime()+currentTs);
				sched.add(myDeparture);

				arrivalNeedingDeparture = e;

			}else
			{
				isBusy = false;
			}
			//if(numInQueue < K)
			//{
//			schedule next arrival
			currentIAT = randExp(1.0 / Lambda);
			sumIAT += currentIAT;


			Event nextArrival = new Event("A", e.getTime()+currentIAT);
			sched.add(nextArrival);
			//}
			/*
			else
			{
				numRejected ++;
			}
			 */

			//Departure
		}else if(e.getType() == "D")
		{
			currentTq = e.getTime() - arrivalNeedingDeparture.getTime();
			currentTw = currentTq - currentTs;	//Tq = Tw + Ts

			if(isBusy == true)
			{
				currentQ = numInQueue;
			}
			else
			{
				currentQ = numInQueue-1;
			}

			//only calculate if the corresponding arrival was not rejected
			if(arrivalNeedingRejected == false)
			{
				numAccepted ++; //a request has finished, increment counter

				sumTq += currentTq;
				TqList.add(currentTq);

				sumTw += currentTw;

				currentRho = (1.0/currentIAT) * currentTs;
				sumRho += currentRho;

				sumQ += currentQ;
				qList.add(currentQ);

				currentW = currentQ - currentRho;
				sumW += currentW;

				//Data table of stats for all requests so far.
				//This is commented because it makes run time very slow.
				//dataTable = dataTable + (
				//System.out.println(
				psDataTable.println(
						+ cleanDouble(time) + "    \t"							//time
						+ cleanDouble(sumIAT/(numAccepted+numRejected)) + " \t"	//IAT
						+ cleanDouble(sumTs/(numAccepted+numRejected)) + " \t"	//Ts
						+ cleanDouble(sumRho/numAccepted) + " \t"				//Rho
						+ cleanDouble(arrivalNeedingDeparture.getTime()) + "    \t" //Arr
						+ cleanDouble(e.getTime()) + "    \t"						//Dep
						+ cleanDouble(sumTw/numAccepted ) + " \t"				//Tw
						+ cleanDouble(sumTq/numAccepted ) + " \t" 				//Tq
						+ cleanDouble(sumW/numAccepted) + " \t"					//w
						+ cleanDouble(sumQ/numAccepted) + " \t"					//q
						);
			}

			//Don't need to do anything with an empty Queue!
			if(numInQueue>0)
			{
				numInQueue--;
				isBusy = true;

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
			else
			{
				isBusy = false;
			}
		}
		//Monitor
		else if(e.getType() == "M")
		{
			if(psMonitorLog != null)
			{
				psMonitorLog.println(cleanDouble(e.getTime()) + "\t\t" + numInQueue);
			}
			//schedule next monitor event
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

		//ANALYTICALLY
		report += "\nUsing equations (analytically):\n";
		
		report += "IAT: " + cleanDouble( 1.0 / Lambda) + " \t";
		report += "Ts: " + cleanDouble( Ts ) + " \t";

		//Calculations from parameters
		double Rho = Lambda*Ts;				//Rho = Lambda / Mew, Rho = Lambda * Ts

		double myQ = 0.0;
		if(Rho == 1.0)
		{
			myQ = (double)K / 2.0;
		}
		else	//Rho != 1
		{
			//q = [Rho / (1-Rho)] - [ (K+1)*Rho^(K+1) / (1 - Rho^(K+1)) ]
			myQ = (Rho / (1.0-Rho)) - 
			( ((double)K+1.0) * Math.pow(Rho, (double)K+1.0) 
					/ (1 - Math.pow(Rho, (double)K+1.0)) );
		}

		double rejProb = 0.0;
		if(Rho == 1)
		{
			rejProb = 1.0 / (K + 1.0);
		}
		else	//Rho != 1
		{
			rejProb = ( (1.0 - Rho) * Math.pow(Rho, K) ) / (1 - Math.pow(Rho, (K+1.0) ));
		}
		double lambdaPrime = Lambda * (1.0 - rejProb);
		double myTq = myQ / lambdaPrime; 		//q = Lambda * Tq, Tq = q / Lambda
		
		double myTw = myTq - Ts;			//Tq = Tw + Ts, Tw = Tq - Ts
		double myW = myQ - Rho;

		report += "Rho: " + cleanDouble(Rho) + "\t";
		report += "Tw: "+ cleanDouble(myTw) + " \t"; 
		report += "Tq: " + cleanDouble(myTq) + " \t";
		report += "w: " + cleanDouble(myW) + " \t";//(int)myW + "\t";
		report += "q: " + cleanDouble(myQ) + " \t";//(int)myQ + "\t";
		report += "RejectionProb: " + cleanDouble(rejProb);
		
		//SIMULATION
		report += "\n\nUsing simulation:\n";
		report += "IAT: " + cleanDouble( sumIAT / (numAccepted+numRejected)) + " \t";
		report += "Ts: " + cleanDouble( sumTs / (numAccepted+numRejected)) + " \t";
		report += "Rho: " + cleanDouble( sumRho / numAccepted) + "\t";
		report += "Tw: " + cleanDouble( sumTw / (numAccepted)) + " \t";
		report += "Tq: " + cleanDouble( sumTq / (numAccepted)) + " \t";
		report += "w: " + cleanDouble( sumW / (numAccepted)) + " \t";//(int)( sumW / numRequests) + "\t";
		report += "q: " + cleanDouble( sumQ / (numAccepted)) + " \t";//(int)( sumQ / numRequests) + "\t";
		report += "RejectionProb: " + cleanDouble((double)numRejected / ((double)numRejected + (double)numAccepted));

		
		//CONFIDENCE INTERVALS
		report += "\n\n95th Percentile Confidence Intervals:\n";
		double qMean = cleanDouble(sumQ/numAccepted);
		double qError = cleanDouble(ConfidenceIntervalError(qMean ,numAccepted));
		report += "q: " + " \t" + "[" + qMean + "-" + qError + ", " + qMean + "+" + qError + "]";
		report += " = [" + cleanDouble(qMean - qError) + ", " + cleanDouble(qMean + qError) + "]\n";
		
		double TqMean = cleanDouble(sumTq/numAccepted);
		double TqError = cleanDouble(ConfidenceIntervalError(TqMean ,numAccepted));
		report += "Tq: " + " \t" + "[" + TqMean + "-" + TqError + ", " + TqMean + "+" + TqError + "]";
		report += " = [" + cleanDouble(TqMean - TqError) + ", " + cleanDouble(TqMean + TqError) + "]";
		
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

		System.out.println(
				"M/M/1/K Queue Simulation\n"
				+ "K: " + K + "\n"
				+ "Lambda: " + Lambda + "\n" 
				+  "Ts: " + Ts + "\n" 
				+  "Simulation Time: " + SimTime);

		System.out.println("\nPlease wait...");

		try
		{
			fileDataTable = new FileOutputStream("DataTable.txt");
			fileMonitorLog = new FileOutputStream("MonitorLog.txt");

			//Connect print stream to output stream
			psDataTable = new PrintStream(fileDataTable);
			psMonitorLog = new PrintStream(fileMonitorLog);

			psMonitorLog.println("Time \tNumber in Queue");

			psDataTable.println(
					"K: " + K + "\n"
					+ "Lambda: " + Lambda + "\n" 
					+  "Ts: " + Ts + "\n" 
					+  "Simulation Time: " + SimTime + "\n");

			psDataTable.println("time      \tIAT  \tTs	\tRho" + 
					"    \tArr      \tDep     \tTw     " +
					"\tTq     \tw   \tq   \n");

			simulate();

			
			psDataTable.println("time      \tIAT  \tTs	\tRho" + 
					"    \tArr      \tDep     \tTw     " +
					"\tTq     \tw   \tq   \n");

			psDataTable.println(endStats());

			psDataTable.println("\nNumber rejected: " + numRejected);

			psMonitorLog.close();

			psDataTable.close();

		}
		catch (Exception e)
		{
			System.err.println("Error.");
		}

		System.out.println(toString());
	}


	public double stdDev(double mean, LinkedList<Double> list)
	{
		double difference = 0.0;
		double sum = 0.0;
		double variance = 0.0;
		double deviation = 0.0;

		Iterator iter = list.iterator();
		while(iter.hasNext())
		{
			difference = ((Double)(iter.next()) - mean);
			sum += Math.pow(difference, 2.0);
		}

		variance = sum / list.size();
		deviation = Math.sqrt(variance);
		return deviation;
	}

	public double ConfidenceIntervalError(double StdDev, int SampleSize)
	{
		//for 95th percentile confidence interval
		//Z alpha/2 = 1.96 from the lookup table
		//double Z = 1.96;

		double Z = 2.31;

		double error = Z * ((StdDev) / Math.sqrt(SampleSize));
		return error;
	}

	public static void main(String[] args)
	{
		//ControlMM1K c = new ControlMM1K(5, 30, 0.03, 100);
		ControlMM1K c = new ControlMM1K(5, 50, 0.03, 100);

		c.run();
	}
}
