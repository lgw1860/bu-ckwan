Christopher Kwan
ckwan@bu.edu U37-02-3645
CS350 HW6 Problem 3: Disk Scheduler Simulators

compile: javac *.java
run: 	java DiskRequestsFCFS
		java DiskRequestsRandom
		java DiskRequestsSCAN
		

Analysis:
As expected, FCFS had high head movement and high average response time, random also had high head movement and medium average response time, SCAN had the lowest head movement and average response time.




Simulation results: 

Number of requests: 10000.000000
Lambda: 0.050000 requests per msec
N: 500.000000 tracks
U: 2.000000 msec
V: 0.100000 msec/track


FCFS:

Total head movement: 1664760.000000
Average response time: 198.102740
95th percentile confidence interval of response time: [198.102740 +/- 2.914782]