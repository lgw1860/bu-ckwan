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
	double monitorInc = 100;//10;//1000;//.01; //time between monitor events

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
	int theQueueNetwork;
	Event lastArrNetwork;   //prev arrival that just departed
	Event arrInNeedNetwork; //next arrival in need of a departure
	boolean isBusyNetwork = false;
	int numRequestsNetwork;
	double sumTsNetwork;
	double sumTqNetwork;
	double sumOtherTqNetwork;
	int sumQNetwork;
	int sumWNetwork;

	//Whole system
	int numRequestsSystem;
	int sumTqSystem;
	int sumDA;
	int sumExit;
	int sumNA;
	int sumCA;
	int sumQSystem;
	int sumWSystem;

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
		QueueNet qn = new QueueNet(100, .0085, 100000);//1000);//100000);
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

		while(time <= maxTime && currentEvent.getNext() != null)
		{
			currentEvent = currentEvent.getNext();
			time = currentEvent.getTime();
			if(currentEvent.getTime() <= maxTime)
			{execute(currentEvent);}
		}

		//Print stats
		System.out.println("\nSimulation results: ");

		System.out.println("\nCPU: ");
		double meanTq = (double)sumTqCPU/(numRequestsCPU);
		double meanTq2 = (double)sumOtherTqCPU/(numRequestsCPU);
		double meanTs = (double)sumTsCPU/(numRequestsCPU);
		double meanQCPU = (double)sumQCPU/(monitorCount);
		double meanWCPU = (double)sumWCPU/monitorCount;
		System.out.println("mean IAT: " + (double)sumIATCPU/(numRequestsCPU));
		System.out.println("mean Ts: " + meanTs);
		System.out.println("mean q: " + meanQCPU);
		System.out.println("mean w: " + meanWCPU);
		System.out.println("mean Tq: " + meanTq);
		System.out.println("mean Tq2: " + meanTq2);
		System.out.println("mean Tw: " + (meanTq - meanTs));
		System.out.println("Prob drop: " + (double)numDropped / (numRequestsCPU + numDropped));

		System.out.println("mon: " + monitorCount);
		System.out.println("sumTq: " + sumTqCPU);
		System.out.println("sumTq2: " + sumOtherTqCPU);
		System.out.println("requests: " + numRequestsCPU);
		System.out.println("dropped: " + numDropped);

		System.out.println("numProcesses: " + numRequestsSystem);
		System.out.println("sumDA: " + sumDA);
		System.out.println("sumNA: " + sumNA);
		System.out.println("sumExit: " + sumExit);

		//Disk stats
		System.out.println("\nDisk: ");
		double meanTqDisk = (double)sumTqDisk/(numRequestsDisk);
		double meanTq2Disk = (double)sumOtherTqDisk/(numRequestsDisk);
		double meanTsDisk = (double)sumTsDisk/(numRequestsDisk);
		double meanQDisk = (double)sumQDisk/(monitorCount);
		double meanWDisk = (double)sumWDisk/monitorCount;
		System.out.println("mean Ts: " + meanTsDisk);
		System.out.println("mean q: " + meanQDisk);
		System.out.println("mean w: " + meanWDisk);
		System.out.println("mean Tq: " + meanTqDisk);
		System.out.println("mean Tq2: " + meanTq2Disk);
		System.out.println("mean Tw: " + (meanTqDisk - meanTsDisk));

		System.out.println("sumTqDisk: " + sumTqDisk);
		System.out.println("sumTq2Disk: " + sumOtherTqDisk);
		System.out.println("requestsDisk: " + numRequestsDisk);
		
		//Network stats
		System.out.println("\nNetwork: ");
		double meanTqNetwork = (double)sumTqNetwork/(numRequestsNetwork);
		double meanTq2Network = (double)sumOtherTqNetwork/(numRequestsNetwork);
		double meanTsNetwork = (double)sumTsNetwork/(numRequestsNetwork);
		double meanQNetwork = (double)sumQNetwork/(monitorCount);
		double meanWNetwork = (double)sumWNetwork/monitorCount;
		System.out.println("mean Ts: " + meanTsNetwork);
		System.out.println("mean q: " + meanQNetwork);
		System.out.println("mean w: " + meanWNetwork);
		System.out.println("mean Tq: " + meanTqNetwork);
		System.out.println("mean Tq2: " + meanTq2Network);
		System.out.println("mean Tw: " + (meanTqNetwork - meanTsNetwork));

		System.out.println("sumTqNetwork: " + sumTqNetwork);
		System.out.println("sumTq2Network: " + sumOtherTqNetwork);
		System.out.println("requestsNetwork: " + numRequestsNetwork);
		
		//System stats
		System.out.println("\nSystem: ");
		double meanTqSystem = (double)sumTqSystem/(numRequestsSystem);
		//double meanTq2System = (double)sumOtherTqSystem/(numRequestsSystem);
		System.out.println("mean q: " + (double)sumQSystem/(monitorCount));
		//System.out.println("mean q2: " + (meanQCPU + meanQDisk + meanQNetwork) );
		System.out.println("mean w: " + (double)sumWSystem/monitorCount);
		//System.out.println("mean w2: " + (meanWCPU + meanWDisk + meanWNetwork) );
		System.out.println("mean Tq: " + meanTqSystem);
		//System.out.println("mean Tq2: " + meanTq2System);
		
		System.out.println("sumTqSystem: " + sumTqSystem);
		//System.out.println("sumTq2System: " + sumOtherTqSystem);
		System.out.println("requestsSystem: " + numRequestsSystem);
		
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
		else if(cur.getType() == "NA" || cur.getType() == "ND")
		{
			executeNetwork(cur);
		}
		else if(cur.getType() == "M")
		{
			monitorCount++;
			//System.out.println("Time: " + time + "\tCPUQueue: " + theQueueCPU);

		
			
			//CPU
			int curWCPU = 0;
			int curQCPU = 0;
			if(theQueueCPU>0)
			{
				if(isBusyCPU)
				{
					curQCPU = theQueueCPU;
					sumQCPU += curQCPU;
					if(theQueueCPU-1>0)
					{
						curWCPU = theQueueCPU-1;
						sumWCPU += curWCPU;
					}
				}else
				{
					if(theQueueCPU-1 > 0)
					{
						curQCPU = theQueueCPU-1;
						sumQCPU += curQCPU;
					}
					if(theQueueCPU-2>0)
					{
						curWCPU = theQueueCPU-2;
						sumWCPU += curWCPU;
					}
				}
			}

			if(time % 1000 == 0)
			{
				System.out.println(time + "\t" + curWCPU);	
			}
			
			//Disk
			int curQDisk = 0;
			int curWDisk = 0;
			if(theQueueDisk>0)
			{
				if(isBusyDisk)
				{
					curQDisk = theQueueDisk;
					sumQDisk += curQDisk;
					if(theQueueDisk-1>0)
					{
						curWDisk = theQueueDisk-1;
						sumWDisk += curWDisk;
					}
				}else
				{
					if(theQueueDisk-1 > 0)
					{
						curQDisk = theQueueDisk-1;
						sumQDisk += curQDisk;
					}
					if(theQueueDisk-2>0)
					{
						curWDisk = theQueueDisk-2;
						sumWDisk += curWDisk;
					}
				}
			}
			
			//Network
			int curQNetwork = 0;
			int curWNetwork = 0;
			if(theQueueNetwork>0)
			{
				if(isBusyNetwork)
				{
					curQNetwork = theQueueNetwork;
					sumQNetwork += curQNetwork;
					if(theQueueNetwork-1>0)
					{
						curWNetwork = theQueueNetwork-1;
						sumWNetwork += curWNetwork;
					}
				}else
				{
					if(theQueueNetwork-1 > 0)
					{
						curQNetwork = theQueueNetwork-1;
						sumQNetwork += curQNetwork;
					}
					if(theQueueNetwork-2>0)
					{
						curWNetwork = theQueueNetwork-2;
						sumWNetwork += curWNetwork;
					}
				}
			}
			
			//System
			//sumQSystem += (((double)sumQCPU/numRequestsCPU) + ((double)sumQDisk/numRequestsDisk) + ((double)sumQNetwork/numRequestsNetwork));
			//sumWSystem += (sumWCPU + sumWDisk + sumWNetwork);
			sumQSystem += (curQCPU + curQDisk + curQNetwork);
			sumWSystem += (curWCPU + curWDisk + curWNetwork);
			
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
				numRequestsSystem++;
				//System.out.println("Exit");
				sumExit++;
				double SystemTq = cur.getTime() - cur.getOrigArrival();
				sumTqSystem += SystemTq;
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
				if(arrInNeedNetwork == null)
				{
					arrInNeedNetwork = destEvent;
				}
			}

			//System.out.println(destEvent);


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
				if(arrInNeedNetwork == null)
				{
					arrInNeedNetwork = destEvent;
				}
			}

			//System.out.println(destEvent);


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

	public void executeNetwork(Event cur)
	{
		if(cur.getType() == "NA")
		{
			isBusyNetwork = false;
			theQueueNetwork++;

			//if I'm only one in queue, sched my departure
			if(theQueueNetwork==1)
			{
				isBusyNetwork = true;

				double myTs = NetworkServiceTime();//randExp(Ts);
				sumTsNetwork += myTs;
				Event myDepart = new Event("ND", cur.getTime() + myTs, cur.getTime(), cur.getOrigArrival());
				sched.add(myDepart);

				lastArrNetwork = cur;
				updateNextArr("NA");
			}

		}

		else if(cur.getType() == "ND")
		{
			isBusyNetwork = false;
			numRequestsNetwork ++;
			theQueueNetwork--;
			double Tq = cur.getTime() - lastArrNetwork.getTime();
			sumTqNetwork += Tq;
			sumOtherTqNetwork += (cur.getTime() - cur.getArrival());

			//Arrive at CPU queue upon departure

			Event destEvent = new Event("CA",cur.getTime(),cur.getArrival(),cur.getOrigArrival(),true);
			sched.add(destEvent);
			//System.out.println("CPU Arrival");
			sumCA++;

			//System.out.println(destEvent);


			if(theQueueNetwork>0)
			{
				isBusyNetwork = true;

				double nextTs = NetworkServiceTime();//randExp(Ts);
				sumTsNetwork += nextTs;
				double startTime = max(cur.getTime(), arrInNeedNetwork.getTime());
				Event nextDepart = new Event("ND",nextTs + startTime,arrInNeedNetwork.getTime());
				sched.add(nextDepart);

				lastArrNetwork = arrInNeedNetwork;
				updateNextArr("NA");
			}
		}
	}//end of executeNetwork

	
	public void executeExit(Event cur)
	{
		numRequestsSystem ++;
		double Tq = cur.getTime() - cur.getOrigArrival();
		sumTqSystem += Tq;
	}//end of executeExit
	
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
