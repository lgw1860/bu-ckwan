Christopher Kwan  ckwan@bu.edu  U37-02-3645
CS350 HW4 Problem 6 M/M/1/K and M/D/1/K Queue Simulators
2-26-2008

Instructions:
-------------
Compile all files: "javac *.java"
Run Simulation: "java SimulatorMM1K 5 30 0.03 100"
		format: (java SimulatorMM1K K<int> Lambda<double> Ts<double> SimTim<int>)

				"java SimulatorMD1K 5 30 0.03 100"
		format: (java SimulatorMD1K K<int> Lambda<double> Ts<double> SimTim<int>)


Contents of Directory:
----------------------
General:
- Event.java (Represents Arrival/Departure/Monitor)
- Schedule.java (Schedule of Events for Simulator)
- DataTable.txt (Statistics for simulation)
- MonitorLog.txt (Number of items in queue over time)

MM1K:
- SimulatorMM1K.java (M/M/1/K Simulator)
- ControlMM1K.java (Controller of Simulator)
- DataTableMM1KA.txt (K:5, Lambda:30, Ts:0.03, SimTime:100)
- MonitorLogMM1KA.txt (K:5, Lambda:30, Ts:0.03, SimTime:100)
- DataTableMM1KB.txt (K:5, Lambda:50, Ts:0.03, SimTime:100)
- MonitorLogMM1KB.txt (K:5, Lambda:50, Ts:0.03, SimTime:100)

MD1K:
- SimulatorMD1K.java (M/D/1/K Simulator)
- ControlMD1K.java (Controller of Simulator)
- DataTableMD1KA.txt (K:5, Lambda:30, Ts:0.03, SimTime:100)
- MonitorLogMD1KA.txt (K:5, Lambda:30, Ts:0.03, SimTime:100)
- DataTableMD1KB.txt (K:5, Lambda:50, Ts:0.03, SimTime:100)
- MonitorLogMD1KB.txt (K:5, Lambda:50, Ts:0.03, SimTime:100)
