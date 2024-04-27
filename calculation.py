num_pipes = 5
v = 0.01
fric_f = 0.07
rho = 1000
tolerance = 1e-6
"""
dp = pressure drop in Pa
v = velocity in m/s
pipe_l = pipe length in m
pipe_dia = pipe diameter in m
rho = density in kg/m^3
tolerance = pressure drop convergence
num_pipes = number of pipes
fric_f = friction factor
      
"""
pipe_length_list = []

pipe_dia = 0.001 #initial values of pipe diameter
pipe_dia_list = [pipe_dia]*num_pipes
converged = False
    
for i in range(num_pipes):
    pipe_length = float(input(f"Enter length of pipe {i+1} in meters: "))
    pipe_length_list.append(pipe_length)
        
    
while not converged:
    # Calculate pressure drop for each pipe using current diameters
    dp = []
    for i in range(num_pipes):
        dp.append((fric_f * (pipe_length_list[i]) * (v**2) * rho) / (2 * (pipe_dia_list[i])))
        
    # Calculate the average pressure drop
    avg_dp = sum(dp) / num_pipes
        
    # Update diameters based on the average pressure drop
    for i in range(num_pipes):
        pipe_dia_list[i] = (fric_f * (pipe_length_list[i]) * (v**2) * rho) / (2 * (avg_dp))

    # Check for convergence
    converged = all(abs(dp0 - avg_dp) < tolerance for dp0 in dp)    
    print(dp,avg_dp)
    
for i in range (num_pipes):
    print(f"The diameter for pipe length {pipe_length_list[i]} is {pipe_dia_list[i]}")
    

    