/**
 * 
 * @author Christopher Kwan
 *
 */
public class QueueNet {

	double Lambda;
	double Ts;
	double maxTime;
	int K;	//maximum size of waiting queue
	double monitorInc = .01; //time between monitor events

	double time;	//current time
	Schedule sched;
	Event currentEvent;
	int monitorCount;
	int numDropped;	//number of packets dropped

	//CPU
	int theQueueCPU;	 //the actual wait queue
	Event lastArrCPU;   //prev arrival that just departed
	Event arrInNeedCPU; //next arrival in need of a departure
	boolean isBusyCPU = false; //is the system processing a request?
	int numRequestsCPU; //# completed requests (ie: departures)
	double sumIATCPU;
	double sumTsCPU;
	int sumQCPU;
	int sumWCPU;
	double sumTqCPU;

	//Disk

	Event lastArrDisk;   //prev arrival that just departed
	Event arrInNeedDisk; //next arrival in need of a departure

	//Network

	Event lastArrNetwork;   //prev arrival that just departed
	Event arrInNeedNetwork; //next arrival in need of a departure

	public QueueNet(double Lambda, double Ts, double maxTime)
	{
		this.Lambda = Lambda;
		this.Ts = Ts;
		this.maxTime = maxTime;
		this.K = 1000;
	}

	public QueueNet(int K, double Lambda, double Ts, double maxTime)
	{
		this.Lambda = Lambda;
		this.Ts = Ts;
		this.maxTime = maxTime;
		this.K = K;
	}

	public static void main(String[] args)
	{
		//QueueNet qn = new QueueNet(5, 30, 0.03, 100);
		QueueNet qn = new QueueNet(100, .0085, 100);
		//QueueNet qn = new QueueNet(.08, 20, 100);
		//qn.run();
		
		int neg2 = 0;
		int neg1 = 0;
		int mid = 0;
		int pos1 = 0;
		int pos2 = 0;
		
		for(int i=0; i<10000; i++)
		{
			double num = qn.NetworkServiceTime();
			if(num<=(100-40))
			{
				neg2++;
			}
			else if( num>=(100+40))
			{
				pos2++;
			}
			else if(num<=(100-20) && num>(100-40))
			{
				neg1++;
			}
			else if( num>=(100+20) && num<(100+40))
			{
				pos1++;
			}
			else
			{
				mid++;
			}
			System.out.println(num);
			//System.out.println(qn.randExp(1.0/40.0));
		}
		System.out.println("neg2: " + neg2);
		System.out.println("neg1: " + neg1);
		System.out.println("mid: " + mid);
		System.out.println("pos1: " + pos1);
		System.out.println("pos2: " + pos2);
	}

	public void initializeSchedule()
	{
		Event firstMon = new Event("M",0.0+monitorInc);
		sched = new Schedule(firstMon);

		Event firstArr = new Event("CA", 0.0 + ProcessArrivalTime());//("CA", 0.0 + randExp(1.0/Lambda));
		arrInNeedCPU = firstArr;
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

		System.out.println("\nCPU: ");
		double meanTq = (double)sumTqCPU/(numRequestsCPU);
		double meanTs = (double)sumTsCPU/(numRequestsCPU);
		System.out.println("mean IAT: " + (double)sumIATCPU/(numRequestsCPU));
		System.out.println("mean Ts: " + meanTs);
		System.out.println("mean q: " + (double)sumQCPU/(monitorCount));
		System.out.println("mean w: " + (double)sumWCPU/monitorCount);
		System.out.println("mean Tq: " + meanTq);
		System.out.println("mean Tw: " + (meanTq - meanTs));
		System.out.println("Prob drop: " + (double)numDropped / (numRequestsCPU + numDropped));

		System.out.println("mon: " + monitorCount);
		System.out.println("sumTq: " + sumTqCPU);
		System.out.println("requests: " + numRequestsCPU);
		System.out.println("dropped: " + numDropped);
	}

	public void execute(Event cur)
	{
		if(cur.getType() == "CA" || cur.getType() == "CD")
		{
			executeCPU(cur);
		}
		else if(cur.getType() == "M")
		{
			monitorCount++;
			System.out.println("Time: " + time + "\tCPUQueue: " + theQueueCPU);

			//CPU
			if(theQueueCPU>0)
			{
				if(isBusyCPU)
				{
					sumQCPU += theQueueCPU;
					if(theQueueCPU-1>0)
					{sumWCPU += theQueueCPU-1;}
				}else
				{
					if(theQueueCPU-1 > 0)
					{sumQCPU += theQueueCPU-1;}
					if(theQueueCPU-2>0)
					{sumWCPU += theQueueCPU-2;}
				}
			}

			Event nextMon = new Event("M",cur.getTime() + monitorInc);
			sched.add(nextMon);
		}
	}//end execute

