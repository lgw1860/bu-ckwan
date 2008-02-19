
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
		
		System.out.println();
	}
}
