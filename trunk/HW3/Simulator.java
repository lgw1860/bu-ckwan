
/**
 * @author Christopher Kwan  ckwan@bu.edu  U37-02-3645
 * @name CS350 HW3 Problem 4 M/M/1 Queue Simulator
 * @date 2-19-2008
 * @class Simulator.java - Simulator of M/M/1 Queue
 * 			compile:	"javac Simulator.java"
 * 			run:		"java Simulator Lambda<double> Ts<double> SimTime<int>"
 *					ie: "java Simulator 100.0 0.0085 100"
 */
 
public class Simulator {

	public static void main(String[] args)
	{
		double Lambda = 0.0;
		double Ts = 0.0;
		int SimTime = 0;
		
		Control c;
		
		Lambda = Double.parseDouble(args[0]);
		Ts = Double.parseDouble(args[1]);
		SimTime = Integer.parseInt(args[2]);
		
		System.out.println("Lambda: " + Lambda);
		System.out.println("Ts: " + Ts);
		System.out.println("Simulation Time: " + SimTime);
		
		c = new Control(Lambda, Ts, SimTime);
		c.run();
		
		System.out.println("\nSimulation is done.  Please see DataTable.txt and MonitorLog.txt.");
	}
}
