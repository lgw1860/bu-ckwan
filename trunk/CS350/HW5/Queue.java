/**
 * 
 * @author Christopher Kwan
 *
 */
public class Queue {
	
	double Lambda;
	double Ts;
	double maxTime;
	double monitorInc = .01; //time between monitor events
	
	double time;	//current time
	Schedule sched;
	Event currentEvent;
	int theQueue;	 //the actual wait queue
	Event lastArr;   //prev arrival that just departed
	Event arrInNeed; //next arrival in need of a departure
	boolean isBusy = false; //is the system processing a request?
	
	int monitorCount;
	int numRequests; //# completed requests (ie: departures)
	double sumIAT;
	double sumTs;
	int sumQ;
	int sumW;
	double sumTq;

	public Queue(double Lambda, double Ts, double maxTime)
	{
		this.Lambda = Lambda;
		this.Ts = Ts;
		this.maxTime = maxTime;
	}

	public static void main(String[] args)
	{
		Queue qu = new Queue(100,0.0085,.1);
		qu.run();
	}

	public void initializeSchedule()
	{
		Event firstMon = new Event("M",0.0+monitorInc);
		sched = new Schedule(firstMon);
		
		Event firstArr = new Event("A", 0.0 + randExp(1.0/Lambda));
		arrInNeed = firstArr;
		sched.add(firstArr);
		
		currentEvent = sched.getFirstEvent();
		time = currentEvent.getTime();
		execute(currentEvent);
	}

	public void run()
	{
		initializeSchedule();

		while(time < maxTime && currentEvent.getNext() != null)
		{
			currentEvent = currentEvent.getNext();
			time = currentEvent.getTime();
			if(currentEvent.getTime() < maxTime)
			{execute(currentEvent);}
		}

		//Print stats
		System.out.println("\nSimulation results: ");
		double meanTq = (double)sumTq/(numRequests);
		double meanTs = (double)sumTs/(numRequests);
		System.out.println("mean IAT: " + (double)sumIAT/(numRequests));
		System.out.println("mean Ts: " + meanTs);
		System.out.println("mean q: " + (double)sumQ/monitorCount);
		System.out.println("mean w: " + (double)sumW/monitorCount);
		System.out.println("mean Tq: " + meanTq);
		System.out.println("mean Tw: " + (meanTq - meanTs));
		
		System.out.println("mon: " + monitorCount);
		System.out.println("sumTq: " + sumTq);
		System.out.println("requests: " + numRequests);
	}

	public void execute(Event cur)
	{
		if(cur.getType() == "A")
		{
			isBusy = false;
			theQueue++;

			//schedule next Arrival
			double nextIAT = randExp(1.0/Lambda);
			sumIAT += nextIAT;
			Event nextArr = new Event("A",cur.getTime() + nextIAT);
			sched.add(nextArr);

			//if I'm only one in queue, sched my departure
			if(theQueue==1)
			{
				isBusy = true;
				
				double myTs = randExp(Ts);
				sumTs += myTs;
				Event myDepart = new Event("D", cur.getTime() + myTs);
				sched.add(myDepart);
				
				lastArr = cur;
				updateNextArr();
			}
		}

		else if(cur.getType() == "D")
		{
			isBusy = false;
			numRequests ++;
			theQueue--;
			double Tq = cur.getTime() - lastArr.getTime();
			sumTq += Tq;

			if(theQueue>0)
			{
				isBusy = true;
				
				double nextTs = randExp(Ts);
				sumTs += nextTs;
				double startTime = max(cur.getTime(), arrInNeed.getTime());
				Event nextDepart = new Event("D",nextTs + startTime);
				sched.add(nextDepart);
				
				lastArr = arrInNeed;
				updateNextArr();
			}

		}
		else if(cur.getType() == "M")
		{
			monitorCount++;
			System.out.println("Time: " + time + "\tQueue: " + theQueue);

			if(theQueue>0)
			{
				if(isBusy)
				{
					sumQ += theQueue;
					if(theQueue-1>0)
						{sumW += theQueue-1;}
				}else
				{
					if(theQueue-1 > 0)
						{sumQ += theQueue-1;}
					if(theQueue-2>0)
						{sumW += theQueue-2;}
				}
			}
			
			Event nextMon = new Event("M",cur.getTime() + monitorInc);
			sched.add(nextMon);
		}
	}//end of execute

	private double max(double a, double b)
	{
		if(a > b)
		{
			return a;
		}
		return b;
	}

	private void updateNextArr()
	{
		while(arrInNeed.getNext() != null)
		{
			arrInNeed = arrInNeed.getNext();
			if(arrInNeed.getType() == "A")
			{
				return;
			}
		}
	}

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

}//end of class
