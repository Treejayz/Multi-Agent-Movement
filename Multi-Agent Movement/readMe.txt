CJ Legato & Alex Fig

1. The alignment force was 1, the cohesion force was 1.5, 
	and the seperation force was 3 (multiplied by exponential distance,
	within 2 units)
	
2. Each flock has the same forces as in part 1, 
	but with additional forces of cone check and 
	collision prediction. The weight of these avoidance forces were 3, 
	with the average of the two being taken if both were selected. 
	
3. We used three ray casts (one long, two whiskers) because with 
	just a single ray cast the boid would often get stuck when 
	flying directly at something and it would cut through corners 
	or scrape badly along edges. 