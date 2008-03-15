
public class QueuingNetwork {

	private double SimTime;
	public double currentTime;
	private Schedule sched;
	private Event currentEvent;
	
	public QueuingNetwork(double SimTime)
	{
		this.SimTime = SimTime;
		initialize();
	}
	
	public static void main(String[] args)
	{
		//100 sec = 100*1000 msec
		QueuingNetwork qn = new QueuingNetwork(100);

		for(int i=0; i<1000; i++)
		{
			//sum += (qn.NetworkServiceTime());
			System.out.println(qn.DiskDepartProb());
		}
		//System.out.println(sum/1000.0);
	}
	
	public void initialize()
	{
		currentTime = 0.0;
		sched = new Schedule(new Event("A", currentTime + ProcessArrivalTime()));
		currentEvent = sched.getFirstEvent();
		currentTime = currentEvent.getTime();
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
	}
	
	public void execute(Event e)
	{
		
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
			return ("Leave system");
		}
		else if(prob>0.5 && prob<=(0.5+0.1)) //0.1 chance
		{
			return ("To Disk");
		}
		else	//0.4 chance
		{
			return ("To network");
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
			return ("To CPU");
		}
		else
		{
			return ("To Network");
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