	public void executeCPU(Event cur)
	{
		if(cur.getType() == "CA")
		{
			isBusyCPU = false;

			if(theQueueCPU < K)
			{
				theQueueCPU++;
			}
			else
			{
				cur.setDropped();
				numDropped++;
			}


			//schedule next Arrival
			double nextIAT = ProcessArrivalTime();//randExp(1.0/Lambda);
			sumIATCPU += nextIAT;
			Event nextArr = new Event("CA",cur.getTime() + nextIAT);
			sched.add(nextArr);

			//if I'm only one in queue, sched my departure
			if((theQueueCPU==1 || theQueueCPU==2)&& (theQueueCPU < K+1))
			//if((theQueueCPU==1)&& (theQueueCPU < K+1))
			{
				isBusyCPU = true;

				double myTs = CPUServiceTime();//randExp(Ts);
				sumTsCPU += myTs;
				Event myDepart = new Event("CD", cur.getTime() + myTs);
				sched.add(myDepart);

				lastArrCPU = cur;
				updateNextArr("CA");
			}

		}

		else if(cur.getType() == "CD")
		{
			isBusyCPU = false;
			numRequestsCPU ++;
			theQueueCPU--;
			double Tq = cur.getTime() - lastArrCPU.getTime();
			sumTqCPU += Tq;

			
			if(theQueueCPU>0)
			{
				isBusyCPU = true;
			}
			
			if(theQueueCPU>1) //>0 )
			{
				isBusyCPU = true;

				double nextTs = CPUServiceTime();//randExp(Ts);
				sumTsCPU += nextTs;
				double startTime = max(cur.getTime(), arrInNeedCPU.getTime());
				Event nextDepart = new Event("CD",nextTs + startTime);
				sched.add(nextDepart);

				lastArrCPU = arrInNeedCPU;
				updateNextArr("CA");
			}
			

		}
	}//end of executeCPU

	private double max(double a, double b)
	{
		if(a > b)
		{
			return a;
		}
		return b;
	}

	private void updateNextArr(String type)
	{
		if(type == "CA")
		{
			while(arrInNeedCPU.getNext() != null)
			{
				arrInNeedCPU = arrInNeedCPU.getNext();
				if((arrInNeedCPU.getType() == "CA") && (!arrInNeedCPU.getDropped()))
				{
					return;
				}
			}
		}
		else if(type == "DA")
		{
			while(arrInNeedDisk.getNext() != null)
			{
				arrInNeedDisk = arrInNeedDisk.getNext();
				if(arrInNeedDisk.getType() == "DA")
				{
					return;
				}
			}
		}
		else if(type == "NA")
		{
			while(arrInNeedNetwork.getNext() != null)
			{
				arrInNeedNetwork = arrInNeedNetwork.getNext();
				if(arrInNeedNetwork.getType() == "NA")
				{
					return;
				}
			}
		}

	}

	public double randExp(double T)
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
	 * Arrival rate are Poisson with rate 40 proc/sec = 0.04 proc/msec
	 */ 
	public double ProcessArrivalTime()
	{
		double U = Math.random();
		double lambda = 0.04;
		double V = ( -1 * (Math.log(1.0 - U)) ) / lambda; 
		return V;
	}
		
	/*
	 * CPU service time is uniformly distributed btn 10 and 30 msec
	 */
	public double CPUServiceTime()
	{
		return Math.random()* (30.0 - 10.0) + 10.0;
	}
	
	/* 
	 * Disk I/O service time is normally distributed 
	 * with mean 100 msec and stddev 20 msec (never neg)
	 */
	public double DiskServiceTime()
	{
		double rand = 0.0;
		double sum = 0.0;
		for(int i=0; i<10; i++)
		{
			sum += Math.random();
		}
		sum = (sum - 10.0/2.0) / Math.sqrt(10.0/12.0);
		rand = 20.0 * sum - 100.0;
		rand = Math.abs(rand);
		return rand;
	}


	/*
	 * Network service time is constant with mean 25 msec
	 */
	protected double NetworkServiceTime()
	{
		return 25.0;
	}
	
}//end of class
