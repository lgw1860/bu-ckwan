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
	double monitorInc = 1;//1000;//.01; //time between monitor events

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
	double sumOtherTqCPU;

	//Disk
	int theQueueDisk;
	Event lastArrDisk;   //prev arrival that just departed
	Event arrInNeedDisk; //next arrival in need of a departure
	boolean isBusyDisk = false;
	int numRequestsDisk;
	double sumTsDisk;
	double sumTqDisk;
	double sumOtherTqDisk;
	int sumQDisk;
	int sumWDisk;

	//Network

	Event lastArrNetwork;   //prev arrival that just departed
	Event arrInNeedNetwork; //next arrival in need of a departure

	//Whole system
	int numProcesses;
	int sumDA;
	int sumExit;
	int sumNA;
	int sumCA;

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
		QueueNet qn = new QueueNet(100, .0085, 1000);//100000);
		//QueueNet qn = new QueueNet(.08, 20, 100);
		qn.run();
	}

	public void initializeSchedule()
	{
		Event firstMon = new Event("M",0.0+monitorInc);
		sched = new Schedule(firstMon);

		double firstArrTime = ProcessArrivalTime();
		Event firstArr = new Event("CA", 0.0 + firstArrTime, 0.0 + firstArrTime, 0.0 + firstArrTime );//("CA", 0.0 + randExp(1.0/Lambda));
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
		double meanTq2 = (double)sumOtherTqCPU/(numRequestsCPU);
		double meanTs = (double)sumTsCPU/(numRequestsCPU);
		System.out.println("mean IAT: " + (double)sumIATCPU/(numRequestsCPU));
		System.out.println("mean Ts: " + meanTs);
		System.out.println("mean q: " + (double)sumQCPU/(monitorCount));
		System.out.println("mean w: " + (double)sumWCPU/monitorCount);
		System.out.println("mean Tq: " + meanTq);
		System.out.println("mean Tq2: " + meanTq2);
		System.out.println("mean Tw: " + (meanTq - meanTs));
		System.out.println("Prob drop: " + (double)numDropped / (numRequestsCPU + numDropped));

		System.out.println("mon: " + monitorCount);
		System.out.println("sumTq: " + sumTqCPU);
		System.out.println("sumTq2: " + sumOtherTqCPU);
		System.out.println("requests: " + numRequestsCPU);
		System.out.println("dropped: " + numDropped);

		System.out.println("numProcesses: " + numProcesses);
		System.out.println("sumDA: " + sumDA);
		System.out.println("sumNA: " + sumNA);
		System.out.println("sumExit: " + sumExit);

		System.out.println("\nDisk: ");
		double meanTqDisk = (double)sumTqDisk/(numRequestsDisk);
		double meanTq2Disk = (double)sumOtherTqDisk/(numRequestsDisk);
		double meanTsDisk = (double)sumTsDisk/(numRequestsDisk);
		System.out.println("mean Ts: " + meanTsDisk);
		System.out.println("mean q: " + (double)sumQDisk/(monitorCount));
		System.out.println("mean w: " + (double)sumWDisk/monitorCount);
		System.out.println("mean Tq: " + meanTqDisk);
		System.out.println("mean Tq2: " + meanTq2Disk);
		System.out.println("mean Tw: " + (meanTqDisk - meanTsDisk));

		System.out.println("sumTqDisk: " + sumTqDisk);
		System.out.println("sumTq2Disk: " + sumOtherTqDisk);
		System.out.println("requestsDisk: " + numRequestsDisk);
	}

	public void execute(Event cur)
	{
		if(cur.getType() == "CA" || cur.getType() == "CD")
		{
			executeCPU(cur);
		}
		else if(cur.getType() == "DA" || cur.getType() == "DD")
		{
			executeDisk(cur);
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

			//Disk
			if(theQueueDisk>0)
			{
				if(isBusyDisk)
				{
					sumQDisk += theQueueDisk;
					if(theQueueDisk-1>0)
					{sumWDisk += theQueueDisk-1;}
				}else
				{
					if(theQueueDisk-1 > 0)
					{sumQDisk += theQueueDisk-1;}
					if(theQueueDisk-2>0)
					{sumWDisk += theQueueDisk-2;}
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


			//schedule next Arrival if process did not arrive from another queue
			if(cur.getRepeat() == false)
			{
				double nextIAT = ProcessArrivalTime();//randExp(1.0/Lambda);
				sumIATCPU += nextIAT;
				Event nextArr = new Event("CA",cur.getTime() + nextIAT, cur.getTime() + nextIAT, cur.getTime() + nextIAT);
				sched.add(nextArr);
			}

			//if I'm only one in queue, sched my departure
			if((theQueueCPU==1 || theQueueCPU==2)&& (theQueueCPU < K+1))
				//if((theQueueCPU==1)&& (theQueueCPU < K+1))
			{
				isBusyCPU = true;

				double myTs = CPUServiceTime();//randExp(Ts);
				sumTsCPU += myTs;
				Event myDepart = new Event("CD", cur.getTime() + myTs, cur.getTime(), cur.getOrigArrival());
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
			sumOtherTqCPU += (cur.getTime() - cur.getArrival());

			//Determine destination upon departure
			String dest = CPUDepartProb();
			Event destEvent = null;
			if(dest == "E")
			{
				//have to remember to retain first arrival
				destEvent = new Event("EXIT",cur.getTime(),cur.getArrival(),cur.getOrigArrival());
				numProcesses++;
				//System.out.println("Exit");
				sumExit++;
			}
			else if(dest == "D")
			{
				destEvent = new Event("DA",cur.getTime(),cur.getArrival(),cur.getOrigArrival());
				sched.add(destEvent);
				//System.out.println("Disk Arrival");
				sumDA++;
				if(arrInNeedDisk == null)
				{
					arrInNeedDisk = destEvent;
				}
			}
			else if(dest == "N")
			{
				destEvent = new Event("NA",cur.getTime(),cur.getArrival(),cur.getOrigArrival());
				sched.add(destEvent);
				//System.out.println("Network Arrival");
				sumNA++;
			}

			System.out.println(destEvent);


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
				Event nextDepart = new Event("CD",nextTs + startTime,arrInNeedCPU.getTime());
				sched.add(nextDepart);

				lastArrCPU = arrInNeedCPU;
				updateNextArr("CA");
			}




		}
	}//end of executeCPU

	public void executeDisk(Event cur)
	{
		if(cur.getType() == "DA")
		{
			isBusyDisk = false;
			theQueueDisk++;

			//if I'm only one in queue, sched my departure
			if(theQueueDisk==1)
			{
				isBusyDisk = true;

				double myTs = DiskServiceTime();//randExp(Ts);
				sumTsDisk += myTs;
				Event myDepart = new Event("DD", cur.getTime() + myTs, cur.getTime(), cur.getOrigArrival());
				sched.add(myDepart);

				lastArrDisk = cur;
				updateNextArr("DA");
			}

		}

		else if(cur.getType() == "DD")
		{
			isBusyDisk = false;
			numRequestsDisk ++;
			theQueueDisk--;
			double Tq = cur.getTime() - lastArrDisk.getTime();
			sumTqDisk += Tq;
			sumOtherTqDisk += (cur.getTime() - cur.getArrival());

			//Determine destination upon departure
			String dest = DiskDepartProb();
			Event destEvent = null;
			if(dest == "C")
			{
				destEvent = new Event("CA",cur.getTime(),cur.getArrival(),cur.getOrigArrival(),true);
				sched.add(destEvent);
				//System.out.println("CPU Arrival");
				sumCA++;
			}
			else if(dest == "N")
			{
				destEvent = new Event("NA",cur.getTime(),cur.getArrival(),cur.getOrigArrival());
				sched.add(destEvent);
				//System.out.println("Network Arrival");
				sumNA++;
			}

			System.out.println(destEvent);


			if(theQueueDisk>0)
			{
				isBusyDisk = true;

				double nextTs = DiskServiceTime();//randExp(Ts);
				sumTsDisk += nextTs;
				double startTime = max(cur.getTime(), arrInNeedDisk.getTime());
				Event nextDepart = new Event("DD",nextTs + startTime,arrInNeedDisk.getTime());
				sched.add(nextDepart);

				lastArrDisk = arrInNeedDisk;
				updateNextArr("DA");
			}




		}
	}//end of executeDisk

	
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

	/*
	 * Where will process go after leaving CPU?
	 */
	public String CPUDepartProb()
	{
		double prob = Math.random();
		//System.out.println("prob: " + prob);

		if(prob<=0.5) //0.5 chance
		{
			return ("E");
		}
		else if(prob>0.5 && prob<=(0.5+0.1)) //0.1 chance
		{
			return ("D");
		}
		else	//0.4 chance
		{
			return ("N");
		}
	}

	/*
	 * Where will process go after leaving Disk?
	 */
	public String DiskDepartProb()
	{
		double prob = Math.random();
		//System.out.println("prob: " + prob);
		if(prob <= 0.5)
		{
			return ("C");
		}
		else
		{
			return ("N");
		}
	}

}//end of class
