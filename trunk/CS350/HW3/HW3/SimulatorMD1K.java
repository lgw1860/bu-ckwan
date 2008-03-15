
/**
 * @author Christopher Kwan  ckwan@bu.edu  U37-02-3645
 * @name CS350 HW4 Problem 6 Part 2 M/D/1/K Queue Simulator
 * @date 2-26-2008
 * @class SimulatorMD1K.java - Simulator of M/D/1/K Queue
 * 			compile:	"javac SimulatorMD1K.java"
 * 			run:		"java SimulatorMD1K K<int> Lambda<double> Ts<double> SimTime<int>"
 *					ie: "java SimulatorMD1K 5 30 0.03 100"
 */

public class SimulatorMD1K {

	public static void main(String[] args)
	{
		int K = 0;
		double Lambda = 0.0;
		double Ts = 0.0;
		int SimTime = 0;
		
		ControlMD1K c;
		
		K = Integer.parseInt(args[0]);
		Lambda = Double.parseDouble(args[1]);
		Ts = Double.parseDouble(args[2]);
		SimTime = Integer.parseInt(args[3]);
		
		c = new ControlMD1K(K, Lambda, Ts, SimTime);
		c.run();
		
		System.out.println("\nSimulation is done.  Please see DataTable.txt and MonitorLog.txt.");
	}
}
