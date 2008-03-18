
public class QueuingNetwork {

	private double SimTime;
	public double currentTime;
	private Schedule sched;
	private int monitorInc = 1000;
	private Event currentEvent;
	
	private Event currentCPUArr; 
	private Event currentDiskArr; 
	private Event currentNetworkArr; 

	private int qCPU;
	private int qDisk;
	private int qNetwork;

	private int numProcesses;
	
	private int numCPUDepart;
	private int numDiskDepart;
	private int numNetworkDepart;
	
	private double sumCPUTq;
	private int sumCPUQ;
	private double sumCPUArr;
	
	private double sumDiskTq;
	private int sumDiskQ;
	
	private double sumNetworkTq;
	private int sumNetworkQ;
	
	private int sumDA;
	private int sumNA;
	private int sumExit;

	public QueuingNetwork(double SimTime)
	{
		this.SimTime = SimTime;
		initialize();
	}

	public static void main(String[] args)
	{
		//100 sec = 100*1000 msec
		QueuingNetwork qn = new QueuingNetwork(1000);
		qn.simulate();

		for(int i=0; i<10; i++)
		{
			//System.out.println(qn.CPUServiceTime());
			//sum += (qn.NetworkServiceTime());
			//System.out.println(qn.DiskDepartProb());
		}
		//System.out.println(sum/1000.0);


	}

	public void initialize()
	{
		currentTime = 0.0;
		sched = new Schedule(new Event("CA", currentTime + ProcessArrivalTime()));
		sched.add(new Event("M",monitorInc));
		currentEvent = sched.getFirstEvent();
		currentTime = currentEvent.getTime();
		sched.add(new Event("M",0));//currentEvent.getTime() + monitorInc));
		execute(currentEvent);
	}

	public void simulate()
	{
		while(currentTime<SimTime)
		{
			if(currentEvent.getNext() != null)
			{
				currentEvent = currentEvent.getNext();
				currentTime = currentEvent.getTime();

				//only execute events sched before end of sim
				if(currentTime < SimTime)
				{
					execute(currentEvent);
				}
			}
		}
		System.out.println("ave CPUTq: " + sumCPUTq/numCPUDepart);
		System.out.println("ave CPUQ: " + (double)sumCPUQ/(double)numCPUDepart);
		System.out.println("ave CPUArr: " + sumCPUArr/numCPUDepart);
		
		System.out.println("ave DiskTq: " + sumDiskTq/numDiskDepart);
		System.out.println("ave DiskQ: " + (double)sumDiskQ/(double)numDiskDepart);
		
		/*
		System.out.println("E: " + (double)sumExit/(sumExit+sumDA+sumNA));
		System.out.println("DA: " + (double)sumDA/(sumExit+sumDA+sumNA));
		System.out.println("NA: " + (double)sumNA/(sumExit+sumDA+sumNA));
		*/
	}

	public void execute(Event currentEvent)
	{
		if(currentEvent.getType() == "CA" || currentEvent.getType() == "CD")
		{
			executeCPUEvent(currentEvent);
		}
		else if(currentEvent.getType() == "DA" || currentEvent.getType() == "DD")
		{
			executeDiskEvent(currentEvent);
		}
		else if(currentEvent.getType() == "NA" || currentEvent.getType() == "ND")
		{
			executeNetworkEvent(currentEvent);
		}
		else if(currentEvent.getType() == "M")
		{
			executeMonitor(currentEvent);
		}
	}
	

