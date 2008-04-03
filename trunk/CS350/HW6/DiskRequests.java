/**
 * 
 * @author Christopher Kwan
 *
 */
import java.util.*;
public class DiskRequests {
	
	 private double lambda;
     private double n;
     private double u;
     private double v;
     private double y;       //last track
     private double x;       //requested track
     private double totalHeadMovement;
     
	
	//double lambda;
	double Ts;
	double maxTime;
	double monitorInc = 10;//1000;//.01; //time between monitor events
	double maxRequests = 5;//10; //10000;
	
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
	
	LinkedList<Double> tqList = new LinkedList<Double>();

	public DiskRequests(double Lambda, double Ts, double maxTime)
	{
		this.lambda = Lambda;
		this.Ts = Ts;
		this.maxTime = maxTime;
	}

	public static void main(String[] args)
	{
		//DiskRequests qu = new DiskRequests(0.05,0.0085,.1);
		//qu.run();
		DiskRequests dr = new DiskRequests(0.05, 500, 2, 0.1);
		dr.run();
		
	}


    public DiskRequests(double lambda, double n, double u, double v)
    {
            this.lambda = lambda;
            this.n = n;
            this.u = u;
            this.v = v;
            this.y = 0;
            this.x = 0;
            
            this.Ts = 1;
            
    }

	
	
	public void initializeSchedule()
	{
		Event firstMon = new Event("M",0.0+monitorInc);
		sched = new Schedule(firstMon);
		
		Event firstArr = new Event("A", 0.0 + randExp(1.0/lambda));
		arrInNeed = firstArr;
		sched.add(firstArr);
		
		currentEvent = sched.getFirstEvent();
		time = currentEvent.getTime();
		execute(currentEvent);
	}

	public void run()
	{
		initializeSchedule();

		while(numRequests < maxRequests && currentEvent.getNext() != null)
		//while(time < maxTime && currentEvent.getNext() != null)
		{
			currentEvent = currentEvent.getNext();
			time = currentEvent.getTime();
			//if(currentEvent.getTime() < maxTime)
			//{
				execute(currentEvent);
				//}
		}

		//Print stats
		System.out.println("\nSimulation results: ");
		double meanTq = (double)sumTq/(numRequests);
		double meanTs = (double)sumTs/(numRequests);
		System.out.println("mean Ts: " + meanTs);
		
		System.out.println("mean Tq: " + meanTq);;
		System.out.println("mean q: " + (double)sumQ/monitorCount);
		
		System.out.println("mon: " + monitorCount);
		System.out.println("sumTq: " + sumTq);
		System.out.println("requests: " + numRequests);
		System.out.println("total head movement: " + totalHeadMovement);
		
		Iterator iter = tqList.iterator();
		while(iter.hasNext())
		{
			System.out.println(iter.next());
		}
		
		System.out.println("standard dev: " + stdDev(meanTq));
	}

	public void execute(Event cur)
	{
		if(cur.getType() == "A")
		{
			isBusy = false;
			theQueue++;

			//schedule next Arrival
			double nextIAT = randExp(1.0/lambda);
			sumIAT += nextIAT;
			Event nextArr = new Event("A",cur.getTime() + nextIAT, track());
			sched.add(nextArr);

			//if I'm only one in queue, sched my departure
			if(theQueue==1)
			{
				isBusy = true;
				
				//x = currentEvent.getTrack();
				
				double myTs = ts();
				//double myTs = randExp(Ts);
				sumTs += myTs;
				Event myDepart = new Event("D", cur.getTime() + myTs);
				sched.add(myDepart);
				
				lastArr = cur;
				y = lastArr.getTrack();
				updateNextArr();
				x = arrInNeed.getTrack();
				
				totalHeadMovement += Math.abs(y-x);
			}
		}

		else if(cur.getType() == "D")
		{
			isBusy = false;
			numRequests ++;
			theQueue--;
			double Tq = cur.getTime() - lastArr.getTime();
			sumTq += Tq;
			tqList.add(Tq);

			if(theQueue>0)
			{
				isBusy = true;
				
				
				
				double nextTs = ts();
				//double nextTs = randExp(Ts);
				sumTs += nextTs;
				double startTime = max(cur.getTime(), arrInNeed.getTime());
				Event nextDepart = new Event("D",nextTs + startTime);
				sched.add(nextDepart);
				
				
				
				lastArr = arrInNeed;
				y = lastArr.getTrack();
				
				updateNextArr();
				x = arrInNeed.getTrack();
			
				totalHeadMovement += Math.abs(y-x);
				
			}

		}
		else if(cur.getType() == "M")
		{
			monitorCount++;
			System.out.println("Time: " + time + "\tQueue: " + theQueue + 
					"\ty: " + y + "\tx: " + x);

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

	public double stdDev(double mean)
	{
		double diffs = 0.0;
		Iterator iter = tqList.iterator();
		while(iter.hasNext())
		{
			diffs += Math.pow( ((Double)(iter.next()) - mean), 2);
		}
		return Math.sqrt(diffs/tqList.size());
		
	}
	
	public int track()
	{
		return (int)(Math.random()*n);
	}
	
	public double ts()
	{
		return (u + v * Math.abs(y - x));
	}
	
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
