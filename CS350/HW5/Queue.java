
public class Queue {

	double IAT[] = {0.33,1.17,0.24,0.45,1.01,1.02,1.26,0.73,1.37,1.86,1.33,
			0.72,1.21,1.81,1.03,1.51,1.85,0.54,0.87,0.35,0.10,1.26,0.73,1.73};
	double Ts[] = {0.56,0.57,0.95,0.83,1.39,0.51,1.33,1.45,1.41,0.52,0.69,
			1.40,1.48,0.62,1.40,1.45,1.00,1.24,1.34,0.64,1.26,0.62,1.03,1.29};
	
	int iatIndex = -1;
	int tsIndex = -1;
	
	double maxTime;
	double time;
	Schedule sched;
	Event currentEvent;
	int q;
	
	Event lastArr;
	Event arrInNeed; //next arrival in need of a departure
	
	int numRequests;
	int sumQ;
	int sumW;
	double sumTq;
	
	public Queue(double maxTime)
	{
		this.maxTime = maxTime;
	}
	
	public static void main(String[] args)
	{
		Queue qu = new Queue(24);
		qu.run();
	}
	
	public void initializeSchedule()
	{
		System.out.println(IAT.length);
		System.out.println(Ts.length);
		
		iatIndex++;
		Event firstArr = new Event("A", 0.0 + IAT[iatIndex]);
		sched = new Schedule(firstArr);
		arrInNeed = firstArr;
		
		System.out.print("IATf: " + IAT[iatIndex]);
		System.out.println();
		
		currentEvent = firstArr;
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
			{
				execute(currentEvent);
			}
			
		}
		
		System.out.println("mean q: " + (double)sumQ/numRequests);
		System.out.println("mean w: " + (double)sumW/numRequests);
		System.out.println("mean Tq: " + (double)sumTq/numRequests);
		
		double tsSum = 0;
		double iatSum = 0;
		for(int i=0; i<numRequests; i++)
		{
			tsSum += Ts[i];
			iatSum += IAT[i];
		}
		System.out.println("mean IAT: " + iatSum/numRequests);
		System.out.println("mean Ts: " + tsSum/numRequests);
	}
	
	public void execute(Event cur)
	{
		if(cur.getType() == "A")
		{
			q++;
			
			//schedule next Arrival
			iatIndex++;
			double nextIAT = IAT[iatIndex];
			Event nextArr = new Event("A",cur.getTime() + nextIAT);
			sched.add(nextArr);
			
			//System.out.print("q: " + q + "\t");
			//System.out.print("IAT: " + nextIAT + "\t");
			
			int w = 0;
			//if I'm only one in queue, sched my departure
			if(q==1)
			{
				tsIndex++;
				double myTs = Ts[tsIndex];
				Event myDepart = new Event("D", cur.getTime() + myTs);
				sched.add(myDepart);
				
				/*
				System.out.print("w: " + w + "\t");
				System.out.print("q: " + q + "\t");
				System.out.print("IAT: " + IAT[iatIndex-1] + "\t");
				System.out.print("Ts: " + myTs + "\t");
				System.out.print("Arr: " + cur.getTime() + "\t");
				System.out.print("Dep: " + myDepart.getTime() + "\t");
				double Tq = myDepart.getTime() - cur.getTime();
				System.out.print("Tq: " + Tq + "\t");
				double Tw = (Tq - myTs);
				System.out.println("Tw: " + Tw);
				*/
				
				lastArr = cur;
				updateNextArr();
				
				//lastTs = myTs;
				//lastIAT = nextIAT;
				
				//System.out.print("Ts: " + myTs + "\t");
			}
			
			//System.out.print("Arr: " + cur.getTime());
			//System.out.println();
			//System.out.println("Q: " + q);
		}
		
		else if(cur.getType() == "D")
		{
			
			
			//q--;

			System.out.print("time: " + time + "\t");
			System.out.print("q: " + q + "\t");
			int w = 0;
			if(q>1)
			{
				w = q-1;
			}
			System.out.print("w: " + w + "\t");
			
			double Tq = cur.getTime() - lastArr.getTime();
			System.out.print("Tq: " + Tq + "\t");
			
			System.out.print("IAT: " + IAT[iatIndex] + "\t");
			System.out.println("Ts: " + Ts[tsIndex] + "\t");
			
			numRequests ++;
			sumQ += q;
			sumW += w;
			sumTq += Tq;
			
			q--;
			
			//System.out.print("q: " + q + "\t");
			
			/*
			int w = 0;
			if(q>1)
			{
				w = q-1;
			}
			System.out.println("w: " + w);
			*/
			
			if(q>0)
			{
				
				
				tsIndex++;
				double nextTs = Ts[tsIndex];
				
				double startTime = max(cur.getTime(), arrInNeed.getTime());
				Event nextDepart = new Event("D",nextTs + startTime);
				sched.add(nextDepart);
				
				
				
				/*
				System.out.print("w: " + w + "\t");
				System.out.print("q: " + q + "\t");
				
				System.out.print("IAT: " + IAT[iatIndex-1] + "\t");
				System.out.print("Ts: " + nextTs + "\t");
				System.out.print("Arr: " + arrInNeed.getTime() + "\t");
				System.out.print("Dep: " + nextDepart.getTime() + "\t");
				double Tq = nextDepart.getTime() - arrInNeed.getTime();
				System.out.print("Tq: " + Tq + "\t");
				double Tw = (Tq - nextTs);
				System.out.println("Tw: " + Tw);
				
				*/
				
				
				lastArr = arrInNeed;
				updateNextArr();
				
				//lastTs = nextTs;
				
				//printStuff();
				//System.out.print("Tq: " + Tq);
				
			}
			
			
			
			
		}
	}

	public void printStuff()
	{
		String report = "";
		//report += "IAT: " + lastIAT;
		//report += "\tTs: " + lastTs;
		report += "\tArr: " + arrInNeed;
		System.out.println(report);
		
	}
	
	public double max(double a, double b)
	{
		if(a > b)
		{
			return a;
		}
		return b;
	}
	
	
	public void updateNextArr()
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
}