	private void executeCPUEvent(Event currentEvent)
	{
		if(currentEvent.getType() == "CA")
		{
			System.out.print(currentEvent + " -> ");
			qCPU++; //change
			Event myDepart = null;
			double Ts = CPUServiceTime(); //change
			double IAT = ProcessArrivalTime(); //change
			sumCPUArr += IAT;
			if(qCPU == 1)
			{
				myDepart = new Event("CD",currentEvent.getTime()+Ts,Ts);
				sched.add(myDepart);
				currentCPUArr = currentEvent;
				
			}
			else {
				System.out.println("WAIT in queue");
			}
			if(currentEvent.getRepeat() == false)
			{
				Event nextArrive = new Event("CA",currentEvent.getTime() + IAT);
				sched.add(nextArrive);
			}
			
		}
		else if(currentEvent.getType() == "CD")
		{

			if(qCPU>0)
			{
				numCPUDepart++;
				qCPU--;
				
				if(qCPU>0)
				sumCPUQ += qCPU;
				
				double Tq = currentEvent.getTime() - currentCPUArr.getTime();
				sumCPUTq += Tq;

				//System.out.print("q: " + qCPU + "\t");
				//System.out.print("Tq: " + Tq + "\t");
				//System.out.print(currentArrival + "\t");
				//System.out.println(currentEvent);

				
				//Determine destination upon departure
				String dest = CPUDepartProb();
				Event destEvent = null;
				if(dest == "E")
				{
					destEvent = new Event("EXIT",currentEvent.getTime());
					numProcesses++;
					//System.out.println("Exit");
					sumExit++;
				}
				else if(dest == "D")
				{
					destEvent = new Event("DA",currentEvent.getTime());
					sched.add(destEvent);
					//System.out.println("Disk Arrival");
					sumDA++;
				}
				else if(dest == "N")
				{
					destEvent = new Event("NA",currentEvent.getTime());
					sched.add(destEvent);
					//System.out.println("Network Arrival");
					sumNA++;
				}
				
				System.out.println(destEvent);
				
				
				double Ts = CPUServiceTime();
				double actualServiceStart = max(currentEvent.getTime(),currentCPUArr.getTime());
				Event nextDepart = new Event("CD", actualServiceStart+Ts, Ts);
				sched.add(nextDepart);
				updateCurrentArrival("CA");
			}
		}
	}


	private void executeDiskEvent(Event currentEvent)
	{
		if(currentEvent.getType() == "DA")
		{
			System.out.print(currentEvent + " -> ");

			qDisk++;
			Event myDepart = null;
			double Ts = DiskServiceTime();
			
			if(qDisk == 1)
			{
				myDepart = new Event("DD",currentEvent.getTime() + Ts);
				sched.add(myDepart);
				currentDiskArr = currentEvent;
			}
			
			//don't schedule next arrival
			
			//System.out.println(destEvent);
		}else if(currentEvent.getType() == "DD")
		{
			if(qDisk > 0)
			{
				numDiskDepart++;
				qDisk--;
				System.out.println("\nq: " + qDisk);
				sumDiskQ += qDisk;
				double Tq = currentEvent.getTime() - currentDiskArr.getTime();
				sumDiskTq += Tq;
				
			}
			//determine destination upon departure
			String dest = DiskDepartProb();
			Event destEvent = null;
			
			if(dest == "C")
			{
				destEvent = new Event("CA",currentEvent.getTime());
				destEvent.setRepeat(true);
				sched.add(destEvent);
			}
			else if(dest == "N")
			{
				destEvent = new Event("NA",currentEvent.getTime());
				sched.add(destEvent);;
			}
			
			double Ts = DiskServiceTime();
			double actualServiceStart = max(currentEvent.getTime(),currentDiskArr.getTime());
			Event nextDepart = new Event("DD", actualServiceStart+Ts);
			sched.add(nextDepart);
			updateCurrentArrival("DA");
		}
	}
	
	private void executeNetworkEvent(Event currentEvent)
	{
		
	}
	
	private void executeMonitor(Event currentEvent)
	{
		/*
		System.out.print("time: " + currentTime);
		System.out.print("\tave CPUTq: " + sumTq/numDepart);
		System.out.print("\tave CPUQ: " + (double)sumQ/(double)numDepart);
		System.out.println("\tave CPUArr: " + sumArr/numDepart);
		*/
		Event nextMonitor = new Event("M",currentEvent.getTime() + monitorInc);
		sched.add(nextMonitor);
	}
	
	public double max(double a, double b)
	{
		if(a > b)
		{
			return a;
		}
		return b;
	}

	public void updateCurrentArrival(String type)
	{
		Event currentArrival = null;
		if(type == "CA")
		{
			currentArrival = currentCPUArr;
		}
		else if(type == "DA")
		{
			currentArrival = currentDiskArr;
		}
		else if(type == "NA")
		{
			currentArrival = currentNetworkArr;
		}
		
		if(currentArrival != null)
		{
			while(currentArrival.getNext() != null)
			{
				currentArrival = currentArrival.getNext();
				if(currentArrival.getType() == type)
				{
					return;
				}
			}
		}
		
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

}
